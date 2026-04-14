using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Cards.Token;
using Watcher.Code.Character;

namespace Watcher.Code.Cards.Uncommon;

[Pool(typeof(WatcherCardPool))]
public sealed class CarveReality : WatcherCardModel
{
    public CarveReality() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
    {
        WithDamage(6, 4);
        WithTip(typeof(Smite));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardAttack(this, cardPlay).Execute(choiceContext);

        var insightCard = CombatState?.CreateCard<Smite>(Owner);
        if (insightCard == null) return;
        var card = await CardPileCmd.AddGeneratedCardToCombat(
            insightCard,
            PileType.Hand,
            true,
            CardPilePosition.Top
        );
        CardCmd.PreviewCardPileAdd(card);
    }

    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(4m);
    }
}