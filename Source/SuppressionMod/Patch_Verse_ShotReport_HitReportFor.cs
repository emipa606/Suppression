using System.Reflection;
using HarmonyLib;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000006 RID: 6
    [HarmonyPatch(typeof(ShotReport), "HitReportFor", typeof(Thing), typeof(Verb), typeof(LocalTargetInfo))]
    internal static class Patch_Verse_ShotReport_HitReportFor
    {
        // Token: 0x06000008 RID: 8 RVA: 0x000026E8 File Offset: 0x000008E8
        [HarmonyPostfix]
        public static void ApplySuppressionFactors(ref Thing caster, ref LocalTargetInfo target,
            ref ShotReport __result)
        {
            if (target.Pawn == null)
            {
                return;
            }

            var hediff = target.Pawn.health.hediffSet.hediffs.Find(x => x.def == SuppressionUtil.suppressed);

            var pawn = Traverse.Create(target).Field("thingInt").GetValue<Thing>() as Pawn;
            if (!SuppressionUtil.ShouldPawnDuck(ref pawn, ref hediff))
            {
                return;
            }

            var num = SuppressionUtil.coverAdvantageFactorByHediffStage[hediff.CurStageIndex];
            object obj = __result;
            var field = obj.GetType().GetField("factorFromTargetSize",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (field is not null)
            {
                var num2 = (float) field.GetValue(obj);
                field.SetValue(obj, num2 * num);
            }

            __result = (ShotReport) obj;
        }
    }
}