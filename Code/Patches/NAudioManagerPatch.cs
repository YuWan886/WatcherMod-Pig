using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.TestSupport;

namespace Watcher.Code.Patches;

[HarmonyPatch(typeof(NAudioManager), nameof(NAudioManager.PlayOneShot), typeof(string),
    typeof(Dictionary<string, float>), typeof(float))]
internal class NAudioManagerPatch
{
    private static bool Prefix(string path, Dictionary<string, float> parameters, float volume,
        NAudioManager __instance)
    {
        if (TestMode.IsOn) return true;
        if (!path.StartsWith("res://")) return true;
        var audioStream = GD.Load<AudioStream>(path);
        if (audioStream is null) return true;
        AudioStreamPlayer2D audioPlayer = new()
        {
            Bus = "SFX",
            VolumeDb = Mathf.LinearToDb(volume * 7),
            Stream = GD.Load<AudioStream>(path)
        };
        __instance.GetTree().Root.AddChild(audioPlayer);
        audioPlayer.Finished += audioPlayer.QueueFree;
        audioPlayer.Play();
        return false;
    }
}