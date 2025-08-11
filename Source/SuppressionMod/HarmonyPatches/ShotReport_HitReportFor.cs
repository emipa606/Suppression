using HarmonyLib;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(ShotReport), nameof(ShotReport.HitReportFor), typeof(Thing), typeof(Verb),
    typeof(LocalTargetInfo))]
internal static class ShotReport_HitReportFor
{
    public static void Postfix(ref LocalTargetInfo target, ref ShotReport __result)
    {
        if (target.Pawn == null)
        {
            return;
        }

        if (target.thingInt is not Pawn pawn || pawn.IsSubhuman || pawn.IsShambler)
        {
            return;
        }

        if (pawn.ShouldDuckOrCrawl(out var hediff))
        {
            __result.factorFromTargetSize *= SuppressionUtil.coverAdvantageFactorByHediffStage[hediff.CurStageIndex];
        }
    }
}