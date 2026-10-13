using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A36 RID: 14902
	public class QuestNode_GetAnimalToHunt : QuestNode
	{
		// Token: 0x06014658 RID: 83544 RVA: 0x006180E6 File Offset: 0x006162E6
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x06014659 RID: 83545 RVA: 0x006180EF File Offset: 0x006162EF
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0601465A RID: 83546 RVA: 0x00618100 File Offset: 0x00616300
		private bool DoWork(Slate slate)
		{
			Map map = slate.Get<Map>("map", null, false);
			if (map == null)
			{
				return false;
			}
			float x2 = slate.Get<float>("points", 0f, false);
			float animalDifficultyFromPoints = this.pointsToAnimalDifficultyCurve.GetValue(slate).Evaluate(x2);
			PawnKindDef pawnKindDef;
			if (!map.Biome.AllWildAnimals.Where(delegate(PawnKindDef x)
			{
				if (map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race, 0f))
				{
					return map.listerThings.ThingsOfDef(x.race).Any((Thing p) => p.Faction == null);
				}
				return false;
			}).TryRandomElementByWeight((PawnKindDef x) => this.AnimalCommonalityByDifficulty(x, animalDifficultyFromPoints), out pawnKindDef))
			{
				return false;
			}
			int num = 0;
			for (int i = 0; i < map.mapPawns.AllPawnsSpawned.Count; i++)
			{
				Pawn pawn = map.mapPawns.AllPawnsSpawned[i];
				if (pawn.def == pawnKindDef.race && !pawn.IsQuestLodger() && pawn.Faction == null)
				{
					num++;
				}
			}
			SimpleCurve value = this.pointsToAnimalsToHuntCountCurve.GetValue(slate);
			float randomInRange = (this.animalsToHuntCountRandomFactorRange.GetValue(slate) ?? FloatRange.One).RandomInRange;
			int num2 = Mathf.RoundToInt(value.Evaluate(x2) * randomInRange);
			num2 = Mathf.Min(num2, num);
			num2 = Mathf.Max(num2, 1);
			slate.Set<ThingDef>(this.storeAnimalToHuntAs.GetValue(slate), pawnKindDef.race, false);
			slate.Set<int>(this.storeCountToHuntAs.GetValue(slate), num2, false);
			return true;
		}

		// Token: 0x0601465B RID: 83547 RVA: 0x0061828C File Offset: 0x0061648C
		private float AnimalCommonalityByDifficulty(PawnKindDef animalKind, float animalDifficultyFromPoints)
		{
			float num = Mathf.Abs(animalKind.GetAnimalPointsToHuntOrSlaughter() - animalDifficultyFromPoints);
			return 1f / num;
		}

		// Token: 0x0400E16F RID: 57711
		[NoTranslate]
		public SlateRef<string> storeAnimalToHuntAs;

		// Token: 0x0400E170 RID: 57712
		[NoTranslate]
		public SlateRef<string> storeCountToHuntAs;

		// Token: 0x0400E171 RID: 57713
		public SlateRef<SimpleCurve> pointsToAnimalsToHuntCountCurve;

		// Token: 0x0400E172 RID: 57714
		public SlateRef<SimpleCurve> pointsToAnimalDifficultyCurve;

		// Token: 0x0400E173 RID: 57715
		public SlateRef<FloatRange?> animalsToHuntCountRandomFactorRange;
	}
}
