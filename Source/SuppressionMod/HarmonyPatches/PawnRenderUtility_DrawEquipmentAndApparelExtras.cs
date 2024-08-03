using HarmonyLib;
using UnityEngine;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(PawnRenderUtility), nameof(PawnRenderUtility.DrawEquipmentAndApparelExtras))]
public static class PawnRenderUtility_DrawEquipmentAndApparelExtras
{
    public static void Prefix(Pawn pawn, ref Vector3 drawPos, Rot4 facing)
    {
        if (pawn.Rotation != Rot4.South || !pawn.ShouldCrawl())
        {
            return;
        }

        drawPos.y -= 0.1f;
        drawPos += new Vector3(0, 0, 0.25f).RotatedBy(facing.AsAngle);
    }
}