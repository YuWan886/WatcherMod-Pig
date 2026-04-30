using YuWanCard.Core.Abstracts;
using Godot;

namespace Watcher.Code.Character;

public class WatcherPotionPool : YuWanPotionPoolModel
{
    public override string EnergyColorName => Watcher.CharacterId;
    public override Color LabOutlineColor => Watcher.Color;
}