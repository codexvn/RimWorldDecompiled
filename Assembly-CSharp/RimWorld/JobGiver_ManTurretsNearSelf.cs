using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001466 RID: 5222
	public class JobGiver_ManTurretsNearSelf : JobGiver_ManTurrets
	{
		// Token: 0x06007E12 RID: 32274 RVA: 0x001FF049 File Offset: 0x001FD249
		protected override IntVec3 GetRoot(Pawn pawn)
		{
			return pawn.Position;
		}
	}
}
