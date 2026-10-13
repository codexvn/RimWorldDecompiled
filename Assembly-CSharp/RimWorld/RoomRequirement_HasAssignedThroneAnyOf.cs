using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020037B8 RID: 14264
	public class RoomRequirement_HasAssignedThroneAnyOf : RoomRequirement_ThingAnyOf
	{
		// Token: 0x06013C4A RID: 80970 RVA: 0x005DDF7C File Offset: 0x005DC17C
		public override bool Met(Room r, Pawn p = null)
		{
			if (p == null)
			{
				return false;
			}
			if (!r.ProperRoom)
			{
				return false;
			}
			foreach (Thing thing in r.ContainedAndAdjacentThings)
			{
				if (this.things.Contains(thing.def) && p.ownership.AssignedThrone == thing)
				{
					return true;
				}
			}
			return false;
		}
	}
}
