using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Cards.Token;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Rare;

[Pool(typeof(WatcherCardPool))]
public sealed class ConjureBlade : WatcherCardModel
{
    public ConjureBlade() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self)
    {
        WithKeywords(CardKeyword.Exhaust);
        WithTip(typeof(Expunger));
    }

    protected override bool HasEnergyCostX => true;

    protected override async Task OnPlay(PlayerChoiceContext context, CardPlay play)
    {
        var x = ResolveEnergyXValue();
        if (IsUpgraded)
            x += 1;
        var expunger = CombatState?.CreateCard<Expunger>(Owner);
        if (expunger == null)
            return;
        expunger.DynamicVars.Repeat.UpgradeValueBy(x + 1);
        var card = await CardPileCmd.AddGeneratedCardToCombat(
            expunger,
            PileType.Draw,
            true,
            CardPilePosition.Random
        );
        CardCmd.PreviewCardPileAdd(card);
    }
}