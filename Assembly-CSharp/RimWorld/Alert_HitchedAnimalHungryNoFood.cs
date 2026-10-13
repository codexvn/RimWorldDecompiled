using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02003033 RID: 12339
	public class Alert_HitchedAnimalHungryNoFood : Alert
	{
		// Token: 0x0601158A RID: 71050 RVA: 0x004F9D68 File Offset: 0x004F7F68
		public Alert_HitchedAnimalHungryNoFood()
		{
			this.defaultLabel = "AlertHitchedAnimalHungryNoFood".Translate();
		}

		// Token: 0x0601158B RID: 71051 RVA: 0x004F9D9C File Offset: 0x004F7F9C
		private void CalculateTargets()
		{
			this.targets.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				Pawn_RopeTracker roping = pawn.roping;
				if (roping != null && roping.IsRopedToHitchingPost && pawn.needs.food.TicksStarving > 2500)
				{
					this.targets.Add(pawn);
				}
			}
		}

		// Token: 0x0601158C RID: 71052 RVA: 0x004F9E34 File Offset: 0x004F8034
		public override TaggedString GetExplanation()
		{
			this.animalNames.Clear();
			foreach (GlobalTargetInfo globalTargetInfo in this.targets)
			{
				this.animalNames.Add(((Pawn)globalTargetInfo.Thing).NameShortColored.Resolve());
			}
			return "AlertHitchedAnimalHungryNoFoodDesc".Translate(this.animalNames.ToLineList("  - "));
		}

		// Token: 0x0601158D RID: 71053 RVA: 0x004F9ED0 File Offset: 0x004F80D0
		public override AlertReport GetReport()
		{
			this.CalculateTargets();
			return AlertReport.CulpritsAre(this.targets);
		}

		// Token: 0x0400B3A6 RID: 45990
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x0400B3A7 RID: 45991
		private List<string> animalNames = new List<string>();
	}
}
