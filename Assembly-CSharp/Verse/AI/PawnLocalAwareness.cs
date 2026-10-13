using System;

namespace Verse.AI
{
	// Token: 0x02000FC5 RID: 4037
	public static class PawnLocalAwareness
	{
		// Token: 0x0600635E RID: 25438 RVA: 0x001F64D0 File Offset: 0x001F46D0
		public static bool AnimalAwareOf(this Pawn p, Thing t)
		{
			return p.RaceProps.ToolUser || p.Faction != null || ((float)(p.Position - t.Position).LengthHorizontalSquared <= 900f && p.GetRoom(RegionType.Set_All) == t.GetRoom(RegionType.Set_All) && GenSight.LineOfSight(p.Position, t.Position, p.Map));
		}

		// Token: 0x04004716 RID: 18198
		private const float SightRadius = 30f;
	}
}
