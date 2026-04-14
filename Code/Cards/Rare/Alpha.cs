using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using Watcher.Code.Abstract;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Cards.Token;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Rare;

[Pool(typeof(WatcherCardPool))]
public sealed class Alpha : WatcherCardModel
{
    public Alpha() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        HoverTipFactory.FromCardWithCardHoverTips<Beta>()
            .Select(m => new TooltipSource(_ => m))
            .ToList()
            .ForEach(t => WithTip(t));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        if (CombatState == null) return;
        var insightCard = CombatState.CreateCard<Beta>(Owner);
        var card = await CardPileCmd.AddGeneratedCardToCombat(
            insightCard,
            PileType.Draw,
            true,
            CardPilePosition.Random
        );
        CardCmd.PreviewCardPileAdd(card);
    }


    protected override void OnUpgrade()
    {
        AddKeyword(CardKeyword.Innate);
    }
}