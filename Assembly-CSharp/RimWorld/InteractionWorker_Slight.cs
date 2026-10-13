using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200240A RID: 9226
	public class InteractionWorker_Slight : InteractionWorker
	{
		// Token: 0x0600C7DA RID: 51162 RVA: 0x003A5D2A File Offset: 0x003A3F2A
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.IsSlave && !recipient.IsSlave)
			{
				return 0f;
			}
			return 0.02f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04008859 RID: 34905
		private const float BaseSelectionWeight = 0.02f;
	}
}
