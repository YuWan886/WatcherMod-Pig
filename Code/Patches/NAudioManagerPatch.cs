using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Audio;

namespace Watcher.Code.Patches;

[HarmonyPatch(typeof(NAudioManager), nameof(NAudioManager.PlayOneShot), typeof(string), typeof(float))]
public static class NAudioManagerPatch
{
    private static bool Prefix(string path, float volume = 1f)
    {
        if (!path.StartsWith("res://"))
            return true;

        var stream = ResourceLoader.Load<AudioStream>(path);
        if (stream == null)
            return false;

        var player = new AudioStreamPlayer();
        player.Stream = stream;
        player.VolumeDb = Mathf.LinearToDb(volume);
        if (NAudioManager.Instance == null)
            return false;
        NAudioManager.Instance.AddChild(player);
        player.Play();
        player.Finished += () => player.QueueFree();

        return false;
    }
}