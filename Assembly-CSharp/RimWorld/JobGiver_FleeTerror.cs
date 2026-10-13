using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200148A RID: 5258
	public class JobGiver_FleeTerror : ThinkNode_JobGiver
	{
		// Token: 0x06007E7C RID: 32380 RVA: 0x002610C0 File Offset: 0x0025F2C0
		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState mentalState = pawn.MentalState;
			Pawn pawn2 = (mentalState != null) ? mentalState.causedByPawn : null;
			if (pawn2 == null)
			{
				return null;
			}
			return FleeUtility.FleeJob(pawn, pawn2, Mathf.CeilToInt(this.fleeDistRange.RandomInRange));
		}

		// Token: 0x040054D4 RID: 21716
		private FloatRange fleeDistRange = new FloatRange(50f, 75f);
	}
}
