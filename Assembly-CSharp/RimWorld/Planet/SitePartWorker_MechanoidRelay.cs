using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DF4 RID: 15860
	public class SitePartWorker_MechanoidRelay : SitePartWorker
	{
		// Token: 0x06015A2F RID: 88623 RVA: 0x0067A19C File Offset: 0x0067839C
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.things = new ThingOwner<Thing>(sitePart);
			sitePart.things.TryAdd(ThingMaker.MakeThing(ThingDefOf.MechRelay, null), true);
			for (int i = 0; i < sitePart.parms.stabilizerCount; i++)
			{
				sitePart.things.TryAdd(ThingMaker.MakeThing(ThingDefOf.MechStabilizer, null), true);
			}
		}
	}
}
