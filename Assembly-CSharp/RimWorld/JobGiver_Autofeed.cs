using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200147A RID: 5242
	public class JobGiver_Autofeed : ThinkNode_JobGiver
	{
		// Token: 0x06007E54 RID: 32340 RVA: 0x00260720 File Offset: 0x0025E920
		protected override Job TryGiveJob(Pawn pawn)
		{
			if (!pawn.CanReserve(pawn, 1, -1, null, false))
			{
				return null;
			}
			Thing thing;
			Pawn pawn2 = ChildcareUtility.FindAutofeedBaby(pawn, AutofeedMode.Urgent, out thing);
			if (pawn2 == null || !pawn2.Spawned)
			{
				return null;
			}
			Pawn pawn3;
			ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
			if (thing != pawn && ChildcareUtility.ImmobileBreastfeederAvailable(pawn, pawn2, false, out pawn3, out breastfeedFailReason))
			{
				thing = pawn3;
			}
			if (thing == null)
			{
				return null;
			}
			return ChildcareUtility.MakeAutofeedBabyJob(pawn, pawn2, thing);
		}

		// Token: 0x06007E55 RID: 32341 RVA: 0x00260779 File Offset: 0x0025E979
		public override float GetPriority(Pawn pawn)
		{
			return 9.5f;
		}
	}
}
