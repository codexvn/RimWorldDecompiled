using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CF RID: 6351
	public class ThoughtWorker_ToxicFallout : ThoughtWorker
	{
		// Token: 0x060090C3 RID: 37059 RVA: 0x002AA209 File Offset: 0x002A8409
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.SpawnedOrAnyParentSpawned && !p.PositionHeld.Roofed(p.MapHeld) && p.MapHeld.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout))
			{
				return ThoughtState.ActiveDefault;
			}
			return ThoughtState.Inactive;
		}
	}
}
