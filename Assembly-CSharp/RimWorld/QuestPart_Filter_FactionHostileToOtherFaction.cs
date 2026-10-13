using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BB6 RID: 7094
	public class QuestPart_Filter_FactionHostileToOtherFaction : QuestPart_Filter
	{
		// Token: 0x06009E00 RID: 40448 RVA: 0x002DBC28 File Offset: 0x002D9E28
		protected override bool Pass(SignalArgs args)
		{
			return this.faction != null && this.other != null && this.faction != this.other && this.faction.HostileTo(this.other);
		}

		// Token: 0x06009E01 RID: 40449 RVA: 0x002DBC5B File Offset: 0x002D9E5B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Faction>(ref this.other, "other", false);
		}

		// Token: 0x04006D6B RID: 28011
		public Faction faction;

		// Token: 0x04006D6C RID: 28012
		public Faction other;
	}
}
