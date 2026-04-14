using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Watcher.Code.Stances;

namespace Watcher.Code.Events;

public class WatcherHook
{
    private static async Task Dispatch<T>(PlayerChoiceContext ctx, Player player, Func<T, Task> invoke)
        where T : class
    {
        var combatState = player.Creature.CombatState;
        if (combatState == null) return;
        foreach (var model in combatState.IterateHookListeners().OfType<T>())
        {
            var abstractModel = (AbstractModel)(object)model;
            ctx.PushModel(abstractModel);
            await invoke(model);
            ctx.PopModel(abstractModel);
        }
    }

    public static Task OnStanceChange(PlayerChoiceContext ctx, Player player, WatcherStanceModel oldStance, WatcherStanceModel newStance)
        => Dispatch<IOnStanceChange>(ctx, player, m => m.OnStanceChange(ctx, player, oldStance, newStance));

    public static Task OnScryed(PlayerChoiceContext ctx, Player player, int amount)
        => Dispatch<IOnScryed>(ctx, player, m => m.OnScryed(ctx, player, amount));
}