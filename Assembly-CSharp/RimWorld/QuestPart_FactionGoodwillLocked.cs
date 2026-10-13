using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BC5 RID: 7109
	public class QuestPart_FactionGoodwillLocked : QuestPartActivable
	{
		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x06009E41 RID: 40513 RVA: 0x002DCAAD File Offset: 0x002DACAD
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction1 != null)
				{
					yield return this.faction1;
				}
				if (this.faction2 != null)
				{
					yield return this.faction2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06009E42 RID: 40514 RVA: 0x002DCABD File Offset: 0x002DACBD
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (faction == this.faction1)
			{
				this.faction1 = null;
			}
			if (faction == this.faction2)
			{
				this.faction2 = null;
			}
		}

		// Token: 0x06009E43 RID: 40515 RVA: 0x002DCADF File Offset: 0x002DACDF
		public bool AppliesTo(Faction a, Faction b)
		{
			return base.State == QuestPartState.Enabled && ((this.faction1 == a && this.faction2 == b) || (this.faction1 == b && this.faction2 == a));
		}

		// Token: 0x06009E44 RID: 40516 RVA: 0x002DCB14 File Offset: 0x002DAD14
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
		}

		// Token: 0x04006D92 RID: 28050
		public Faction faction1;

		// Token: 0x04006D93 RID: 28051
		public Faction faction2;
	}
}
