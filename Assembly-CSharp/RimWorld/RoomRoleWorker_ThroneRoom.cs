using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200216F RID: 8559
	public class RoomRoleWorker_ThroneRoom : RoomRoleWorker
	{
		// Token: 0x0600BADA RID: 47834 RVA: 0x0035F045 File Offset: 0x0035D245
		public static string Validate(Room room)
		{
			if (room == null || room.OutdoorsForWork)
			{
				return "ThroneMustBePlacedInside".Translate();
			}
			return null;
		}

		// Token: 0x0600BADB RID: 47835 RVA: 0x0035F064 File Offset: 0x0035D264
		public override float GetScore(Room room)
		{
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			bool flag = false;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				if (containedAndAdjacentThings[i] is Building_Throne)
				{
					flag = true;
					break;
				}
			}
			return (float)((flag && RoomRoleWorker_ThroneRoom.Validate(room) == null) ? 10000 : 0);
		}

		// Token: 0x0600BADC RID: 47836 RVA: 0x0035F0B1 File Offset: 0x0035D2B1
		public override float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			if (room.Role == RoomRoleDefOf.ThroneRoom)
			{
				return 0f;
			}
			if (buildingDef != ThingDefOf.Throne)
			{
				return 0f;
			}
			return (float)((RoomRoleWorker_ThroneRoom.Validate(room) == null) ? 10000 : 0);
		}
	}
}
