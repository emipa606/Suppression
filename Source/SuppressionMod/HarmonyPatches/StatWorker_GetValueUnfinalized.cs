using System;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(StatWorker), nameof(StatWorker.GetValueUnfinalized), typeof(StatRequest), typeof(bool))]
internal static class StatWorker_GetValueUnfinalized
{
    private static float CalcScaledAccuracy(float accuracy, ref Hediff hediff)
    {
        return accuracy * SuppressionUtil.accuracyFactorByHediffStage[hediff.CurStageIndex];
    }

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

    private static float CalcScaledAimingDelayFactor(float aimingDelayFactor, ref Hediff hediff)
    {
        return aimingDelayFactor * SuppressionUtil.aimingDelayFactorByHediffStage[hediff.CurStageIndex];
    }

    [HarmonyPostfix]
    public static void ApplySuppressionFactors(StatRequest req, ref float __result, StatDef ___stat)
    {
        if (req.Thing is not Pawn pawn)
        {
            return;
        }

        if (___stat != StatDefOf.MoveSpeed && ___stat != StatDefOf.ShootingAccuracyPawn &&
            ___stat != StatDefOf.AimingDelayFactor)
        {
            return;
        }

        var hediff = pawn.health.hediffSet.hediffs.Find(x => x.def == SuppressionUtil.suppressed);
        if (hediff?.CurStage == null)
        {
            return;
        }

        if (___stat == StatDefOf.MoveSpeed)
        {
            __result = CalcScaledMoveSpeed(__result, ref hediff);
            return;
        }

        if (___stat == StatDefOf.ShootingAccuracyPawn)
        {
            __result = CalcScaledAccuracy(__result, ref hediff);
            return;
        }

        if (___stat == StatDefOf.AimingDelayFactor)
        {
            __result = CalcScaledAimingDelayFactor(__result, ref hediff);
        }
    }
}