using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using XIVLogger.Resources;

namespace XIVLogger.Windows.Config;

public partial class ConfigWindow
{
    private void Advanced()
    {
        using var tabItem = ImRaii.TabItem(Language.TabAdvanced);
        if (!tabItem.Success)
            return;

        ImGui.TextUnformatted(Language.TabAdvancedTooltip);
        ImGui.Spacing();

        using var table = ImRaii.Table("ConfigList", 4, ImGuiTableFlags.BordersInner);
        if (!table.Success)
            return;

        ImGui.TableNextRow();

        ImGui.TableNextColumn();
        ImGui.TextUnformatted(Language.TabAdvancedActive);

        ImGui.TableNextColumn();
        ImGui.TextUnformatted(Language.TabAdvancedConfigName);

        ImGui.TableNextColumn();
        ImGui.TextUnformatted(Language.TabAdvancedEdit);

        ImGui.TableNextColumn();
        ImGui.TextUnformatted(Language.TabAdvancedRemove);

        ImGui.TableNextRow();

        ImGui.TableNextColumn();
        if (ImGui.Button($"{(Plugin.Configuration.NewDefaultConfig.IsActive ? "@" : "")}##active_default", ImGuiHelpers.ScaledVector2(20.0f)))
            Plugin.Configuration.SetActiveConfig(Plugin.Configuration.NewDefaultConfig);

        ImGui.TableNextColumn();
        ImGui.TextUnformatted(Plugin.Configuration.NewDefaultConfig.Name);

        foreach (var (config, idx) in Plugin.Configuration.ImprovedConfigList.ToArray().WithIndex())
        {
            var id = $"{config.Name}{idx}";

            ImGui.TableNextRow();

            ImGui.TableNextColumn();
            if (ImGui.Button($"{(config.IsActive ? "@" : "")}##active_{id}", ImGuiHelpers.ScaledVector2(20.0f)))
                Plugin.Configuration.SetActiveConfig(config);

            ImGui.TableNextColumn();
            ImGui.TextUnformatted($"{config.Name}");


            ImGui.TableNextColumn();
            if (ImGui.Button($"{Language.TabAdvancedEdit}##{id}"))
            {
                Plugin.NewConfigWindow.SelectedConfig = config;
                Plugin.NewConfigWindow.IsOpen = true;
            }

            ImGui.TableNextColumn();
            if (ImGui.Button($"{Language.TabAdvancedRemove}##{id}"))
            {
                Plugin.Configuration.RemoveConfig(config);
                Plugin.NewConfigWindow.IsOpen = false;
            }
        }

        ImGui.TableNextRow();

        ImGui.TableNextColumn();
        if (ImGui.Button("+"))
            Plugin.Configuration.AddNewConfig(Language.TabAdvancedNewConfigName);
    }
}
