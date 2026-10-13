using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02002FC7 RID: 12231
	public class Alert_ColonistNeedsRescuing : Alert_Critical
	{
		// Token: 0x17002CB1 RID: 11441
		// (get) Token: 0x060113D3 RID: 70611 RVA: 0x004F0690 File Offset: 0x004EE890
		private List<Pawn> ColonistsNeedingRescue
		{
			get
			{
				this.colonistsNeedingRescueResult.Clear();
				foreach (Pawn pawn in PawnsFinder.AllMaps_FreeColonistsSpawned)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(pawn))
					{
						this.colonistsNeedingRescueResult.Add(pawn);
					}
				}
				foreach (Pawn pawn2 in PawnsFinder.AllMaps_ColonySubhumansSpawnedPlayerControlled)
				{
					if (Alert_ColonistNeedsRescuing.NeedsRescue(pawn2))
					{
						this.colonistsNeedingRescueResult.Add(pawn2);
					}
				}
				return this.colonistsNeedingRescueResult;
			}
		}

		// Token: 0x060113D4 RID: 70612 RVA: 0x004F0750 File Offset: 0x004EE950
		public static bool NeedsRescue(Pawn p)
		{
			if (p.Downed && HealthAIUtility.WantsToBeRescued(p) && !p.InBed() && !(p.ParentHolder is Pawn_CarryTracker))
			{
				Pawn_JobTracker jobs = p.jobs;
				return (((jobs != null) ? jobs.jobQueue : null) == null || p.jobs.jobQueue.Count <= 0 || !p.jobs.jobQueue.Peek().job.CanBeginNow(p, false)) && !ChildcareUtility.BabyBeingPlayedWith(p);
			}
			return false;
		}

		// Token: 0x060113D5 RID: 70613 RVA: 0x004F07D6 File Offset: 0x004EE9D6
		public override string GetLabel()
		{
			if (this.colonistsNeedingRescueResult.Count == 1)
			{
				return "ColonistNeedsRescue".Translate();
			}
			return "ColonistsNeedRescue".Translate();
		}

		// Token: 0x060113D6 RID: 70614 RVA: 0x004F0808 File Offset: 0x004EEA08
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.colonistsNeedingRescueResult)
			{
				stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				return "ColonistsNeedRescueDescController".Translate(stringBuilder.ToString());
			}
			return "ColonistsNeedRescueDesc".Translate(stringBuilder.ToString());
		}

		// Token: 0x060113D7 RID: 70615 RVA: 0x004F08AC File Offset: 0x004EEAAC
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ColonistsNeedingRescue);
		}

		// Token: 0x0400B2F3 RID: 45811
		private List<Pawn> colonistsNeedingRescueResult = new List<Pawn>();
	}
}
