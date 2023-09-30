using System.Linq;
using HarmonyLib;
using RimWorld;
using Verse;

namespace SuppressionMod;

[HarmonyPatch(typeof(Bullet), "Impact")]
internal static class Patch_Bullet_Impact
{
    [HarmonyPrefix]
    public static bool BulletImpactStuff(Bullet __instance, Thing ___launcher)
    {
        if (___launcher == null)
        {
            return true;
        }

        var damageAmount = __instance.def.projectile.GetDamageAmount(1f);
        var num = SuppressionUtil.CalcImpactSeverity(damageAmount);
        var allPawnsSpawned = __instance.Map.mapPawns.AllPawnsSpawned.Where(pawn =>
            pawn != null && pawn != ___launcher && pawn.RaceProps is { Humanlike: true, IsFlesh: true });
        foreach (var pawn in allPawnsSpawned)
        {
            if (SuppressionMod.instance.Settings.OnlyRangedPawns &&
                (pawn.equipment.Primary == null || !pawn.equipment.Primary.def.IsRangedWeapon))
            {
                continue;
            }

            if (pawn.Faction == ___launcher.Faction)
            {
                continue;
            }

            if (SuppressionUtil.biotechActive)
            {
                if (SuppressionUtil.unstoppablePawns.TryGetValue(pawn, out var unstoppable))
                {
                    if (unstoppable)
                    {
                        continue;
                    }
                }
                else
                {
                    SuppressionUtil.unstoppablePawns.Add(pawn,
                        pawn.genes?.HasGene(SuppressionUtil.unstoppableGene) == true);
                    if (SuppressionUtil.unstoppablePawns[pawn])
                    {
                        continue;
                    }
                }
            }

            if (SuppressionMod.instance.Settings.MoodAffectsChance &&
                Rand.Value < pawn.needs?.mood?.CurLevelPercentage - 0.1f)
            {
                continue;
            }

            var distanceToShooter = pawn.PositionHeld.DistanceToSquared(___launcher.PositionHeld);
            var distanceToImpact = pawn.PositionHeld.DistanceToSquared(__instance.PositionHeld);

            if (distanceToImpact > SuppressionUtil.maxDistanceToImpactSquared ||
                distanceToShooter < SuppressionUtil.minDistanceFromLauncherSquared)
            {
                continue;
            }

            var impactSquaredInv = 1f - (distanceToImpact * SuppressionUtil.maxDistanceToImpactSquaredInv);
            var hediff = HediffMaker.MakeHediff(SuppressionUtil.suppressed, pawn);
            hediff.Severity = num * impactSquaredInv;
            pawn.health.AddHediff(hediff);
        }

        return true;
    }
}