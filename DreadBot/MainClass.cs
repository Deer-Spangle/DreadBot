﻿//MIT License
//Copyright(c) [2019]
//[Xylex Sirrush Rayne]
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.IO;
using System.Threading;

namespace DreadBot
{
    class MainClass
    {
        #region Main GetUpdates Loop

        public static long UpdateId = 0;
        public static readonly int LauchTime = Utilities.EpochTime();
        static void Main()
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ExitCleanUp);

            Database.Init();

            Result<User> res = Methods.getMe();
            if (!res.ok) {
                Logger.LogFatal("Error getting bot info: " + res.description);
                Environment.Exit(-1);
            }
            Configs.Me = res.result;
            Result<WebhookInfo> webres = Methods.getWebhookInfo();
            if (!webres.ok)
            {
                Logger.LogFatal("Error getting bot info: " + webres.description);
                Environment.Exit(-1);
            }
            Configs.webhookinfo = webres.result;

            PluginManager.Init();

            Cron.CronInit();

            if (Configs.RunningConfig.Owner == 0)
            {
                Console.WriteLine("There is no owner assigned to this bot.\r\nPlease send me the command below to claim ownership.\r\nThis token will be deleted once you use it.\r\n\r\n");
                Console.WriteLine("/token " + Utilities.CreateAdminToken(true));
            }

            if (Configs.RunningConfig.AdminChat < 0 || Configs.RunningConfig.AdminChat > 0)
            {
                string text = "**DreadBot Started**\n" + Utilities.NormalTime(Utilities.EpochTime()) + "\n\n" + PluginManager.getPluginList();
                Methods.sendMessage(Configs.RunningConfig.AdminChat, text);
            }

            Console.Title = "DreadBot: @" + Configs.Me.username;
            Console.WriteLine(PluginManager.getPluginList() + "\n");
            Console.WriteLine("DreadBot Loaded, and Started!\n");

            while (true)
            {
                if (Configs.RunningConfig.GetupdatesMode) //GetUpdates Mode
                {
                    Update[] updates = null;
                    if (UpdateId == 0) {
                        Result<Update[]> updateres = Methods.getOneUpdate(3600);
                        if (!updateres.ok)
                        {
                            Logger.LogError("Error fetching first update: (" + updateres.errorCode + ") " + updateres.description);
                            Thread.Sleep(10000);
                            continue;
                        }
                        updates = updateres.result;
                        if (updates == null || updates.Length < 1) { continue; }
                        else
                        {
                            UpdateId = updates[0].update_id;
                            if (Configs.webhookinfo.pendingUpdateCount > 3) { Logger.LogInfo("Playing catchup; " + Configs.webhookinfo.pendingUpdateCount + " updates behind."); }
                        }
                    }
                    else
                    {
                        GetUpdates request = new GetUpdates() { timeout = 20, offset = ++UpdateId, limit = Configs.RunningConfig.GULimit };
                        Result<Update[]> updatesres = Methods.getUpdates(request);
                        if (!updatesres.ok)
                        {
                            Logger.LogError("Error fetching updates: (" + updatesres.errorCode + ") " + updatesres.description);
                            Thread.Sleep(10000);
                            continue;
                        }
                        updates = updatesres.result;
                    }
                    if (updates.Length < 1) { continue; }
                    foreach (Update update in updates)
                    {
                        UpdateId = update.update_id;
                        Events.ParseUpdate(update);
                    }
                }
                else // Webhook mode
                {
                    // Webhook mode init Code.
                }
            }
        }

        #endregion

        #region Shutdown Cleanup

        private static void ExitCleanUp(object o, EventArgs e)
        {
            Logger.LogAdmin("Exiting DreadBot. Cleaning up...");

            Database.databaseCron.Abort();
            Database.dbEvent.OnDatabseFire();

            Cron.CronThread.Abort();

            if (Configs.RunningConfig.AdminChat != 0)
            {
                Methods.sendMessage(Configs.RunningConfig.AdminChat, "DreadBot is termining Cleaningly.");
            }
        }

        #endregion
    }
}
