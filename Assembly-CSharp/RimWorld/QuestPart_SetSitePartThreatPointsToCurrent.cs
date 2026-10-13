using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001CAC RID: 7340
	public class QuestPart_SetSitePartThreatPointsToCurrent : QuestPart
	{
		// Token: 0x0600A382 RID: 41858 RVA: 0x002EE84C File Offset: 0x002ECA4C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.useMapParentThreatPoints != null && this.useMapParentThreatPoints.HasMap && this.site != null && this.sitePartDef != null)
			{
				List<SitePart> parts = this.site.parts;
				for (int i = 0; i < parts.Count; i++)
				{
					if (parts[i].def == this.sitePartDef)
					{
						parts[i].parms.threatPoints = StorytellerUtility.DefaultThreatPointsNow(this.useMapParentThreatPoints.Map) * this.threatPointsFactor;
					}
				}
			}
		}

		// Token: 0x0600A383 RID: 41859 RVA: 0x002EE8F4 File Offset: 0x002ECAF4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<SitePartDef>(ref this.sitePartDef, "sitePartDef");
			Scribe_References.Look<Site>(ref this.site, "site", false);
			Scribe_References.Look<MapParent>(ref this.useMapParentThreatPoints, "useMapParentThreatPoints", false);
			Scribe_Values.Look<float>(ref this.threatPointsFactor, "threatPointsFactor", 0f, false);
		}

		// Token: 0x040070F2 RID: 28914
		public string inSignal;

		// Token: 0x040070F3 RID: 28915
		public SitePartDef sitePartDef;

		// Token: 0x040070F4 RID: 28916
		public Site site;

		// Token: 0x040070F5 RID: 28917
		public MapParent useMapParentThreatPoints;

		// Token: 0x040070F6 RID: 28918
		public float threatPointsFactor = 1f;
	}
}
