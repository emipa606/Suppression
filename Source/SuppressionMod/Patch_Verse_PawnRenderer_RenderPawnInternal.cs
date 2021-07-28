using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000005 RID: 5
    [HarmonyPatch(typeof(PawnRenderer), "RenderPawnInternal")]
    internal static class Patch_Verse_PawnRenderer_RenderPawnInternal
    {
        // Token: 0x06000007 RID: 7 RVA: 0x000024EC File Offset: 0x000006EC
        [HarmonyPrefix]
        public static void TestMethod(ref Vector3 rootLoc, ref float angle, ref Rot4 bodyFacing,
            PawnRenderer __instance)
        {
            var value = Traverse.Create(__instance).Field("pawn").GetValue<Pawn>();
            if (value.GetPosture() > PawnPosture.Standing)
            {
                return;
            }

            var hediff = value.health.hediffSet.hediffs.Find(x => x.def == SuppressionUtil.suppressed);
            if (hediff is not {CurStageIndex: >= 3})
            {
                return;
            }

            var num = 0f;
            var num2 = 0f;
            var num3 = 0f;
            switch (hediff.CurStageIndex)
            {
                case >= 4:
                    switch (bodyFacing.AsInt)
                    {
                        case 0:
                            num = 0f;
                            num2 = 0f;
                            num3 = 0f;
                            break;
                        case 1:
                            num = 35f;
                            num2 = 0.075f;
                            num3 = -0.1f;
                            break;
                        case 2:
                            num = 0f;
                            num2 = 0f;
                            num3 = 0f;
                            break;
                        case 3:
                            num = -35f;
                            num2 = -0.075f;
                            num3 = -0.1f;
                            break;
                    }

                    break;
                case >= 3:
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

                    break;
            }

            angle += num;
            rootLoc.z += num3;
            rootLoc.x += num2;
        }
    }
}