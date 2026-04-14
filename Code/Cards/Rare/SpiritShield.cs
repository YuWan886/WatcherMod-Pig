using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Rare;

[Pool(typeof(WatcherCardPool))]
public sealed class SpiritShield : WatcherCardModel
{
    public SpiritShield() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithCalculatedBlock(3, Calc, ValueProp.Move, 1);
    }

    private static decimal Calc(CardModel card, Creature? creature)
    {
        return PileType.Hand.GetPile(card.Owner).Cards.Count;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
    }
}