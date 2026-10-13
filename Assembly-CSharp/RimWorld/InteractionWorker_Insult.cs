using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023FD RID: 9213
	public class InteractionWorker_Insult : InteractionWorker
	{
		// Token: 0x0600C7AC RID: 51116 RVA: 0x003A38E9 File Offset: 0x003A1AE9
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.IsSlave && !recipient.IsSlave)
			{
				return 0f;
			}
			return 0.007f * NegativeInteractionUtility.NegativeInteractionChanceFactor(initiator, recipient);
		}

		// Token: 0x04008838 RID: 34872
		private const float BaseSelectionWeight = 0.007f;
	}
}
