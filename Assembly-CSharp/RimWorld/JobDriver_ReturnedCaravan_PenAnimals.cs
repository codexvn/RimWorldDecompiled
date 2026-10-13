using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001133 RID: 4403
	public class JobDriver_ReturnedCaravan_PenAnimals : JobDriver_RopeToDestination
	{
		// Token: 0x06006B70 RID: 27504 RVA: 0x0021D118 File Offset: 0x0021B318
		protected override bool UpdateDestination()
		{
			this.tmpRopees.Clear();
			this.tmpRopees.AddRange(this.pawn.roping.Ropees);
			bool result;
			try
			{
				IntVec3 cell = this.job.GetTarget(TargetIndex.B).Cell;
				District district = cell.IsValid ? cell.GetDistrict(base.Map, RegionType.Set_Passable) : null;
				IntVec3 c = IntVec3.Invalid;
				Thing t = null;
				foreach (Pawn pawn in this.tmpRopees)
				{
					string text;
					CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, pawn, out text, false, true, true, true, RopingPriority.Closest, null);
					if (penAnimalShouldBeTakenTo == null)
					{
						this.pawn.roping.DropRope(pawn);
					}
					else
					{
						if (penAnimalShouldBeTakenTo.parent.GetDistrict(RegionType.Set_Passable) == district)
						{
							return false;
						}
						if (!c.IsValid)
						{
							t = penAnimalShouldBeTakenTo.parent;
							c = penAnimalShouldBeTakenTo.parent.Position;
						}
					}
				}
				if (c.IsValid)
				{
					this.job.SetTarget(TargetIndex.B, c);
					this.job.SetTarget(TargetIndex.C, t);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			finally
			{
				this.tmpRopees.Clear();
			}
			return result;
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x0021D294 File Offset: 0x0021B494
		private bool PenIsEnclosed()
		{
			LocalTargetInfo target = this.job.GetTarget(TargetIndex.C);
			if (!target.HasThing)
			{
				return false;
			}
			ThingWithComps thingWithComps = target.Thing as ThingWithComps;
			if (thingWithComps == null)
			{
				return false;
			}
			CompAnimalPenMarker comp = thingWithComps.GetComp<CompAnimalPenMarker>();
			return comp != null && comp.PenState.Enclosed;
		}

		// Token: 0x06006B72 RID: 27506 RVA: 0x0021D2E2 File Offset: 0x0021B4E2
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			if (this.job.ropeToUnenclosedPens && !this.PenIsEnclosed())
			{
				return roperWaitingAtDest;
			}
			return AnimalPenUtility.GetCurrentPenOf(ropee, this.job.ropeToUnenclosedPens) != null;
		}

		// Token: 0x06006B73 RID: 27507 RVA: 0x0021D310 File Offset: 0x0021B510
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			Lord lord = ropee.GetLord();
			if (lord == null)
			{
				return;
			}
			lord.Notify_PawnLost(ropee, PawnLostCondition.LeftVoluntarily, null);
		}

		// Token: 0x06006B74 RID: 27508 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			return false;
		}

		// Token: 0x04004B9B RID: 19355
		public const TargetIndex DestPenInd = TargetIndex.C;

		// Token: 0x04004B9C RID: 19356
		private List<Pawn> tmpRopees = new List<Pawn>();
	}
}
