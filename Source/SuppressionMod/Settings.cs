using HugsLib;
using Verse;

namespace SuppressionMod
{
    // Token: 0x02000007 RID: 7
    public class Settings : ModBase
    {
        // Token: 0x06000009 RID: 9 RVA: 0x000027CE File Offset: 0x000009CE
        public Settings()
        {
            SuppressionUtil.suppressed = HediffDef.Named("Suppressed");
        }

        // Token: 0x17000001 RID: 1
        // (get) Token: 0x0600000A RID: 10 RVA: 0x000027E7 File Offset: 0x000009E7
        public override string ModIdentifier => "da_SuppressionMod";
    }
}