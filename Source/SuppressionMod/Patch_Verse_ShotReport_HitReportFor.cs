using HarmonyLib;
using Verse;

namespace SuppressionMod;

[HarmonyPatch(typeof(ShotReport), "HitReportFor", typeof(Thing), typeof(Verb), typeof(LocalTargetInfo))]
internal static class Patch_Verse_ShotReport_HitReportFor
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