using BaseLib.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Character;
using Watcher.Code.Commands;
using Watcher.Code.Powers;
using Watcher.Code.Stances;

namespace Watcher.Code.Cards.Rare;

[Pool(typeof(WatcherCardPool))]
public sealed class Blasphemy : WatcherCardModel
{
    public Blasphemy() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithPower<BlasphemerPower>(1);
        WithKeywords(CardKeyword.Exhaust);
        WithTip(typeof(DivinityStance));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.ApplySelf<BlasphemerPower>(this);
        await StanceCmd.EnterDivinity(Owner.Creature, cardPlay.Card);
    }

    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Retain);
    }
}