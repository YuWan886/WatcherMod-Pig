using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Character;
using Watcher.Code.Commands;
using Watcher.Code.Stances;

namespace Watcher.Code.Cards.Uncommon;

[Pool(typeof(WatcherCardPool))]
public sealed class InnerPeace : WatcherCardModel
{
    public InnerPeace() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithCards(3, 1);
        WithTip(typeof(CalmStance));
    }


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var isInCalm = Owner.Creature.Powers.OfType<CalmStance>().Any();
        if (isInCalm)
            await CommonActions.Draw(this, choiceContext);
        else
            await StanceCmd.EnterCalm(Owner.Creature, cardPlay.Card);
    }
}