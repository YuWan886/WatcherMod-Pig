using YuWanCard.Core.Abstracts;
using YuWanCard.Core.Extensions;
using Watcher.Code.Extensions;

namespace Watcher.Code.Abstract;

public abstract class WatcherPowerModel : YuWanPowerModel
{
    public sealed override string? CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public sealed override string? CustomBigIconPath => CustomPackedIconPath;

}
