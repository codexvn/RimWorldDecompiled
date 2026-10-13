using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EFB RID: 7931
	public class RoomContents_Checkpoint_Corridor : RoomContents_Corridor
	{
		// Token: 0x17001AE2 RID: 6882
		// (get) Token: 0x0600AF9F RID: 44959 RVA: 0x0032142B File Offset: 0x0031F62B
		protected override TerrainDef StripCorridorTerrain
		{
			get
			{
				return TerrainDefOf.AncientTile;
			}
		}

		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x0600AFA0 RID: 44960 RVA: 0x00321432 File Offset: 0x0031F632
		protected override ThingDef DoorThing
		{
			get
			{
				return ThingDefOf.AncientBlastDoor;
			}
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x0600AFA1 RID: 44961 RVA: 0x00321439 File Offset: 0x0031F639
		protected override IntRange ExteriorDoorCount
		{
			get
			{
				return IntRange.Between(4, 4);
			}
		}

		// Token: 0x0600AFA2 RID: 44962 RVA: 0x00321442 File Offset: 0x0031F642
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_Checkpoint_Corridor.SpawnCheckpoints(map, room, faction);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AFA3 RID: 44963 RVA: 0x00321458 File Offset: 0x0031F658
		private static void SpawnCheckpoints(Map map, LayoutRoom room, Faction faction)
		{
			foreach (CellRect cellRect in room.rects)
			{
				Rot4 rot = (cellRect.Width > cellRect.Height) ? Rot4.East : Rot4.North;
				Rot4 rot2 = rot.Rotated(RotationDirection.Clockwise);
				CellRect cellRect2 = cellRect.ContractedBy(1);
				int sideLength = cellRect2.GetSideLength(rot2);
				float chance = (float)sideLength / 100f * 8f / (float)sideLength;
				IntVec3 intVec = cellRect2.GetCenterCellOnEdge(rot.Opposite);
				while (cellRect2.Contains(intVec))
				{
					CellRect rect = intVec.RectAbout(new IntVec2(3, 3));
					if (Rand.Chance(chance) && RoomContents_Checkpoint_Corridor.CanSpawnCheckpoint(map, rect, rot2))
					{
						RoomContents_Checkpoint_Corridor.SpawnCheckpoint(map, rect, rot2, faction);
						intVec += rot.FacingCell * 20;
					}
					else
					{
						intVec += rot.FacingCell;
					}
				}
			}
		}

		// Token: 0x0600AFA4 RID: 44964 RVA: 0x00321570 File Offset: 0x0031F770
		private static bool CanSpawnCheckpoint(Map map, CellRect rect, Rot4 rot)
		{
			using (CellRect.Enumerator enumerator = rect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			CellRect cellRect = rect.ExpandedBy(1);
			for (int i = 0; i < 2; i++)
			{
				Rot4 dir = rot;
				if (i == 1)
				{
					dir = dir.Opposite;
				}
				using (IEnumerator<IntVec3> enumerator2 = cellRect.GetEdgeCells(dir).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.GetEdifice(map) == null)
						{
							return false;
						}
					}
				}
			}
			using (IEnumerator<IntVec3> enumerator2 = rect.AdjacentCellsCardinal.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					if (enumerator2.Current.GetDoor(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600AFA5 RID: 44965 RVA: 0x0032167C File Offset: 0x0031F87C
		private static void SpawnCheckpoint(Map map, CellRect rect, Rot4 rot, Faction faction)
		{
			foreach (IntVec3 loc in rect.Corners)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Barricade, ThingDefOf.Steel), loc, map, WipeMode.Vanish);
			}
			for (int i = 0; i < 2; i++)
			{
				Rot4 rot2 = rot;
				if (i == 1)
				{
					rot2 = rot.Opposite;
				}
				IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot2);
				Thing thing = ThingMaker.MakeThing(Rand.Bool ? ThingDefOf.Turret_AncientArmoredTurret : ThingDefOf.AncientSecurityTurret, null);
				thing.SetFaction(faction ?? Faction.OfAncientsHostile, null);
				GenSpawn.Spawn(thing, centerCellOnEdge, map, WipeMode.Vanish);
			}
		}

		// Token: 0x0400787A RID: 30842
		private const float CheckpointsPerHundredCells = 8f;

		// Token: 0x0400787B RID: 30843
		private const int MinTilesPerCheckpoint = 20;
	}
}
