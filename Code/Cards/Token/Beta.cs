using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models.CardPools;
using Watcher.Code.Abstract;
using Watcher.Code.Cards.CardModels;

namespace Watcher.Code.Cards.Token;

[Pool(typeof(TokenCardPool))]
public sealed class Beta : WatcherCardModel
{
    public Beta() : base(2, CardType.Skill, CardRarity.Token, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithTip(typeof(Omega));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;
        var insightCard = CombatState.CreateCard<Omega>(Owner);
        var card = await CardPileCmd.AddGeneratedCardToCombat(
            insightCard,
            PileType.Draw,
            true,
            CardPilePosition.Random
        );
        CardCmd.PreviewCardPileAdd(card);
    }
}