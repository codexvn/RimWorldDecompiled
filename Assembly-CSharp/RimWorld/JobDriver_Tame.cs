using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001137 RID: 4407
	public class JobDriver_Tame : JobDriver_InteractAnimal
	{
		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06006B87 RID: 27527 RVA: 0x0021D52F File Offset: 0x0021B72F
		protected override bool CanInteractNow
		{
			get
			{
				return !TameUtility.TriedToTameTooRecently(base.Animal);
			}
		}

		// Token: 0x06006B88 RID: 27528 RVA: 0x0021D53F File Offset: 0x0021B73F
		protected override IEnumerable<Toil> MakeNewToils()
		{
			Func<bool> noLongerDesignated = () => base.Map.designationManager.DesignationOn(base.Animal, DesignationDefOf.Tame) == null;
			if (this.job.GetTarget(TargetIndex.C).HasThing)
			{
				yield return Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOn(noLongerDesignated);
				yield return Toils_Haul.TakeToInventory(TargetIndex.C, this.job.count).FailOn(noLongerDesignated);
			}
			foreach (Toil toil in base.MakeNewToils())
			{
				toil.FailOn(noLongerDesignated);
				yield return toil;
			}
			IEnumerator<Toil> enumerator = null;
			yield return Toils_Interpersonal.TryRecruit(TargetIndex.A);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Pawn pawn = this.job.GetTarget(TargetIndex.A).Thing as Pawn;
				if (pawn == null || !AnimalPenUtility.NeedsToBeManagedByRope(pawn))
				{
					return;
				}
				if (pawn.Faction != Faction.OfPlayer)
				{
					return;
				}
				if (AnimalPenUtility.GetCurrentPenOf(pawn, false) == null)
				{
					RopingPriority ropingPriority = RopingPriority.Closest;
					string text;
					CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, pawn, out text, false, true, true, true, ropingPriority, null);
					Job job = null;
					if (penAnimalShouldBeTakenTo != null)
					{
						job = WorkGiver_TakeToPen.MakeJob(this.pawn, pawn, penAnimalShouldBeTakenTo, true, ropingPriority, out text);
					}
					if (job != null)
					{
						this.pawn.jobs.StartJob(job, JobCondition.Succeeded, null, false, true, null, null, false, false, null, false, true, false);
						return;
					}
					Messages.Message("MessageTameNoSuitablePens".Translate(pawn.Named("ANIMAL")), pawn, MessageTypeDefOf.NeutralEvent, true);
				}
			};
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			yield return toil2;
			yield break;
			yield break;
		}

		// Token: 0x04004BA2 RID: 19362
		private const TargetIndex FoodIndex = TargetIndex.C;
	}
}
