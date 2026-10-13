using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200301D RID: 12317
	public class Alert_AwaitingMedicalOperation : Alert
	{
		// Token: 0x17002CE6 RID: 11494
		// (get) Token: 0x06011538 RID: 70968 RVA: 0x004F8064 File Offset: 0x004F6264
		private List<Pawn> AwaitingMedicalOperation
		{
			get
			{
				this.awaitingMedicalOperationResult.Clear();
				List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<get_AwaitingMedicalOperation>g__IsAwaiting|2_0(list[i]))
					{
						this.awaitingMedicalOperationResult.Add(list[i]);
					}
				}
				List<Pawn> allMaps_PrisonersOfColonySpawned = PawnsFinder.AllMaps_PrisonersOfColonySpawned;
				for (int j = 0; j < allMaps_PrisonersOfColonySpawned.Count; j++)
				{
					if (this.<get_AwaitingMedicalOperation>g__IsAwaiting|2_0(allMaps_PrisonersOfColonySpawned[j]) && (!ModsConfig.BiotechActive || allMaps_PrisonersOfColonySpawned[j].health.surgeryBills.Count != 1 || allMaps_PrisonersOfColonySpawned[j].health.surgeryBills[0].recipe != RecipeDefOf.ExtractHemogenPack || allMaps_PrisonersOfColonySpawned[j].guest.IsInteractionDisabled(PrisonerInteractionModeDefOf.HemogenFarm)))
					{
						this.awaitingMedicalOperationResult.Add(allMaps_PrisonersOfColonySpawned[j]);
					}
				}
				return this.awaitingMedicalOperationResult;
			}
		}

		// Token: 0x06011539 RID: 70969 RVA: 0x004F8158 File Offset: 0x004F6358
		public override string GetLabel()
		{
			return "PatientsAwaitingMedicalOperation".Translate(this.awaitingMedicalOperationResult.Count.ToStringCached());
		}

		// Token: 0x0601153A RID: 70970 RVA: 0x004F8180 File Offset: 0x004F6380
		public override TaggedString GetExplanation()
		{
			this.sb.Length = 0;
			foreach (Pawn pawn in this.awaitingMedicalOperationResult)
			{
				this.sb.AppendLine("  - " + pawn.NameShortColored.Resolve());
			}
			return "PatientsAwaitingMedicalOperationDesc".Translate(this.sb.ToString().TrimEndNewlines());
		}

		// Token: 0x0601153B RID: 70971 RVA: 0x004F821C File Offset: 0x004F641C
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.AwaitingMedicalOperation);
		}

		// Token: 0x0601153D RID: 70973 RVA: 0x004F8247 File Offset: 0x004F6447
		[CompilerGenerated]
		private bool <get_AwaitingMedicalOperation>g__IsAwaiting|2_0(Pawn p)
		{
			return HealthAIUtility.ShouldHaveSurgeryDoneNow(p) && p.InBed() && !this.awaitingMedicalOperationResult.Contains(p);
		}

		// Token: 0x0400B382 RID: 45954
		private List<Pawn> awaitingMedicalOperationResult = new List<Pawn>();

		// Token: 0x0400B383 RID: 45955
		private StringBuilder sb = new StringBuilder();
	}
}
