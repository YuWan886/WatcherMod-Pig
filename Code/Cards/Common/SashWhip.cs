using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Watcher.Code.Abstract;
using Watcher.Code.Character;
using Watcher.Code.Core;
using Watcher.Code.Stances;

namespace Watcher.Code.Cards.Common;

[Pool(typeof(WatcherCardPool))]
public class SashWhip : WatcherCardModel
{
    public SashWhip() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
    {
        WithDamage(6, 3);
        WithPower<WeakPower>(1, 1);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);
        if (WatcherModel.IsInStance<WrathStance>(cardPlay.Card.Owner))
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target);
            var amount = DynamicVars["WeakPower"].BaseValue;
            await PowerCmd.Apply<WeakPower>(cardPlay.Target, amount, cardPlay.Card.Owner.Creature, this);
        }
    }
}
