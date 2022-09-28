using System.Reflection;
using HarmonyLib;
using Verse;

namespace SuppressionMod;

[StaticConstructorOnStartup]
public static class Settings
{
    static Settings()
    {
        SuppressionUtil.suppressed = HediffDef.Named("Suppressed");
        new Harmony("Mlie.SuppressionMod").PatchAll(Assembly.GetExecutingAssembly());
    }
}