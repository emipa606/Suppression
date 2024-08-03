using System;
using Verse;

namespace RimWorld;

public class Hediff_Suppressed : HediffWithComps
{
    private const float rapidImpactBonusFactorInitial = 1f;

    private float rapidImpactBonusFactor = 1f;

    private int stageCanBeChangedTick;

    public override float Severity
    {
        get => base.Severity;
        set
        {
            var oldValue = base.Severity;
            var oldStage = base.CurStageIndex;
            base.Severity = value;
            if (oldStage == base.CurStageIndex)
            {
                return;
            }

            if (stageCanBeChangedTick == 0 || Find.TickManager.TicksGame >= stageCanBeChangedTick)
            {
                stageCanBeChangedTick = (int)(Find.TickManager.TicksGame + (Rand.Range(1f, 4f) * 60));
            }
            else
            {
                base.Severity = oldValue;
            }
        }
    }

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
            rapidImpactBonusFactor = rapidImpactBonusFactorInitial;
        }

        return true;
    }

    public override void Tick()
    {
        base.Tick();
        const float num = 0.6f;
        const float num2 = 4f;
        const float num3 = 1.5f;
        var ageTickPart = ageTicks / 60f;
        var tickPart = ageTickPart / num3;
        var pow = (float)Math.Pow(tickPart, num2);
        var f = num * pow;
        Severity -= Math.Min(f / 60f, num / 60f);
    }

    public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
    {
        pawn.health.RemoveHediff(this);
        base.Notify_PawnDied(dinfo, culprit);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref rapidImpactBonusFactor, "rapidImpactBonusFactor", 1f);
        Scribe_Values.Look(ref stageCanBeChangedTick, "stageCanBeChangedTick");
    }
}