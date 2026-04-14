using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace Watcher.Code.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<CardModel> FilterForPlayerCount(
        this IEnumerable<CardModel> options,
        IRunState runState)
    {
        return runState.Players.Count > 1 ? 
            options.Where(c => c.MultiplayerConstraint != CardMultiplayerConstraint.SingleplayerOnly) : 
            options.Where(c => c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly);
    }
}