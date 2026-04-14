using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Cards.Token;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Common;

[Pool(typeof(WatcherCardPool))]
public sealed class Evaluate : WatcherCardModel
{
    public Evaluate() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(6, 4);
        WithTip(typeof(Insight));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        var insightCard = CombatState?.CreateCard<Insight>(Owner);
        if (insightCard == null) return;
        var card = await CardPileCmd.AddGeneratedCardToCombat(
            insightCard,
            PileType.Draw,
            true,
            CardPilePosition.Random
        );
        CardCmd.PreviewCardPileAdd(card);
    }
}