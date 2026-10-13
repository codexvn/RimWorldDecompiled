using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002375 RID: 9077
	public class PawnGroupKindWorker_Normal : PawnGroupKindWorker
	{
		// Token: 0x0600C4C4 RID: 50372 RVA: 0x00393B2C File Offset: 0x00391D2C
		public override float MinPointsToGenerateAnything(PawnGroupMaker groupMaker, FactionDef faction, PawnGroupMakerParms parms = null)
		{
			float num = (parms != null && parms.points > 0f) ? parms.points : 100000f;
			float num2 = float.MaxValue;
			List<PawnGenOptionWithXenotype> options = PawnGroupMakerUtility.GetOptions(parms, faction, groupMaker.options, num, num, new float?(float.MaxValue), null, false);
			foreach (PawnGenOptionWithXenotype pawnGenOptionWithXenotype in options)
			{
				if (pawnGenOptionWithXenotype.Option.kind.isFighter && pawnGenOptionWithXenotype.Cost < num2 && PawnGroupMakerUtility.PawnGenOptionValid(pawnGenOptionWithXenotype.Option, parms, null))
				{
					num2 = pawnGenOptionWithXenotype.Cost;
				}
			}
			if (num2 == 3.4028235E+38f)
			{
				foreach (PawnGenOptionWithXenotype pawnGenOptionWithXenotype2 in options)
				{
					if (pawnGenOptionWithXenotype2.Cost < num2 && PawnGroupMakerUtility.PawnGenOptionValid(pawnGenOptionWithXenotype2.Option, parms, null))
					{
						num2 = pawnGenOptionWithXenotype2.Cost;
					}
				}
			}
			return num2;
		}

		// Token: 0x0600C4C5 RID: 50373 RVA: 0x00393C4C File Offset: 0x00391E4C
		public override bool CanGenerateFrom(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			if (!base.CanGenerateFrom(parms, groupMaker))
			{
				return false;
			}
			Faction faction = parms.faction;
			return PawnGroupMakerUtility.AnyOptions(parms, (faction != null) ? faction.def : null, groupMaker.options, parms.points);
		}

		// Token: 0x0600C4C6 RID: 50374 RVA: 0x00393C84 File Offset: 0x00391E84
		protected override void GeneratePawns(PawnGroupMakerParms parms, PawnGroupMaker groupMaker, List<Pawn> outPawns, bool errorOnZeroResults = true)
		{
			if (!this.CanGenerateFrom(parms, groupMaker))
			{
				if (errorOnZeroResults)
				{
					string[] array = new string[5];
					array[0] = "Cannot generate pawns for ";
					int num = 1;
					Faction faction = parms.faction;
					array[num] = ((faction != null) ? faction.ToString() : null);
					array[2] = " with ";
					array[3] = parms.points.ToString();
					array[4] = ". Defaulting to a single random cheap group.";
					Log.Error(string.Concat(array));
				}
				return;
			}
			bool flag = parms.raidStrategy == null || parms.raidStrategy.pawnsCanBringFood || (parms.faction != null && !parms.faction.HostileTo(Faction.OfPlayer));
			Predicate<Pawn> predicate = (parms.raidStrategy != null) ? ((Pawn p) => parms.raidStrategy.Worker.CanUsePawn(parms.points, p, outPawns)) : null;
			bool flag2 = false;
			foreach (PawnGenOptionWithXenotype pawnGenOptionWithXenotype in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				PawnKindDef kind = pawnGenOptionWithXenotype.Option.kind;
				Faction faction2 = parms.faction;
				PawnGenerationContext context = PawnGenerationContext.NonPlayer;
				Ideo ideo = parms.ideo;
				XenotypeDef xenotype = pawnGenOptionWithXenotype.Xenotype;
				PlanetTile? tile = new PlanetTile?(parms.tile);
				bool forceGenerateNewPawn = false;
				bool allowDead = false;
				bool allowFood = flag;
				bool inhabitants = parms.inhabitants;
				Predicate<Pawn> validatorPostGear = predicate;
				PawnGenerationRequest request = new PawnGenerationRequest(kind, faction2, context, tile, forceGenerateNewPawn, allowDead, parms.faction.deactivated, true, true, 1f, false, true, true, allowFood, true, inhabitants, false, false, false, 0f, 0f, null, 1f, null, validatorPostGear, null, null, null, null, null, null, null, null, null, ideo, false, false, false, false, null, null, xenotype, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
				if (parms.raidAgeRestriction != null && parms.raidAgeRestriction.Worker.ShouldApplyToKind(pawnGenOptionWithXenotype.Option.kind))
				{
					request.BiologicalAgeRange = parms.raidAgeRestriction.ageRange;
					request.AllowedDevelopmentalStages = parms.raidAgeRestriction.developmentStage;
				}
				if (pawnGenOptionWithXenotype.Option.kind.pawnGroupDevelopmentStage != null)
				{
					request.AllowedDevelopmentalStages = pawnGenOptionWithXenotype.Option.kind.pawnGroupDevelopmentStage.Value;
				}
				if (!Find.Storyteller.difficulty.ChildRaidersAllowed && parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer))
				{
					request.AllowedDevelopmentalStages = DevelopmentalStage.Adult;
				}
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (parms.forceOneDowned && !flag2)
				{
					pawn.health.forceDowned = true;
					if (pawn.guest != null)
					{
						pawn.guest.Recruitable = true;
					}
					pawn.mindState.canFleeIndividual = false;
					flag2 = true;
				}
				outPawns.Add(pawn);
			}
		}

		// Token: 0x0600C4C7 RID: 50375 RVA: 0x00393FF0 File Offset: 0x003921F0
		public override IEnumerable<PawnKindDef> GeneratePawnKindsExample(PawnGroupMakerParms parms, PawnGroupMaker groupMaker)
		{
			foreach (PawnGenOptionWithXenotype pawnGenOptionWithXenotype in PawnGroupMakerUtility.ChoosePawnGenOptionsByPoints(parms.points, groupMaker.options, parms))
			{
				yield return pawnGenOptionWithXenotype.Option.kind;
			}
			IEnumerator<PawnGenOptionWithXenotype> enumerator = null;
			yield break;
			yield break;
		}
	}
}
