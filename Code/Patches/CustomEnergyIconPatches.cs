using BaseLib.Utils.Patching;
using Watcher.Code.Abstracts;
using HarmonyLib;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.Formatters;
using MegaCrit.Sts2.Core.Models;


namespace Watcher.Code.Patches;

public class CustomEnergyIconPatches {
    public const char Delimiter = '∴';
    public static string GetEnergyColorName(ModelId id) => id.Category + CustomEnergyIconPatches.Delimiter + id.Entry;

    [HarmonyPatch(typeof(EnergyIconHelper), nameof(EnergyIconHelper.GetPath), typeof(string))]
    private static class IconPatch {
        static bool Prefix(string prefix, ref string __result) {
            var index = prefix.IndexOf(Delimiter);
            if (index < 0) return true;
            var model = ModelDb.GetById<AbstractModel>(new ModelId(prefix[..index], prefix[(index+1)..]));
            if (model is not ICustomEnergyIconPool { BigEnergyIconPath: { } path }) return true;
            __result = path;
            return false;
        }
    }

    [HarmonyPatch(typeof(EnergyIconsFormatter), nameof(EnergyIconsFormatter.TryEvaluateFormat))]
    private static class TextIconPatch {
        static List<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) {
            return new InstructionPatcher(instructions)
                .Match(new InstructionMatcher()
                    .call(AccessTools.Method(typeof(string), nameof(string.Concat), [typeof(string), typeof(string), typeof(string)]))
                    .stloc_3()
                )
                .Insert([
                    CodeInstruction.LoadLocal(0),
                    CodeInstruction.LoadLocal(3),
                    CodeInstruction.Call(typeof(CustomEnergyIconPatches), nameof(GetTextIcon)),
                    CodeInstruction.StoreLocal(3),
                ]);
        }
    }

    static string GetTextIcon(string prefix, string oldText) {
        var index = prefix.IndexOf(Delimiter);
        if (index < 0) return oldText;
        var model = ModelDb.GetById<AbstractModel>(new ModelId(prefix[..index], prefix[(index+1)..]));
        return model is not ICustomEnergyIconPool { TextEnergyIconPath: { } path } ? oldText : $"[img]{path}[/img]";
    }
}