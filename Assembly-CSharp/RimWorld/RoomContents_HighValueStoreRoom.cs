using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ED9 RID: 7897
	public class RoomContents_HighValueStoreRoom : RoomContentsWorker
	{
		// Token: 0x0600AEF7 RID: 44791 RVA: 0x0031EA80 File Offset: 0x0031CC80
		public override void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			RoomContents_HighValueStoreRoom.SpawnShelves(map, room);
			RoomGenUtility.SpawnHermeticCrateInRoom(room, map, ThingSetMakerDefOf.MapGen_HighValueCrate, true, null, null);
			base.FillRoom(map, room, faction, threatPoints);
		}

		// Token: 0x0600AEF8 RID: 44792 RVA: 0x0031EAB8 File Offset: 0x0031CCB8
		private static void SpawnShelves(Map map, LayoutRoom room)
		{
			RoomContents_HighValueStoreRoom.<>c__DisplayClass4_0 CS$<>8__locals1 = new RoomContents_HighValueStoreRoom.<>c__DisplayClass4_0();
			CS$<>8__locals1.map = map;
			CS$<>8__locals1.possibleSpots = new List<IntVec3>(6);
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(RoomContents_HighValueStoreRoom.ShelvesPer10EdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(ThingDefOf.Shelf, count, RoomContents_HighValueStoreRoom.ShelfGroupSizeRange, room, CS$<>8__locals1.map, null, null, 1, 0, null, true, RotationDirection.Opposite, new Func<IntVec3, Rot4, Map, Thing>(CS$<>8__locals1.<SpawnShelves>g__SpawnAction|1), null);
			int num2 = RoomContents_HighValueStoreRoom.GravlitePanelCount.RandomInRange;
			while (num2 > 0 && CS$<>8__locals1.possibleSpots.Any<IntVec3>())
			{
				IntVec3 intVec = CS$<>8__locals1.possibleSpots.RandomElement<IntVec3>();
				CS$<>8__locals1.possibleSpots.Remove(intVec);
				Thing thing = ThingMaker.MakeThing(ThingDefOf.GravlitePanel, null);
				thing.stackCount = Mathf.Min(num2, thing.def.stackLimit);
				num2 -= thing.stackCount;
				GenPlace.TryPlaceThing(thing, intVec, CS$<>8__locals1.map, ThingPlaceMode.Near, null, null, null, 1);
			}
		}

		// Token: 0x0400783B RID: 30779
		private static readonly FloatRange ShelvesPer10EdgeCells = new FloatRange(1f, 3f);

		// Token: 0x0400783C RID: 30780
		private static readonly IntRange ShelfGroupSizeRange = new IntRange(2, 3);

		// Token: 0x0400783D RID: 30781
		private static readonly IntRange GravlitePanelCount = new IntRange(150, 200);
	}
}
