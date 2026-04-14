using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Cards.Token;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Uncommon;

[Pool(typeof(WatcherCardPool))]
public sealed class DeceiveReality : WatcherCardModel
{
    public DeceiveReality() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
    {
        WithBlock(4, 3);
        WithTip(typeof(Safety));
    }


    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);

        var insightCard = CombatState?.CreateCard<Safety>(Owner);
        if (insightCard == null) return;
        var card = await CardPileCmd.AddGeneratedCardToCombat(
            insightCard,
            PileType.Hand,
            false,
            CardPilePosition.Top
        );
        CardCmd.PreviewCardPileAdd(card);
    }
}