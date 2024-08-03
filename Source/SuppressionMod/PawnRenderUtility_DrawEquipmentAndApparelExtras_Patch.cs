using HarmonyLib;
using UnityEngine;
using Verse;

namespace SuppressionMod;

[HarmonyPatch(typeof(PawnRenderUtility), "DrawEquipmentAndApparelExtras")]
public static class PawnRenderUtility_DrawEquipmentAndApparelExtras_Patch
{
    public static void Prefix(Pawn pawn, ref Vector3 drawPos, Rot4 facing, PawnRenderFlags flags)
    { 
        if (pawn.Rotation == Rot4.South && pawn.ShouldCrawl())
        {
            drawPos.y -= 0.1f;
            drawPos += new Vector3(0, 0, 0.25f).RotatedBy(facing.AsAngle);
        }
    }
}
