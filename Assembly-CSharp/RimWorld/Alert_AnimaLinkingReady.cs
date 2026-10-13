using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200303B RID: 12347
	public class Alert_AnimaLinkingReady : Alert
	{
		// Token: 0x060115AB RID: 71083 RVA: 0x004FA678 File Offset: 0x004F8878
		public Alert_AnimaLinkingReady()
		{
			this.defaultLabel = "AnimaLinkingReadyLabel".Translate();
			this.requireRoyalty = true;
		}

		// Token: 0x060115AC RID: 71084 RVA: 0x004FA6C8 File Offset: 0x004F88C8
		private void GetTargets()
		{
			this.culprits.Clear();
			this.targetLabels.Clear();
			this.tempPawns.Clear();
			foreach (LordJob_Ritual lordJob_Ritual in Find.IdeoManager.GetActiveRituals(Find.CurrentMap))
			{
				if (lordJob_Ritual.Ritual != null && lordJob_Ritual.Ritual.def == PreceptDefOf.AnimaTreeLinking)
				{
					return;
				}
			}
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonistsSpawned)
			{
				if (pawn.GetPsylinkLevel() < pawn.GetMaxPsylinkLevel() && MeditationFocusDefOf.Natural.CanPawnUse(pawn) && pawn.psychicEntropy.IsPsychicallySensitive && (!ModsConfig.BiotechActive || !pawn.DevelopmentalStage.Juvenile()))
				{
					this.tempPawns.Add(pawn);
				}
			}
			foreach (Thing thing in Find.CurrentMap.listerThings.ThingsOfDef(ThingDefOf.Plant_TreeAnima))
			{
				CompPsylinkable compPsylinkable = thing.TryGetComp<CompPsylinkable>();
				int count = compPsylinkable.CompSubplant.SubplantsForReading.Count;
				bool flag = false;
				foreach (Pawn pawn2 in this.tempPawns)
				{
					if (compPsylinkable.GetRequiredPlantCount(pawn2) <= count)
					{
						if (!this.culprits.Contains(pawn2))
						{
							this.culprits.Add(pawn2);
							this.targetLabels.Add(pawn2.NameFullColored);
						}
						flag = true;
					}
				}
				if (flag)
				{
					this.culprits.Add(thing);
				}
			}
		}

		// Token: 0x060115AD RID: 71085 RVA: 0x004FA8EC File Offset: 0x004F8AEC
		public override TaggedString GetExplanation()
		{
			return "AnimaLinkingReadyDesc".Translate() + ":\n\n" + this.targetLabels.ToLineList("  - ") + "\n\n" + "AnimaLinkingReadyDescExtra".Translate();
		}

		// Token: 0x060115AE RID: 71086 RVA: 0x004FA93B File Offset: 0x004F8B3B
		public override AlertReport GetReport()
		{
			if (Find.CurrentMap == null)
			{
				return AlertReport.Inactive;
			}
			this.GetTargets();
			return AlertReport.CulpritsAre(this.culprits);
		}

		// Token: 0x0400B3B8 RID: 46008
		private List<Thing> culprits = new List<Thing>();

		// Token: 0x0400B3B9 RID: 46009
		private List<string> targetLabels = new List<string>();

		// Token: 0x0400B3BA RID: 46010
		private List<Pawn> tempPawns = new List<Pawn>();
	}
}
