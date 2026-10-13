using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D98 RID: 7576
	public class IncidentWorker_FarmAnimalsWanderIn : IncidentWorker
	{
		// Token: 0x0600A97D RID: 43389 RVA: 0x003081A2 File Offset: 0x003063A2
		public override float ChanceFactorNow(IIncidentTarget target)
		{
			if (!ModsConfig.IdeologyActive || !IdeoUtility.AnyColonistWithRanchingIssue())
			{
				return base.ChanceFactorNow(target);
			}
			return IncidentWorker_FarmAnimalsWanderIn.BaseChanceFactorByAnimalBodySizePerCapitaCurve.Evaluate(PawnUtility.PlayerAnimalBodySizePerCapita()) * base.ChanceFactorNow(target);
		}

		// Token: 0x0600A97E RID: 43390 RVA: 0x003081D4 File Offset: 0x003063D4
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!base.CanFireNowSub(parms))
			{
				return false;
			}
			Map map = (Map)parms.target;
			IntVec3 intVec;
			PawnKindDef pawnKindDef;
			return RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null) && this.TryFindRandomPawnKind(map, out pawnKindDef);
		}

		// Token: 0x0600A97F RID: 43391 RVA: 0x00308214 File Offset: 0x00306414
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 intVec;
			if (!RCellFinder.TryFindRandomPawnEntryCell(out intVec, map, CellFinder.EdgeRoadChance_Animal, false, null))
			{
				return false;
			}
			PawnKindDef pawnKindDef;
			if (!this.TryFindRandomPawnKind(map, out pawnKindDef))
			{
				return false;
			}
			int num = Mathf.Clamp(GenMath.RoundRandom(((parms.totalBodySize > 0f) ? parms.totalBodySize : 2.5f) / pawnKindDef.RaceProps.baseBodySize), 2, 10);
			if (num >= 2)
			{
				this.SpawnAnimal(intVec, map, pawnKindDef, new Gender?(Gender.Female));
				this.SpawnAnimal(intVec, map, pawnKindDef, new Gender?(Gender.Male));
				num -= 2;
			}
			for (int i = 0; i < num; i++)
			{
				this.SpawnAnimal(intVec, map, pawnKindDef, null);
			}
			base.SendStandardLetter(parms.customLetterLabel ?? "LetterLabelFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)).CapitalizeFirst(), parms.customLetterText ?? "LetterFarmAnimalsWanderIn".Translate(pawnKindDef.GetLabelPlural(-1)), LetterDefOf.PositiveEvent, parms, new TargetInfo(intVec, map, false), Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A980 RID: 43392 RVA: 0x00308348 File Offset: 0x00306548
		private void SpawnAnimal(IntVec3 location, Map map, PawnKindDef pawnKind, Gender? gender = null)
		{
			IntVec3 loc = CellFinder.RandomClosewalkCellNear(location, map, 12, null);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(pawnKind, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, gender, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			GenSpawn.Spawn(pawn, loc, map, Rot4.Random, WipeMode.Vanish, false, false);
			pawn.SetFaction(Faction.OfPlayer, null);
		}

		// Token: 0x0600A981 RID: 43393 RVA: 0x00308408 File Offset: 0x00306608
		private bool TryFindRandomPawnKind(Map map, out PawnKindDef kind)
		{
			return (from x in DefDatabase<PawnKindDef>.AllDefs
			where x.RaceProps.Animal && x.race.GetStatValueAbstract(StatDefOf.Wildness, null) < 0.35f && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(x.race, 0f) && !x.race.tradeTags.NullOrEmpty<string>() && x.race.tradeTags.Contains("AnimalFarm") && !x.RaceProps.Dryad && !x.RaceProps.neverIncludeInQuests
			select x).TryRandomElementByWeight((PawnKindDef k) => this.SelectionChance(k), out kind);
		}

		// Token: 0x0600A982 RID: 43394 RVA: 0x00308454 File Offset: 0x00306654
		private float SelectionChance(PawnKindDef pawnKind)
		{
			float num = 0.42000002f - pawnKind.race.GetStatValueAbstract(StatDefOf.Wildness, null);
			if (PawnUtility.PlayerHasReproductivePair(pawnKind))
			{
				num *= 0.5f;
			}
			return num;
		}

		// Token: 0x04007539 RID: 30009
		private const string FarmAnimalTradeTag = "AnimalFarm";

		// Token: 0x0400753A RID: 30010
		private const float MaxWildness = 0.35f;

		// Token: 0x0400753B RID: 30011
		private const float TotalBodySizeToSpawn = 2.5f;

		// Token: 0x0400753C RID: 30012
		private static SimpleCurve BaseChanceFactorByAnimalBodySizePerCapitaCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1.5f),
				true
			},
			{
				new CurvePoint(4f, 1f),
				true
			}
		};

		// Token: 0x0400753D RID: 30013
		private const float SelectionChanceFactorIfExistingMatingPair = 0.5f;
	}
}
