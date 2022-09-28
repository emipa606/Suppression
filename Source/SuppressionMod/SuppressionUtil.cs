using System;
using RimWorld;
using Verse;

namespace SuppressionMod;

public static class SuppressionUtil
{
    public const int facingNorth = 0;

    public const int facingEast = 1;

    public const int facingSouth = 2;

    public const int facingWest = 3;

    public const float suppressedMovespeedMin = 0.7f;

    public const int duckingHediffStage = 3;

    public const int proneHediffStage = 4;

    public const float ticksPerSecond = 60f;

    public const float severityDelaySeconds = 1f;

    public static HediffDef suppressed;

    public static readonly float[] movespeedFactorByHediffStage =
    {
        1f,
        1f,
        1f,
        0.8f,
        0.65f
    };

    public static readonly float[] accuracyFactorByHediffStage =
    {
        1f,
        1f,
        1f,
        0.8f,
        0.4f
    };

    // ReSharper disable once FieldCanBeMadeReadOnly.Global
    public static float[] aimingDelayFactorByHediffStage =
    {
        1f,
        1f,
        1f,
        1.5f,
        3f
    };

    public static readonly float[] coverAdvantageFactorByHediffStage =
    {
        1f,
        1f,
        1f,
        0.85f,
        0.7f
    };

    public static readonly float severityReductionPerSecond = 0.1f;

    public static float severityReductionPerTick = severityReductionPerSecond / 60f;

    public static int severityDelayTicks = 60;

    public static readonly int maxDistanceToImpact = 3;

    public static readonly int minDistanceFromLauncher = 5;

    public static readonly int maxDistanceToImpactSquared = maxDistanceToImpact * maxDistanceToImpact;

    public static readonly int minDistanceFromLauncherSquared = minDistanceFromLauncher * minDistanceFromLauncher;

    public static readonly float maxDistanceToImpactSquaredInv = 1f / minDistanceFromLauncherSquared;

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

    public static bool ShouldPawnDuck(ref Pawn pawn, ref Hediff suppressedHediff)
    {
        return pawn != null && pawn.GetPosture() == PawnPosture.Standing &&
               suppressedHediff is { CurStageIndex: >= 3 };
    }
}