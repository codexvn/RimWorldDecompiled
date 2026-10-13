using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FE3 RID: 12259
	public class Alert_HypothermicAnimals : Alert
	{
		// Token: 0x17002CC9 RID: 11465
		// (get) Token: 0x06011453 RID: 70739 RVA: 0x004F2958 File Offset: 0x004F0B58
		private List<Pawn> HypothermicAnimals
		{
			get
			{
				this.hypothermicAnimalsResult.Clear();
				IReadOnlyList<Pawn> allMaps_Spawned = PawnsFinder.AllMaps_Spawned;
				for (int i = 0; i < allMaps_Spawned.Count; i++)
				{
					if (allMaps_Spawned[i].IsAnimal && allMaps_Spawned[i].Faction == null && allMaps_Spawned[i].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false) != null)
					{
						this.hypothermicAnimalsResult.Add(allMaps_Spawned[i]);
					}
				}
				return this.hypothermicAnimalsResult;
			}
		}

		// Token: 0x06011454 RID: 70740 RVA: 0x004F29D9 File Offset: 0x004F0BD9
		public override string GetLabel()
		{
			return "Hypothermic wild animals (debug)";
		}

		// Token: 0x06011455 RID: 70741 RVA: 0x004F29E0 File Offset: 0x004F0BE0
		public override TaggedString GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Debug alert:\n\nThese wild animals are hypothermic. This may indicate a bug (but it may not, if the animals are trapped or in some other wierd but legitimate situation):");
			foreach (Pawn pawn in this.HypothermicAnimals)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				string str = "    ";
				Pawn pawn2 = pawn;
				stringBuilder2.AppendLine(str + ((pawn2 != null) ? pawn2.ToString() : null) + " at " + pawn.Position.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06011456 RID: 70742 RVA: 0x004F2A80 File Offset: 0x004F0C80
		public override AlertReport GetReport()
		{
			if (!Prefs.DevMode)
			{
				return false;
			}
			return AlertReport.CulpritsAre(this.HypothermicAnimals);
		}

		// Token: 0x0400B31C RID: 45852
		private List<Pawn> hypothermicAnimalsResult = new List<Pawn>();
	}
}
