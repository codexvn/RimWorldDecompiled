using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C9D RID: 7325
	public class QuestPart_ReserveFaction : QuestPart
	{
		// Token: 0x0600A333 RID: 41779 RVA: 0x002EDDD3 File Offset: 0x002EBFD3
		public override bool QuestPartReserves(Faction f)
		{
			return f != null && f == this.faction;
		}

		// Token: 0x0600A334 RID: 41780 RVA: 0x002EDDE3 File Offset: 0x002EBFE3
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x0600A335 RID: 41781 RVA: 0x002EDDF5 File Offset: 0x002EBFF5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
		}

		// Token: 0x040070CB RID: 28875
		public Faction faction;
	}
}
