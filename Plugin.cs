using IL.OTAPI;
using System.Data;
using System.Diagnostics;
using Terraria;
using TerrariaApi.Server;
using TShockAPI;

namespace TimeStatistic
{
    [ApiVersion(2, 1)]
    public class TimeStatisticPlugin : TerrariaPlugin
    {
        #region Data
        public override string Author => "HeatSpace";
        public override string Name => "TimeStatistic";
        public TimeStatisticPlugin(Main game) : base(game) { }

        public override Version Version => new Version(1, 0, 0);

        public static Config config { get; set; }

        #endregion

        public override async void Initialize()
        {
            config = Config.Read();
            TShockAPI.Hooks.GeneralHooks.ReloadEvent += OnReload;
            Database.InitializeBD();
            Commands.ChatCommands.Add(new Command("timestat.checktime", PluginCommands.CheckTimeStatCommand, "checktime", "playtime", "ptime"));
            Commands.ChatCommands.Add(new Command("timestat.pchecktime", PluginCommands.CheckPlayerTimeStatCommand, "cptime"));
            Commands.ChatCommands.Add(new Command("timestat.gonline", PluginCommands.CheckGroupTimeStatCommand, "gptime", "gonline"));
            await Task.Run(() => UpdateTime());
        }

        private void OnReload(TShockAPI.Hooks.ReloadEventArgs e)
        {
            config = Config.Read();
        }

        public async Task UpdateTime()
        {
            try
            {
                while (true)
                {
                    Stopwatch st = Stopwatch.StartNew();

                    if (config.updateTime)
                    {
                        foreach (var plr in TShock.Players.Where(plr => plr != null && plr.IsLoggedIn))
                        {
                            Console.WriteLine(Database.GetPlaytime(plr.Name).ToString());
                            Database.CreateOrUpdatePlaytime(plr.Group.Name, plr.Name, Database.GetPlaytime(plr.Name).AddSeconds(1));
                        }
                    }
                    else
                    {
                        if(DateTime.Now == config.wipeStart)
                        {
                            config.updateTime = true;
                            config.Write();
                        }
                    }

                    

                    st.Stop();
                    await Task.Delay(1000 - (int)st.ElapsedMilliseconds);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                await Task.Run(() => UpdateTime());
            }
        }
    }

}
