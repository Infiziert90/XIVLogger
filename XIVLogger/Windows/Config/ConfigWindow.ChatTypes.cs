using Dalamud.Interface.Utility.Raii;
using XIVLogger.Resources;

namespace XIVLogger.Windows.Config;

public partial class ConfigWindow
{
    private void ChatTypes()
    {
        using var tabItem = ImRaii.TabItem(Language.TabChatTypes);
        if (!tabItem.Success)
            return;

        ImGui.TextUnformatted(Language.TabChatTypesTooltip);
        foreach (var type in ChatTypeExtensions.PossibleTypes)
        {
            var enabled = Plugin.Configuration.defaultConfig.TypeConfig[(int)type];
            if (ImGui.Checkbox(type.ToFullName(), ref enabled))
            {
                Plugin.Configuration.defaultConfig.TypeConfig[(int)type] = enabled;
                Plugin.Configuration.Save();
            }
        }
    }
}
