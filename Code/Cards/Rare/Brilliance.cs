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

    public override bool ShouldReceiveCombatHooks => true;

    private static decimal MantraGainedThisCombat(CardModel card, Creature? creature)
        => CombatManager.Instance.History.Entries.OfType<PowerReceivedEntry>()
            .Where(e => e is { Power: MantraPower, Applier: not null } && e.Applier.Player == card.Owner)
            .Sum(e => e.Amount);

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
    }
}
