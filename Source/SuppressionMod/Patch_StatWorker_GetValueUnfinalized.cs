using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000003 RID: 3
    [HarmonyPatch(typeof(StatWorker), "GetValueUnfinalized", typeof(StatRequest), typeof(bool))]
    internal static class Patch_StatWorker_GetValueUnfinalized
    {
        // Token: 0x06000002 RID: 2 RVA: 0x000021FC File Offset: 0x000003FC
        private static float CalcScaledAccuracy(float accuracy, ref Hediff hediff)
        {
            return accuracy * SuppressionUtil.accuracyFactorByHediffStage[hediff.CurStageIndex];
        }

        // Token: 0x06000003 RID: 3 RVA: 0x00002220 File Offset: 0x00000420
        private static float CalcScaledMoveSpeed(float moveSpeed, ref Hediff hediff)
        {
            float result;
            if (moveSpeed <= 0.7f)
            {
                result = moveSpeed;
            }
            else
            {
                var val = moveSpeed * SuppressionUtil.movespeedFactorByHediffStage[hediff.CurStageIndex];
                result = Math.Max(0.7f, val);
            }

            return result;
        }

        // Token: 0x06000004 RID: 4 RVA: 0x00002264 File Offset: 0x00000464
        private static float CalcScaledAimingDelayFactor(float aimingDelayFactor, ref Hediff hediff)
        {
            return aimingDelayFactor * SuppressionUtil.aimingDelayFactorByHediffStage[hediff.CurStageIndex];
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002288 File Offset: 0x00000488
        [HarmonyPostfix]
        public static void ApplySuppressionFactors(StatRequest req, StatWorker __instance, ref float __result)
        {
            if (!(req.Thing is Pawn pawn))
            {
                return;
            }

            var value = Traverse.Create(__instance).Field("stat").GetValue<StatDef>();
            if (value != StatDefOf.MoveSpeed && value != StatDefOf.ShootingAccuracyPawn &&
                value != StatDefOf.AimingDelayFactor)
            {
                return;
            }

            var hediff = pawn.health.hediffSet.hediffs.Find(x => x.def == SuppressionUtil.suppressed);
            if (hediff == null || hediff.CurStage == null)
            {
                return;
            }

            if (value == StatDefOf.MoveSpeed)
            {
                __result = CalcScaledMoveSpeed(__result, ref hediff);
                return;
            }

            if (value == StatDefOf.ShootingAccuracyPawn)
            {
                __result = CalcScaledAccuracy(__result, ref hediff);
                return;
            }

            if (value == StatDefOf.AimingDelayFactor)
            {
                __result = CalcScaledAimingDelayFactor(__result, ref hediff);
            }
        }
    }
}