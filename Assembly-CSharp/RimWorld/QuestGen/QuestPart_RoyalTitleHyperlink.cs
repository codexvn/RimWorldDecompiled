using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003C08 RID: 15368
	public class QuestPart_RoyalTitleHyperlink : QuestPart
	{
		// Token: 0x17003389 RID: 13193
		// (get) Token: 0x06014D7B RID: 85371 RVA: 0x0063E54A File Offset: 0x0063C74A
		public override IEnumerable<Dialog_InfoCard.Hyperlink> Hyperlinks
		{
			get
			{
				if (this.titleDef != null && this.factionDef != null)
				{
					Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
					yield return new Dialog_InfoCard.Hyperlink(this.titleDef, faction, -1);
				}
				yield break;
			}
		}

		// Token: 0x06014D7C RID: 85372 RVA: 0x0063E55A File Offset: 0x0063C75A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<RoyalTitleDef>(ref this.titleDef, "titleDef");
			Scribe_Defs.Look<FactionDef>(ref this.factionDef, "factionDef");
		}

		// Token: 0x0400E863 RID: 59491
		public RoyalTitleDef titleDef;

		// Token: 0x0400E864 RID: 59492
		public FactionDef factionDef;
	}
}
