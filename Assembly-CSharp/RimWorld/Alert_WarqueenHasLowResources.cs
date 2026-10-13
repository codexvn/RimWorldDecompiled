using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003045 RID: 12357
	public class Alert_WarqueenHasLowResources : Alert
	{
		// Token: 0x060115D3 RID: 71123 RVA: 0x004FB52A File Offset: 0x004F972A
		public Alert_WarqueenHasLowResources()
		{
			this.requireBiotech = true;
		}

		// Token: 0x060115D4 RID: 71124 RVA: 0x004FB544 File Offset: 0x004F9744
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				CompMechCarrier compMechCarrier;
				if (pawn.IsColonyMech && pawn.TryGetComp(out compMechCarrier) && compMechCarrier.LowIngredientCount)
				{
					this.targets.Add(pawn);
				}
			}
		}

		// Token: 0x060115D5 RID: 71125 RVA: 0x004FB5CC File Offset: 0x004F97CC
		public override string GetLabel()
		{
			string result;
			if ((result = this.defaultLabel) == null)
			{
				result = (this.defaultLabel = "AlertWarqueenHasLowResources".Translate(PawnKindDefOf.Mech_Warqueen.LabelCap));
			}
			return result;
		}

		// Token: 0x060115D6 RID: 71126 RVA: 0x004FB60C File Offset: 0x004F980C
		public override TaggedString GetExplanation()
		{
			string str;
			if ((str = this.defaultExplanation) == null)
			{
				str = (this.defaultExplanation = "AlertWarqueenHasLowResourcesDesc".Translate(PawnKindDefOf.Mech_Warqueen.labelPlural));
			}
			return str;
		}

		// Token: 0x060115D7 RID: 71127 RVA: 0x004FB64F File Offset: 0x004F984F
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3D0 RID: 46032
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();
	}
}
