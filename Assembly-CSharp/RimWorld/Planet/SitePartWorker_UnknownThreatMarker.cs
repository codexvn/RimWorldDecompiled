using System;

namespace RimWorld.Planet
{
	// Token: 0x02003DF7 RID: 15863
	public class SitePartWorker_UnknownThreatMarker : SitePartWorker
	{
		// Token: 0x06015A37 RID: 88631 RVA: 0x0067A3CC File Offset: 0x006785CC
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == SitePartDefOf.PreciousLump)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
