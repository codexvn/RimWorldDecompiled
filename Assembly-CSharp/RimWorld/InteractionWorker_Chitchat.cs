using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020023F6 RID: 9206
	public class InteractionWorker_Chitchat : InteractionWorker
	{
		// Token: 0x0600C79B RID: 51099 RVA: 0x003A30BD File Offset: 0x003A12BD
		public override float RandomSelectionWeight(Pawn initiator, Pawn recipient)
		{
			if (initiator.Inhumanized())
			{
				return 0f;
			}
			return 1f;
		}
	}
}
