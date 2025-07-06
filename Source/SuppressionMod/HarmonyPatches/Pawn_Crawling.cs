using HarmonyLib;
using Verse;

namespace SuppressionMod.HarmonyPatches;

[HarmonyPatch(typeof(Pawn), nameof(Pawn.Crawling), MethodType.Getter)]
internal static class Pawn_Crawling
{
    public static bool Prefix(Pawn __instance, ref bool __result)
    {
        if (!__instance.ShouldCrawl())
        {
            return true;
        }

        __result = true;
        return false;
    }
}