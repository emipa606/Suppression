using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace SuppressionMod;

[StaticConstructorOnStartup]
public static class Main
{
    static Main()
    {
        SuppressionUtil.suppressed = HediffDef.Named("Suppressed");
        SuppressionUtil.biotechActive = ModLister.BiotechInstalled;
        if (SuppressionUtil.biotechActive)
        {
            SuppressionUtil.unstoppableGene = DefDatabase<GeneDef>.GetNamedSilentFail("Unstoppable");
            SuppressionUtil.unstoppablePawns = new Dictionary<Pawn, bool>();
        }

        new Harmony("Mlie.SuppressionMod").PatchAll(Assembly.GetExecutingAssembly());
    }
}