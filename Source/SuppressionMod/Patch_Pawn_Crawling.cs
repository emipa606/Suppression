using HarmonyLib;
using Verse;

namespace SuppressionMod;

[HarmonyPatch(typeof(Pawn), "Crawling", MethodType.Getter)]
internal static class Patch_Pawn_Crawling
{
    [HarmonyPrefix]
    public static bool SetToCrawlingWhenCowering(Pawn __instance, ref bool __result)
    {
        if (__instance.ShouldCrawl())
        {
            __result = true;
            return false;
        }
        return true;
    }
}