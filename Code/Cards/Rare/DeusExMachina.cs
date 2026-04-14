using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Cards.Token;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Rare;

[Pool(typeof(WatcherCardPool))]
public sealed class DeusExMachina : WatcherCardModel
{
    public DeusExMachina() : base(-1, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Unplayable);
        WithCards(2, 1);
        WithTip(typeof(Miracle));
        WithTip(CardKeyword.Exhaust);
    }


    public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel drawnCard, bool fromHandDraw)
    {
        if (drawnCard != this)
            return;
        var miracleCount = DynamicVars.Cards.IntValue;

        var miracles = new List<CardModel>();
        for (var i = 0; i < miracleCount; i++)
        {
            var miracle = CombatState?.CreateCard<Miracle>(Owner);
            if (miracle != null)
                miracles.Add(miracle);
        }

        if (miracles.Count > 0) await CardPileCmd.AddGeneratedCardsToCombat(miracles, PileType.Hand, true);
        await CardPileCmd.Add(this, PileType.Exhaust, CardPilePosition.Top);
    }
}