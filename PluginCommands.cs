using TShockAPI;

namespace TimeStatistic
{
    public static class PluginCommands
    {
        public static void CheckTimeStatCommand(CommandArgs e)
        {
            e.Player.SendInfoMessage($"Вы играете в этом вайпе уже {Database. GetPlaytime(e.Player.Name)}");
        }

        public static void TimeUpdateToggleCommand(CommandArgs e)
        {
            TimeStatisticPlugin.config.updateTime = !Config.Read().updateTime;
            TShock.Log.Debug($"Update time set to {Config.Read().updateTime}");
            e.Player.SendSuccessMessage($"Update time set to {Config.Read().updateTime}");
        }

        public static void CheckPlayerTimeStatCommand(CommandArgs e)
        {
            if (e.Parameters.Count > 0)
            {
                TimeDuration time = Database.GetPlaytime(e.Parameters[0]);
                if (time != null)
                {
                    e.Player.SendInfoMessage($"'{e.Parameters[0]}' играет в этом вайпе уже {time}");
                }
                else
                {
                    e.Player.SendErrorMessage($"В базе данных нет игрока '{e.Parameters[0]}'!");
                }
            }
            else
            {
                e.Player.SendErrorMessage("Неверный формат! /ptime <Полное имя игрока>");
            }
        }
        public static void CheckGroupTimeStatCommand(CommandArgs e)
        {
            if (e.Parameters.Count > 0)
            {
                string group = e.Parameters[0];
                if (group == TShock.Config.Settings.DefaultGuestGroupName || group == TShock.Config.Settings.DefaultRegistrationGroupName)
                {
                    e.Player.SendErrorMessage("Нельзя ввести название стандартной группы!");
                }
                else
                {
                    var times = Database.GetPlaytimeByGroup(e.Parameters[0]);
                    if (times.Count > 0)
                    {
                        foreach (PlayerTime time in times)
                        {
                            e.Player.SendInfoMessage($"{time.Username} в этом вайпе играл {time.Time}");
                        }
                    }
                    else
                    {
                        e.Player.SendErrorMessage("Нет игроков в базе данных с группой '{0}'!", e.Parameters[0]);
                    }
                }
            }
            else
            {
                e.Player.SendErrorMessage("Неверный формат! /gonline <group>");
            }
        }
    }
}
