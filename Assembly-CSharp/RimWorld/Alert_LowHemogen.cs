using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003046 RID: 12358
	public class Alert_LowHemogen : Alert
	{
		// Token: 0x060115D8 RID: 71128 RVA: 0x004FB662 File Offset: 0x004F9862
		public Alert_LowHemogen()
		{
			this.requireBiotech = true;
			this.defaultLabel = "AlertLowHemogen".Translate();
		}

		// Token: 0x060115D9 RID: 71129 RVA: 0x004FB69C File Offset: 0x004F989C
		public override string GetLabel()
		{
			string text = this.defaultLabel;
			if (this.targets.Count == 1)
			{
				text = text + ": " + this.targetLabels[0];
			}
			return text;
		}

		// Token: 0x060115DA RID: 71130 RVA: 0x004FB6D8 File Offset: 0x004F98D8
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.targetLabels.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravellingTransporters_AliveSpawned)
			{
				if (pawn.genes != null && pawn.RaceProps.Humanlike && pawn.Faction == Faction.OfPlayer)
				{
					Gene_Hemogen firstGeneOfType = pawn.genes.GetFirstGeneOfType<Gene_Hemogen>();
					if (firstGeneOfType != null && firstGeneOfType.Value < firstGeneOfType.MinLevelForAlert)
					{
						this.targets.Add(pawn);
						this.targetLabels.Add(pawn.NameShortColored.Resolve());
					}
				}
			}
		}

		// Token: 0x060115DB RID: 71131 RVA: 0x004FB79C File Offset: 0x004F999C
		public override TaggedString GetExplanation()
		{
			return "AlertLowHemogenDesc".Translate() + ":\n" + this.targetLabels.ToLineList("  - ");
		}

		// Token: 0x060115DC RID: 71132 RVA: 0x004FB7C7 File Offset: 0x004F99C7
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3D1 RID: 46033
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3D2 RID: 46034
		private List<string> targetLabels = new List<string>();
	}
}
