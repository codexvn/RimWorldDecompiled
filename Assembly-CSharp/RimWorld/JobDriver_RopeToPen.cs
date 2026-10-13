using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200112E RID: 4398
	public class JobDriver_RopeToPen : JobDriver_RopeToDestination
	{
		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06006B52 RID: 27474 RVA: 0x001E5D79 File Offset: 0x001E3F79
		private Thing DestinationThing
		{
			get
			{
				return this.job.targetC.Thing;
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06006B53 RID: 27475 RVA: 0x0021CD93 File Offset: 0x0021AF93
		protected CompAnimalPenMarker DestinationMarker
		{
			get
			{
				return this.DestinationThing.TryGetComp<CompAnimalPenMarker>();
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06006B54 RID: 27476 RVA: 0x0021CDA0 File Offset: 0x0021AFA0
		private District DestinationDistrict
		{
			get
			{
				return this.DestinationThing.GetDistrict(RegionType.Set_Passable);
			}
		}

		// Token: 0x06006B55 RID: 27477 RVA: 0x0021CDAF File Offset: 0x0021AFAF
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedNullOrForbidden(TargetIndex.C);
			return base.MakeNewToils();
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x0021CDC0 File Offset: 0x0021AFC0
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			PenMarkerState penState = this.DestinationMarker.PenState;
			return !penState.Enclosed || penState.ContainsConnectedRegion(ropee.GetRegion(RegionType.Set_Passable));
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x000026EA File Offset: 0x000008EA
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x0021CDF4 File Offset: 0x0021AFF4
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal)
		{
			if (animal.roping.RopedByPawn == this.pawn)
			{
				return false;
			}
			string text;
			CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(this.pawn, animal, out text, false, true, this.job.ropeToUnenclosedPens, true, this.job.ropingPriority, null);
			return penAnimalShouldBeTakenTo != null && this.ShouldOpportunisticallyRopeAnimal(animal, penAnimalShouldBeTakenTo);
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x0021CE4C File Offset: 0x0021B04C
		protected virtual bool ShouldOpportunisticallyRopeAnimal(Pawn animal, CompAnimalPenMarker targetPenMarker)
		{
			return targetPenMarker.parent.GetDistrict(RegionType.Set_Passable) == this.DestinationDistrict;
		}

		// Token: 0x04004B9A RID: 19354
		public const TargetIndex DestMarkerInd = TargetIndex.C;
	}
}
