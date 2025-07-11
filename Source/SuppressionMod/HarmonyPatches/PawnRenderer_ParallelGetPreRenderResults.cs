using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(PawnRenderer), "ParallelGetPreRenderResults")]
internal static class PawnRenderer_ParallelGetPreRenderResults
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
    {
        foreach (var codeInstruction in codeInstructions)
        {
            yield return codeInstruction;
            // Inject after stloc.s 9 (bodyFacing)
            if (codeInstruction.opcode != OpCodes.Stloc_S ||
                codeInstruction.operand is not LocalBuilder { LocalIndex: 9 })
            {
                continue;
            }

            yield return new CodeInstruction(OpCodes.Ldarg_0); // this (PawnRenderer)
            yield return new CodeInstruction(OpCodes.Ldloca_S, 6); // ref Vector3 bodyPos
            yield return new CodeInstruction(OpCodes.Ldloca_S, 8); // ref float bodyAngle
            yield return new CodeInstruction(OpCodes.Ldloc_S, 9); // Rot4 bodyFacing
            yield return new CodeInstruction(OpCodes.Call,
                AccessTools.Method(typeof(PawnRenderer_ParallelGetPreRenderResults),
                    nameof(ModifyBodyPosAndAngle)));
        }
    }

    public static void ModifyBodyPosAndAngle(PawnRenderer __instance, ref Vector3 bodyPos, ref float bodyAngle,
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
                case 0:
                    num = 0f;
                    break;
                case 1:
                    num = 45f;
                    break;
                case 2:
                    num = 180f;
                    break;
                case 3:
                    num = -45f;
                    break;
            }
        }

        bodyAngle += num;
        bodyPos.z += num3;
        bodyPos.x += num2;
    }
}