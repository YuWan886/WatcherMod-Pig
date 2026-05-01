using System;
using System.Reflection;
using Godot;
using Godot.Bridge;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Models;
using Watcher.Code.Events;
using YuWanCard.Core.Registration;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace Watcher.Code;

[ModInitializer(nameof(Initialize))]
public partial class WatcherMainFile : Node
{
    public const string ModId = "Watcher";

    public static Logger Logger { get; } =
        new(ModId, LogType.Generic);

    public static void Initialize()
    {
        try { WatcherSubscriber.Subscribe(); }
        catch (Exception ex) { Logger.Warn($"Subscribe failed (may be mobile): {ex.Message}"); }

        Harmony harmony = new(ModId);
        var assembly = Assembly.GetExecutingAssembly();

        try { ScriptManagerBridge.LookupScriptsInAssembly(assembly); }
        catch (Exception ex) { Logger.Warn($"LookupScriptsInAssembly failed (may be mobile): {ex.Message}"); }

        try { harmony.PatchAll(assembly); }
        catch (Exception ex) { Logger.Warn($"PatchAll failed (may be mobile): {ex.Message}"); }

        try { ContentRegistry.RegisterAll(assembly); }
        catch (Exception ex) { Logger.Warn($"RegisterAll failed (may be mobile): {ex.Message}"); }

        Logger.Info("Watcher mod initialized");
    }
}

[HarmonyPatch(typeof(ModelDb), "InitIds")]
internal static class ModelDbInitIdsPatch
{
    [HarmonyPostfix]
    private static void LogRegisteredCounts()
    {
        var modAssembly = typeof(WatcherMainFile).Assembly;
        var characters = ModelDb.AllCharacters
            .Where(c => c.GetType().Assembly == modAssembly)
            .ToList();

        foreach (var character in characters.OrderBy(c => c.Id.Entry))
        {
            var charName = character.GetType().Name;
            var cards = ModelDb.AllCards.Count(c => c.Pool == character.CardPool);
            var relics = ModelDb.AllRelics.Count(r => r.Pool == character.RelicPool);
            var potions = ModelDb.AllPotions.Count(p => p.Pool == character.PotionPool);
            WatcherMainFile.Logger.Info($"{charName}: {cards} cards, {relics} relics, {potions} potions");
        }

        var powers = ModelDb.AllPowers.Count(p => p.GetType().Assembly == modAssembly);
        WatcherMainFile.Logger.Info($"Powers: {powers}");
    }
}