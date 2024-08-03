using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace SuppressionMod;

[HarmonyPatch(typeof(PawnRenderer), "ParallelGetPreRenderResults")]
internal static class Patch_Verse_PawnRenderer_ParallelGetPreRenderResults
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
    {
        var codes = codeInstructions.ToList();
        foreach (var codeInstruction in codes)
        {
            yield return codeInstruction;
            if (codeInstruction.opcode == OpCodes.Stloc_S && codeInstruction.operand is LocalBuilder lb && lb.LocalIndex == 8)
            {
                yield return new CodeInstruction(OpCodes.Ldarg_0);
                yield return new CodeInstruction(OpCodes.Ldloca_S, 5);
                yield return new CodeInstruction(OpCodes.Ldloca_S, 7);
                yield return new CodeInstruction(OpCodes.Ldloc_S, 8);
                yield return new CodeInstruction(OpCodes.Call, 
                    AccessTools.Method(typeof(Patch_Verse_PawnRenderer_ParallelGetPreRenderResults), "ModifyBobyPosAndAngle"));
            }
        }
    }

    public static void ModifyBobyPosAndAngle(PawnRenderer __instance, ref Vector3 bodyPos, ref float bodyAngle, 
        Rot4 bodyFacing)
    {
        var num = 0f;
        var num2 = 0f;
        var num3 = 0f;
        if (__instance.pawn.ShouldDuck())
        {
            switch (bodyFacing.AsInt)
            {
                case 0:
                    num = 0f;
                    num2 = 0f;
                    num3 = 0f;
                    break;
                case 1:
                    num = 15f;
                    num2 = -0.05f;
                    num3 = -0.1f;
                    break;
                case 2:
                    num = 0f;
                    num2 = 0f;
                    num3 = 0f;
                    break;
                case 3:
                    num = -15f;
                    num2 = 0.05f;
                    num3 = -0.1f;
                    break;
            }
        }
        else if (__instance.pawn.ShouldCrawl())
        {
            switch (bodyFacing.AsInt)
            {
                case 0: num = 0f; break;
                case 1: num = 45f; break;
                case 2: num = 180f; break;
                case 3: num = -45f; break;
            }
        }

        bodyAngle += num;
        bodyPos.z += num3;
        bodyPos.x += num2;
    }
}