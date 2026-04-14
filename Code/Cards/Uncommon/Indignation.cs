using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.Powers;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Character;
using Watcher.Code.Commands;
using Watcher.Code.Stances;

namespace Watcher.Code.Cards.Uncommon;

[Pool(typeof(WatcherCardPool))]
public sealed class Indignation : WatcherCardModel
{
    public Indignation() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies)
    {
        WithPower<VulnerablePower>(3, 2);
        WithTip(typeof(WrathStance));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (cardPlay.Target == null) return;
        var isInWrath = Owner.Creature.Powers.OfType<WrathStance>().Any();
        if (isInWrath)
            await CommonActions.Apply<VulnerablePower>(cardPlay.Target, this);
        else
            await StanceCmd.EnterWrath(Owner.Creature, cardPlay.Card);
    }
}