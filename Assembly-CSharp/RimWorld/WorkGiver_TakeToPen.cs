using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001552 RID: 5458
	public class WorkGiver_TakeToPen : WorkGiver_InteractAnimal
	{
		// Token: 0x06008196 RID: 33174 RVA: 0x002701A7 File Offset: 0x0026E3A7
		public WorkGiver_TakeToPen()
		{
			this.canInteractWhileSleeping = true;
		}

		// Token: 0x06008197 RID: 33175 RVA: 0x0026F7EF File Offset: 0x0026D9EF
		public override IEnumerable<Thing> PotentialWorkThingsGlobal(Pawn pawn)
		{
			return pawn.Map.mapPawns.SpawnedPawnsInFaction(pawn.Faction);
		}

		// Token: 0x06008198 RID: 33176 RVA: 0x002701C8 File Offset: 0x0026E3C8
		public override Job JobOnThing(Pawn pawn, Thing t, bool forced = false)
		{
			Pawn pawn2 = t as Pawn;
			if (pawn2 == null || !pawn2.IsAnimal)
			{
				return null;
			}
			bool flag = pawn2.MentalStateDef == MentalStateDefOf.Roaming;
			if (this.targetRoamingAnimals && !flag)
			{
				return null;
			}
			if (!this.targetRoamingAnimals && !flag && pawn2.MentalStateDef != null)
			{
				JobFailReason.Is("CantRopeAnimalMentalState".Translate(pawn2, pawn2.MentalStateDef.label), null);
				return null;
			}
			if (pawn2.Position.IsForbidden(pawn))
			{
				JobFailReason.Is(string.Format("{0} ({1})", "ForbiddenOutsideAllowedAreaLower".Translate().CapitalizeFirst(), pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label), null);
				return null;
			}
			if (t.Map.designationManager.DesignationOn(t, DesignationDefOf.ReleaseAnimalToWild) != null)
			{
				return null;
			}
			Map map = pawn2.Map;
			if (this.balanceCalculatorsCachedTick != Find.TickManager.TicksGame || this.balanceCalculatorsCachedForPawn != pawn)
			{
				foreach (KeyValuePair<Map, AnimalPenBalanceCalculator> keyValuePair in this.balanceCalculatorsCached)
				{
					keyValuePair.Value.MarkDirty();
				}
				this.balanceCalculatorsCachedTick = Find.TickManager.TicksGame;
				this.balanceCalculatorsCachedForPawn = pawn;
			}
			AnimalPenBalanceCalculator animalPenBalanceCalculator;
			if (this.balanceCalculatorsCached.ContainsKey(map))
			{
				animalPenBalanceCalculator = this.balanceCalculatorsCached[map];
			}
			else
			{
				animalPenBalanceCalculator = new AnimalPenBalanceCalculator(map, true);
				this.balanceCalculatorsCached.Add(map, animalPenBalanceCalculator);
			}
			string text = null;
			CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(pawn, pawn2, out text, forced, this.canInteractWhileSleeping, false, true, this.ropingPriority, animalPenBalanceCalculator);
			if (penAnimalShouldBeTakenTo != null)
			{
				Job job = WorkGiver_TakeToPen.MakeJob(pawn, pawn2, penAnimalShouldBeTakenTo, false, this.ropingPriority, out text);
				if (job != null)
				{
					return job;
				}
			}
			string text2 = null;
			Building hitchingPostAnimalShouldBeTakenTo = AnimalPenUtility.GetHitchingPostAnimalShouldBeTakenTo(pawn, pawn2, out text2, forced);
			if (hitchingPostAnimalShouldBeTakenTo != null)
			{
				return JobMaker.MakeJob(JobDefOf.RopeRoamerToHitchingPost, pawn2, hitchingPostAnimalShouldBeTakenTo);
			}
			if (this.allowUnenclosedPens)
			{
				penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(pawn, pawn2, out text, forced, this.canInteractWhileSleeping, true, true, this.ropingPriority, animalPenBalanceCalculator);
				if (penAnimalShouldBeTakenTo != null)
				{
					Job job2 = WorkGiver_TakeToPen.MakeJob(pawn, pawn2, penAnimalShouldBeTakenTo, true, this.ropingPriority, out text);
					if (job2 != null)
					{
						return job2;
					}
				}
			}
			if (penAnimalShouldBeTakenTo == null && hitchingPostAnimalShouldBeTakenTo == null && AnimalPenUtility.IsUnnecessarilyRoped(pawn2))
			{
				Job job3 = this.MakeUnropeJob(pawn, pawn2, forced, out text);
				if (job3 != null)
				{
					return job3;
				}
			}
			if (text != null)
			{
				JobFailReason.Is(text, null);
			}
			else if (text2 != null)
			{
				JobFailReason.Is(text2, null);
			}
			return null;
		}

		// Token: 0x06008199 RID: 33177 RVA: 0x0027044C File Offset: 0x0026E64C
		private Job MakeUnropeJob(Pawn roper, Pawn animal, bool forced, out string jobFailReason)
		{
			jobFailReason = null;
			if (AnimalPenUtility.RopeAttachmentInteractionCell(roper, animal) == IntVec3.Invalid)
			{
				jobFailReason = "CantRopeAnimalCantTouch".Translate();
				return null;
			}
			if (!forced && !roper.CanReserve(animal, 1, -1, null, false))
			{
				return null;
			}
			if (!roper.CanReach(animal, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return JobMaker.MakeJob(JobDefOf.Unrope, animal);
		}

		// Token: 0x0600819A RID: 33178 RVA: 0x002704C0 File Offset: 0x0026E6C0
		public static Job MakeJob(Pawn pawn, Pawn animal, CompAnimalPenMarker targetPenMarker, bool allowUnenclosedPens, RopingPriority ropingPriority, out string jobFailReason)
		{
			jobFailReason = null;
			IntVec3 c = AnimalPenUtility.FindPlaceInPenToStand(targetPenMarker, pawn);
			if (!c.IsValid)
			{
				jobFailReason = "CantRopeAnimalNoSpace".Translate();
				return null;
			}
			Job job = JobMaker.MakeJob(targetPenMarker.PenState.Enclosed ? JobDefOf.RopeToPen : JobDefOf.RopeRoamerToUnenclosedPen, animal, c, targetPenMarker.parent);
			job.ropingPriority = ropingPriority;
			job.ropeToUnenclosedPens = allowUnenclosedPens;
			return job;
		}

		// Token: 0x0400564D RID: 22093
		protected bool targetRoamingAnimals;

		// Token: 0x0400564E RID: 22094
		protected bool allowUnenclosedPens;

		// Token: 0x0400564F RID: 22095
		protected RopingPriority ropingPriority;

		// Token: 0x04005650 RID: 22096
		private readonly Dictionary<Map, AnimalPenBalanceCalculator> balanceCalculatorsCached = new Dictionary<Map, AnimalPenBalanceCalculator>();

		// Token: 0x04005651 RID: 22097
		private int balanceCalculatorsCachedTick = -1;

		// Token: 0x04005652 RID: 22098
		private Pawn balanceCalculatorsCachedForPawn;
	}
}
