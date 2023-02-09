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
        new Harmony("Mlie.SuppressionMod").PatchAll(Assembly.GetExecutingAssembly());
    }
}