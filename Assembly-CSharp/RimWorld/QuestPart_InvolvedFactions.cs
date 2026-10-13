using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C53 RID: 7251
	public class QuestPart_InvolvedFactions : QuestPart
	{
		// Token: 0x170018D6 RID: 6358
		// (get) Token: 0x0600A17E RID: 41342 RVA: 0x002E6B73 File Offset: 0x002E4D73
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				foreach (Faction faction2 in this.factions)
				{
					yield return faction2;
				}
				List<Faction>.Enumerator enumerator2 = default(List<Faction>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x0600A17F RID: 41343 RVA: 0x002E6B83 File Offset: 0x002E4D83
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Faction>(ref this.factions, "factions", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04006F97 RID: 28567
		public List<Faction> factions = new List<Faction>();
	}
}
