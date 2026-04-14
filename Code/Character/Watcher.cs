using BaseLib.Abstracts;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using Watcher.Code.Cards.Basic;
using Watcher.Code.Relics;
using MegaCrit.Sts2.Core.Entities.Players;
using System.Reflection;
using MegaCrit.Sts2.Core.Assets;
using BaseLib.Utils.Patching;
using System.Reflection.Emit;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes.Combat;
using Watcher.Code.Patches;

namespace Watcher.Code.Character;

public class Watcher : CustomCharacterModel
{
    public const string CharacterId = "Watcher";

    public static readonly Color Color = StsColors.purple;
    public virtual CustomEnergyCounter? CustomEnergyCounter => null;
    public override string CustomIconTexturePath => "res://Watcher/images/watcher/character_icon_watcher.png";
    public override string CustomCharacterSelectIconPath => "res://Watcher/images/watcher/char_select_watcher.png";

    public override string CustomCharacterSelectLockedIconPath =>
        "res://Watcher/images/watcher/char_select_watcher_locked.png";

    public override string CustomVisualPath => "res://Watcher/scenes/watcher/watcher.tscn";
    public override string CustomTrailPath => "res://Watcher/scenes/watcher/card_trail_watcher.tscn";
    public override string CustomIconPath => "res://Watcher/scenes/watcher/watcher_icon.tscn";
    public override string CustomEnergyCounterPath => "res://Watcher/scenes/watcher/watcher_energy_counter.tscn";
    public override string CustomRestSiteAnimPath => "res://Watcher/scenes/watcher/watcher_rest_site.tscn";
    public override string CustomMerchantAnimPath => "res://Watcher/scenes/watcher/watcher_merchant.tscn";

    public override string CustomArmPointingTexturePath =>
        "res://Watcher/images/watcher/hands/multiplayer_hand_watcher_point.png";

    public override string CustomArmRockTexturePath =>
        "res://Watcher/images/watcher/hands/multiplayer_hand_watcher_rock.png";

    public override string CustomArmPaperTexturePath =>
        "res://Watcher/images/watcher/hands/multiplayer_hand_watcher_paper.png";

    public override string CustomArmScissorsTexturePath =>
        "res://Watcher/images/watcher/hands/multiplayer_hand_watcher_scissors.png";

    public override string CustomCharacterSelectBg => "res://Watcher/scenes/watcher/char_select_bg_watcher.tscn";

    public override string CustomCharacterSelectTransitionPath =>
        "res://Watcher/images/watcher/transitions/watcher_transition_mat.tres";

    public override string CustomMapMarkerPath => "res://Watcher/images/watcher/map_marker_watcher.png";
    public override string CustomAttackSfx => "res://";
    public override string CustomCastSfx => "res://";
    public override string CustomDeathSfx => "res://";

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    protected override CharacterModel? UnlocksAfterRunAs => null;
    public override int StartingHp => 72;
    public override int StartingGold => 99;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<StrikeWatcher>(),
        ModelDb.Card<StrikeWatcher>(),
        ModelDb.Card<StrikeWatcher>(),
        ModelDb.Card<StrikeWatcher>(),
        ModelDb.Card<DefendWatcher>(),
        ModelDb.Card<DefendWatcher>(),
        ModelDb.Card<DefendWatcher>(),
        ModelDb.Card<DefendWatcher>(),
        ModelDb.Card<Vigilance>(),
        ModelDb.Card<Eruption>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<PureWater>()
    ];

    public override float AttackAnimDelay => 0.15f;

    public override float CastAnimDelay => 0.25f;

    public override CardPoolModel CardPool => ModelDb.CardPool<WatcherCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<WatcherRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<WatcherPotionPool>();

    public override List<string> GetArchitectAttackVfx()
    {
        return
        [
            "vfx/vfx_attack_blunt", "vfx/vfx_heavy_blunt", "vfx/vfx_attack_slash", "vfx/vfx_bloody_impact",
            "vfx/vfx_rock_shatter"
        ];
    }
    
}

public readonly struct CustomEnergyCounter(Func<int, string> pathFunc, Color outlineColor, Color burstColor) {
    private readonly Func<int, string> _getPath = pathFunc;
    public readonly Color OutlineColor = outlineColor;
    public readonly Color BurstColor = burstColor;

    public string LayerImagePath(int layer) => _getPath(layer);
} 

[HarmonyPatch(typeof(NEnergyCounter), "OutlineColor", MethodType.Getter)]
public class EnergyCounterOutlineColorPatch {
    private static readonly FieldInfo? PlayerProp = typeof(NEnergyCounter).GetField("_player", BindingFlags.NonPublic | BindingFlags.Instance);

    static bool Prefix(NEnergyCounter __instance, ref Color __result) {
        if (PlayerProp?.GetValue(__instance) is not Player
            {
                Character: Watcher
                {
                    CustomEnergyCounter: { } counter
                }
            }) return true;
        __result = counter.OutlineColor;
        return false;
    }
}

[HarmonyPatch(typeof(NEnergyCounter), nameof(NEnergyCounter.Create))]
class EnergyCounterPatch {
    static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
        return new InstructionPatcher(instructions)
            .Match(new MyInstructionMatcher()
                .opcode(OpCodes.Ldc_I4_0)
                .opcode(OpCodes.Conv_I8)
                .callvirt(null)
                .stloc_0()
            )
            .Insert([
                CodeInstruction.LoadLocal(0),
                CodeInstruction.LoadArgument(0),
                CodeInstruction.Call(typeof(EnergyCounterPatch), nameof(ChangeIroncladEnergy)),
                CodeInstruction.StoreLocal(0),
            ]);
    }

    static NEnergyCounter ChangeIroncladEnergy(NEnergyCounter defaultCounter, Player player) {
        if (player.Character is not Watcher { CustomEnergyCounter: { } counter })
            return defaultCounter;
        var energyCounter = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(string.Concat("combat/energy_counters/ironclad_energy_counter"))).Instantiate<NEnergyCounter>();
        energyCounter.GetNode<TextureRect>("%Layers/Layer1").Texture = ResourceLoader.Load<Texture2D>(counter.LayerImagePath(1));
        energyCounter.GetNode<TextureRect>("%RotationLayers/Layer2").Texture = ResourceLoader.Load<Texture2D>(counter.LayerImagePath(2));
        energyCounter.GetNode<TextureRect>("%RotationLayers/Layer3").Texture = ResourceLoader.Load<Texture2D>(counter.LayerImagePath(3));
        energyCounter.GetNode<TextureRect>("%Layers/Layer4").Texture = ResourceLoader.Load<Texture2D>(counter.LayerImagePath(4));
        energyCounter.GetNode<TextureRect>("%Layers/Layer5").Texture = ResourceLoader.Load<Texture2D>(counter.LayerImagePath(5));
        energyCounter.GetNode<CpuParticles2D>("%BurstBack").Color = counter.BurstColor;
        energyCounter.GetNode<CpuParticles2D>("%BurstFront").Color = counter.BurstColor;
        return energyCounter;
    }
}
