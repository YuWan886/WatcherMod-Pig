using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
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
    : ConstructedCardModel(canonicalEnergyCost, type, rarity, targetType, shouldShowInCardLibrary)
{
    public sealed override string CustomPortraitPath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".CardImagePath();



    public WatcherCardModel WithStanceTip<T>() where T : WatcherStanceModel
    {
        WithTip(new TooltipSource(_ => WatcherHoverTipFactory.FromStance<T>()));
        return this;
    }
}