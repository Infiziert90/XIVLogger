﻿using Dalamud.Configuration;
using Dalamud.Game.Text;
using Dalamud.Plugin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace XIVLogger
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;

        public List<ChatConfig> configList;

        public ChatConfig defaultConfig;

        public ChatConfig activeConfig;

        public Dictionary<int, Boolean> EnabledChatTypes;

        public Dictionary<int, string> PossibleChatTypes;

        public string filePath = string.Empty;

        public string fileName = string.Empty;

        public bool fTimestamp = false;

        public bool fAutosave = false;

        public DateTime lastAutosave;

        public float fAutosaveMin = 5;

        public string autoFilePath = string.Empty;

        public string autoFileName = string.Empty;

        [NonSerialized]
        private DalamudPluginInterface pluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.pluginInterface = pluginInterface;

            PossibleChatTypes = new Dictionary<int, string>
                {
                    { (int) XivChatType.Say, "Say"},
                    { (int) XivChatType.Shout, "Shout" },
                    { (int) XivChatType.Yell, "Yell" },
                    { (int) XivChatType.Party, "Party" },
                    { (int) XivChatType.CrossParty, "Cross World Party" },
                    { (int) XivChatType.Alliance, "Alliance" },
                    { (int) XivChatType.TellIncoming, "Tell Incoming" },
                    { (int) XivChatType.TellOutgoing, "Tell Outgoing" },
                    { (int) XivChatType.CustomEmote, "Custom Emotes" },
                    { (int) XivChatType.StandardEmote, "Standard Emotes" },
                    { (int) XivChatType.CrossLinkShell1, "Cross Link Shell 1" },
                    { (int) XivChatType.CrossLinkShell2, "Cross Link Shell 2" },
                    { (int) XivChatType.CrossLinkShell3, "Cross Link Shell 3" },
                    { (int) XivChatType.CrossLinkShell4, "Cross Link Shell 4" },
                    { (int) XivChatType.CrossLinkShell5, "Cross Link Shell 5" },
                    { (int) XivChatType.CrossLinkShell6, "Cross Link Shell 6" },
                    { (int) XivChatType.CrossLinkShell7, "Cross Link Shell 7" },
                    { (int) XivChatType.CrossLinkShell8, "Cross Link Shell 8" },
                    { (int) XivChatType.Ls1, "Linkshell 1" },
                    { (int) XivChatType.Ls2, "Linkshell 2" },
                    { (int) XivChatType.Ls3, "Linkshell 3" },
                    { (int) XivChatType.Ls4, "Linkshell 4" },
                    { (int) XivChatType.Ls5, "Linkshell 5" },
                    { (int) XivChatType.Ls6, "Linkshell 6" },
                    { (int) XivChatType.Ls7, "Linkshell 7" },
                    { (int) XivChatType.Ls8, "Linkshell 8" },
                    { (int) XivChatType.PvPTeam, "PVP Team" },
                    { (int) XivChatType.NoviceNetwork, "Novice Network" },
                    { (int) XivChatType.FreeCompany, "Free Company" },
                    { (int) XivChatType.Echo, "Echo (Some System Messages)" },
                    { (int) XivChatType.SystemMessage, "System Messages" },
                    { (int) XivChatType.SystemError, "System Error" },
                    { (int) XivChatType.Notice, "Notice" }
                };


            defaultConfig = new ChatConfig();

            configList = new List<ChatConfig>();

            configList.Add(defaultConfig);

            setActiveConfig(defaultConfig);

        }

        public void Save()
        {
            this.pluginInterface.SavePluginConfig(this);
        }

        public void setActiveConfig(ChatConfig aConfig)
        {
            activeConfig.IsActive = false;
            activeConfig = aConfig;
            activeConfig.IsActive = true;
        }

        public ChatConfig addNewConfig(string name)
        {
            configList.Add(new ChatConfig(name));

            return configList.Last();
        }

        public void removeConfig(ChatConfig aConfig)
        {
            if (aConfig.IsActive)
            {
                setActiveConfig(defaultConfig);
            }

            configList.Remove(aConfig);
        }

        public bool checkTime()
        {
            return lastAutosave.AddMinutes(fAutosaveMin) > DateTime.UtcNow;
        }

        public void updateAutosaveTime()
        {
            lastAutosave = DateTime.UtcNow;
        }

    }

    public class ChatConfig
    {
        private string name;

        private Dictionary<int, bool> typeConfig;

        private Dictionary<string, string> nameReplacements;

        private bool isActive;

        public Dictionary<int, bool> TypeConfig { get => typeConfig; set => typeConfig = value; }
        public string Name { get => name; set => name = value; }
        public bool IsActive { get => isActive; set => isActive = value; }

        public ChatConfig()
        {
            name = "Default";

            isActive = false;

            typeConfig = new Dictionary<int, bool>
                {
                    { (int) XivChatType.Say, true },
                    { (int) XivChatType.Shout, true },
                    { (int) XivChatType.Yell, true },
                    { (int) XivChatType.Party, true },
                    { (int) XivChatType.CrossParty, true },
                    { (int) XivChatType.Alliance, true },
                    { (int) XivChatType.TellIncoming, true },
                    { (int) XivChatType.TellOutgoing, true },
                    { (int) XivChatType.CustomEmote, true },
                    { (int) XivChatType.StandardEmote, true },
                    { (int) XivChatType.CrossLinkShell1, false },
                    { (int) XivChatType.CrossLinkShell2, false },
                    { (int) XivChatType.CrossLinkShell3, false },
                    { (int) XivChatType.CrossLinkShell4, false },
                    { (int) XivChatType.CrossLinkShell5, false },
                    { (int) XivChatType.CrossLinkShell6, false },
                    { (int) XivChatType.CrossLinkShell7, false },
                    { (int) XivChatType.CrossLinkShell8, false },
                    { (int) XivChatType.Ls1, false },
                    { (int) XivChatType.Ls2, false },
                    { (int) XivChatType.Ls3, false },
                    { (int) XivChatType.Ls4, false },
                    { (int) XivChatType.Ls5, false },
                    { (int) XivChatType.Ls6, false },
                    { (int) XivChatType.Ls7, false },
                    { (int) XivChatType.Ls8, false },
                    { (int) XivChatType.PvPTeam, false },
                    { (int) XivChatType.NoviceNetwork, false },
                    { (int) XivChatType.FreeCompany, false },
                    { (int) XivChatType.Echo, false },
                    { (int) XivChatType.SystemMessage, false },
                    { (int) XivChatType.SystemError, false },
                    { (int) XivChatType.Notice, false }
                };
        }

        public ChatConfig(string aName)
        {
            name = aName;

            isActive = false;

            typeConfig = new Dictionary<int, bool>
                {
                    { (int) XivChatType.Say, true },
                    { (int) XivChatType.Shout, true },
                    { (int) XivChatType.Yell, true },
                    { (int) XivChatType.Party, true },
                    { (int) XivChatType.CrossParty, true },
                    { (int) XivChatType.Alliance, true },
                    { (int) XivChatType.TellIncoming, true },
                    { (int) XivChatType.TellOutgoing, true },
                    { (int) XivChatType.CustomEmote, true },
                    { (int) XivChatType.StandardEmote, true },
                    { (int) XivChatType.CrossLinkShell1, false },
                    { (int) XivChatType.CrossLinkShell2, false },
                    { (int) XivChatType.CrossLinkShell3, false },
                    { (int) XivChatType.CrossLinkShell4, false },
                    { (int) XivChatType.CrossLinkShell5, false },
                    { (int) XivChatType.CrossLinkShell6, false },
                    { (int) XivChatType.CrossLinkShell7, false },
                    { (int) XivChatType.CrossLinkShell8, false },
                    { (int) XivChatType.Ls1, false },
                    { (int) XivChatType.Ls2, false },
                    { (int) XivChatType.Ls3, false },
                    { (int) XivChatType.Ls4, false },
                    { (int) XivChatType.Ls5, false },
                    { (int) XivChatType.Ls6, false },
                    { (int) XivChatType.Ls7, false },
                    { (int) XivChatType.Ls8, false },
                    { (int) XivChatType.PvPTeam, false },
                    { (int) XivChatType.NoviceNetwork, false },
                    { (int) XivChatType.FreeCompany, false },
                    { (int) XivChatType.Echo, false },
                    { (int) XivChatType.SystemMessage, false },
                    { (int) XivChatType.SystemError, false },
                    { (int) XivChatType.Notice, false }
                };
        }


    }


    public class ChatLog
    {
        private readonly List<ChatMessage> log;
        private DalamudPluginInterface pi;
        private Configuration config;

        public List<ChatMessage> Log { get => log; }

        public ChatLog(Configuration aConfig, DalamudPluginInterface aPi)
        {
            log = new List<ChatMessage>();
            config = aConfig;
            pi = aPi;
        }

        public void addMessage(XivChatType type, string sender, string message)
        {
            Log.Add(new ChatMessage(type, sender, message));
        }

        private string getTimeStamp()
        {
            return DateTime.Now.ToString("dd-MM-yyyy_hh.mm.ss");
        }

        public bool checkValidPath(string path)
        {
            if (String.IsNullOrEmpty(path)) { return false; }

            if (Path.IsPathRooted(path))
            {
                return Directory.Exists(path);
            }

            return false;
        }

        public string replaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }

        public string printLog(string args, bool aClipboard = false)
        {

            List<String> printedLog;

            int lastN = 0;

            if (!string.IsNullOrEmpty(args))
            {
                Int32.TryParse(args, out lastN);
            }

            printedLog = prepareLog(aLastN: lastN, aTimestamp: config.fTimestamp);

            if (aClipboard)
            {
                string clip = String.Empty;

                foreach (string message in printedLog)
                {
                    clip += message;
                    clip += Environment.NewLine;
                }

                if (lastN > 0)
                {
                    this.pi.Framework.Gui.Chat.PrintChat(new XivChatEntry
                    {
                        MessageBytes = Encoding.UTF8.GetBytes($"Last {lastN} messages copied to clipboard."),
                        Type = XivChatType.Echo
                    });
                }
                else
                {
                    this.pi.Framework.Gui.Chat.PrintChat(new XivChatEntry
                    {
                        MessageBytes = Encoding.UTF8.GetBytes($"Chat log copied to clipboard."),
                        Type = XivChatType.Echo
                    });
                }

                return clip;
            }
            else
            {
                string name = getTimeStamp();

                string folder;

                if (!checkValidPath(config.filePath))
                {
                    folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                else
                {
                    folder = config.filePath;
                }

                if (!string.IsNullOrEmpty(config.fileName) && !string.IsNullOrWhiteSpace(config.fileName))
                {
                    name = replaceInvalidChars(config.fileName);
                }

                string path = folder + @"\" + name + ".txt";

                int count = 0;

                while (File.Exists(path))
                {
                    count++;
                    path = folder + @"\" + name + count + ".txt";

                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine(name + "\n");

                    foreach (string message in printedLog)
                    {
                        file.WriteLine(message);
                    }

                }

                if (lastN > 0)
                {
                    this.pi.Framework.Gui.Chat.PrintChat(new XivChatEntry
                    {
                        MessageBytes = Encoding.UTF8.GetBytes($"Last {lastN} messages saved at {path}."),
                        Type = XivChatType.Echo
                    });
                }
                else
                {
                    this.pi.Framework.Gui.Chat.PrintChat(new XivChatEntry
                    {
                        MessageBytes = Encoding.UTF8.GetBytes($"Chat log saved at {path}."),
                        Type = XivChatType.Echo
                    });
                }

                return path;
            }

        }

        private List<string> prepareLog(int aLastN = 0, bool aTimestamp = false)
        {
            ChatConfig activeConfig = config.activeConfig;

            List<string> result = new List<string>();

            foreach (ChatMessage message in Log)
            {
                if (activeConfig.TypeConfig.ContainsKey((int)message.Type) && activeConfig.TypeConfig[(int)message.Type])
                {
                    string text = String.Empty;

                    if (aTimestamp)
                    {
                        text += $"[{message.Timestamp:t}] ";
                    }

                    switch (message.Type)
                    {
                        case XivChatType.CustomEmote:
                            text += message.Sender + message.Message;
                            break;
                        case XivChatType.StandardEmote:
                            text += message.Message;
                            break;
                        case XivChatType.TellIncoming:
                            text += message.Sender + " >> " + message.Message;
                            break;
                        case XivChatType.TellOutgoing:
                            text += ">> " + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.FreeCompany:
                            text += "[FC]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.NoviceNetwork:
                            text += "[NN]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell1:
                            text += "[CWLS1]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell2:
                            text += "[CWLS2]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell3:
                            text += "[CWLS3]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell4:
                            text += "[CWLS4]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell5:
                            text += "[CWLS5]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell6:
                            text += "[CWLS6]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell7:
                            text += "[CWLS7]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.CrossLinkShell8:
                            text += "[CWLS8]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls1:
                            text += "[LS1]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls2:
                            text += "[LS2]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls3:
                            text += "[LS3]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls4:
                            text += "[LS4]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls5:
                            text += "[LS5]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls6:
                            text += "[LS6]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls7:
                            text += "[LS7]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Ls8:
                            text += "[LS8]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.PvPTeam:
                            text += "[PvP]" + message.Sender + ": " + message.Message;
                            break;
                        case XivChatType.Say:
                        case XivChatType.Shout:
                        case XivChatType.Yell:
                        case XivChatType.Party:
                        case XivChatType.CrossParty:
                        case XivChatType.Alliance:
                            text += message.Sender + ": " + message.Message;
                            break;
                        default:
                            text += message.Message;
                            break;
                    }

                    result.Add(text);
                }
            }

            if (aLastN > 0)
            {
                result = result.Skip(Math.Max(0, result.Count - aLastN)).ToList();
            }

            return result;
        }

        public void autoSave()
        {
            if (config.fAutosave)
            {
                List<String> printedLog;

                printedLog = prepareLog(aLastN: 0, aTimestamp: config.fTimestamp);

                string name = getTimeStamp();

                string folder;

                if (!checkValidPath(config.autoFilePath))
                {
                    folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }
                else
                {
                    folder = config.autoFilePath;
                }

                if (!string.IsNullOrEmpty(config.autoFileName) && !string.IsNullOrWhiteSpace(config.autoFileName))
                {
                    name = replaceInvalidChars(config.autoFileName);
                }

                string path = folder + @"\" + name + ".txt";

                int count = 0;

                while (File.Exists(path))
                {
                    count++;
                    path = folder + @"\" + name + count + ".txt";

                }

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, false))
                {
                    file.WriteLine(name + "\n");

                    foreach (string message in printedLog)
                    {
                        file.WriteLine(message);
                    }

                }

                this.pi.Framework.Gui.Chat.PrintChat(new XivChatEntry
                {
                    MessageBytes = Encoding.UTF8.GetBytes($"Autosaved chat log to {path}."),
                    Type = XivChatType.Echo
                });
            }
        }

    }

    public class ChatMessage
    {
        private XivChatType type;
        private string message;
        private string sender;
        private DateTime timestamp;

        public string Message { get => message; set => message = value; }
        public XivChatType Type { get => type; set => type = value; }
        public string Sender { get => sender; set => sender = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }

        public ChatMessage(XivChatType type, string sender, string message)
        {
            this.Sender = sender;
            this.Type = type;
            this.Message = message;
            this.Timestamp = DateTime.Now;
        }
    }

}



