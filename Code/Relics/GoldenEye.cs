using YuWanCard.Core.Utils;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using Watcher.Code.Abstract;
using Watcher.Code.Character;
using Watcher.Code.Keywords;

namespace Watcher.Code.Relics;

[Pool(typeof(WatcherRelicPool))]
public sealed class GoldenEye : WatcherRelicModel
{
    public override RelicRarity Rarity => RelicRarity.Rare;

    protected override IEnumerable<IHoverTip> ExtraHoverTips =>
    [
        HoverTipFactory.FromKeyword(WatcherKeywords.Scry)
    ];
}