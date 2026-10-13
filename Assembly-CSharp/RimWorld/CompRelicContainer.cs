using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E90 RID: 11920
	public class CompRelicContainer : CompThingContainer
	{
		// Token: 0x06010CA1 RID: 68769 RVA: 0x004D585F File Offset: 0x004D3A5F
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckIdeology("Reliqary"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
		}

		// Token: 0x06010CA2 RID: 68770 RVA: 0x004D5881 File Offset: 0x004D3A81
		public static bool IsRelic(Thing thing)
		{
			return thing.IsRelic();
		}

		// Token: 0x06010CA3 RID: 68771 RVA: 0x004D5889 File Offset: 0x004D3A89
		public override bool Accepts(Thing thing)
		{
			return CompRelicContainer.IsRelic(thing);
		}

		// Token: 0x06010CA4 RID: 68772 RVA: 0x00002501 File Offset: 0x00000701
		public override bool Accepts(ThingDef thingDef)
		{
			return false;
		}

		// Token: 0x06010CA5 RID: 68773 RVA: 0x004D5894 File Offset: 0x004D3A94
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!base.Empty)
			{
				text = text + ("\n" + "StatsReport_RelicAtRitualMoodBonus".Translate() + ": ") + ThoughtDefOf.RelicAtRitual.stages[0].baseMoodEffect.ToString();
			}
			return text;
		}
	}
}
