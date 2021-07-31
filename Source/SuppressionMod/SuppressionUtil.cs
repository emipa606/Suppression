using System;
using RimWorld;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000008 RID: 8
    public static class SuppressionUtil
    {
        // Token: 0x04000002 RID: 2
        public const int facingNorth = 0;

        // Token: 0x04000003 RID: 3
        public const int facingEast = 1;

        // Token: 0x04000004 RID: 4
        public const int facingSouth = 2;

        // Token: 0x04000005 RID: 5
        public const int facingWest = 3;

        // Token: 0x0400000A RID: 10
        public const float suppressedMovespeedMin = 0.7f;

        // Token: 0x0400000B RID: 11
        public const int duckingHediffStage = 3;

        // Token: 0x0400000C RID: 12
        public const int proneHediffStage = 4;

        // Token: 0x0400000D RID: 13
        public const float ticksPerSecond = 60f;

        // Token: 0x04000010 RID: 16
        public const float severityDelaySeconds = 1f;

        // Token: 0x04000001 RID: 1
        public static HediffDef suppressed;

        // Token: 0x04000006 RID: 6
        public static readonly float[] movespeedFactorByHediffStage =
        {
            1f,
            1f,
            1f,
            0.8f,
            0.65f
        };

        // Token: 0x04000007 RID: 7
        public static readonly float[] accuracyFactorByHediffStage =
        {
            1f,
            1f,
            1f,
            0.8f,
            0.4f
        };

        // Token: 0x04000008 RID: 8
        // ReSharper disable once FieldCanBeMadeReadOnly.Global
        public static float[] aimingDelayFactorByHediffStage =
        {
            1f,
            1f,
            1f,
            1.5f,
            3f
        };

        // Token: 0x04000009 RID: 9
        public static readonly float[] coverAdvantageFactorByHediffStage =
        {
            1f,
            1f,
            1f,
            0.85f,
            0.7f
        };

        // Token: 0x0400000E RID: 14
        public static readonly float severityReductionPerSecond = 0.1f;

        // Token: 0x0400000F RID: 15
        public static float severityReductionPerTick = severityReductionPerSecond / 60f;

        // Token: 0x04000011 RID: 17
        public static int severityDelayTicks = 60;

        // Token: 0x04000012 RID: 18
        public static readonly int maxDistanceToImpact = 3;

        // Token: 0x04000013 RID: 19
        public static readonly int minDistanceFromLauncher = 5;

        // Token: 0x04000014 RID: 20
        public static readonly int maxDistanceToImpactSquared = maxDistanceToImpact * maxDistanceToImpact;

        // Token: 0x04000015 RID: 21
        public static readonly int minDistanceFromLauncherSquared = minDistanceFromLauncher * minDistanceFromLauncher;

        // Token: 0x04000016 RID: 22
        public static readonly float maxDistanceToImpactSquaredInv = 1f / minDistanceFromLauncherSquared;

        // Token: 0x0600000B RID: 11 RVA: 0x000027F0 File Offset: 0x000009F0
        public static float CalcImpactSeverity(float impactDamage)
        {
            var damageSeverity = Math.Min(impactDamage / 100f, 100f);
            var num = TuneSeverity_SmoothInOut(damageSeverity);
            return Math.Max(num * 3.2f, 0.01f);
        }

        // Token: 0x0600000C RID: 12 RVA: 0x0000282C File Offset: 0x00000A2C
        private static float TuneSeverity_FlatUpperRange(float damageSeverity)
        {
            return 1f - (float) Math.Pow(1f - damageSeverity, 9.0);
        }

        // Token: 0x0600000D RID: 13 RVA: 0x0000285C File Offset: 0x00000A5C
        private static float TuneSeverity_HighLowerRange(float damageSeverity)
        {
            return (float) Math.Pow(damageSeverity, 0.66666668653488159);
        }

        // Token: 0x0600000E RID: 14 RVA: 0x00002880 File Offset: 0x00000A80
        private static float TuneSeverity_Linear(float damageSeverity)
        {
            return damageSeverity;
        }

        // Token: 0x0600000F RID: 15 RVA: 0x00002894 File Offset: 0x00000A94
        private static float TuneSeverity_SmoothInOut(float damageSeverity)
        {
            var num = damageSeverity * damageSeverity;
            var num2 = num * damageSeverity;
            return (3f * num) + (-2f * num2);
        }

        // Token: 0x06000010 RID: 16 RVA: 0x000028C0 File Offset: 0x00000AC0
        public static bool ShouldPawnDuck(ref Pawn pawn, ref Hediff suppressedHediff)
        {
            return pawn != null && pawn.GetPosture() == PawnPosture.Standing &&
                   suppressedHediff is {CurStageIndex: >= 3};
        }
    }
}