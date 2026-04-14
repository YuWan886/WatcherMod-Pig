using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;

namespace Watcher.Code.Patches;

[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter._Ready))]
internal static class NEnergyCounterReadyPatch
{
    [HarmonyPrefix]
    private static void SafeReady(NEnergyCounter __instance)
    {
        if (!__instance.HasNode("%BurstBack"))
        {
            var node = new CpuParticles2D
            {
                Name = (StringName)"BurstBack", Emitting = false, Amount = 1, Visible = false
            };
            __instance.AddChild(node);
            node.Owner = __instance;
            node.UniqueNameInOwner = true;
        }

        if (__instance.HasNode("%BurstFront")) return;
        {
            var node = new CpuParticles2D
            {
                Name = (StringName)"BurstFront", Emitting = false, Amount = 1, Visible = false
            };
            __instance.AddChild(node);
            node.Owner = __instance;
            node.UniqueNameInOwner = true;
        }
    }
}