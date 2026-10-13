using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DEC RID: 15852
	public abstract class SitePartWorker_DistressCall : SitePartWorker
	{
		// Token: 0x06015A17 RID: 88599 RVA: 0x00679660 File Offset: 0x00677860
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			site.customLabel = sitePart.def.label.Formatted(site.Faction.Named("FACTION"));
		}

		// Token: 0x06015A18 RID: 88600 RVA: 0x00679698 File Offset: 0x00677898
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return "Hostiles".Translate() + ": " + "Unknown".Translate().CapitalizeFirst() + ("\n" + "Contains".Translate() + ": " + "Unknown".Translate().CapitalizeFirst());
		}
	}
}
