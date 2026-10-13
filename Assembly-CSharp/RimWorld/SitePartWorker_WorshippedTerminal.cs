using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002861 RID: 10337
	public class SitePartWorker_WorshippedTerminal : SitePartWorker
	{
		// Token: 0x0600E200 RID: 57856 RVA: 0x0042347C File Offset: 0x0042167C
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.things = new ThingOwner<Thing>(sitePart);
			Thing item = ThingMaker.MakeThing(ThingDefOf.AncientTerminal_Worshipful, null);
			sitePart.things.TryAdd(item, true);
		}

		// Token: 0x0600E201 RID: 57857 RVA: 0x004234B8 File Offset: 0x004216B8
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			return base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets).Formatted(map.Parent.GetComponent<TimedMakeFactionHostile>().TicksLeft.Value.ToStringTicksToPeriod(true, false, true, true, false).Named("TIMER"));
		}
	}
}
