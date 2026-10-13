using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FD1 RID: 12241
	public class Alert_MeatHunger : Alert_Critical
	{
		// Token: 0x06011409 RID: 70665 RVA: 0x004F188C File Offset: 0x004EFA8C
		public Alert_MeatHunger()
		{
			this.defaultLabel = "AlertMeatHunger".Translate();
			this.defaultPriority = AlertPriority.High;
			this.requireAnomaly = true;
		}

		// Token: 0x17002CC1 RID: 11457
		// (get) Token: 0x0601140A RID: 70666 RVA: 0x004F18D8 File Offset: 0x004EFAD8
		private List<Pawn> MeatHungerPawns
		{
			get
			{
				this.targets.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					Pawn pawn = list[i];
					if (pawn.IsGhoul)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (((needs != null) ? needs.food : null) != null && pawn.needs.food.Starving)
						{
							this.targets.Add(pawn);
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0601140B RID: 70667 RVA: 0x004F1954 File Offset: 0x004EFB54
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.targets)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return string.Format("{0}:\n{1}\n\n{2}", "AlertMeatHungerDesc".Translate(), this.sb.ToString().TrimEndNewlines(), "AlertMeatHungerDescAppended".Translate());
		}

		// Token: 0x0601140C RID: 70668 RVA: 0x004F1A10 File Offset: 0x004EFC10
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.MeatHungerPawns);
		}

		// Token: 0x0400B2FF RID: 45823
		private List<Pawn> targets = new List<Pawn>();

		// Token: 0x0400B300 RID: 45824
		private StringBuilder sb = new StringBuilder();
	}
}
