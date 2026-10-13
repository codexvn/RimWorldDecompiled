using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FA8 RID: 4008
	public class MentalStateWorker_EntityKiller : MentalStateWorker
	{
		// Token: 0x0600629A RID: 25242 RVA: 0x001F15AF File Offset: 0x001EF7AF
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && AnomalyUtility.FindEntityOnPlatform(pawn.Map, EntityQueryType.ForSlaughter) != null;
		}
	}
}
