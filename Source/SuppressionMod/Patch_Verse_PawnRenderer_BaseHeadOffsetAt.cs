using HarmonyLib;
using UnityEngine;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000004 RID: 4
    [HarmonyPatch(typeof(PawnRenderer), "BaseHeadOffsetAt", typeof(Rot4))]
    internal static class Patch_Verse_PawnRenderer_BaseHeadOffsetAt
    {
        // Token: 0x06000006 RID: 6 RVA: 0x00002394 File Offset: 0x00000594
        [HarmonyPostfix]
        public static void OffsetSuppressedHead(Rot4 rotation, PawnRenderer __instance, ref Vector3 __result,
            ref Pawn ___pawn)
        {
            var hediff = ___pawn.health.hediffSet.hediffs.Find(x => x.def == SuppressionUtil.suppressed);
            if (!SuppressionUtil.ShouldPawnDuck(ref ___pawn, ref hediff))
            {
                return;
            }

            var num = 0f;
            var num2 = 0f;
            switch (hediff.CurStageIndex)
            {
                case >= 4:
                    switch (rotation.AsInt)
                    {
                        case 0:
                            num2 = -0.225f;
                            break;
                        case 1:
                            num = 0.1f;
                            num2 = -0.1f;
                            break;
                        case 2:
                            num2 = -0.225f;
                            break;
                        case 3:
                            num = -0.1f;
                            num2 = -0.1f;
                            break;
                    }

                    break;
                case >= 3:
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

                    break;
            }

            __result.x += num;
            __result.z += num2;
        }
    }
}