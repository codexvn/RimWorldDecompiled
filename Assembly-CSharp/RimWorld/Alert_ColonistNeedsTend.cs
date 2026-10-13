using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02002FFC RID: 12284
	public class Alert_ColonistNeedsTend : Alert
	{
		// Token: 0x060114B6 RID: 70838 RVA: 0x004F51B4 File Offset: 0x004F33B4
		public Alert_ColonistNeedsTend()
		{
			this.defaultLabel = "ColonistNeedsTreatment".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x17002CCF RID: 11471
		// (get) Token: 0x060114B7 RID: 70839 RVA: 0x004F51F0 File Offset: 0x004F33F0
		private List<Pawn> NeedingColonists
		{
			get
			{
				this.needingColonistsResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if ((pawn.Spawned || pawn.BrieflyDespawned()) && pawn.health.HasHediffsNeedingTendByPlayer(true))
					{
						Building_Bed building_Bed = pawn.CurrentBed();
						if ((building_Bed == null || !building_Bed.Medical) && !Alert_ColonistNeedsRescuing.NeedsRescue(pawn) && !ChildcareUtility.BabyBeingPlayedWith(pawn))
						{
							this.needingColonistsResult.Add(pawn);
						}
					}
				}
				return this.needingColonistsResult;
			}
		}

		// Token: 0x060114B8 RID: 70840 RVA: 0x004F5298 File Offset: 0x004F3498
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.needingColonistsResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				return "ColonistNeedsTreatmentDescController".Translate(this.sb.ToString().TrimEndNewlines());
			}
			return "ColonistNeedsTreatmentDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x060114B9 RID: 70841 RVA: 0x004F535C File Offset: 0x004F355C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.NeedingColonists);
		}

		// Token: 0x0400B34A RID: 45898
		private List<Pawn> needingColonistsResult = new List<Pawn>();

		// Token: 0x0400B34B RID: 45899
		private StringBuilder sb = new StringBuilder();
	}
}
