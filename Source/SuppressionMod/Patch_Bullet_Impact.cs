using HarmonyLib;
using RimWorld;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000002 RID: 2
    [HarmonyPatch(typeof(Bullet), "Impact")]
    internal static class Patch_Bullet_Impact
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
        [HarmonyPrefix]
        public static bool BulletImpactStuff(ref Thing hitThing, Bullet __instance)
        {
            var value = Traverse.Create(__instance).Field("launcher").GetValue<Thing>();
            var damageAmount = __instance.def.projectile.GetDamageAmount(1f);
            var num = SuppressionUtil.CalcImpactSeverity(damageAmount);
            var allPawnsSpawned = __instance.Map.mapPawns.AllPawnsSpawned;
            foreach (var pawn in allPawnsSpawned)
            {
                if (pawn == null || pawn == value || pawn.RaceProps.Animal || !pawn.RaceProps.Humanlike ||
                    !pawn.RaceProps.IsFlesh)
                {
                    continue;
                }

                var num2 = pawn.PositionHeld.DistanceToSquared(value.PositionHeld);
                var num3 = pawn.PositionHeld.DistanceToSquared(__instance.PositionHeld);
                var differentFactions = false;
                if (value.Faction != null)
                {
                    differentFactions = pawn.Faction != value.Faction;
                }

                if (!differentFactions || num3 > SuppressionUtil.maxDistanceToImpactSquared ||
                    num2 < SuppressionUtil.minDistanceFromLauncherSquared)
                {
                    continue;
                }

                var impactSquaredInv = 1f - (num3 * SuppressionUtil.maxDistanceToImpactSquaredInv);
                var hediff = HediffMaker.MakeHediff(SuppressionUtil.suppressed, pawn);
                hediff.Severity = num * impactSquaredInv;
                pawn.health.AddHediff(hediff);
            }

            return true;
        }
    }
}