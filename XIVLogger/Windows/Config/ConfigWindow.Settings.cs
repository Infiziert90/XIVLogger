using Dalamud.Interface.Components;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using XIVLogger.Resources;

namespace XIVLogger.Windows.Config;

public partial class ConfigWindow
{
    private void Settings()
    {
        using var tabItem = ImRaii.TabItem(Language.TabSettings);
        if (!tabItem.Success)
            return;

        var save = false;
        var longText = Language.TabSettingsFileName;
        var width = ImGui.CalcTextSize(longText).X + (25.0f * ImGuiHelpers.GlobalScale);

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(longText);
        ImGui.SameLine(width);
        ImGui.InputText("##filename", ref Plugin.Configuration.FileNameV2, 256);

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(Language.TabSettingsFilePath);
        ImGui.SameLine(width);
        ImGui.InputText("##filepath", ref Plugin.Configuration.FilePathV2, 256);
        ImGui.TextUnformatted(Language.TabSettingsPathTooltip);

        ImGuiHelpers.ScaledDummy(5.0f);
        ImGui.Separator();
        ImGuiHelpers.ScaledDummy(5.0f);

        save |= ImGui.Checkbox(Language.TabSettingsIgnoreChatTypes, ref Plugin.Configuration.StoreEveryMessage);
        ImGuiComponents.HelpMarker(Language.TabSettingsIgnoreChatTypesTooltip);

        save |= ImGui.Checkbox(Language.TabSettingsTimestamp, ref Plugin.Configuration.Timestamp);
        if (Plugin.Configuration.Timestamp)
        {
            using var indent = ImRaii.PushIndent(10.0f);
            save |= ImGui.Checkbox(Language.TabSettingsUse24h, ref Plugin.Configuration.Timestamp24H);
            if (Plugin.Configuration.Timestamp24H)
            {
                using var innerIndent = ImRaii.PushIndent(10.0f);
                save |= ImGui.Checkbox(Language.TabSettingsShowSeconds, ref Plugin.Configuration.TimeSeconds);
            }
            save |= ImGui.Checkbox(Language.TabSettingsIncludeTimestamp, ref Plugin.Configuration.Datestamp);
        }

        save |= ImGui.Checkbox(Language.TabSettingsSortableFilenames, ref Plugin.Configuration.FileSortableDatetime);

        ImGuiHelpers.ScaledDummy(5.0f);
        ImGui.Separator();
        ImGuiHelpers.ScaledDummy(5.0f);

        if (ImGui.Checkbox(Language.TabSettingsAutosave, ref Plugin.Configuration.Autosave))
        {
            Plugin.ChatLog.SetupAutosave();
            Plugin.ChatLog.AutoSave();

            Plugin.Configuration.UpdateAutosaveTime();
            Plugin.Configuration.Save();
        }
        save |= ImGui.Checkbox(Language.TabSettingsAutosaveNotification, ref Plugin.Configuration.AutosaveNotif);

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(Language.TabSettingsEvery);
        ImGui.SameLine();

        var currentAutoMin = Plugin.Configuration.AutoMin;
        ImGui.InputInt("##autosavemin", ref currentAutoMin, 1);
        if (currentAutoMin != Plugin.Configuration.AutoMin)
        {
            Plugin.Configuration.AutoMin = Math.Clamp(currentAutoMin, 1, int.MaxValue);
            Plugin.Configuration.Save();
        }

        ImGui.SameLine();
        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(Language.TabSettingsMinutes);

        ImGui.AlignTextToFramePadding();
        ImGui.TextUnformatted(Language.TabSettingsAutosavePath);
        ImGui.SameLine();
        ImGui.InputText("##autofilepath", ref Plugin.Configuration.AutoFilePathV2, 256);

        if (save)
        {
            Plugin.ChatLog.SetupAutosave();
            Plugin.Configuration.Save();
        }
    }
}
