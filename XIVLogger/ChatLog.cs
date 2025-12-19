using System.IO;
using Dalamud.Game.Text;
using XIVLogger.Resources;

namespace XIVLogger;

public class ChatMessage
{
    public XivChatType Type { get; }
    public string Message { get; }
    public string Sender { get; }
    public DateTime Timestamp { get; }

    public ChatMessage(XivChatType type, string sender, string message)
    {
        Sender = sender;
        Type = type;
        Message = message;
        Timestamp = DateTime.Now;
    }
}

public class ChatStorage
{
    private const string LegacyDatetimeFormat = "dd-MM-yyyy_hh.mm.ss";
    private const string SortableDatetimeFormat = "yyyy-MM-dd_HH.mm.ss";

    // We never modify this reference once it is set in the constructor.
    private readonly Configuration Config;

    // This can be readonly, but it would require using Clear() to empty it.
    // See WipeLog() for why this is not ideal.
    private List<ChatMessage> LogList;

    private int AutoMsgCounter;
    private string AutoFileName;

    public ChatStorage(Configuration config)
    {
        AutoMsgCounter = 0;
        AutoFileName = string.Empty;
        Config = config;
        LogList = [];
    }

    public void WipeLog()
    {
        AutoMsgCounter = 0;

        // We could invoke Clear here, but, this ensures the memory is released.
        // List is backed by an array, similar to Java's ArrayList and clearing the
        // array does not reduce its size.
        LogList = [];
    }

    public void AddMessage(XivChatType type, string sender, string message)
    {
        LogList.Add(new ChatMessage(type, sender, message));
    }

    private string GetTimeStamp()
    {
        return DateTime.Now.ToString(Config.fileSortableDatetime ? SortableDatetimeFormat : LegacyDatetimeFormat, System.Globalization.CultureInfo.InvariantCulture);
    }

    private static bool CheckValidPath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return false;

        return Path.IsPathRooted(path) && Directory.Exists(path);
    }

    private static string ReplaceInvalidChars(string filename)
    {
        return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
    }

    public string PrintLog(string args, bool aClipboard = false)
    {
        var lastN = 0;
        if (!string.IsNullOrEmpty(args))
            int.TryParse(args, out lastN);

        var printedLog = PrepareLog(aLastN: lastN, aTimestamp: Config.fTimestamp, aTimeSeconds: Config.fTimeSeconds, a24hTimestamp: Config.f24hTimestamp, aDatestamp: Config.fDatestamp);

        if (aClipboard)
        {
            var clip = string.Empty;

            foreach (var message in printedLog)
            {
                clip += message;
                clip += Environment.NewLine;
            }

            if (lastN > 0)
            {
                Plugin.Chat.Print(new XivChatEntry
                {
                    Message = string.Format(Language.ChatMessageCopyNumber, lastN),
                    Type = XivChatType.Echo
                });
            }
            else
            {
                Plugin.Chat.Print(new XivChatEntry
                {
                    Message = Language.ChatMessageCopy,
                    Type = XivChatType.Echo
                });
            }

            return clip;
        }

        var name = GetTimeStamp();
        var folder = !CheckValidPath(Config.filePath)
            ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            : Config.filePath;

        if (!string.IsNullOrEmpty(Config.fileName) && !string.IsNullOrWhiteSpace(Config.fileName))
            name = ReplaceInvalidChars(Config.fileName);

        var path = $@"{folder}\{name}.txt";
        var count = 0;

        while (File.Exists(path))
        {
            count++;
            path = $@"{folder}\{name}{count}.txt";
        }

        using (var file = new StreamWriter(path, true))
        {
            file.WriteLine($"{name}\n");

            foreach (var message in printedLog)
                file.WriteLine(message);
        }

        Plugin.Chat.Print(lastN > 0 ? string.Format(Language.ChatMessageLastCopyNumber, lastN, path) : string.Format(Language.ChatMessageLastCopy, path));
        return path;
    }

    private List<string> PrepareLog(int aLastN = 0, bool aTimestamp = false, bool aTimeSeconds = false, bool a24hTimestamp = false, bool aDatestamp = false, bool auto = false)
    {
        var activeConfig = Config.activeConfig;
        var result = new List<string>();

        foreach (var message in LogList)
        {
            activeConfig.TypeConfig.TryGetValue((int)message.Type, out var ok);
            if (!ok && !Config.StoreEveryMessage)
                continue;

            var text = string.Empty;
            if (aTimestamp)
            {
                text += "[";
                if (aDatestamp)
                {
                    text += $"{message.Timestamp:yyyy}-{message.Timestamp:MM}-{message.Timestamp:dd} ";
                }
                if (a24hTimestamp)
                {
                    if (aTimeSeconds)
                    {
                        text += $"{message.Timestamp:HH}:{message.Timestamp:mm}:{message.Timestamp:ss}";
                    } else {
                        text += $"{message.Timestamp:HH}:{message.Timestamp:mm}";
                    }
                } else {
                    text += $"{message.Timestamp:t}";
                }
                text += "] ";
            }

            text += $"{message.Type.ToChatName(message.Sender)}{message.Message}";
            result.Add(text);
        }

        if (aLastN > 0)
            result = result.Skip(Math.Max(0, result.Count - aLastN)).ToList();

        // Only return the messages which have been received since the last autosave, and update the counter
        if (auto)
        {
            result = result.Skip(AutoMsgCounter).ToList();
            AutoMsgCounter += result.Count;
        }

        return result;
    }

    public void SetupAutosave()
    {
        AutoFileName = $"{GetTimeStamp()} {Plugin.PlayerState.CharacterName}";
    }

    public void AutoSave()
    {
        if (!Config.fAutosave)
            return;

        var printedLog = PrepareLog(aLastN: 0, aTimestamp: Config.fTimestamp, aTimeSeconds: Config.fTimeSeconds, a24hTimestamp: Config.f24hTimestamp, aDatestamp: Config.fDatestamp, true);
        var folder = !CheckValidPath(Config.autoFilePath)
            ? Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            : Config.autoFilePath;

        if (printedLog.Count == 0)
            return;

        var path = Path.Combine(folder, $"{AutoFileName}.txt");

        using var file = new StreamWriter(path, true);
        foreach (var message in printedLog)
            file.WriteLine(message);

        if (Config.fAutosaveNotif)
            Plugin.Chat.Print(new XivChatEntry { Message = string.Format(Language.ChatMessageAutosave, path), Type = XivChatType.Echo });
    }
}