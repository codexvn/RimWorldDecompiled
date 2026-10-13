using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EFE RID: 7934
	public class RoomContents_Corridor : RoomContentsWorker
	{
		// Token: 0x17001AE6 RID: 6886
		// (get) Token: 0x0600AFAF RID: 44975 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual TerrainDef StripCorridorTerrain
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AE7 RID: 6887
		// (get) Token: 0x0600AFB0 RID: 44976 RVA: 0x0031F50D File Offset: 0x0031D70D
		protected virtual IntRange ExteriorDoorCount
		{
			get
			{
				return new IntRange(1, 3);
			}
		}

		// Token: 0x17001AE8 RID: 6888
		// (get) Token: 0x0600AFB1 RID: 44977 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual ThingDef DoorThing
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17001AE9 RID: 6889
		// (get) Token: 0x0600AFB2 RID: 44978 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual ThingDef DoorStuff
		{
			get
			{
				return null;
			}
		}

		// Token: 0x0600AFB3 RID: 44979 RVA: 0x00321821 File Offset: 0x0031FA21
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.SpawnStrip(map, room);
			this.SpawnDoors(map, room, this.ExteriorDoorCount);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AFB4 RID: 44980 RVA: 0x00321844 File Offset: 0x0031FA44
		private void SpawnStrip(Map map, LayoutRoom room)
		{
			if (this.StripCorridorTerrain == null)
			{
				return;
			}
			foreach (CellRect rect in room.rects)
			{
				Rot4 rot = (rect.Width > rect.Height) ? Rot4.East : Rot4.North;
				IntVec3 intVec = rect.GetCenterCellOnEdge(rot.Opposite);
				while (rect.Contains(intVec))
				{
					RoomContents_Corridor.TrySpawnAdjacentCorridorTiles(map, room, rect, rot, intVec);
					map.terrainGrid.SetTerrain(intVec, this.StripCorridorTerrain);
					intVec += rot.FacingCell;
				}
			}
		}

		// Token: 0x0600AFB5 RID: 44981 RVA: 0x003218FC File Offset: 0x0031FAFC
		private void SpawnDoors(Map map, LayoutRoom room, IntRange countRange)
		{
			int randomInRange = countRange.RandomInRange;
			if (randomInRange <= 0)
			{
				return;
			}
			List<IntVec3> list = new List<IntVec3>();
			foreach (CellRect cellRect in room.rects)
			{
				Rot4 rot = (cellRect.Width > cellRect.Height) ? Rot4.East : Rot4.North;
				IntVec3 centerCellOnEdge = cellRect.GetCenterCellOnEdge(rot);
				IntVec3 centerCellOnEdge2 = cellRect.GetCenterCellOnEdge(rot.Opposite);
				RoomContents_Corridor.ProcessPossibleDoorCell(centerCellOnEdge, room, map, list, ref randomInRange);
				RoomContents_Corridor.ProcessPossibleDoorCell(centerCellOnEdge2, room, map, list, ref randomInRange);
			}
			if (list.Empty<IntVec3>())
			{
				return;
			}
			int num = 0;
			while (num < randomInRange && list.Any<IntVec3>())
			{
				IntVec3 intVec = list.RandomElement<IntVec3>();
				list.Remove(intVec);
				ThingDef thingDef;
				if ((thingDef = this.DoorThing) == null)
				{
					thingDef = (room.sketch.layoutSketch.door ?? ThingDefOf.Door);
				}
				ThingDef thingDef2 = thingDef;
				ThingDef thingDef3;
				if (thingDef2.MadeFromStuff)
				{
					if ((thingDef3 = this.DoorStuff) == null)
					{
						thingDef3 = (room.sketch.layoutSketch.doorStuff ?? ThingDefOf.Steel);
					}
				}
				else
				{
					thingDef3 = null;
				}
				ThingDef stuff = thingDef3;
				GenSpawn.Spawn(ThingMaker.MakeThing(thingDef2, stuff), intVec, map, WipeMode.Vanish);
				num++;
			}
		}

		// Token: 0x0600AFB6 RID: 44982 RVA: 0x00321A44 File Offset: 0x0031FC44
		private static void ProcessPossibleDoorCell(IntVec3 cell, LayoutRoom room, Map map, List<IntVec3> possible, ref int count)
		{
			if (cell.GetDoor(map) != null)
			{
				count--;
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 c = cell + GenAdj.CardinalDirections[i];
				if (!room.sketch.AnyRoomContains(c, null))
				{
					possible.Add(cell);
					return;
				}
			}
		}

		// Token: 0x0600AFB7 RID: 44983 RVA: 0x00321A98 File Offset: 0x0031FC98
		private static void TrySpawnAdjacentCorridorTiles(Map map, LayoutRoom room, CellRect rect, Rot4 rot, IntVec3 pos)
		{
			if (room.rects.Count <= 1)
			{
				return;
			}
			Rot4 rot2 = (rot == Rot4.North) ? Rot4.East : Rot4.North;
			foreach (CellRect lhs in room.rects)
			{
				if (!(lhs == rect) && !(((lhs.Width > lhs.Height) ? Rot4.East : Rot4.North) == rot) && (!rot.IsHorizontal || pos.x == lhs.CenterCell.x) && (!rot.IsVertical || pos.z == lhs.CenterCell.z))
				{
					IntVec3 c;
					if ((rot.IsVertical && lhs.CenterCell.x > pos.x) || (rot.IsHorizontal && lhs.CenterCell.z > pos.z))
					{
						c = pos + rot2.FacingCell;
					}
					else
					{
						c = pos - rot2.FacingCell;
					}
					map.terrainGrid.SetTerrain(c, TerrainDefOf.MetalTile);
				}
			}
		}
	}
}
