using System;
using Verse;

namespace RimWorld
{
    // Token: 0x02000009 RID: 9
    public class Hediff_Suppressed : HediffWithComps
    {
        // Token: 0x04000018 RID: 24
        private const float rapidImpactBonusFactorInitial = 1f;

        // Token: 0x04000017 RID: 23
        private float rapidImpactBonusFactor = 1f;

        // Token: 0x06000012 RID: 18 RVA: 0x000029B8 File Offset: 0x00000BB8
        public override bool TryMergeWith(Hediff other)
        {
            if (pawn.Dead || pawn.Downed)
            {
                return true;
            }

            float num = ageTicks;
            if (!base.TryMergeWith(other))
            {
                return false;
            }

            if (num < 20f)
            {
                rapidImpactBonusFactor = Math.Min(rapidImpactBonusFactor * 1.7f, 10f);
                var num2 = (other.Severity * rapidImpactBonusFactor) - other.Severity;
                Severity += num2;
            }
            else
            {
                rapidImpactBonusFactor = 1f;
            }

            return true;
        }

        // Token: 0x06000013 RID: 19 RVA: 0x00002A70 File Offset: 0x00000C70
        public override void Tick()
        {
            base.Tick();
            const float num = 0.6f;
            const float num2 = 4f;
            const float num3 = 1.5f;
            var ageTickPart = ageTicks / 60f;
            var tickPart = ageTickPart / num3;
            var pow = (float) Math.Pow(tickPart, num2);
            var f = num * pow;
            Severity -= Math.Min(f / 60f, num / 60f);
        }

        // Token: 0x06000014 RID: 20 RVA: 0x00002ADF File Offset: 0x00000CDF
        public override void Notify_PawnDied()
        {
            pawn.health.RemoveHediff(this);
            base.Notify_PawnDied();
        }

        // Token: 0x06000015 RID: 21 RVA: 0x00002AFB File Offset: 0x00000CFB
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref rapidImpactBonusFactor, "rapidImpactBonusFactor", 1f);
        }
    }
}