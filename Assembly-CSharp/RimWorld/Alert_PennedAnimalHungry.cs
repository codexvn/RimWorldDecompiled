using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003032 RID: 12338
	public class Alert_PennedAnimalHungry : Alert
	{
		// Token: 0x06011586 RID: 71046 RVA: 0x004F9C38 File Offset: 0x004F7E38
		public Alert_PennedAnimalHungry()
		{
			this.defaultLabel = "AlertPennedAnimalHungry".Translate();
		}

		// Token: 0x06011587 RID: 71047 RVA: 0x004F9C6C File Offset: 0x004F7E6C
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnNames.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn) && pawn.needs.food != null && pawn.needs.food.TicksStarving > 2500 && AnimalPenUtility.GetCurrentPenOf(pawn, false) != null)
				{
					this.targets.Add(pawn);
					this.pawnNames.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x06011588 RID: 71048 RVA: 0x004F9D34 File Offset: 0x004F7F34
		public override TaggedString GetExplanation()
		{
			return "AlertPennedAnimalHungryDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x06011589 RID: 71049 RVA: 0x004F9D55 File Offset: 0x004F7F55
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3A3 RID: 45987
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3A4 RID: 45988
		private List<string> pawnNames = new List<string>();

		// Token: 0x0400B3A5 RID: 45989
		public const int TicksStarvingThresh = 2500;
	}
}
