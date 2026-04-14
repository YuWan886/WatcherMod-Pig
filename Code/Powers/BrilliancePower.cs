using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.Models;

namespace Watcher.Code.Powers;

#pragma warning disable STS001
public sealed class BrilliancePower : CustomPowerModel
#pragma warning restore STS001
{
    public override PowerType Type => PowerType.Debuff;
    protected override bool IsVisibleInternal => true;

    public override PowerStackType StackType => PowerStackType.Counter;
   
    public override async Task BeforePowerAmountChanged(
        PowerModel power,
        decimal amount,
        Creature target,
        Creature? applier,
        CardModel? cardSource)
    {
        if (power is not MantraPower || target != Owner || amount <= 0)
            return;
        await PowerCmd.Apply<BrilliancePower>(Owner, amount, Owner, null);
    }
    
}