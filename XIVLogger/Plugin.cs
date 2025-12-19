using System.Globalization;
using Dalamud.Game.Command;
using Dalamud.Game.Text;
using Dalamud.Game.Text.SeStringHandling;
using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.IoC;
using Dalamud.Plugin.Services;
using XIVLogger.Resources;
using XIVLogger.Windows;
using XIVLogger.Windows.Config;

namespace XIVLogger;

public class Plugin : IDalamudPlugin
{
    private const string CommandName = "/xivlogger";

    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static IChatGui Chat { get; private set; } = null!;
    [PluginService] public static IFramework Framework { get; private set; } = null!;
    [PluginService] public static IPlayerState PlayerState { get; private set; } = null!;
    [PluginService] public static IClientState ClientState { get; private set; } = null!;
    [PluginService] public static ICommandManager CommandManager { get; private set; } = null!;
    [PluginService] public static INotificationManager Notification { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!;

    public Configuration Configuration;
    public ChatStorage ChatLog;

    private WindowSystem WindowSystem = new("XIVLogger");
    private ConfigWindow ConfigWindow { get; init; }
    public NewConfigWindow NewConfigWindow { get; init; }

    public Plugin()
    {
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        Configuration.Initialize();

        LanguageChanged(PluginInterface.UiLanguage);

        ConfigWindow = new ConfigWindow(this);
        NewConfigWindow = new NewConfigWindow(this);
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(NewConfigWindow);

        CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
        {
            HelpMessage = "Opens settings window for XIVLogger"
        });

        CommandManager.AddHandler("/savelog", new CommandInfo(OnSaveCommand)
        {
            HelpMessage = "Saves a chat log as a text file with the current settings, /savelog <number> to save the last <number> messages"
        });

        CommandManager.AddHandler("/copylog", new CommandInfo(OnCopyCommand)
        {
            HelpMessage = "Copies a chat log to your clipboard with the current settings, /copylog <number> to copy the last <number> messages"
        });

        ChatLog = new ChatStorage(Configuration);

        PluginInterface.UiBuilder.Draw += DrawUi;
        PluginInterface.UiBuilder.OpenConfigUi += OpenConfig;

        ClientState.Login += OnLogin;
        ClientState.Logout += OnLogout;
        Chat.ChatMessage += OnChatMessage;
        PluginInterface.LanguageChanged += LanguageChanged;

        Framework.Update += OnUpdate;

        // Call it just to make sure a name is set, if login wasn't called
        Framework.RunOnFrameworkThread(() => ChatLog.SetupAutosave());
    }

    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        CommandManager.RemoveHandler(CommandName);
        CommandManager.RemoveHandler("/savelog");
        CommandManager.RemoveHandler("/copylog");

        Framework.Update -= OnUpdate;
        PluginInterface.LanguageChanged -= LanguageChanged;
        Chat.ChatMessage -= OnChatMessage;
        ClientState.Login -= OnLogin;
        ClientState.Logout -= OnLogout;
    }

    private void LanguageChanged(string langCode)
    {
        Language.Culture = new CultureInfo(langCode);
    }

    private void OnLogin()
    {
        ChatLog.SetupAutosave();
    }

    private void OnLogout(int _, int __)
    {
        if (!Configuration.fAutosave)
            return;

        ChatLog.AutoSave();
        ChatLog.WipeLog();
    }

    private void OnUpdate(IFramework framework)
    {
        if (!Configuration.fAutosave)
            return;

        if (!ClientState.IsLoggedIn)
            return;

        if (!Configuration.CheckTime())
            return;

        ChatLog.AutoSave();
        Configuration.UpdateAutosaveTime();
    }

    private void OnChatMessage(XivChatType type, int _, ref SeString sender, ref SeString message, ref bool handled)
    {
        ChatLog.AddMessage(type, sender.TextValue, message.TextValue);
    }

    private void OnCommand(string command, string args)
    {
        OpenConfig();
    }

    private void OnSaveCommand(string command, string args)
    {
        ChatLog.PrintLog(args);
    }

    private void OnCopyCommand(string command, string args)
    {
        ImGui.SetClipboardText(ChatLog.PrintLog(args, aClipboard: true));
    }

    private void DrawUi() => WindowSystem.Draw();
    private void OpenConfig() => ConfigWindow.IsOpen = true;
}