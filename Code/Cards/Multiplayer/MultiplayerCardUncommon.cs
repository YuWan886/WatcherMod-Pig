using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using Watcher.Code.Abstract;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Multiplayer;

[Pool(typeof(WatcherCardPool))]
public class MultiplayerCardUncommon : WatcherCardModel
{
    public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

    public MultiplayerCardUncommon() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
 
    }

}