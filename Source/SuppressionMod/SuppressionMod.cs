using Mlie;
using UnityEngine;
using Verse;

namespace SuppressionMod;

[StaticConstructorOnStartup]
internal class SuppressionMod : Mod
{
    /// <summary>
    ///     The instance of the settings to be read by the mod
    /// </summary>
    public static SuppressionMod instance;

    private static string currentVersion;

    /// <summary>
    ///     The private settings
    /// </summary>
    public readonly SuppressionModSettings Settings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="content"></param>
    public SuppressionMod(ModContentPack content) : base(content)
    {
        instance = this;
        Settings = GetSettings<SuppressionModSettings>();
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    /// <summary>
    ///     The title for the mod-settings
    /// </summary>
    /// <returns></returns>
    public override string SettingsCategory()
    {
        return "Suppression";
    }

    /// <summary>
    ///     The settings-window
    ///     For more info: https://rimworldwiki.com/wiki/Modding_Tutorials/ModSettings
    /// </summary>
    /// <param name="rect"></param>
    public override void DoSettingsWindowContents(Rect rect)
    {
        var listing_Standard = new Listing_Standard();
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("Sup.OnlyRangedPawns".Translate(), ref Settings.OnlyRangedPawns,
            "Sup.OnlyRangedPawnsTT".Translate());
        listing_Standard.CheckboxLabeled("Sup.MoodAffectsChance".Translate(), ref Settings.MoodAffectsChance,
            "Sup.MoodAffectsChanceTT".Translate());
        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("Sup.modVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
    }
}