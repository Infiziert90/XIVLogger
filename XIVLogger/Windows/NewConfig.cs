using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using XIVLogger.Resources;

namespace XIVLogger.Windows;

public class NewConfigWindow : Window, IDisposable
{
    private readonly Plugin Plugin;
    public ChatConfigV2? SelectedConfig = null;

    public NewConfigWindow(Plugin plugin) : base("New Configuration##XIVLogger")
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(500, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        Plugin = plugin;
    }

    public void Dispose() { }

    public override void Draw()
    {
        if (SelectedConfig == null)
        {
            ImGui.TextUnformatted(Language.NewConfigNotSelected);
            return;
        }

        if (ImGui.Button(Language.NewConfigSetActive))
            Plugin.Configuration.SetActiveConfig(SelectedConfig);

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(Language.NewConfigName);
        ImGui.SameLine();
        ImGui.InputText("##confname", ref SelectedConfig.Name, 256);
        ImGui.Spacing();

        using var tabBar = ImRaii.TabBar("##IndieTabs");
        if (!tabBar.Success)
            return;

        using var tabItem = ImRaii.TabItem(Language.NewConfigChatConfig);
        if (!tabItem.Success)
            return;

        foreach (var type in ChatTypeExtensions.PossibleTypes)
        {
            var enabled = SelectedConfig.TypeConfig[type];
            if (ImGui.Checkbox(type.ToFullName(), ref enabled))
            {
                SelectedConfig.TypeConfig[type] = enabled;
                Plugin.Configuration.Save();
            }
        }
    }
}
