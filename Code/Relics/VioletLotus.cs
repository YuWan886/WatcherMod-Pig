using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using Watcher.Code.Abstract;
using Watcher.Code.Character;
using Watcher.Code.Core;
using Watcher.Code.Events;
using Watcher.Code.Stances;

namespace Watcher.Code.Relics;

[Pool(typeof(WatcherRelicPool))]
public sealed class VioletLotus : WatcherRelicModel, IModifyCalmEnergyGain
{
    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        WatcherHoverTipFactory.FromStance<CalmStance>(),
        HoverTipFactory.ForEnergy(this)
    ];


    // TODO - give to ancient
    public override RelicRarity Rarity => RelicRarity.Ancient;

    public int ModifyCalmEnergyGain(Player player, int amount)
    {
        return Owner == player ? amount + 1 : amount;
    }
}