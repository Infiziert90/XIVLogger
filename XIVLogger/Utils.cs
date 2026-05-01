using Dalamud.Game.Text;
using XIVLogger.Resources;

namespace XIVLogger;

public static class Utils
{
    public static string ToChatName(this XivChatType type, string sender)
    {
        return type switch
        {
            XivChatType.CustomEmote => $"[{Language.ChatTypeEmote}]{sender}",
            XivChatType.StandardEmote => $"[{Language.ChatTypeEmote}]",
            XivChatType.TellIncoming => $"{sender} >> ",
            XivChatType.TellOutgoing => $">> {sender}: ",
            XivChatType.FreeCompany => $"[{Language.ChatTypeFC}]{sender}: ",
            XivChatType.NoviceNetwork => $"[{Language.ChatTypeNN}]{sender}: ",
            XivChatType.CrossLinkShell1 => $"[{Language.ChatTypeCWLS}1]{sender}: ",
            XivChatType.CrossLinkShell2 => $"[{Language.ChatTypeCWLS}2]{sender}: ",
            XivChatType.CrossLinkShell3 => $"[{Language.ChatTypeCWLS}3]{sender}: ",
            XivChatType.CrossLinkShell4 => $"[{Language.ChatTypeCWLS}4]{sender}: ",
            XivChatType.CrossLinkShell5 => $"[{Language.ChatTypeCWLS}5]{sender}: ",
            XivChatType.CrossLinkShell6 => $"[{Language.ChatTypeCWLS}6]{sender}: ",
            XivChatType.CrossLinkShell7 => $"[{Language.ChatTypeCWLS}7]{sender}: ",
            XivChatType.CrossLinkShell8 => $"[{Language.ChatTypeCWLS}8]{sender}: ",
            XivChatType.Ls1 => $"[{Language.ChatTypeLS}1]{sender}: ",
            XivChatType.Ls2 => $"[{Language.ChatTypeLS}2]{sender}: ",
            XivChatType.Ls3 => $"[{Language.ChatTypeLS}3]{sender}: ",
            XivChatType.Ls4 => $"[{Language.ChatTypeLS}4]{sender}: ",
            XivChatType.Ls5 => $"[{Language.ChatTypeLS}5]{sender}: ",
            XivChatType.Ls6 => $"[{Language.ChatTypeLS}6]{sender}: ",
            XivChatType.Ls7 => $"[{Language.ChatTypeLS}7]{sender}: ",
            XivChatType.Ls8 => $"[{Language.ChatTypeLS}8]{sender}: ",
            XivChatType.PvPTeam => $"[{Language.ChatTypePvP}]{sender}: ",
            XivChatType.Debug => $"[{Language.ChatTypeDBG}]",
            XivChatType.Say => $"[{Language.ChatTypeSay}]{sender}: ",
            XivChatType.Shout => $"[{Language.ChatTypeShout}]{sender}: ",
            XivChatType.Yell => $"[{Language.ChatTypeYell}]{sender}: ",
            XivChatType.Party or XivChatType.CrossParty => $"[{Language.ChatTypeParty}]{sender}: ",
            XivChatType.Alliance => $"[{Language.ChatTypeAlliance}]{sender}: ",
            _ => ""
        };
    }

    public static string ToFullName(this XivChatType type)
    {
        return type switch
        {
            XivChatType.Say => Language.FullNameSay,
            XivChatType.Shout => Language.FullNameShout,
            XivChatType.Yell => Language.FullNameYell,
            XivChatType.Party => Language.FullNameParty,
            XivChatType.CrossParty => Language.FullNameCWParty,
            XivChatType.Alliance => Language.FullNameAlliance,
            XivChatType.TellIncoming => Language.FullNameInTell,
            XivChatType.TellOutgoing => Language.FullNameOutTell,
            XivChatType.CustomEmote => Language.FullNameCustomEmote,
            XivChatType.StandardEmote => Language.FullNameEmote,
            XivChatType.RandomNumber => Language.FullNameRandom,
            XivChatType.CrossLinkShell1 => $"{Language.FullNameCWLS} 1",
            XivChatType.CrossLinkShell2 => $"{Language.FullNameCWLS} 2",
            XivChatType.CrossLinkShell3 => $"{Language.FullNameCWLS} 3",
            XivChatType.CrossLinkShell4 => $"{Language.FullNameCWLS} 4",
            XivChatType.CrossLinkShell5 => $"{Language.FullNameCWLS} 5",
            XivChatType.CrossLinkShell6 => $"{Language.FullNameCWLS} 6",
            XivChatType.CrossLinkShell7 => $"{Language.FullNameCWLS} 7",
            XivChatType.CrossLinkShell8 => $"{Language.FullNameCWLS} 8",
            XivChatType.Ls1 => $"{Language.FullNameLS} 1",
            XivChatType.Ls2 => $"{Language.FullNameLS} 2",
            XivChatType.Ls3 => $"{Language.FullNameLS} 3",
            XivChatType.Ls4 => $"{Language.FullNameLS} 4",
            XivChatType.Ls5 => $"{Language.FullNameLS} 5",
            XivChatType.Ls6 => $"{Language.FullNameLS} 6",
            XivChatType.Ls7 => $"{Language.FullNameLS} 7",
            XivChatType.Ls8 => $"{Language.FullNameLS} 8",

            XivChatType.PvPTeam => Language.FullNamePvP,
            XivChatType.NoviceNetwork => Language.FullNameNN,
            XivChatType.FreeCompany => Language.FullNameFC,
            XivChatType.Echo => Language.FullNameEcho,
            XivChatType.SystemMessage => Language.FullNameSystemMessage,
            XivChatType.SystemError => Language.FullNameSystemError,
            XivChatType.Notice => Language.FullNameNotice,
            XivChatType.Debug => Language.FullNameDebug,

            _ => ""
        };
    }

    public static IEnumerable<(T Val, int Idx)> WithIndex<T>(this IEnumerable<T> list)
    {
        return list.Select((val, idx) => (val, idx));
    }
}