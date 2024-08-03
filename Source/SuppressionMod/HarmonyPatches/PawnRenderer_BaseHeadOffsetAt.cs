using HarmonyLib;
using UnityEngine;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(PawnRenderer), nameof(PawnRenderer.BaseHeadOffsetAt), typeof(Rot4))]
internal static class PawnRenderer_BaseHeadOffsetAt
{
    [HarmonyPostfix]
    public static void OffsetSuppressedHead(Rot4 rotation, ref Vector3 __result, ref Pawn ___pawn)
    {
        var num = 0f;
        var num2 = 0f;
        if (___pawn.ShouldDuck())
        {
            switch (rotation.AsInt)
            {
                case 0:
                    num2 = -0.2f;
                    break;
                case 1:
                    num = 0.1f;
                    break;
                case 2:
                    num2 = -0.175f;
                    break;
                case 3:
                    num = -0.1f;
                    break;
            }
        }

        __result.x += num;
        __result.z += num2;
    }
}