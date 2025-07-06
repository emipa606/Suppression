using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace SuppressionMod;

public static class SuppressionUtil
{
    private const float suppressedMovespeedMin = 0.7f;

    private const int duckingHediffStage = 3;

    private const int proneHediffStage = 4;

    private const float ticksPerSecond = 60f;

    public static HediffDef suppressed;

    public static readonly float[] movespeedFactorByHediffStage =
    [
        1f,
        1f,
        1f,
        0.8f,
        0.65f
    ];

    public static readonly float[] accuracyFactorByHediffStage =
    [
        1f,
        1f,
        1f,
        0.8f,
        0.4f
    ];

    public static readonly float[] aimingDelayFactorByHediffStage =
    [
        1f,
        1f,
        1f,
        1.5f,
        3f
    ];

    public static readonly float[] coverAdvantageFactorByHediffStage =
    [
        1f,
        1f,
        1f,
        0.85f,
        suppressedMovespeedMin
    ];

    private static readonly float severityReductionPerSecond = 0.1f;

    public static float severityReductionPerTick = severityReductionPerSecond / ticksPerSecond;

    public static int severityDelayTicks = 60;

    private static readonly int maxDistanceToImpact = 3;

    private static readonly int minDistanceFromLauncher = 5;

    public static readonly int maxDistanceToImpactSquared = maxDistanceToImpact * maxDistanceToImpact;

    public static readonly int minDistanceFromLauncherSquared = minDistanceFromLauncher * minDistanceFromLauncher;

    public static readonly float maxDistanceToImpactSquaredInv = 1f / minDistanceFromLauncherSquared;
    public static bool biotechActive;
    public static GeneDef unstoppableGene;

    public static Dictionary<Pawn, bool> unstoppablePawns;

    public static float CalcImpactSeverity(float impactDamage)
    {
        var damageSeverity = Math.Min(impactDamage / 100f, 100f);
        var num = TuneSeverity_SmoothInOut(damageSeverity);
        return Math.Max(num * 3.2f, 0.01f);
    }

    private static float TuneSeverity_FlatUpperRange(float damageSeverity)
    {
        return 1f - (float)Math.Pow(1f - damageSeverity, 9.0);
    }

    private static float TuneSeverity_HighLowerRange(float damageSeverity)
    {
        return (float)Math.Pow(damageSeverity, 0.66666668653488159);
    }

    private static float TuneSeverity_Linear(float damageSeverity)
    {
        return damageSeverity;
    }

    private static float TuneSeverity_SmoothInOut(float damageSeverity)
    {
        var num = damageSeverity * damageSeverity;
        var num2 = num * damageSeverity;
        return (3f * num) + (-2f * num2);
    }

    public static bool ShouldDuck(this Pawn pawn)
    {
        var posture = pawn?.GetPosture();
        if (posture != PawnPosture.Standing)
        {
            return false;
        }

        var suppressedHediff = pawn.health.hediffSet.GetFirstHediffOfDef(suppressed);
        return suppressedHediff is { CurStageIndex: duckingHediffStage };
    }

    public static bool ShouldCrawl(this Pawn pawn)
    {
        var posture = pawn?.GetPosture();
        if (posture != PawnPosture.Standing)
        {
            return false;
        }

        var suppressedHediff = pawn.health.hediffSet.GetFirstHediffOfDef(suppressed);
        return suppressedHediff is { CurStageIndex: >= proneHediffStage };
    }

    public static bool ShouldDuckOrCrawl(this Pawn pawn, out Hediff suppressedHediff)
    {
        var posture = pawn?.GetPosture();
        if (posture == PawnPosture.Standing)
        {
            suppressedHediff = pawn.health.hediffSet.GetFirstHediffOfDef(suppressed);
            return suppressedHediff is { CurStageIndex: >= duckingHediffStage };
        }

        suppressedHediff = null;
        return false;
    }
}