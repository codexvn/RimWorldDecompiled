using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001130 RID: 4400
	public class JobDriver_RopeRoamerToHitchingPost : JobDriver_RopeToDestination
	{
		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06006B60 RID: 27488 RVA: 0x0021CEA8 File Offset: 0x0021B0A8
		protected Building HitchingPost
		{
			get
			{
				return base.TargetThingB as Building;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06006B61 RID: 27489 RVA: 0x0021CEB5 File Offset: 0x0021B0B5
		protected IntVec3 HitchingPostLoc
		{
			get
			{
				return this.HitchingPost.Position;
			}
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x0021CEC4 File Offset: 0x0021B0C4
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			if (!this.HitchingPostLoc.IsValid)
			{
				return false;
			}
			if (!this.pawn.Position.InHorDistOf(this.HitchingPostLoc, 2f))
			{
				return false;
			}
			District district = this.HitchingPostLoc.GetDistrict(this.pawn.Map, RegionType.Set_Passable);
			return district == this.pawn.GetDistrict(RegionType.Set_Passable) && district == ropee.GetDistrict(RegionType.Set_Passable);
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x0021CF3C File Offset: 0x0021B13C
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			if (this.HitchingPostLoc.IsValid)
			{
				ropee.roping.RopeToSpot(this.HitchingPostLoc);
			}
		}

		// Token: 0x06006B64 RID: 27492 RVA: 0x0021CF6C File Offset: 0x0021B16C
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			if (animal.roping.RopedByPawn == this.pawn)
			{
				return false;
			}
			string text;
			Building hitchingPostAnimalShouldBeTakenTo = AnimalPenUtility.GetHitchingPostAnimalShouldBeTakenTo(this.pawn, animal, out text, false);
			return hitchingPostAnimalShouldBeTakenTo != null && this.HitchingPost == hitchingPostAnimalShouldBeTakenTo && AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, animal, out text, false, true, false, true, this.job.ropingPriority, null) == null;
		}
	}
}
