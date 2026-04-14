using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.ValueProps;
using Watcher.Code.Cards.CardModels;
using Watcher.Code.Character;
using Watcher.Code.Stances;

namespace Watcher.Code.Cards.Common;

[Pool(typeof(WatcherCardPool))]
public sealed class Halt : WatcherCardModel
{
    public Halt() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self)
    {
        WithBlock(3, 1);
        WithVar("WrathBlock", 9, 5);
        WithTip(typeof(WrathStance));
    }

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await CommonActions.CardBlock(this, cardPlay);
        var isInWrath = Owner.Creature.Powers.OfType<WrathStance>().Any();
        if (isInWrath)
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars["WrathBlock"].BaseValue, ValueProp.Move, cardPlay);
    }
}