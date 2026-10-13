using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017BA RID: 6074
	public class ThinkNode_ConditionalAnyUndownedColonistSpawnedNearby : ThinkNode_Conditional
	{
		// Token: 0x06008DC3 RID: 36291 RVA: 0x002A12D0 File Offset: 0x0029F4D0
		protected override bool Satisfied(Pawn pawn)
		{
			if (pawn.Spawned)
			{
				return pawn.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed);
			}
			return false;
		}
	}
}
