using MegaCrit.Sts2.Core.Entities.Cards;
using YuWanCard.Core.Abstracts;
using YuWanCard.Core.Extensions;
using YuWanCard.Core.Utils;
using Watcher.Code.Core;
using Watcher.Code.Extensions;
using Watcher.Code.Stances;

namespace Watcher.Code.Abstract;

public abstract class WatcherCardModel(
    int canonicalEnergyCost,
    CardType type,
    CardRarity rarity,
    TargetType targetType,
    bool shouldShowInCardLibrary = true)
    : YuWanCardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    public sealed override string? CustomPortraitPath
    {
        get
        {
            var entry = Id.Entry.RemovePrefix();
            return $"{entry.ToLowerInvariant()}.png".CardImagePath();
        }
    }

    public WatcherCardModel WithStanceTip<T>() where T : WatcherStanceModel
    {
        WithTip(new TooltipSource(_ => WatcherHoverTipFactory.FromStance<T>()));
        return this;
    }
}
