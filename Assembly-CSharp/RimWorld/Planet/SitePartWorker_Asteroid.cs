using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DEB RID: 15851
	public class SitePartWorker_Asteroid : SitePartWorker
	{
		// Token: 0x06015A15 RID: 88597 RVA: 0x00679626 File Offset: 0x00677826
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			site.customLabel = sitePart.def.label.Formatted(sitePart.parms.preciousLumpResources.Named("RESOURCE"));
		}
	}
}
