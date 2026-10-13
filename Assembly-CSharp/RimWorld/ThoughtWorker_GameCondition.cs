using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001885 RID: 6277
	public class ThoughtWorker_GameCondition : ThoughtWorker
	{
		// Token: 0x06008FF9 RID: 36857 RVA: 0x002A7BCC File Offset: 0x002A5DCC
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.SpawnedOrAnyParentSpawned && p.MapHeld.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			if (Find.World.gameConditionManager.ConditionIsActive(this.def.gameCondition))
			{
				return true;
			}
			return false;
		}
	}
}
