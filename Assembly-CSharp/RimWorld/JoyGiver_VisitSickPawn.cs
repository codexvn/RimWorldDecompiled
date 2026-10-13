using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014FF RID: 5375
	public class JoyGiver_VisitSickPawn : JoyGiver
	{
		// Token: 0x06008015 RID: 32789 RVA: 0x00268F20 File Offset: 0x00267120
		public override Job TryGiveJob(Pawn pawn)
		{
			if (!SocialInteractionUtility.CanInitiateInteraction(pawn, null))
			{
				return null;
			}
			Pawn pawn2 = SickPawnVisitUtility.FindRandomSickPawn(pawn, JoyCategory.Low);
			if (pawn2 == null)
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, pawn2, SickPawnVisitUtility.FindChair(pawn, pawn2));
		}
	}
}
