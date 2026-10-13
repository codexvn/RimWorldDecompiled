using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D1 RID: 6097
	public class ThinkNode_ConditionalSpawnedOrAnyParentSpawned : ThinkNode_Conditional
	{
		// Token: 0x06008DF6 RID: 36342 RVA: 0x002A19C1 File Offset: 0x0029FBC1
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.SpawnedOrAnyParentSpawned;
		}
	}
}
