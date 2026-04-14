using System.Reflection;
using System.Reflection.Emit;
using BaseLib.Utils.Patching;
using HarmonyLib;

namespace Watcher.Code.Patches;

public class MyInstructionMatcher : InstructionMatcher
{
    public MyInstructionMatcher opcode(OpCode opCode)
    {
        var target = typeof(InstructionMatcher)
            .GetField("_target", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(this) as List<CodeInstruction>;
        target!.Add(new CodeInstruction(opCode));
        return this;
    }

}