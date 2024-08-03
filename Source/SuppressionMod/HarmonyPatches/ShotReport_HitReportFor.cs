using HarmonyLib;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitReportFor), typeof(Thing), typeof(Verb),
    typeof(LocalTargetInfo))]
internal static class ShotReport_HitReportFor
{
    [HarmonyPostfix]
    public static void ApplySuppressionFactors(ref LocalTargetInfo target, ref ShotReport __result)
    {
        if (target.Pawn == null)
        {
            return;
        }

        var pawn = target.thingInt as Pawn;
        if (pawn.ShouldDuckOrCrawl(out var hediff))
        {
            __result.factorFromTargetSize *= SuppressionUtil.coverAdvantageFactorByHediffStage[hediff.CurStageIndex];
        }
    }
}