using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200112F RID: 4399
	public class JobDriver_RopeRoamerToUnenclosedPen : JobDriver_RopeToPen
	{
		// Token: 0x06006B5B RID: 27483 RVA: 0x00161C9F File Offset: 0x0015FE9F
		protected override bool HasRopeeArrived(Pawn ropee, bool roperWaitingAtDest)
		{
			return roperWaitingAtDest;
		}

		// Token: 0x06006B5C RID: 27484 RVA: 0x0021CE6B File Offset: 0x0021B06B
		protected override void ProcessArrivedRopee(Pawn ropee)
		{
			MentalState_Roaming mentalState_Roaming = JobDriver_RopeRoamerToUnenclosedPen.RoamingMentalState(ropee);
			if (mentalState_Roaming == null)
			{
				return;
			}
			mentalState_Roaming.RecoverFromState();
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x0021CE7D File Offset: 0x0021B07D
		protected override bool ShouldOpportunisticallyRopeAnimal(Pawn animal, CompAnimalPenMarker targetPenMarker)
		{
			return targetPenMarker == base.DestinationMarker && JobDriver_RopeRoamerToUnenclosedPen.RoamingMentalState(animal) != null;
		}

		// Token: 0x06006B5E RID: 27486 RVA: 0x0021CE93 File Offset: 0x0021B093
		private static MentalState_Roaming RoamingMentalState(Pawn ropee)
		{
			return ropee.MentalState as MentalState_Roaming;
		}
	}
}
