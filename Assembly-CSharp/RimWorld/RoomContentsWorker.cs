using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F28 RID: 7976
	public class RoomContentsWorker
	{
		// Token: 0x17001AF4 RID: 6900
		// (get) Token: 0x0600B03F RID: 45119 RVA: 0x003236BC File Offset: 0x003218BC
		// (set) Token: 0x0600B040 RID: 45120 RVA: 0x003236C4 File Offset: 0x003218C4
		public LayoutRoomDef RoomDef { get; private set; }

		// Token: 0x17001AF5 RID: 6901
		// (get) Token: 0x0600B041 RID: 45121 RVA: 0x003236CD File Offset: 0x003218CD
		// (set) Token: 0x0600B042 RID: 45122 RVA: 0x003236D5 File Offset: 0x003218D5
		protected virtual string ThreatSignal { get; set; }

		// Token: 0x0600B043 RID: 45123 RVA: 0x003236DE File Offset: 0x003218DE
		public void Initialize(LayoutRoomDef roomDef)
		{
			this.RoomDef = roomDef;
		}

		// Token: 0x0600B044 RID: 45124 RVA: 0x003236E7 File Offset: 0x003218E7
		private bool GenerateRoof(LayoutRoom room)
		{
			return !this.RoomDef.noRoof && !room.sketch.layoutDef.noRoof;
		}

		// Token: 0x0600B045 RID: 45125 RVA: 0x0032370B File Offset: 0x0032190B
		private IEnumerable<LayoutScatterParms> ScatterParms(LayoutRoom room)
		{
			List<LayoutScatterParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutScatterParms layoutScatterParms in layoutRoomDef.scatter)
				{
					yield return layoutScatterParms;
				}
				enumerator2 = default(List<LayoutScatterParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			using (List<LayoutRoomDef>.Enumerator enumerator3 = room.defs.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					if (!enumerator3.Current.spawnJunk)
					{
						yield break;
					}
				}
			}
			foreach (LayoutScatterParms layoutScatterParms2 in room.sketch.layoutDef.junkScaterrers)
			{
				yield return layoutScatterParms2;
			}
			enumerator2 = default(List<LayoutScatterParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B046 RID: 45126 RVA: 0x0032371B File Offset: 0x0032191B
		private IEnumerable<LayoutScatterTerrainParms> ScatterTerrainParms(LayoutRoom room)
		{
			List<LayoutScatterTerrainParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutScatterTerrainParms layoutScatterTerrainParms in layoutRoomDef.scatterTerrain)
				{
					yield return layoutScatterTerrainParms;
				}
				enumerator2 = default(List<LayoutScatterTerrainParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			foreach (LayoutScatterTerrainParms layoutScatterTerrainParms2 in room.sketch.layoutDef.scatterTerrain)
			{
				yield return layoutScatterTerrainParms2;
			}
			enumerator2 = default(List<LayoutScatterTerrainParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B047 RID: 45127 RVA: 0x0032372B File Offset: 0x0032192B
		private IEnumerable<LayoutFillEdgesParms> FillEdgeParms(LayoutRoom room)
		{
			List<LayoutFillEdgesParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutFillEdgesParms layoutFillEdgesParms in layoutRoomDef.fillEdges)
				{
					yield return layoutFillEdgesParms;
				}
				enumerator2 = default(List<LayoutFillEdgesParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			foreach (LayoutFillEdgesParms layoutFillEdgesParms2 in room.sketch.layoutDef.fillEdges)
			{
				yield return layoutFillEdgesParms2;
			}
			enumerator2 = default(List<LayoutFillEdgesParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B048 RID: 45128 RVA: 0x0032373B File Offset: 0x0032193B
		private IEnumerable<LayoutFillInteriorParms> FillInteriorParms(LayoutRoom room)
		{
			List<LayoutFillInteriorParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutFillInteriorParms layoutFillInteriorParms in layoutRoomDef.fillInterior)
				{
					yield return layoutFillInteriorParms;
				}
				enumerator2 = default(List<LayoutFillInteriorParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			foreach (LayoutFillInteriorParms layoutFillInteriorParms2 in room.sketch.layoutDef.fillInterior)
			{
				yield return layoutFillInteriorParms2;
			}
			enumerator2 = default(List<LayoutFillInteriorParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B049 RID: 45129 RVA: 0x0032374B File Offset: 0x0032194B
		private IEnumerable<LayoutWallAttatchmentParms> WallAttachmentParms(LayoutRoom room)
		{
			StructureLayoutDef structureLayoutDef = room.sketch.layoutDef as StructureLayoutDef;
			if (structureLayoutDef != null && structureLayoutDef.wallLampDef != null)
			{
				yield return new LayoutWallAttatchmentParms
				{
					def = structureLayoutDef.wallLampDef,
					spawnChancePerPosition = structureLayoutDef.wallLampChancePerPosition
				};
			}
			List<LayoutWallAttatchmentParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutWallAttatchmentParms layoutWallAttatchmentParms in layoutRoomDef.wallAttachments)
				{
					yield return layoutWallAttatchmentParms;
				}
				enumerator2 = default(List<LayoutWallAttatchmentParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			foreach (LayoutWallAttatchmentParms layoutWallAttatchmentParms2 in room.sketch.layoutDef.wallAttachments)
			{
				yield return layoutWallAttatchmentParms2;
			}
			enumerator2 = default(List<LayoutWallAttatchmentParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B04A RID: 45130 RVA: 0x0032375B File Offset: 0x0032195B
		private IEnumerable<LayoutPrefabParms> PrefabParms(LayoutRoom room)
		{
			List<LayoutPrefabParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutPrefabParms layoutPrefabParms in layoutRoomDef.prefabs)
				{
					yield return layoutPrefabParms;
				}
				enumerator2 = default(List<LayoutPrefabParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			foreach (LayoutPrefabParms layoutPrefabParms2 in room.sketch.layoutDef.prefabs)
			{
				yield return layoutPrefabParms2;
			}
			enumerator2 = default(List<LayoutPrefabParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B04B RID: 45131 RVA: 0x0032376B File Offset: 0x0032196B
		private IEnumerable<LayoutPartParms> PartParms(LayoutRoom room)
		{
			List<LayoutPartParms>.Enumerator enumerator2;
			foreach (LayoutRoomDef layoutRoomDef in room.defs)
			{
				foreach (LayoutPartParms layoutPartParms in layoutRoomDef.parts)
				{
					yield return layoutPartParms;
				}
				enumerator2 = default(List<LayoutPartParms>.Enumerator);
			}
			List<LayoutRoomDef>.Enumerator enumerator = default(List<LayoutRoomDef>.Enumerator);
			foreach (LayoutPartParms layoutPartParms2 in room.sketch.layoutDef.parts)
			{
				yield return layoutPartParms2;
			}
			enumerator2 = default(List<LayoutPartParms>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600B04C RID: 45132 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
		}

		// Token: 0x0600B04D RID: 45133 RVA: 0x0032377B File Offset: 0x0032197B
		public virtual void PostFillRooms(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			this.TrySpawnParts(map, room, faction, threatPoints, true);
		}

		// Token: 0x0600B04E RID: 45134 RVA: 0x0032378C File Offset: 0x0032198C
		public virtual void FillRoom(Map map, LayoutRoom room, Faction faction, float? threatPoints = null)
		{
			if (this.RoomDef == null)
			{
				return;
			}
			this.TrySetRoof(map, room);
			this.RemoveWalls(map, room);
			this.TryScatterTerrain(map, room);
			this.TrySpawnParts(map, room, faction, threatPoints, false);
			this.TryPlacePrefabs(map, room, faction);
			this.TryFillContents(map, room, faction);
			this.TryScatterContents(map, room, faction);
			this.TryPlaceWallAttachments(map, room, faction);
		}

		// Token: 0x0600B04F RID: 45135 RVA: 0x003237EC File Offset: 0x003219EC
		private void TrySetRoof(Map map, LayoutRoom room)
		{
			if (!this.GenerateRoof(room))
			{
				return;
			}
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 cell in cellRect)
				{
					this.SpawnRoof(map, cell);
				}
			}
		}

		// Token: 0x0600B050 RID: 45136 RVA: 0x00323884 File Offset: 0x00321A84
		protected void SpawnRoof(Map map, IntVec3 cell)
		{
			RoofDef roof = cell.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				map.roofGrid.SetRoof(cell, this.RoomDef.roofDef ?? RoofDefOf.RoofConstructed);
			}
		}

		// Token: 0x0600B051 RID: 45137 RVA: 0x003238C4 File Offset: 0x00321AC4
		private void RemoveWalls(Map map, LayoutRoom room)
		{
			if (!RoomContentsWorker.CanRemoveWalls(room))
			{
				return;
			}
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 intVec in cellRect.EdgeCells)
				{
					if (this.CanRemoveWall(intVec, map, room))
					{
						bool flag = true;
						for (int i = 0; i < 4; i++)
						{
							if ((intVec + GenAdj.CardinalDirections[i]).GetDoor(map) != null)
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							foreach (LayoutRoom layoutRoom in room.sketch.structureLayout.Rooms)
							{
								if (!RoomContentsWorker.CanRemoveWalls(layoutRoom) && layoutRoom.Contains(intVec, 0))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								Building building = map.edificeGrid[intVec];
								if (building != null && building.def.IsWall)
								{
									building.Destroy(DestroyMode.Vanish);
									map.roofGrid.SetRoof(intVec, null);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B052 RID: 45138 RVA: 0x00323A60 File Offset: 0x00321C60
		private void TryScatterTerrain(Map map, LayoutRoom room)
		{
			float num = (float)room.Area / 100f;
			foreach (LayoutScatterTerrainParms layoutScatterTerrainParms in this.ScatterTerrainParms(room))
			{
				int num2;
				if (layoutScatterTerrainParms.groupCount != IntRange.Invalid)
				{
					num2 = layoutScatterTerrainParms.groupCount.RandomInRange;
				}
				else
				{
					num2 = Mathf.RoundToInt(layoutScatterTerrainParms.groupsPerHundredCells.RandomInRange * num);
				}
				num2 = Mathf.Max(num2, layoutScatterTerrainParms.minGroups);
				for (int i = 0; i < num2; i++)
				{
					this.SpawnIrregularTerrainLump(layoutScatterTerrainParms, map, room, null);
				}
			}
		}

		// Token: 0x0600B053 RID: 45139 RVA: 0x00323B10 File Offset: 0x00321D10
		private void TrySpawnParts(Map map, LayoutRoom room, Faction faction, float? threatPoints, bool post)
		{
			float num;
			if (threatPoints != null)
			{
				SimpleCurve threatPointsScaleCurve = this.RoomDef.threatPointsScaleCurve;
				num = ((threatPointsScaleCurve != null) ? threatPointsScaleCurve.Evaluate(threatPoints.Value) : threatPoints.Value);
			}
			else
			{
				num = 300f;
			}
			foreach (LayoutPartParms layoutPartParms in this.PartParms(room))
			{
				int num2 = 1;
				if (layoutPartParms.def.Worker.FillOnPost == post)
				{
					if (layoutPartParms.countRange != IntRange.Invalid)
					{
						num2 = layoutPartParms.countRange.RandomInRange;
					}
					else if (!Rand.Chance(layoutPartParms.chance))
					{
						continue;
					}
					float threatPoints2 = num;
					if (layoutPartParms.threatPointsRange != IntRange.Invalid)
					{
						threatPoints2 = (float)layoutPartParms.threatPointsRange.RandomInRange;
					}
					for (int i = 0; i < num2; i++)
					{
						layoutPartParms.def.Worker.FillRoom(map, room, faction, threatPoints2);
					}
				}
			}
		}

		// Token: 0x0600B054 RID: 45140 RVA: 0x00323C20 File Offset: 0x00321E20
		private void TryPlacePrefabs(Map map, LayoutRoom room, Faction faction)
		{
			RoomContentsWorker.<>c__DisplayClass26_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass26_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			LayoutSketch layoutSketch = CS$<>8__locals1.room.sketch.layoutSketch;
			float num = (float)CS$<>8__locals1.room.Area / 100f;
			float num2 = (float)CS$<>8__locals1.room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			CS$<>8__locals1.wallTuple = new Tuple<ThingDef, ThingDef>(layoutSketch.wall, layoutSketch.wallStuff);
			CS$<>8__locals1.doorTuple = new Tuple<ThingDef, ThingDef>(layoutSketch.door, layoutSketch.doorStuff);
			Dictionary<LayoutPrefabParms, int> dictionary = new Dictionary<LayoutPrefabParms, int>();
			List<LayoutPrefabParms> list = new List<LayoutPrefabParms>();
			foreach (LayoutPrefabParms layoutPrefabParms in this.PrefabParms(CS$<>8__locals1.room))
			{
				list.Add(layoutPrefabParms);
				int num3;
				if (layoutPrefabParms.countRange != IntRange.Invalid)
				{
					num3 = layoutPrefabParms.countRange.RandomInRange;
				}
				else if (layoutPrefabParms.def.edgeOnly)
				{
					num3 = Mathf.RoundToInt(layoutPrefabParms.countPerTenEdgeCells.RandomInRange * num2);
				}
				else
				{
					num3 = Mathf.RoundToInt(layoutPrefabParms.countPerHundredCells.RandomInRange * num);
				}
				num3 = Mathf.Min(Mathf.Max(num3, layoutPrefabParms.minMaxRange.TrueMin), layoutPrefabParms.minMaxRange.TrueMax);
				dictionary[layoutPrefabParms] = num3;
			}
			int num4 = 0;
			while (dictionary.Any<KeyValuePair<LayoutPrefabParms, int>>())
			{
				RoomContentsWorker.<>c__DisplayClass26_1 CS$<>8__locals2 = new RoomContentsWorker.<>c__DisplayClass26_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.parms = list[num4];
				num4 = (num4 + 1) % list.Count;
				if (dictionary.ContainsKey(CS$<>8__locals2.parms))
				{
					Dictionary<LayoutPrefabParms, int> dictionary2 = dictionary;
					LayoutPrefabParms parms = CS$<>8__locals2.parms;
					int num5 = dictionary2[parms] - 1;
					dictionary2[parms] = num5;
					if (num5 <= 0)
					{
						dictionary.Remove(CS$<>8__locals2.parms);
					}
					if (CS$<>8__locals2.parms.def.edgeOnly)
					{
						RoomGenUtility.FillPrefabsAroundEdges(CS$<>8__locals2.parms, 1, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.CS$<>8__locals1.map, new Func<IntVec3, Rot4, bool>(CS$<>8__locals2.<TryPlacePrefabs>g__Validator|1), null, 1, true, faction, new Func<PrefabThingData, Tuple<ThingDef, ThingDef>>(CS$<>8__locals2.CS$<>8__locals1.<TryPlacePrefabs>g__OverrideSpawnData|2));
					}
					else
					{
						RoomGenUtility.FillPrefabs(CS$<>8__locals2.parms, 1, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.CS$<>8__locals1.map, new Func<IntVec3, Rot4, bool>(CS$<>8__locals2.<TryPlacePrefabs>g__Validator|1), 1, null, false, false, faction, new Func<PrefabThingData, Tuple<ThingDef, ThingDef>>(CS$<>8__locals2.CS$<>8__locals1.<TryPlacePrefabs>g__OverrideSpawnData|2));
					}
				}
			}
		}

		// Token: 0x0600B055 RID: 45141 RVA: 0x00323EF4 File Offset: 0x003220F4
		private void TryPlaceWallAttachments(Map map, LayoutRoom room, Faction faction)
		{
			RoomContentsWorker.<>c__DisplayClass27_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass27_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			using (IEnumerator<LayoutWallAttatchmentParms> enumerator = this.WallAttachmentParms(CS$<>8__locals1.room).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RoomContentsWorker.<>c__DisplayClass27_1 CS$<>8__locals2 = new RoomContentsWorker.<>c__DisplayClass27_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.parms = enumerator.Current;
					if (CS$<>8__locals2.parms.countRange != IntRange.Invalid)
					{
						RoomGenUtility.SpawnWallAttatchments(CS$<>8__locals2.parms.def, CS$<>8__locals2.CS$<>8__locals1.map, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.parms.countRange, new Predicate<IntVec3, Rot4>(CS$<>8__locals2.<TryPlaceWallAttachments>g__Validator|0), CS$<>8__locals2.parms.stuff, faction);
					}
					else
					{
						RoomGenUtility.SpawnWallAttatchments(CS$<>8__locals2.parms.def, CS$<>8__locals2.CS$<>8__locals1.map, CS$<>8__locals2.CS$<>8__locals1.room, CS$<>8__locals2.parms.spawnChancePerPosition, CS$<>8__locals2.parms.thingsPer10EdgeCells.RandomInRange, new Predicate<IntVec3, Rot4>(CS$<>8__locals2.<TryPlaceWallAttachments>g__Validator|0), CS$<>8__locals2.parms.stuff, faction);
					}
				}
			}
		}

		// Token: 0x0600B056 RID: 45142 RVA: 0x00324030 File Offset: 0x00322230
		private void TryFillContents(Map map, LayoutRoom room, Faction faction)
		{
			RoomContentsWorker.<>c__DisplayClass28_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass28_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.room = room;
			CS$<>8__locals1.map = map;
			using (IEnumerator<LayoutFillEdgesParms> enumerator = this.FillEdgeParms(CS$<>8__locals1.room).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RoomContentsWorker.<>c__DisplayClass28_1 CS$<>8__locals2 = new RoomContentsWorker.<>c__DisplayClass28_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					CS$<>8__locals2.parms = enumerator.Current;
					if (CS$<>8__locals2.parms.countRange != IntRange.Invalid)
					{
						ThingDef def = CS$<>8__locals2.parms.def;
						int randomInRange = CS$<>8__locals2.parms.countRange.RandomInRange;
						IntRange groupCountRange = CS$<>8__locals2.parms.groupCountRange;
						LayoutRoom room2 = CS$<>8__locals2.CS$<>8__locals1.room;
						Map map2 = CS$<>8__locals2.CS$<>8__locals1.map;
						Func<IntVec3, Rot4, CellRect, bool> validator = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals2.<TryFillContents>g__Validator|0);
						List<Thing> spawned = null;
						ThingDef stuff = CS$<>8__locals2.parms.stuff;
						int padding = CS$<>8__locals2.parms.padding;
						RoomGenUtility.FillAroundEdges(def, randomInRange, groupCountRange, room2, map2, validator, spawned, CS$<>8__locals2.parms.contractedBy, padding, stuff, true, CS$<>8__locals2.parms.rotOffset, null, faction);
					}
					else
					{
						ThingDef def2 = CS$<>8__locals2.parms.def;
						FloatRange groupsPerTenEdgeCells = CS$<>8__locals2.parms.groupsPerTenEdgeCells;
						IntRange groupCountRange2 = CS$<>8__locals2.parms.groupCountRange;
						LayoutRoom room3 = CS$<>8__locals2.CS$<>8__locals1.room;
						Map map3 = CS$<>8__locals2.CS$<>8__locals1.map;
						Func<IntVec3, Rot4, CellRect, bool> validator2 = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals2.<TryFillContents>g__Validator|0);
						List<Thing> spawned2 = null;
						ThingDef stuff = CS$<>8__locals2.parms.stuff;
						int padding = CS$<>8__locals2.parms.padding;
						RoomGenUtility.FillAroundEdges(def2, groupsPerTenEdgeCells, groupCountRange2, room3, map3, validator2, spawned2, CS$<>8__locals2.parms.contractedBy, padding, stuff, true, CS$<>8__locals2.parms.rotOffset, null, faction);
					}
				}
			}
			using (IEnumerator<LayoutFillInteriorParms> enumerator2 = this.FillInteriorParms(CS$<>8__locals1.room).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RoomContentsWorker.<>c__DisplayClass28_2 CS$<>8__locals3 = new RoomContentsWorker.<>c__DisplayClass28_2();
					CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
					CS$<>8__locals3.parms = enumerator2.Current;
					if (CS$<>8__locals3.parms.countRange != IntRange.Invalid)
					{
						ThingDef def3 = CS$<>8__locals3.parms.def;
						int randomInRange2 = CS$<>8__locals3.parms.countRange.RandomInRange;
						LayoutRoom room4 = CS$<>8__locals3.CS$<>8__locals2.room;
						Map map4 = CS$<>8__locals3.CS$<>8__locals2.map;
						Rot4? fixedRot = CS$<>8__locals3.parms.fixedRot;
						Func<IntVec3, Rot4, CellRect, bool> validator3 = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals3.<TryFillContents>g__Validator|1);
						List<Thing> spawned3 = null;
						ThingDef stuff = CS$<>8__locals3.parms.stuff;
						RoomGenUtility.FillWithPadding(def3, randomInRange2, room4, map4, fixedRot, validator3, spawned3, CS$<>8__locals3.parms.contractedBy, stuff, CS$<>8__locals3.parms.alignWithRect, CS$<>8__locals3.parms.snapToGrid, null, faction);
					}
					else
					{
						ThingDef def4 = CS$<>8__locals3.parms.def;
						FloatRange thingsPerHundredCells = CS$<>8__locals3.parms.thingsPerHundredCells;
						LayoutRoom room5 = CS$<>8__locals3.CS$<>8__locals2.room;
						Map map5 = CS$<>8__locals3.CS$<>8__locals2.map;
						Rot4? fixedRot2 = CS$<>8__locals3.parms.fixedRot;
						Func<IntVec3, Rot4, CellRect, bool> validator4 = new Func<IntVec3, Rot4, CellRect, bool>(CS$<>8__locals3.<TryFillContents>g__Validator|1);
						List<Thing> spawned4 = null;
						ThingDef stuff = CS$<>8__locals3.parms.stuff;
						RoomGenUtility.FillWithPadding(def4, thingsPerHundredCells, room5, map5, fixedRot2, validator4, spawned4, CS$<>8__locals3.parms.contractedBy, stuff, CS$<>8__locals3.parms.alignWithRect, CS$<>8__locals3.parms.snapToGrid, null, faction);
					}
				}
			}
		}

		// Token: 0x0600B057 RID: 45143 RVA: 0x0032436C File Offset: 0x0032256C
		private void TryScatterContents(Map map, LayoutRoom room, Faction faction)
		{
			float num = (float)room.Area / 100f;
			foreach (LayoutScatterParms layoutScatterParms in this.ScatterParms(room))
			{
				int num2;
				if (layoutScatterParms.groupCount != IntRange.Invalid)
				{
					num2 = layoutScatterParms.groupCount.RandomInRange;
				}
				else
				{
					num2 = Mathf.RoundToInt(layoutScatterParms.groupsPerHundredCells.RandomInRange * num);
				}
				num2 = Mathf.Max(num2, layoutScatterParms.minGroups);
				for (int i = 0; i < num2; i++)
				{
					this.SpawnIrregularLump(layoutScatterParms, map, room, faction, null, null);
				}
			}
		}

		// Token: 0x0600B058 RID: 45144 RVA: 0x00324420 File Offset: 0x00322620
		private void SpawnIrregularLump(LayoutScatterParms parms, Map map, LayoutRoom room, Faction faction, List<IntVec3> area = null, List<Thing> spawned = null)
		{
			IntVec3 pos;
			if (!room.TryGetRandomCellInRoom(map, out pos, 2, 0, (IntVec3 c) => this.IsValidScatterCell(parms, c, room, map), false))
			{
				return;
			}
			GenSpawn.SpawnIrregularLump(parms.def, pos, map, parms.itemsPerGroup, parms.groupDistRange, WipeMode.Vanish, (IntVec3 c) => this.IsValidScatterCell(parms, c, room, map), area, spawned, parms.stuff, faction);
		}

		// Token: 0x0600B059 RID: 45145 RVA: 0x003244C4 File Offset: 0x003226C4
		private void SpawnIrregularTerrainLump(LayoutScatterTerrainParms parms, Map map, LayoutRoom room, List<IntVec3> area = null)
		{
			RoomContentsWorker.<>c__DisplayClass31_0 CS$<>8__locals1 = new RoomContentsWorker.<>c__DisplayClass31_0();
			CS$<>8__locals1.room = room;
			IntVec3 center;
			if (!CS$<>8__locals1.room.TryGetRandomCellInRoom(map, out center, 2, 0, new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnIrregularTerrainLump>g__Validator|0), false))
			{
				return;
			}
			int randomInRange = parms.groupDistRange.RandomInRange;
			int randomInRange2 = parms.itemsPerGroup.RandomInRange;
			int num = 0;
			List<IntVec3> list = GridShapeMaker.IrregularLump(center, map, randomInRange, new Predicate<IntVec3>(CS$<>8__locals1.<SpawnIrregularTerrainLump>g__Validator|0));
			for (int i = 0; i < list.Count; i++)
			{
				IntVec3 intVec = list[i];
				if (Rand.DynamicChance(num, randomInRange2, list.Count - i))
				{
					num++;
					map.terrainGrid.SetTerrain(intVec, parms.def);
					if (area != null)
					{
						area.Add(intVec);
					}
				}
			}
		}

		// Token: 0x0600B05A RID: 45146 RVA: 0x0032458C File Offset: 0x0032278C
		protected virtual bool IsValidCellBase(ThingDef thing, ThingDef stuff, IntVec3 cell, LayoutRoom room, Map map)
		{
			TerrainAffordanceDef terrainAffordanceNeed = thing.GetTerrainAffordanceNeed(stuff);
			return room.Contains(cell, 1) && !cell.IsBuildingInteractionCell(map) && (terrainAffordanceNeed == null || cell.GetAffordances(map).Contains(terrainAffordanceNeed)) && (thing.passability != Traversability.Impassable || cell.GetFirstPawn(map) == null) && cell.GetFirstBuilding(map) == null && (thing.IsFilth || !RoomGenUtility.IsDoorAdjacentTo(cell, map, true)) && (!cell.GetTerrain(map).exposesToVacuum || cell.GetRoof(map) != null);
		}

		// Token: 0x0600B05B RID: 45147 RVA: 0x00324618 File Offset: 0x00322818
		protected virtual bool IsValidScatterCell(LayoutScatterParms parms, IntVec3 cell, LayoutRoom room, Map map)
		{
			if (parms.def.passability == Traversability.Impassable && !RoomGenUtility.IsClearAndNotAdjacentToDoor(parms.def, cell, map, Rot4.North))
			{
				return false;
			}
			foreach (Thing thing in cell.GetThingList(map))
			{
				if (parms.def.IsFilth && thing.def.IsFilth)
				{
					return false;
				}
				if (!parms.def.IsFilth && !thing.def.IsFilth)
				{
					return false;
				}
			}
			return this.IsValidCellBase(parms.def, parms.stuff, cell, room, map);
		}

		// Token: 0x0600B05C RID: 45148 RVA: 0x003246E0 File Offset: 0x003228E0
		protected virtual bool IsValidFillEdgeCell(LayoutFillEdgesParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			CellRect cellRect = cell.RectAbout(parms.def.Size, rot);
			foreach (IntVec3 cell2 in cellRect)
			{
				if (!this.IsValidCellBase(parms.def, parms.stuff, cell2, room, map))
				{
					return false;
				}
			}
			if (parms.def.Fillage != FillCategory.Full)
			{
				return true;
			}
			foreach (IntVec3 intVec in cellRect.ExpandedBy(1).EdgeCells)
			{
				if (room.Contains(intVec, 2) && !intVec.Standable(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B05D RID: 45149 RVA: 0x003247D0 File Offset: 0x003229D0
		protected virtual bool IsValidPrefabRect(LayoutPrefabParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			CellRect cellRect = cell.RectAbout(parms.def.size, rot);
			RoomContentsWorker.tmpOccupiedEdges.Clear();
			foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple in PrefabUtility.GetThings(parms.def, cell, rot))
			{
				PrefabThingData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				Rot4 item3 = valueTuple.Item3;
				foreach (IntVec3 intVec in item2.RectAbout(item.def.Size, item3).Cells)
				{
					if (!this.IsValidCellBase(item.def, item.stuff, intVec, room, map))
					{
						return false;
					}
					if (cellRect.EdgeCells.Contains(intVec) && !intVec.Standable(map))
					{
						for (int i = 0; i < 8; i++)
						{
							IntVec3 intVec2 = intVec + GenAdj.AdjacentCellsAround[i];
							if (!cellRect.Contains(intVec2) && !intVec2.Standable(map) && !cellRect.ExpandedBy(1).IsCorner(intVec2) && !RoomContentsWorker.tmpOccupiedEdges.Contains(intVec2))
							{
								RoomContentsWorker.tmpOccupiedEdges.Add(intVec2);
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600B05E RID: 45150 RVA: 0x00324970 File Offset: 0x00322B70
		protected virtual bool IsValidFillInteriorCell(LayoutFillInteriorParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			CellRect cellRect = cell.RectAbout(parms.def.Size, rot);
			foreach (IntVec3 cell2 in cellRect)
			{
				if (!this.IsValidCellBase(parms.def, parms.stuff, cell2, room, map))
				{
					return false;
				}
			}
			if (parms.def.Fillage != FillCategory.Full)
			{
				return true;
			}
			foreach (IntVec3 intVec in cellRect.ExpandedBy(1).EdgeCells)
			{
				if (room.Contains(intVec, 2) && !intVec.Standable(map))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B05F RID: 45151 RVA: 0x00324A60 File Offset: 0x00322C60
		protected virtual bool IsValidWallAttachmentCell(LayoutWallAttatchmentParms parms, IntVec3 cell, Rot4 rot, LayoutRoom room, Map map)
		{
			Building edifice = (cell + rot.FacingCell).GetEdifice(map);
			return ((edifice != null) ? edifice.def : null) == room.sketch.layoutSketch.wall;
		}

		// Token: 0x0600B060 RID: 45152 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool CanRemoveWall(IntVec3 cell, Map map, LayoutRoom room)
		{
			return true;
		}

		// Token: 0x0600B061 RID: 45153 RVA: 0x00324A98 File Offset: 0x00322C98
		private static bool CanRemoveWalls(LayoutRoom room)
		{
			using (List<LayoutRoomDef>.Enumerator enumerator = room.defs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.canRemoveBorderWalls)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x040078C6 RID: 30918
		private const int DefaultThreatPoints = 300;

		// Token: 0x040078C9 RID: 30921
		private static readonly List<IntVec3> tmpOccupiedEdges = new List<IntVec3>();
	}
}
