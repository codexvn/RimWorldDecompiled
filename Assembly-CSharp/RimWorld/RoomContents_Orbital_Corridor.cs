using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EFF RID: 7935
	public class RoomContents_Orbital_Corridor : RoomContents_Checkpoint_Corridor
	{
		// Token: 0x0600AFB9 RID: 44985 RVA: 0x00321BF4 File Offset: 0x0031FDF4
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			base.FillRoom(map, room, faction, threatPoints);
			int count = room.rects.Count;
			RoomGenUtility.SpawnWallAttatchments(ThingDefOf.LifeSupportUnit, map, room, IntRange.Between(count, count), null, null, null);
		}

		// Token: 0x0600AFBA RID: 44986 RVA: 0x00321C2E File Offset: 0x0031FE2E
		protected override bool IsValidWallAttachmentCell(LayoutWallAttatchmentParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			return parms.def != ThingDefOf.LifeSupportUnit && base.IsValidWallAttachmentCell(parms, cell, rot, room, map);
		}
	}
}
