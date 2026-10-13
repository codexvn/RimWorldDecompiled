using System;

namespace Verse
{
	// Token: 0x0200063C RID: 1596
	public class PawnRenderNodeWorker_Apparel_Head : PawnRenderNodeWorker_FlipWhenCrawling
	{
		// Token: 0x0600285E RID: 10334 RVA: 0x000D5BEC File Offset: 0x000D3DEC
		public override bool CanDrawNow(PawnRenderNode n, PawnDrawParms parms)
		{
			return base.CanDrawNow(n, parms) && parms.flags.FlagSet(PawnRenderFlags.Clothes) && parms.flags.FlagSet(PawnRenderFlags.Headgear) && PawnRenderNodeWorker_Apparel_Head.HeadgearVisible(parms) && (!parms.Portrait || !Prefs.HatsOnlyOnMap || parms.flags.FlagSet(PawnRenderFlags.StylingStation));
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000D5C54 File Offset: 0x000D3E54
		public static bool HeadgearVisible(PawnDrawParms parms)
		{
			return parms.flags.FlagSet(PawnRenderFlags.Clothes) && parms.flags.FlagSet(PawnRenderFlags.Headgear) && (parms.Portrait || parms.bed == null || parms.bed.def.building.bed_showSleeperBody) && (!parms.Portrait || !Prefs.HatsOnlyOnMap || parms.flags.FlagSet(PawnRenderFlags.StylingStation));
		}
	}
}
