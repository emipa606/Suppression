using Verse;

namespace SuppressionMod;

/// <summary>
///     Definition of the settings for the mod
/// </summary>
internal class SuppressionModSettings : ModSettings
{
    public bool OnlyRangedPawns;

    /// <summary>
    ///     Saving and loading the values
    /// </summary>
    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref OnlyRangedPawns, "OnlyRangedPawns");
    }
}