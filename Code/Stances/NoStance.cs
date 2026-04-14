using Watcher.Code.Vfx;

namespace Watcher.Code.Stances;

#pragma warning disable STS001
public class NoStance : WatcherStanceModel
#pragma warning restore STS001
{
    public override bool ShouldReceiveCombatHooks => false;
    protected override StanceVfxConfig VfxConfig => new();
}