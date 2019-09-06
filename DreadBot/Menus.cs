﻿#region License
//MIT License
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

#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreadBot
{
    class Menus
    {

        #region Button Event Logic

        internal static void ButtonPush(CallbackQuery callback)
        {
            string arg = callback.data.Split(' ')[1];

            switch (arg)
            {
                case "botadm": {
                        Methods.answerCallbackQuery(callback.id);
                        string text = "*DreadBot Managment Options*\n\nThese options are for the bot owner only (you) and are critical settings for the operation of your bot. Use these settings with care. All of these settings take immediate effect.\n\n`Sensitive Options`\nThese settings control very low level settings. Such as how often the bot will request new messages, and even setting up webhook, and more!\n\n`Add/Remove Bot Admins`\nIn the event you want to grant others control voer the bot, manage who can use it, and what control they have over it, you can use this menu.\n\n`Debug Chat Settings`\nBy Default, the \"Debug Chat\" is PM between you and the bot. These settings let you use a group instead, as well as control what options can be set from this group. This can really help if you are tired of getting Debug messages in PM.\n\n`System Sounds`\nThis allows DreadBot to play custom sounds per events.You send DreadBot WAV/ MP3 Files to play to correlate to specific events.";
                        Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", ManagmentMenu());
                        return;
                    }
                case "tuneables": {

                        if (callback.from.id != Configs.RunningConfig.Owner)
                        {
                            Methods.answerCallbackQuery(callback.id, "You are not allowed to view these options.", true);
                            Logger.LogAdmin("User Attempted to access the Sensitive Options Menu.(" + callback.from.id + ") " + callback.from.first_name);
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id);
                            string text = "*Senitive Tunables*\n\nThese Settings are VERY sensitive and alter how the Bot will communicate with Telegram.\nDo Not Change these settings unless you are familiar with them!\n\n" +
                                "`Show Access Token`\nThis will Display the access token the bot has been assigned to.\n\n" +
                                "`Change Access Token`\nThis will allow you to change the access toke and use a different bot account.\n*WARNING!* This can cause harmful sideeffects to any database entires plugins you have added use.\nUSE WITH CAUTION!\n\n" +
                                "`Change Owner`\nThis allows you to pass Ownership of the bot to another Telegram user. Only the Owner can use this button.\n\n" +
                                "`Get Updates Limit`\nUsing the + - Buttons, you can change the number of updates the bot will ask for when it gets Mssages from Telegram.\n(Min 1 - Max 100 - Default 20)\n\n" +
                                "`Bot Operation Mode`\nThis allows you set change the bot's method of getting updates between getUpdates and WebHook. This can heavily impact the funcationality of the bot and you shouldn't use it unless you know what you're doing.\n\n";

                            Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", TuneablesMenu());
                        }
                        return;
                    }
                case "botadmins": { return; }
                case "debugchatcfg": {
                        Methods.answerCallbackQuery(callback.id);
                        string text = "*Debug Chat Settings*\n\nHere you can configure the Debug Chat. This is used by the bot as a good testing grounds, somewhere to dump errors for all Bot Admins to see, admin commands and more.\n\n`Toggle Debug Chat Commands`\nEnable Bot Admin commands for use within the Debug Chat. Its very useful for troubleshooting, and testing.\n\n`Change Debug Chat`\nThis is what you use to change the Debug to a Group, Supergroup, or back to Private (Bot Owner only).\n\n`Set the Debug Chat Log Level`\nHere you can set how verbose the bot is with its error reporting in the Debug Chat. We recommend atleast [Warn] Level.";
                        Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", DebugChatCfg());
                        return;
                    }
                case "debugchatlevel":
                    {
                        Methods.answerCallbackQuery(callback.id);
                        string text = "*Log Level Configuration*\n\nHere you can set the desired log level of the Console and the Log File.\n\nThe right column is for the Console.\nThe left side is for the log File. (it is log.txt and is off by default.)\n\nWe strongly recommend the console log level be no lower than *WARN* as some of this information can be important to resolve problems.";
                        Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", LogLevelMenu());
                        return;
                    }
                case "resetdebug": {

                        string text = "";
                        if (Configs.RunningConfig.Owner != Configs.RunningConfig.AdminChat)
                        {
                            text = "Debug chat has been reset to PM.";
                            Configs.RunningConfig.AdminChat = callback.from.id;
                        }
                        else { text = "Debug chat is already set to PM."; }
                        Methods.answerCallbackQuery(callback.id, text, true);
                        return;

                    }
                case "changedebugchat":
                    {
                        Methods.answerCallbackQuery(callback.id);
                        string text = "*Debug Chat Location Setting*\n\nHere you can resttore the settings for where the bot sends Debug messages. To set a Debug Group, add me to a group, and perform /setdebug and I will use that group for debug output.\n\n`Reset Debug Chat to Private`\nAs the buttons states, this will reset the location of debug messages to PM with you.";
                        Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", ChangeDebugChat());
                        return;
                    }
                case "operationmode":
                    {
                        Methods.answerCallbackQuery(callback.id);
                        string text = "*Operation Mode Settings*\n\n*WebHook*\nClicking this will bring you to the menu to enable, configure and test your webhook.\nPlease Read [this](http://dreadbot.net/wiki/index.php/WebHook_Explained) page to fully understand what the differences are between Webhook and getUpdates.\n\n`getUpdates`\nThis is a Toggle button specifically for Disabling webhook easily. Clicking it while getUpdates is enable will do nothing.";
                        Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", OperationMenu());
                        return;
                    }
                case "webhookcfg":
                    {
                        Methods.answerCallbackQuery(callback.id);
                        string text = "*WebHook Config*\n\n`Test & Go Live!`\nClicking this will reach out to the Dreadbot site to help you test the accessibility fo your bot to make sure webhook was setup correctly.\nOnce verified, the bot will then offer you the chance to make the switch.\n\n`Set URL`\nUse this to set the Full URL you want telegram to use; Example http://dreadbot.net/bot/ \n\n`Set Certificate`\nHere you can configure the SSL/TLS Certificate the bot will use to encrypt communication between your bot and Telegram.\n\n`Port Cfg`\nYou must select a port that the bot will use to listen on for the HTTPS Traffic that will be sent by Telegram.";
                        Methods.editMessageText(callback.from.id, callback.message.message_id, text, "markdown", WebhookMenu());
                        return;
                    }

                case "showtoken":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            Methods.answerCallbackQuery(callback.id);
                            Methods.editMessageText(callback.from.id, callback.message.message_id, "`" + Configs.RunningConfig.token + "`", "markdown", BackOnly("tuneables"));
                            return;
                        }
                        else {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "changetoken":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            Methods.answerCallbackQuery(callback.id);
                            Methods.editMessageText(callback.from.id, callback.message.message_id, "This is a major change to make. Please read [this](http://dreadbot.net/wiki/index.php/Change_Token) page before proceeding.", "markdown", BackOnly("tuneables"));
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "changeowner":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            Methods.answerCallbackQuery(callback.id);
                            Methods.editMessageText(callback.from.id, callback.message.message_id, "If you do this, you will no longer have ANY ACCESS to this bot's options or administrative tools unless given to you by the new owner after the change has been made.\nPlease read [this](http://dreadbot.net/wiki/index.php/Change_Owner) page before proceeding.", "markdown", BackOnly("tuneables"));
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "gulhelp":
                    {
                        Methods.answerCallbackQuery(callback.id, "Changes the Get Updates Limit", true);
                        return;
                    }
                case "gula1": {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            if (Configs.RunningConfig.GULimit > 100)
                            {
                                Configs.RunningConfig.GULimit++;
                                Methods.answerCallbackQuery(callback.id);
                            }
                            else if (Configs.RunningConfig.GULimit == 100)
                            {
                                Methods.answerCallbackQuery(callback.id, "Maxed out.", true);
                            }
                            else
                            {
                                Configs.RunningConfig.GULimit = 100;
                                Methods.answerCallbackQuery(callback.id, "Maxed out.", true);
                            }
                            Database.SaveConfig();
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "gula10":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            if (Configs.RunningConfig.GULimit >= 90)
                            {
                                Configs.RunningConfig.GULimit = Configs.RunningConfig.GULimit + 10;
                                Methods.answerCallbackQuery(callback.id);
                            }
                            else if (Configs.RunningConfig.GULimit == 100)
                            {
                                Methods.answerCallbackQuery(callback.id, "Maxed out.", true);
                            }
                            else if (Configs.RunningConfig.GULimit < 90 && Configs.RunningConfig.GULimit > 100)
                            {
                                Configs.RunningConfig.GULimit = Configs.RunningConfig.GULimit + (100 - Configs.RunningConfig.GULimit);
                                Methods.answerCallbackQuery(callback.id, "Maxed out.", true);
                            }
                            else if (Configs.RunningConfig.GULimit < 100)
                            {
                                Configs.RunningConfig.GULimit = 100;
                                Methods.answerCallbackQuery(callback.id, "Maxed out.", true);
                            }
                            Database.SaveConfig();
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "gulm10":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            if (Configs.RunningConfig.GULimit >= 11)
                            {
                                Configs.RunningConfig.GULimit = Configs.RunningConfig.GULimit - 10;
                                Methods.answerCallbackQuery(callback.id);
                            }
                            else if (Configs.RunningConfig.GULimit == 1)
                            {
                                Methods.answerCallbackQuery(callback.id, "Maxed out.", true);
                            }
                            else if (Configs.RunningConfig.GULimit < 11  && Configs.RunningConfig.GULimit > 1)
                            {
                                Configs.RunningConfig.GULimit = Configs.RunningConfig.GULimit - Configs.RunningConfig.GULimit + 1;
                                Methods.answerCallbackQuery(callback.id, "Bottomed out.", true);
                            }
                            else if (Configs.RunningConfig.GULimit <= 0)
                            {
                                Configs.RunningConfig.GULimit = 1;
                                Methods.answerCallbackQuery(callback.id, "Bottomed out.", true);
                            }
                            Database.SaveConfig();
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "gulm1":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            if (Configs.RunningConfig.GULimit > 1)
                            {
                                Configs.RunningConfig.GULimit--;
                                Methods.answerCallbackQuery(callback.id);
                            }
                            else if (Configs.RunningConfig.GULimit == 1)
                            {
                                Methods.answerCallbackQuery(callback.id, "Bottomed out.", true);
                            }
                            else
                            {
                                Configs.RunningConfig.GULimit = 1;
                                Methods.answerCallbackQuery(callback.id, "Bottomed out.", true);
                            }
                            Database.SaveConfig();
                            return;
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
                case "systemsounds":
                    {
                        if (callback.from.id == Configs.RunningConfig.Owner)
                        {
                            Methods.answerCallbackQuery(callback.id);
                            Methods.editMessageText(callback.from.id, callback.message.message_id, "*Sound System*\n\nThis system enables DreadBot to play any WAV/MP3 from the console where DreadBot is running as an event sound clip.\n\n- Sned Me any WAV/MP3 No larger than 20MB. \n- Reply to the Uploaded file with /addsound\n\nThe Sound System MUST be enable or the file will be ignored.\n\nClick \"Sound Assignments\" to assign sounds to events.", "makrdown", SoundSystem());
                        }
                        else
                        {
                            Methods.answerCallbackQuery(callback.id, "You are no longer allowed to view these options.", true);
                            Methods.deleteMessage(callback.from.id, callback.message.message_id);
                            Logger.LogAdmin("User Attempted to use a senitive menu Option. (" + callback.from.id + ") " + callback.from.first_name);
                        }
                        return;
                    }
            }
        }



        #endregion

        #region Menu Methods

        internal static InlineKeyboardMarkup AdminMenu()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(3);
            keyboard.addButton(new InlineKeyboardButton() { text = "🎛 DreadBot Managment", callback_data = "dreadbot botadm" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "🗄 DataBase Managment", callback_data = "dreadbot dbadm" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "⚡️ Plugin Manager", callback_data = "dreadbot plugadm" }, 2);
            return keyboard;
        }

        internal static InlineKeyboardMarkup ManagmentMenu()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(5);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔥 Sensitive Options", callback_data = "dreadbot tuneables" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "👮‍♂️ Add/Remove Bot Admins", callback_data = "dreadbot botadmins" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "🗒 Debug Chat Settings", callback_data = "dreadbot debugchatcfg" }, 2);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔉 System Sounds", callback_data = "dreadbot sounds" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot adm" }, 4);
            return keyboard;
        }

        internal static InlineKeyboardMarkup ChangeDebugChat()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(2);
            keyboard.addButton(new InlineKeyboardButton() { text = "Reset Debug Chat to Private", callback_data = "dreadbot resetdebug" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot debugchatcfg" }, 1);
            return keyboard;
        }

        internal static InlineKeyboardMarkup LogLevelMenu()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(8);
            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Debug " + GetLogLevel(LogLevel.Debug, false), callback_data = "dreadbot fll 6" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Debug " + GetLogLevel(LogLevel.Debug), callback_data = "dreadbot cll 6" }, 0);

            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Admin " + GetLogLevel(LogLevel.Admin, false), callback_data = "dreadbot fll 5" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Admin " + GetLogLevel(LogLevel.Admin), callback_data = "dreadbot cll 5" }, 1);

            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Info " + GetLogLevel(LogLevel.Info, false), callback_data = "dreadbot fll 4" }, 2);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Info " + GetLogLevel(LogLevel.Info), callback_data = "dreadbot cll 4" }, 2);

            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Warn " + GetLogLevel(LogLevel.Warn, false), callback_data = "dreadbot fll 3" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Warn " + GetLogLevel(LogLevel.Warn), callback_data = "dreadbot cll 3" }, 3);

            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Error " + GetLogLevel(LogLevel.Error, false), callback_data = "dreadbot fll 2" }, 4);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Error " + GetLogLevel(LogLevel.Error), callback_data = "dreadbot cll 2" }, 4);

            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Fatal " + GetLogLevel(LogLevel.Fatal, false), callback_data = "dreadbot fll 1" }, 5);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Fatal " + GetLogLevel(LogLevel.Fatal), callback_data = "dreadbot cll 1" }, 5);

            keyboard.addButton(new InlineKeyboardButton() { text = "File LogLevel - Off " + GetLogLevel(LogLevel.Off, false), callback_data = "dreadbot fll 0" }, 6);
            keyboard.addButton(new InlineKeyboardButton() { text = "Console LogLevel - Off " + GetLogLevel(LogLevel.Off), callback_data = "dreadbot cll 0" }, 6);

            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot debugchatcfg" }, 7);

            return keyboard;
        }

        internal static InlineKeyboardMarkup DebugChatCfg()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(3);
            keyboard.addButton(new InlineKeyboardButton() { text = "🧾 Change Debug Chat", callback_data = "dreadbot changedebugchat" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔕 Set Debug Chat Log Level", callback_data = "dreadbot debugchatlevel" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot botadm" }, 2);
            return keyboard;
        }
        internal static InlineKeyboardMarkup PluginMgr()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();

            return keyboard;

        }

        private static InlineKeyboardMarkup OperationMenu()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(3);
            string gumode = "";
            string whmode = "";
            if (Configs.RunningConfig.GetupdatesMode) { gumode = " ⬅️"; }
            else { whmode = " ⬅️"; }

            keyboard.addButton(new InlineKeyboardButton() { text = "GetUpdates" + gumode, callback_data = "dreadbot disablewebhook" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "WebHook" + whmode, callback_data = "dreadbot webhookcfg" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot tuneables" }, 2);
            return keyboard;
        }

        private static InlineKeyboardMarkup WebhookMenu()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(5);

            keyboard.addButton(new InlineKeyboardButton() { text = "Test & Go Live!", callback_data = "dreadbot disablewebhook" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "Set URL", callback_data = "dreadbot webhookcfg" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "Set Certificate", callback_data = "dreadbot webhookcfg" }, 2);

            keyboard.addButton(new InlineKeyboardButton() { text = "Port Cfg", callback_data = "dreadbot webhookcfg" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "443", callback_data = "dreadbot webhookcfg" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "+", callback_data = "dreadbot webhookcfg" }, 3);

            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot operationmode" }, 4);
            return keyboard;
        }



        internal static InlineKeyboardMarkup TuneablesMenu()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(6);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔑 Show Access Token", callback_data = "dreadbot showtoken" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔑 Change Access Token", callback_data = "dreadbot changetoken" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "👮‍♂️ Change Owner", callback_data = "dreadbot changeowner" }, 2);

            keyboard.addButton(new InlineKeyboardButton() { text = "+1", callback_data = "dreadbot gula1" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "+10", callback_data = "dreadbot gula10" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = Configs.RunningConfig.GULimit.ToString(), callback_data = "dreadbot gulhelp" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "-10", callback_data = "dreadbot gulm10" }, 3);
            keyboard.addButton(new InlineKeyboardButton() { text = "-1", callback_data = "dreadbot gulm1" }, 3);

            keyboard.addButton(new InlineKeyboardButton() { text = "⚙️ Bot Operation Mode", callback_data = "dreadbot operationmode" }, 4);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot botadm" }, 5);
            return keyboard;

        }

        private static InlineKeyboardMarkup SoundSystem()
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(3);
            string snd = "🔇";
            if (Configs.RunningConfig.SystemSounds) { snd = "🔊"; }
            keyboard.addButton(new InlineKeyboardButton() { text = "Enable/Disable Sounds", callback_data = "dreadbot sndtogglehelp" }, 0);
            keyboard.addButton(new InlineKeyboardButton() { text = snd, callback_data = "dreadbot toggle" }, 0);

            keyboard.addButton(new InlineKeyboardButton() { text = "Sound Assignments", callback_data = "dreadbot soundselect" }, 1);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot botadm" }, 2);
            return keyboard;
        }

        internal static InlineKeyboardMarkup BackOnly(string CallBack)
        {
            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup();
            keyboard.SetRowCount(1);
            keyboard.addButton(new InlineKeyboardButton() { text = "🔙", callback_data = "dreadbot " + CallBack }, 0);
            return keyboard;
        }

        #endregion

        internal static string GetLogLevel(LogLevel level, bool console = true)
        {
            if (console)
            {
                if (Logger.CurrentConsoleLevel == level) { return "⬅️"; }
            }
            else
            {
                if (Logger.CurrentLogLevel == level) { return "⬅️"; }
            }
            return "";
        }
    }
}
