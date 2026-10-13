using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EEE RID: 7918
	public class RoomContents_TransportRoom : RoomContentsWorker
	{
		// Token: 0x0600AF52 RID: 44882 RVA: 0x00320553 File Offset: 0x0031E753
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.subroom = RoomContents_TransportRoom.GetSubroomRect(map, room);
			RoomContents_TransportRoom.SetDefaultRoof(map, room);
			RoomContents_TransportRoom.FillSubroom(map, room, this.subroom);
			this.SpawnShelves(map, room);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AF53 RID: 44883 RVA: 0x0032058C File Offset: 0x0031E78C
		private static void SetDefaultRoof(Map map, LayoutRoom room)
		{
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 c in cellRect.Cells)
				{
					map.roofGrid.SetRoof(c, RoofDefOf.RoofConstructed);
				}
			}
		}

		// Token: 0x0600AF54 RID: 44884 RVA: 0x00320620 File Offset: 0x0031E820
		protected override bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
		{
			return !this.subroom.Contains(cell) && base.IsValidCellBase(thing, stuff, cell, room, map);
		}

		// Token: 0x0600AF55 RID: 44885 RVA: 0x00320640 File Offset: 0x0031E840
		private static void FillSubroom(Map map, LayoutRoom room, CellRect rect)
		{
			CellRect cellRect = rect.ContractedBy(1);
			foreach (IntVec3 intVec in rect.EdgeCells)
			{
				if (intVec.GetEdifice(map) == null)
				{
					GenSpawn.Spawn(ThingDefOf.AncientFortifiedWall, intVec, map, WipeMode.Vanish);
				}
			}
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i);
				IntVec3 centerCellOnEdge = rect.GetCenterCellOnEdge(rot);
				if (room.rects[0].ContractedBy(1).Contains(centerCellOnEdge) && RoomGenUtility.IsGoodForDoor(centerCellOnEdge, map))
				{
					GenSpawn.Spawn(ThingDefOf.AncientBlastDoor, centerCellOnEdge, map, WipeMode.Vanish);
					break;
				}
			}
			foreach (IntVec3 c in cellRect.Cells)
			{
				map.terrainGrid.SetTerrain(c, TerrainDefOf.MetalTile);
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 loc in cellRect.Corners)
			{
				GenSpawn.Spawn(ThingDefOf.AncientShipBeacon, loc, map, WipeMode.Vanish);
			}
			if (Rand.Bool)
			{
				GenSpawn.Spawn(ThingDefOf.AncientTransportPod, rect.CenterCell, map, WipeMode.Vanish);
				return;
			}
			GenSpawn.Spawn(ThingDefOf.MalfunctioningTransportPod, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x0600AF56 RID: 44886 RVA: 0x003207D4 File Offset: 0x0031E9D4
		private static CellRect GetSubroomRect(Map map, LayoutRoom room)
		{
			CellRect result = CellRect.Empty;
			int @int = Rand.Int;
			for (int i = 0; i < 4; i++)
			{
				Rot4 rot = new Rot4(i + @int);
				IntVec3 corner = room.rects[0].GetCorner(rot, true);
				Rot4 rot2 = (rot == Rot4.North || rot == Rot4.West) ? Rot4.South : Rot4.North;
				Rot4 rot3 = (rot == Rot4.North || rot == Rot4.East) ? Rot4.West : Rot4.East;
				int num = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Width - 4);
				int num2 = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Height - 4);
				if ((float)num > (float)num2 * 1.25f)
				{
					num = (int)((float)num2 * 1.25f);
				}
				else if ((float)num2 > (float)num * 1.25f)
				{
					num2 = (int)((float)num * 1.25f);
				}
				for (int j = RoomContents_TransportRoom.RoomSizeRange.TrueMin; j < num; j++)
				{
					bool flag = true;
					for (int k = RoomContents_TransportRoom.RoomSizeRange.TrueMin; k < num2; k++)
					{
						IntVec3 b = rot3.FacingCell * j;
						IntVec3 b2 = rot2.FacingCell * k;
						CellRect cellRect = CellRect.FromLimits(corner + b + b2, corner);
						using (IEnumerator<IntVec3> enumerator = cellRect.EdgeCells.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.GetDoor(map) != null)
								{
									flag = false;
									break;
								}
							}
						}
						if (!flag)
						{
							break;
						}
						if (cellRect.Area > result.Area && cellRect.Width >= RoomContents_TransportRoom.RoomSizeRange.TrueMin && cellRect.Height >= RoomContents_TransportRoom.RoomSizeRange.TrueMin)
						{
							result = cellRect;
						}
					}
					if (!flag)
					{
						break;
					}
				}
			}
			if (result.Area == 0)
			{
				int maxExclusive = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Width / 2 - 3);
				int maxExclusive2 = Mathf.Min(RoomContents_TransportRoom.RoomSizeRange.TrueMax, room.rects[0].Height / 2 - 3);
				result = room.rects[0].CenterCell.RectAbout(new IntVec2(Rand.Range(RoomContents_TransportRoom.RoomSizeRange.TrueMin, maxExclusive), Rand.Range(RoomContents_TransportRoom.RoomSizeRange.TrueMin, maxExclusive2)));
			}
			return result;
		}

		// Token: 0x0600AF57 RID: 44887 RVA: 0x00320ACC File Offset: 0x0031ECCC
		private void SpawnShelves(Map map, LayoutRoom room)
		{
			RoomContents_TransportRoom.<>c__DisplayClass10_0 CS$<>8__locals1 = new RoomContents_TransportRoom.<>c__DisplayClass10_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.possibleSpots = new List<IntVec3>(6);
			float num = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_TransportRoom.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_TransportRoom.ShelfGroupSizeRange, CS$<>8__locals1.room, CS$<>8__locals1.map, new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals1.<SpawnShelves>g__Validator|2), null, 1, 0, null, true, RotationDirection.Opposite, new Func<IntVec3, Rot4, Map, Thing>(CS$<>8__locals1.<SpawnShelves>g__SpawnAction|1), null);
			if (Rand.Bool && CS$<>8__locals1.possibleSpots.Any<IntVec3>())
			{
				IntVec3 intVec = CS$<>8__locals1.possibleSpots.RandomElement<IntVec3>();
				CS$<>8__locals1.possibleSpots.Remove(intVec);
				RoomContents_TransportRoom.SpawnApparel(ThingDefOf.Apparel_VacsuitHelmet, intVec, CS$<>8__locals1.map);
			}
			if (Rand.Bool && CS$<>8__locals1.possibleSpots.Any<IntVec3>())
			{
				IntVec3 intVec2 = CS$<>8__locals1.possibleSpots.RandomElement<IntVec3>();
				CS$<>8__locals1.possibleSpots.Remove(intVec2);
				RoomContents_TransportRoom.SpawnApparel(ThingDefOf.Apparel_Vacsuit, intVec2, CS$<>8__locals1.map);
			}
		}

		// Token: 0x0600AF58 RID: 44888 RVA: 0x00320C10 File Offset: 0x0031EE10
		private static void SpawnApparel(ThingDef apparel, IntVec3 cell, Map map)
		{
			Thing thing = ThingMaker.MakeThing(apparel, null);
			GenPlace.TryPlaceThing(thing, cell, map, ThingPlaceMode.Near, null, null, null, 1);
			thing.TrySetForbidden(true);
		}

		// Token: 0x0400785F RID: 30815
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x04007860 RID: 30816
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(2, 3);

		// Token: 0x04007861 RID: 30817
		private static readonly IntRange RoomSizeRange = new IntRange(5, 12);

		// Token: 0x04007862 RID: 30818
		private const float SubroomRatioMaximum = 1.25f;

		// Token: 0x04007863 RID: 30819
		private CellRect subroom;
	}
}
