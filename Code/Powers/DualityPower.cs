using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using YuWanCard.Core.Abstracts;
using YuWanCard.Core.Extensions;
using Watcher.Code.Extensions;
using Watcher.Code.Relics;

namespace Watcher.Code.Powers;

public class DualityPower : YuWanTemporaryPowerModel
{
    public override PowerModel InternallyAppliedPower => ModelDb.Power<DexterityPower>();
    public override AbstractModel OriginModel => ModelDb.Relic<Duality>();
    public override string? CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string? CustomBigIconPath => CustomPackedIconPath;
}
