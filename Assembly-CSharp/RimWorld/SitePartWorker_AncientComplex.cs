using System;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x0200283F RID: 10303
	public class SitePartWorker_AncientComplex : SitePartWorker
	{
		// Token: 0x0600E16F RID: 57711 RVA: 0x0042199B File Offset: 0x0041FB9B
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
