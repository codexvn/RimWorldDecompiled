using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200302B RID: 12331
	public class Alert_UnusableMeditationFocus : Alert
	{
		// Token: 0x17002CF4 RID: 11508
		// (get) Token: 0x06011569 RID: 71017 RVA: 0x004F8FE0 File Offset: 0x004F71E0
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.targets.Clear();
				this.pawnEntries.Clear();
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					if (pawn.timetable != null && pawn.timetable.CurrentAssignment == TimeAssignmentDefOf.Meditate && pawn.psychicEntropy.IsCurrentlyMeditating && !MeditationFocusDefOf.Natural.CanPawnUse(pawn))
					{
						JobDriver_Meditate jobDriver_Meditate = pawn.jobs.curDriver as JobDriver_Meditate;
						if (jobDriver_Meditate != null && !(jobDriver_Meditate.Focus != null) && !(jobDriver_Meditate is JobDriver_Reign))
						{
							foreach (Thing thing in GenRadial.RadialDistinctThingsAround(pawn.Position, pawn.Map, MeditationUtility.FocusObjectSearchRadius, false))
							{
								if (thing.def == ThingDefOf.Plant_TreeAnima || thing.def == ThingDefOf.AnimusStone || thing.def == ThingDefOf.NatureShrine_Small || thing.def == ThingDefOf.NatureShrine_Large)
								{
									this.targets.Add(pawn);
									this.pawnEntries.Add(pawn.NameShortColored.Resolve() + " (" + thing.LabelShort + ")");
									break;
								}
							}
						}
					}
				}
				return this.targets;
			}
		}

		// Token: 0x0601156A RID: 71018 RVA: 0x004F91A8 File Offset: 0x004F73A8
		public Alert_UnusableMeditationFocus()
		{
			this.defaultLabel = "UnusableMeditationFocusAlert".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x0601156B RID: 71019 RVA: 0x004F91E2 File Offset: 0x004F73E2
		public override TaggedString GetExplanation()
		{
			return "UnusableMeditationFocusAlertDesc".Translate(this.pawnEntries.ToLineList("  - "));
		}

		// Token: 0x0601156C RID: 71020 RVA: 0x004F9203 File Offset: 0x004F7403
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x0400B394 RID: 45972
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B395 RID: 45973
		private List<string> pawnEntries = new List<string>();

		// Token: 0x0200302C RID: 12332
		public class Alert_PermitAvailable : Alert
		{
			// Token: 0x0601156D RID: 71021 RVA: 0x004F9210 File Offset: 0x004F7410
			public Alert_PermitAvailable()
			{
				this.defaultLabel = "PermitChoiceReadyAlert".Translate();
				this.requireRoyalty = true;
			}

			// Token: 0x0601156E RID: 71022 RVA: 0x004F9240 File Offset: 0x004F7440
			private void CalculateTargets()
			{
				this.targets.Clear();
				this.target = null;
				foreach (Pawn pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
				{
					if (pawn.royalty != null && pawn.royalty.PermitPointsAvailable)
					{
						if (this.target == null)
						{
							this.target = pawn;
						}
						this.targets.Add(pawn);
					}
				}
			}

			// Token: 0x0601156F RID: 71023 RVA: 0x004F92D4 File Offset: 0x004F74D4
			public override AlertReport GetReport()
			{
				this.CalculateTargets();
				return AlertReport.CulpritsAre(this.targets);
			}

			// Token: 0x06011570 RID: 71024 RVA: 0x004F92E7 File Offset: 0x004F74E7
			public override TaggedString GetExplanation()
			{
				return "PermitChoiceReadyAlertDesc".Translate(this.target.Named("PAWN"));
			}

			// Token: 0x06011571 RID: 71025 RVA: 0x004F9303 File Offset: 0x004F7503
			protected override void OnClick()
			{
				base.OnClick();
				Pawn pawn = this.target;
				if (pawn == null)
				{
					return;
				}
				Pawn_RoyaltyTracker royalty = pawn.royalty;
				if (royalty == null)
				{
					return;
				}
				royalty.OpenPermitWindow();
			}

			// Token: 0x0400B396 RID: 45974
			private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

			// Token: 0x0400B397 RID: 45975
			private Pawn target;
		}
	}
}
