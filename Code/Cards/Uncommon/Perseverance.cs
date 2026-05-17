using YuWanCard.Core.Utils;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Watcher.Code.Abstract;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Uncommon;

[Pool(typeof(WatcherCardPool))]
public sealed class Perseverance : WatcherCardModel
{
    private const string IncreaseKey = "Increase";

    public Perseverance() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(5, 2);
        WithVar(IncreaseKey, 2, 1);
        WithKeywords(CardKeyword.Retain);
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
    }

    public override Task AfterFlush(PlayerChoiceContext choiceContext, Player player,
        IReadOnlyCollection<CardModel> flushedCards,
        IReadOnlyCollection<CardModel> retainedCards)
    {
        if (!retainedCards.Contains(this)) return Task.CompletedTask;
        DynamicVars.Block.UpgradeValueBy(DynamicVars[IncreaseKey].BaseValue);
        return Task.CompletedTask;
    }
}