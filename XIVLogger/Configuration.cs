using Dalamud.Configuration;
using Dalamud.Game.Text;

namespace XIVLogger;

[Serializable]
public class Configuration : IPluginConfiguration
{
    public int Version { get; set; } = 2;

    public List<ChatConfigV2> ImprovedConfigList = [];

    public ChatConfigV2 NewDefaultConfig;
    public ChatConfigV2 NewActiveConfig;
    public string FilePathV2 = string.Empty;
    public string FileNameV2 = string.Empty;
    public bool Timestamp = false;
    public bool TimeSeconds = false;
    public bool Timestamp24H = false;
    public bool Datestamp = false;
    public bool Autosave = false;
    public bool AutosaveNotif = true;
    public DateTime LastAutosaveV2;
    public int AutoMin = 5;
    public string AutoFilePathV2 = string.Empty;
    public bool StoreEveryMessage = false;

    public List<ChatConfig> configList = [];

    public ChatConfig defaultConfig;
    public ChatConfig activeConfig;
    public string filePath = string.Empty;
    public string fileName = string.Empty;
    public bool fTimestamp = false;
    public bool fTimeSeconds = false;
    public bool f24hTimestamp = false;
    public bool fDatestamp = false;
    public bool fAutosave = false;
    public bool fAutosaveNotif = true;
    public DateTime lastAutosave;
    public int fAutoMin = 5;
    public string autoFilePath = string.Empty;

    // This is for file names
    public bool FileSortableDatetime = false;

    public void Initialize()
    {
        var selectedConfig = ImprovedConfigList.FirstOrDefault(c => c.IsActive);
        if (selectedConfig is not null)
        {
            NewActiveConfig = selectedConfig;
            return;
        }

        // No active config found, so we set the default config
        if (NewDefaultConfig == null)
        {
            NewDefaultConfig = new ChatConfigV2();
            NewActiveConfig = NewDefaultConfig;
        }

        SetActiveConfig(NewDefaultConfig);
    }

    public void Save()
    {
        Plugin.PluginInterface.SavePluginConfig(this);
    }

    public void Migrate()
    {
        Version = 2;

        ChatConfigV2 newConfig;
        foreach (var oldChat in configList)
        {
            newConfig = new ChatConfigV2(oldChat.name);
            newConfig.IsActive = oldChat.IsActive;

            newConfig.TypeConfig.Clear();
            MigrateDict(oldChat.TypeConfig, newConfig.TypeConfig);

            ImprovedConfigList.Add(newConfig.Clone());
        }

        newConfig = new ChatConfigV2(defaultConfig.name);
        newConfig.IsActive = defaultConfig.IsActive;

        newConfig.TypeConfig.Clear();
        MigrateDict(defaultConfig.TypeConfig, newConfig.TypeConfig);

        NewDefaultConfig = newConfig.Clone();

        newConfig = new ChatConfigV2(activeConfig.name);
        newConfig.IsActive = activeConfig.IsActive;

        newConfig.TypeConfig.Clear();
        MigrateDict(activeConfig.TypeConfig, newConfig.TypeConfig);

        NewDefaultConfig = newConfig.Clone();

        FilePathV2 = filePath;
        FileNameV2 = fileName;
        Timestamp = fTimestamp;
        TimeSeconds = fTimeSeconds;
        Timestamp24H = f24hTimestamp;
        Datestamp = fDatestamp;
        Autosave = fAutosave;
        AutosaveNotif = fAutosaveNotif;
        AutoFilePathV2 = autoFilePath;
        AutoMin = fAutoMin;


        return;

        void MigrateDict(Dictionary<int, bool> oldDict, Dictionary<XivChatType, bool> newDict)
        {
            foreach (var (key, value) in oldDict)
            {
                if (key is 8266 or 2122)
                {
                    // set to true if it's not already set
                    if (newDict.ContainsKey(XivChatType.RandomNumber) && value)
                        newDict[XivChatType.RandomNumber] = value;

                    continue;
                }

                newDict[(XivChatType)key] = value;
            }
        }
    }

    public void SetActiveConfig(ChatConfigV2 aConfig)
    {
        NewActiveConfig.IsActive = false;
        NewActiveConfig = aConfig;
        NewActiveConfig.IsActive = true;

        Save();
    }

    public ChatConfigV2 AddNewConfig(string name)
    {
        ImprovedConfigList.Add(new ChatConfigV2(name));

        return ImprovedConfigList.Last();
    }

    public void RemoveConfig(ChatConfigV2 aConfig)
    {
        if (aConfig == NewDefaultConfig)
            return;

        if (aConfig.IsActive)
            SetActiveConfig(NewDefaultConfig);

        ImprovedConfigList.Remove(aConfig);
    }

