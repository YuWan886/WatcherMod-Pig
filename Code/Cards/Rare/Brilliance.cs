using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Watcher.Code.Abstract;
using Watcher.Code.Character;
using Watcher.Code.Core;
using Watcher.Code.Powers;

namespace Watcher.Code.Cards.Rare;

[Pool(typeof(WatcherCardPool))]
public class Brilliance : WatcherCardModel
{
    public Brilliance() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
    {
        WithCalculatedDamage(ValueProp.Move, BrillianceDamage, baseVal: 12, extraVal: 1, baseUpgrade: 4);
        WithTip(typeof(MantraPower));
    }

    private static decimal BrillianceDamage(CardModel card, Creature? target)
    {
        var player = card.Owner;
        if (player == null) return 0;
        var mantra = player.Creature.GetPower<MantraPower>();
        return mantra?.Amount ?? 0;
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
    }
}
