using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FA9 RID: 4009
	public class MentalStateWorker_EntityLiberator : MentalStateWorker
	{
		// Token: 0x0600629C RID: 25244 RVA: 0x001F15CB File Offset: 0x001EF7CB
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && AnomalyUtility.FindEntityOnPlatform(pawn.Map, EntityQueryType.ForRelease) != null;
		}
	}
}
