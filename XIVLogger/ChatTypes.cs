using Dalamud.Game.Text;

namespace XIVLogger;

public static class ChatTypeExtensions
{
    public static readonly XivChatType[] PossibleTypes =
    [
        XivChatType.Say,
        XivChatType.Shout,
        XivChatType.Yell,
        XivChatType.Party,
        XivChatType.CrossParty,
        XivChatType.Alliance,
        XivChatType.TellIncoming,
        XivChatType.TellOutgoing,
        XivChatType.CustomEmote,
        XivChatType.StandardEmote,
        XivChatType.RandomNumber,
        XivChatType.CrossLinkShell1,
        XivChatType.CrossLinkShell2,
        XivChatType.CrossLinkShell3,
        XivChatType.CrossLinkShell4,
        XivChatType.CrossLinkShell5,
        XivChatType.CrossLinkShell6,
        XivChatType.CrossLinkShell7,
        XivChatType.CrossLinkShell8,
        XivChatType.Ls1,
        XivChatType.Ls2,
        XivChatType.Ls3,
        XivChatType.Ls4,
        XivChatType.Ls5,
        XivChatType.Ls6,
        XivChatType.Ls7,
        XivChatType.Ls8,
        XivChatType.PvPTeam,
        XivChatType.NoviceNetwork,
        XivChatType.FreeCompany,
        XivChatType.Echo,
        XivChatType.SystemMessage,
        XivChatType.SystemError,
        XivChatType.Notice,
        XivChatType.Debug,
    ];
}