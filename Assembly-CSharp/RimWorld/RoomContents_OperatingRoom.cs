using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EE6 RID: 7910
	public class RoomContents_OperatingRoom : RoomContentsWorker
	{
		// Token: 0x17001AC7 RID: 6855
		// (get) Token: 0x0600AF38 RID: 44856 RVA: 0x0032008D File Offset: 0x0031E28D
		protected virtual FloatRange ItemsPer100Cells
		{
			get
			{
				return new FloatRange(2f, 4f);
			}
		}

		// Token: 0x0600AF39 RID: 44857 RVA: 0x003200A0 File Offset: 0x0031E2A0
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			Rot4 value = Rot4.North;
			CellRect boundary = room.Boundary;
			if (boundary.Height > boundary.Width)
			{
				value = Rot4.East;
			}
			RoomGenUtility.FillWithPadding(ThingDefOf.AncientOperatingTable, this.GetCount(room), room, map, new Rot4?(value), null, null, 1, null, false, false, null, null);
			foreach (IntVec3 c in room.Cells)
			{
				if (Rand.Chance(0.75f))
				{
					map.terrainGrid.SetTerrain(c, TerrainDefOf.AncientTile);
				}
			}
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF3A RID: 44858 RVA: 0x00320154 File Offset: 0x0031E354
		private int GetCount(LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			return Mathf.Max(Mathf.RoundToInt(this.ItemsPer100Cells.RandomInRange * num), 1);
		}

		// Token: 0x04007854 RID: 30804
		private const float FloorSteelPercentage = 0.75f;
	}
}
