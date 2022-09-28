using HarmonyLib;
using RimWorld;
using Verse;

namespace SuppressionMod;

[HarmonyPatch(typeof(Bullet), "Impact")]
internal static class Patch_Bullet_Impact
{
    [HarmonyPrefix]
    public static bool BulletImpactStuff(ref Thing hitThing, Bullet __instance, Thing ___launcher)
    {
        var damageAmount = __instance.def.projectile.GetDamageAmount(1f);
        var num = SuppressionUtil.CalcImpactSeverity(damageAmount);
        var allPawnsSpawned = __instance.Map.mapPawns.AllPawnsSpawned;
        foreach (var pawn in allPawnsSpawned)
        {
            if (pawn == null || pawn == ___launcher || pawn.RaceProps.Animal || !pawn.RaceProps.Humanlike ||
                !pawn.RaceProps.IsFlesh)
            {
                continue;
            }

            var num2 = pawn.PositionHeld.DistanceToSquared(___launcher.PositionHeld);
            var num3 = pawn.PositionHeld.DistanceToSquared(__instance.PositionHeld);
            var differentFactions = false;
            if (___launcher.Faction != null)
            {
                differentFactions = pawn.Faction != ___launcher.Faction;
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