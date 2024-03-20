using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using System.Data;
using TShockAPI;
using TShockAPI.DB;

namespace TimeStatistic
{
    public static class Database
    {

        public static IDbConnection DB;

        internal static void InitializeBD()
        {
            switch (TShock.Config.Settings.StorageType.ToLower())
            {
                default:
                    return;
                case "mysql":
                    var hostport = TShock.Config.Settings.MySqlHost.Split(':');
                    string connection = string.Format("Server={0}; Port={1}; Database={2}; Userid={3}; Password={4};",
                        hostport[0],
                        hostport.Length > 1 ? hostport[1] : "3306",
                        TShock.Config.Settings.MySqlDbName,
                        TShock.Config.Settings.MySqlUsername,
                        TShock.Config.Settings.MySqlPassword);
                    DB = new MySqlConnection(connection);
                    break;
                case "sqlite":
                    DB = new SqliteConnection(@"Data Source=tshock\PlayTime.sqlite");
                    break;
            }
            SqlTableCreator table = new(DB, DB.GetSqlType() == SqlType.Sqlite ? new SqliteQueryCreator() : new MysqlQueryCreator());
            table.EnsureTableStructure(new SqlTable("Playtime",
                new SqlColumn("Username", MySqlDbType.Text),
                new SqlColumn("Group",MySqlDbType.String),
                new SqlColumn("Time", MySqlDbType.Int32)));
        }


        public static bool Exists(string username)
        {
            using var reader = DB.QueryReader("SELECT * FROM Playtime WHERE Username = @0", username);
            return reader.Read();
        }

        public static TimeDuration GetPlaytime(string username)
        {
            using (var reader = DB.QueryReader("SELECT * FROM Playtime WHERE Username = @0", username))
            {
                if (reader.Read())
                {
                    return new TimeDuration(reader.Get<int>("Time"));
                }
            }
            return new TimeDuration(0);
        }

        public static List<PlayerTime> GetPlaytimeByGroup(string group)
        {
            List<PlayerTime> times = new();
            using var reader = DB.QueryReader("SELECT * FROM Playtime WHERE 'Group' = @0", group);
            if (reader.Read())
            {
                var time = new TimeDuration(reader.Get<int>("Time"));
                var user = reader.Get<string>("Username");
                times.Add(new PlayerTime(time, user));
            }
            return times;
        }

        public static void CreatePlaytimeSave(string group, string username, TimeDuration time)
        {
            DB.Query("INSERT INTO Playtime VALUES (@0, @1, @2)", username, group, time.ToInt32());
        }

        public static void SetPlaytime(string group, string username, TimeDuration time)
        {
            DB.Query("UPDATE Playtime SET Time = @0, 'Group' = @1 WHERE Username = @2", time.ToInt32(), group, username);
        }

        public static void CreateOrUpdatePlaytime(string group, string username, TimeDuration time)
        {
            if (Exists(username))
            {
                SetPlaytime(group, username, time);
            }
            else
            {
                CreatePlaytimeSave(group, username, time);
            }
        }

    }
}