    public bool CheckTime()
    {
        if (AutoMin == 0)
            return false;

        return LastAutosaveV2.AddMinutes(AutoMin) < DateTime.UtcNow;
    }

    public void UpdateAutosaveTime()
    {
        LastAutosaveV2 = DateTime.UtcNow;
    }
}

public class ChatConfig
{
    public string name;
    private Dictionary<int, bool> typeConfig;
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
            { 2122, true },
            { 8266, true },
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
            { (int) XivChatType.Notice, false },
            { (int) XivChatType.Debug, false }
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
            { 2122, true },
            { 8266, true },
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
            { (int) XivChatType.Notice, false },
            { (int) XivChatType.Debug, false }
        };
    }
}

public class ChatConfigV2
{
    public string Name;
    public Dictionary<XivChatType, bool> TypeConfig;
    public bool IsActive;

    public ChatConfigV2()
    {
        Name = "Default";
        IsActive = false;

        TypeConfig = new Dictionary<XivChatType, bool>
        {
            { XivChatType.Say, true },
            { XivChatType.Shout, true },
            { XivChatType.Yell, true },
            { XivChatType.Party, true },
            { XivChatType.CrossParty, true },
            { XivChatType.Alliance, true },
            { XivChatType.TellIncoming, true },
            { XivChatType.TellOutgoing, true },
            { XivChatType.CustomEmote, true },
            { XivChatType.StandardEmote, true },
            { XivChatType.RandomNumber, true },
            { XivChatType.CrossLinkShell1, false },
            { XivChatType.CrossLinkShell2, false },
            { XivChatType.CrossLinkShell3, false },
            { XivChatType.CrossLinkShell4, false },
            { XivChatType.CrossLinkShell5, false },
            { XivChatType.CrossLinkShell6, false },
            { XivChatType.CrossLinkShell7, false },
            { XivChatType.CrossLinkShell8, false },
            { XivChatType.Ls1, false },
            { XivChatType.Ls2, false },
            { XivChatType.Ls3, false },
            { XivChatType.Ls4, false },
            { XivChatType.Ls5, false },
            { XivChatType.Ls6, false },
            { XivChatType.Ls7, false },
            { XivChatType.Ls8, false },
            { XivChatType.PvPTeam, false },
            { XivChatType.NoviceNetwork, false },
            { XivChatType.FreeCompany, false },
            { XivChatType.Echo, false },
            { XivChatType.SystemMessage, false },
            { XivChatType.SystemError, false },
            { XivChatType.Notice, false },
            { XivChatType.Debug, false }
        };
    }

    public ChatConfigV2(string aName)
    {
        Name = aName;
        IsActive = false;

        TypeConfig = new Dictionary<XivChatType, bool>
        {
            { XivChatType.Say, true },
            { XivChatType.Shout, true },
            { XivChatType.Yell, true },
            { XivChatType.Party, true },
            { XivChatType.CrossParty, true },
            { XivChatType.Alliance, true },
            { XivChatType.TellIncoming, true },
            { XivChatType.TellOutgoing, true },
            { XivChatType.CustomEmote, true },
            { XivChatType.StandardEmote, true },
            { XivChatType.RandomNumber, true },
            { XivChatType.CrossLinkShell1, false },
            { XivChatType.CrossLinkShell2, false },
            { XivChatType.CrossLinkShell3, false },
            { XivChatType.CrossLinkShell4, false },
            { XivChatType.CrossLinkShell5, false },
            { XivChatType.CrossLinkShell6, false },
            { XivChatType.CrossLinkShell7, false },
            { XivChatType.CrossLinkShell8, false },
            { XivChatType.Ls1, false },
            { XivChatType.Ls2, false },
            { XivChatType.Ls3, false },
            { XivChatType.Ls4, false },
            { XivChatType.Ls5, false },
            { XivChatType.Ls6, false },
            { XivChatType.Ls7, false },
            { XivChatType.Ls8, false },
            { XivChatType.PvPTeam, false },
            { XivChatType.NoviceNetwork, false },
            { XivChatType.FreeCompany, false },
            { XivChatType.Echo, false },
            { XivChatType.SystemMessage, false },
            { XivChatType.SystemError, false },
            { XivChatType.Notice, false },
            { XivChatType.Debug, false }
        };
    }

    public ChatConfigV2 Clone()
    {
        return new ChatConfigV2
        {
            Name = Name,
            IsActive = IsActive,
            TypeConfig = new Dictionary<XivChatType, bool>(TypeConfig),
        };
    }
}