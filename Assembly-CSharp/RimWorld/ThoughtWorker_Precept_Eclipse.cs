using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001824 RID: 6180
	public class ThoughtWorker_Precept_Eclipse : ThoughtWorker_Precept
	{
		// Token: 0x06008F09 RID: 36617 RVA: 0x002A4A6C File Offset: 0x002A2C6C
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			if (!p.Awake() || PawnUtility.IsBiologicallyOrArtificiallyBlind(p))
			{
				return false;
			}
			Room room = p.GetRoom(RegionType.Set_All);
			if (room != null && !room.PsychologicallyOutdoors)
			{
				return false;
			}
			return p.Map.GameConditionManager.ConditionIsActive(GameConditionDefOf.Eclipse);
		}
	}
}
