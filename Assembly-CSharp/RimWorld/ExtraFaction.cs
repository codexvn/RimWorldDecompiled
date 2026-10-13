using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C43 RID: 7235
	public class ExtraFaction : IExposable
	{
		// Token: 0x0600A109 RID: 41225 RVA: 0x000024DF File Offset: 0x000006DF
		public ExtraFaction()
		{
		}

		// Token: 0x0600A10A RID: 41226 RVA: 0x002E576C File Offset: 0x002E396C
		public ExtraFaction(Faction faction, ExtraFactionType factionType)
		{
			this.faction = faction;
			this.factionType = factionType;
		}

		// Token: 0x0600A10B RID: 41227 RVA: 0x002E5782 File Offset: 0x002E3982
		public void ExposeData()
		{
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<ExtraFactionType>(ref this.factionType, "factionType", ExtraFactionType.HomeFaction, false);
		}

		// Token: 0x04006F53 RID: 28499
		public Faction faction;

		// Token: 0x04006F54 RID: 28500
		public ExtraFactionType factionType;
	}
}
