using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001841 RID: 6209
	public class ThoughtWorker_Precept_NoFish : ThoughtWorker_Precept
	{
		// Token: 0x06008F63 RID: 36707 RVA: 0x002A5A8F File Offset: 0x002A3C8F
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.OdysseyActive || !p.SpawnedOrAnyParentSpawned)
			{
				return ThoughtState.Inactive;
			}
			return !p.MapHeld.waterBodyTracker.AnyBodyContainsFish;
		}
	}
}
