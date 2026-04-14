using System.Threading.Tasks;
using BaseLib.Abstracts;
using BaseLib.Extensions;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Watcher.Code.Abstract;
using Watcher.Code.Events;
using Watcher.Code.Extensions;
using Watcher.Code.Stances;

namespace Watcher.Code.Powers;

public sealed class MentalFortressPower : WatcherPowerModel, IOnStanceChange
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;
    
    public async Task OnStanceChange(PlayerChoiceContext ctx, Player player, WatcherStanceModel oldStance, WatcherStanceModel newStance)
    {
        if (player.Creature != Owner) return;
        
        await CreatureCmd.GainBlock(
            Owner,
            Amount,
            ValueProp.Unpowered,
            null
        );
    }
}