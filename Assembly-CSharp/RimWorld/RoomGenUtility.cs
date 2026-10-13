using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001F3A RID: 7994
	public static class RoomGenUtility
	{
		// Token: 0x0600B0C5 RID: 45253 RVA: 0x00326098 File Offset: 0x00324298
		public static void FillWithPadding(ThingDef thing, FloatRange countPer100Cells, LayoutRoom room, Map map, Rot4? fixedRot = null, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, ThingDef stuff = null, bool alignWithRect = false, bool snapToGrid = false, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.Area) / 100f;
			int count = Mathf.Max(Mathf.RoundToInt(countPer100Cells.RandomInRange * num), 1);
			RoomGenUtility.FillWithPadding(thing, count, room, map, fixedRot, validator, spawned, contractedBy, stuff, alignWithRect, snapToGrid, spawnAction, faction);
		}

		// Token: 0x0600B0C6 RID: 45254 RVA: 0x00326108 File Offset: 0x00324308
		public static void FillWithPadding(ThingDef thing, int count, LayoutRoom room, Map map, Rot4? fixedRot = null, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, ThingDef stuff = null, bool alignWithRect = false, bool snapToGrid = false, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			IntVec2 size = thing.Size;
			int num = 0;
			foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
			{
				if (num >= count)
				{
					break;
				}
				if (room.Contains(intVec, contractedBy))
				{
					foreach (Rot4 rot in Rot4.AllRotations.InRandomOrder(null))
					{
						if (num >= count)
						{
							break;
						}
						if ((fixedRot == null || !(rot != fixedRot.Value)) && (thing.rotatable || !(rot != Rot4.North)))
						{
							CellRect cellRect;
							room.TryGetRectContainingCell(intVec, out cellRect);
							if (alignWithRect)
							{
								Rot4 b = (cellRect.Width >= cellRect.Height) ? Rot4.East : Rot4.North;
								if (rot != b)
								{
									continue;
								}
							}
							if (snapToGrid)
							{
								bool flag = intVec.x - cellRect.minX != 0;
								int num2 = intVec.z - cellRect.minZ;
								if ((flag ? 1 : 0) % (size.x + 1) != 0 || num2 % (size.z + 1) != 0)
								{
									continue;
								}
							}
							CellRect arg = intVec.RectAbout(size, rot);
							bool flag2 = true;
							if (validator == null || validator(intVec, rot, arg))
							{
								foreach (IntVec3 cell in arg.Cells)
								{
									if (!RoomGenUtility.IsFillValidCell(thing, cell, room, map, contractedBy, default(CellRect)))
									{
										flag2 = false;
										break;
									}
								}
								if (flag2)
								{
									Thing thing2;
									if (spawnAction != null)
									{
										thing2 = spawnAction(intVec, rot, map);
										if (thing2.def.Size.x > size.x || thing2.def.Size.z > size.z)
										{
											Log.ErrorOnce(string.Format("Spawned thing {0} with size {1} during room edge filling with larger size as root def {2} with size {3}. Size must be the same or smaller.", new object[]
											{
												thing2.def.defName,
												thing2.def.Size,
												thing.defName,
												thing.Size
											}), thing2.def.GetHashCode() ^ 2945741);
										}
									}
									else
									{
										thing2 = ThingMaker.MakeThing(thing, stuff);
										thing2.SetFactionDirect(faction);
										GenSpawn.Spawn(thing2, intVec, map, rot, WipeMode.Vanish, false, false);
									}
									num++;
									if (spawned != null)
									{
										spawned.Add(thing2);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B0C7 RID: 45255 RVA: 0x00326400 File Offset: 0x00324600
		public static void FillPrefabs(LayoutPrefabParms parms, int count, LayoutRoom room, Map map, Func<IntVec3, Rot4, bool> validator = null, int contractedBy = 1, List<Thing> spawned = null, bool alignWithRect = false, bool snapToGrid = false, Faction faction = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null)
		{
			IntVec2 size = parms.def.size;
			int num = 0;
			foreach (IntVec3 intVec in room.Cells.InRandomOrder(null))
			{
				if (num >= count)
				{
					break;
				}
				if (room.Contains(intVec, contractedBy))
				{
					foreach (Rot4 rot in Rot4.AllRotations.InRandomOrder(null))
					{
						if (num >= count)
						{
							break;
						}
						if (parms.def.rotations.HasFlag(rot))
						{
							CellRect cellRect;
							if (alignWithRect && room.TryGetRectContainingCell(intVec, out cellRect))
							{
								Rot4 b = (cellRect.Width >= cellRect.Height) ? Rot4.East : Rot4.North;
								if (rot != b)
								{
									continue;
								}
							}
							if ((!snapToGrid || (intVec.x % (size.x + 1) == 0 && intVec.z % (size.z + 1) == 0)) && (validator == null || validator(intVec, rot)))
							{
								bool flag = true;
								CellRect ignore = default(CellRect);
								if (!parms.ensureNoBlocks)
								{
									ignore = map.BoundsRect(0);
								}
								foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple in PrefabUtility.GetThings(parms.def, intVec, rot))
								{
									PrefabThingData item = valueTuple.Item1;
									IntVec3 item2 = valueTuple.Item2;
									Rot4 item3 = valueTuple.Item3;
									if (item.def.passability != Traversability.Standable)
									{
										foreach (IntVec3 cell in item2.RectAbout(item.def.Size, item3))
										{
											if (!RoomGenUtility.IsFillValidCell(item.def, cell, room, map, contractedBy, ignore))
											{
												flag = false;
												break;
											}
										}
									}
								}
								if (flag)
								{
									PrefabUtility.SpawnPrefab(parms.def, map, intVec, rot, faction, spawned, overrideSpawnData, null, false);
									num++;
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600B0C8 RID: 45256 RVA: 0x003266A8 File Offset: 0x003248A8
		public static void GenerateRows(ThingDef thingDef, LayoutRoom room, Map map, ThingDef stuff = null)
		{
			CellRect rect = room.Boundary.ContractedBy(2);
			if (stuff == null)
			{
				stuff = GenStuff.DefaultStuffFor(thingDef);
			}
			bool @bool = Rand.Bool;
			if (rect.Height < rect.Width)
			{
				bool flag = false;
				for (int i = 0; i < rect.Width; i++)
				{
					if (!flag)
					{
						for (int j = 0; j < rect.Height; j++)
						{
							IntVec3 cell = rect.Min + new IntVec3(i, 0, j);
							Rot4 rot = @bool ? Rot4.West : Rot4.East;
							RoomGenUtility.CheckAndSpawnShelf(thingDef, map, stuff, cell, rot, rect);
						}
					}
					flag = !flag;
				}
				return;
			}
			bool flag2 = false;
			for (int k = 0; k < rect.Height; k++)
			{
				if (!flag2)
				{
					for (int l = 0; l < rect.Width; l++)
					{
						IntVec3 cell2 = rect.Min + new IntVec3(l, 0, k);
						Rot4 rot2 = @bool ? Rot4.South : Rot4.North;
						RoomGenUtility.CheckAndSpawnShelf(thingDef, map, stuff, cell2, rot2, rect);
					}
				}
				flag2 = !flag2;
			}
		}

		// Token: 0x0600B0C9 RID: 45257 RVA: 0x003267C4 File Offset: 0x003249C4
		private static void CheckAndSpawnShelf(ThingDef thingDef, Map map, ThingDef stuff, IntVec3 cell, Rot4 rot, CellRect rect)
		{
			if (!GenAdj.OccupiedRect(cell, rot, thingDef.size).FullyContainedWithin(rect))
			{
				return;
			}
			if (GenAdj.OccupiedRect(cell, rot, thingDef.size).Any((IntVec3 c) => c.GetEdifice(map) != null))
			{
				return;
			}
			if (GenAdj.OccupiedRect(cell, rot, thingDef.size).All((IntVec3 c) => c.GetFirstThing(map, thingDef) == null))
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, stuff), cell, map, rot, WipeMode.Vanish, false, false);
			}
		}

		// Token: 0x0600B0CA RID: 45258 RVA: 0x0032687C File Offset: 0x00324A7C
		private static bool IsFillValidCell(ThingDef placing, IntVec3 cell, LayoutRoom room, Map map, int contractedBy, CellRect ignore = default(CellRect))
		{
			if (!room.Contains(cell, contractedBy))
			{
				return false;
			}
			if (cell.GetEdifice(map) != null)
			{
				return false;
			}
			if (placing.passability != Traversability.Standable)
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c = cell + GenAdj.AdjacentCellsAround[i];
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def.passability != Traversability.Standable && (ignore == default(CellRect) || !ignore.Contains(c)))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B0CB RID: 45259 RVA: 0x00326900 File Offset: 0x00324B00
		public static void SpawnInCorners(ThingDef thing, LayoutRoom room, Map map, ThingDef stuff = null, float spawnChance = 1f, Rot4 rot = default(Rot4))
		{
			foreach (CellRect rhs in room.rects)
			{
				foreach (IntVec3 intVec in rhs.ContractedBy(1).Corners)
				{
					bool flag = true;
					foreach (CellRect lhs in room.rects)
					{
						if (lhs != rhs && lhs.Contains(intVec))
						{
							flag = false;
							break;
						}
					}
					int num = 0;
					for (int i = 0; i < 4; i++)
					{
						if ((intVec + GenAdj.CardinalDirections[i]).GetEdifice(map) != null)
						{
							num++;
						}
					}
					if (num >= 2 && flag && Rand.Chance(spawnChance))
					{
						GenSpawn.Spawn(ThingMaker.MakeThing(thing, stuff), intVec, map, rot, WipeMode.Vanish, false, false);
					}
				}
			}
		}

		// Token: 0x0600B0CC RID: 45260 RVA: 0x00326A50 File Offset: 0x00324C50
		public static void FillAroundEdges(ThingDef thing, FloatRange groupsPerTenEdgeCells, IntRange groupCountRange, LayoutRoom room, Map map, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, int padding = 0, ThingDef stuff = null, bool avoidDoors = true, RotationDirection rotationDirectionOffset = RotationDirection.Opposite, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			float num = (float)room.rects.Sum((CellRect r) => r.ContractedBy(1).EdgeCellsCount) / 10f;
			int count = Mathf.Max(Mathf.RoundToInt(groupsPerTenEdgeCells.RandomInRange * num), 1);
			RoomGenUtility.FillAroundEdges(thing, count, groupCountRange, room, map, validator, spawned, contractedBy, padding, stuff, avoidDoors, rotationDirectionOffset, spawnAction, faction);
		}

		// Token: 0x0600B0CD RID: 45261 RVA: 0x00326AC4 File Offset: 0x00324CC4
		public static void FillAroundEdges(ThingDef thing, int count, IntRange groupCountRange, LayoutRoom room, Map map, Func<IntVec3, Rot4, CellRect, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, int padding = 0, ThingDef stuff = null, bool avoidDoors = true, RotationDirection rotationDirectionOffset = RotationDirection.Opposite, Func<IntVec3, Rot4, Map, Thing> spawnAction = null, Faction faction = null)
		{
			IntVec2 size = thing.Size;
			Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> edgeSpawnOptions = RoomGenUtility.GetEdgeSpawnOptions(room, map, contractedBy);
			if (edgeSpawnOptions.Count == 0)
			{
				return;
			}
			if (edgeSpawnOptions.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			while (num2 < count && num < count)
			{
				KeyValuePair<IntVec3, ValueTuple<Rot4, CellRect>> keyValuePair = edgeSpawnOptions.RandomElement<KeyValuePair<IntVec3, ValueTuple<Rot4, CellRect>>>();
				edgeSpawnOptions.Remove(keyValuePair.Key);
				IntVec3 key = keyValuePair.Key;
				ValueTuple<Rot4, CellRect> value = keyValuePair.Value;
				Rot4 item = value.Item1;
				CellRect item2 = value.Item2;
				Rot4 rot = item.Rotated(rotationDirectionOffset);
				int randomInRange = groupCountRange.RandomInRange;
				int num3 = 0;
				bool flag = rotationDirectionOffset == RotationDirection.Clockwise || rotationDirectionOffset == RotationDirection.Counterclockwise;
				int num4 = 0;
				while (num4 < randomInRange && num < count)
				{
					int num5 = num3;
					if (num4 % 2 == 1)
					{
						num5 = -num5;
					}
					else
					{
						num3 += (flag ? size.z : size.x) + padding;
					}
					IntVec3 intVec = key + item.RighthandCell * num5;
					CellRect arg = intVec.RectAbout(size, rot);
					bool flag2 = false;
					foreach (CellRect cellRect in room.rects)
					{
						if (arg.FullyContainedWithin(cellRect.ContractedBy(1)))
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						arg = arg.ExpandedBy(padding);
						flag2 = true;
						if (validator == null || validator(key, rot, arg))
						{
							foreach (IntVec3 intVec2 in arg.Cells)
							{
								if (room.Contains(intVec2, 1))
								{
									if (avoidDoors && RoomGenUtility.IsDoorAdjacentTo(intVec2, map, true))
									{
										flag2 = false;
										break;
									}
									if (!RoomGenUtility.IsFillValidCell(thing, intVec2, room, map, contractedBy, item2))
									{
										flag2 = false;
										break;
									}
								}
							}
							if (!flag2)
							{
								break;
							}
							Thing thing2;
							if (spawnAction != null)
							{
								thing2 = spawnAction(intVec, rot, map);
								if (thing2.def.Size != size)
								{
									Log.ErrorOnce(string.Format("Spawned thing {0} with size {1} during room edge filling which did not have same size as root def {2} with size {3}. Size must match.", new object[]
									{
										thing2.def.defName,
										thing2.def.Size,
										thing.defName,
										thing.Size
									}), thing2.def.GetHashCode() ^ 2945741);
								}
							}
							else
							{
								thing2 = ThingMaker.MakeThing(thing, stuff);
								thing2.SetFaction(faction, null);
								GenSpawn.Spawn(thing2, intVec, map, rot, WipeMode.Vanish, false, false);
							}
							num++;
							foreach (IntVec3 key2 in arg.Cells)
							{
								edgeSpawnOptions.Remove(key2);
							}
							if (spawned != null)
							{
								spawned.Add(thing2);
							}
						}
					}
					num4++;
				}
				num2++;
			}
		}

		// Token: 0x0600B0CE RID: 45262 RVA: 0x00326DDC File Offset: 0x00324FDC
		public static void FillPrefabsAroundEdges(LayoutPrefabParms parms, int count, LayoutRoom room, Map map, Func<IntVec3, Rot4, bool> validator = null, List<Thing> spawned = null, int contractedBy = 1, bool avoidDoors = true, Faction faction = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null)
		{
			Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> edgeSpawnOptions = RoomGenUtility.GetEdgeSpawnOptions(room, map, contractedBy);
			if (edgeSpawnOptions.Count == 0)
			{
				return;
			}
			int num = 0;
			int num2 = 0;
			while (num2 < count && num < count)
			{
				if (edgeSpawnOptions.Count == 0)
				{
					return;
				}
				IntVec3 intVec;
				ValueTuple<Rot4, CellRect> valueTuple;
				edgeSpawnOptions.RandomElement<KeyValuePair<IntVec3, ValueTuple<Rot4, CellRect>>>().Deconstruct(out intVec, out valueTuple);
				ValueTuple<Rot4, CellRect> valueTuple2 = valueTuple;
				IntVec3 intVec2 = intVec;
				Rot4 item = valueTuple2.Item1;
				CellRect item2 = valueTuple2.Item2;
				edgeSpawnOptions.Remove(intVec2);
				Rot4 rot = item.Rotated(parms.rotOffset);
				if (parms.def.rotations.HasFlag(rot))
				{
					CellRect cellRect = intVec2.RectAbout(parms.def.size, rot);
					IntVec3 offset = -item.FacingCell;
					int num3 = room.Boundary.GetSideLength(item.Rotated(RotationDirection.Clockwise));
					while (!cellRect.GetEdgeCells(item).Contains(intVec2) && --num3 > 0)
					{
						cellRect = cellRect.MovedBy(offset);
					}
					bool flag = false;
					foreach (CellRect cellRect2 in room.rects)
					{
						if (cellRect.FullyContainedWithin(cellRect2.ContractedBy(1)))
						{
							flag = true;
							break;
						}
					}
					if (flag && (validator == null || validator(cellRect.CenterCell, rot)))
					{
						foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple3 in PrefabUtility.GetThings(parms.def, cellRect.CenterCell, rot))
						{
							PrefabThingData item3 = valueTuple3.Item1;
							IntVec3 item4 = valueTuple3.Item2;
							Rot4 item5 = valueTuple3.Item3;
							if (item3.def.passability != Traversability.Standable)
							{
								foreach (IntVec3 cell in item4.RectAbout(item3.def.Size, item5))
								{
									if (avoidDoors && RoomGenUtility.IsDoorAdjacentTo(cell, map, true))
									{
										flag = false;
										break;
									}
									if (!RoomGenUtility.IsFillValidCell(item3.def, cell, room, map, contractedBy, item2))
									{
										flag = false;
										break;
									}
								}
							}
						}
						if (flag)
						{
							PrefabUtility.SpawnPrefab(parms.def, map, cellRect.CenterCell, rot, faction, spawned, overrideSpawnData, null, false);
							num++;
							foreach (IntVec3 key in cellRect.Cells)
							{
								edgeSpawnOptions.Remove(key);
							}
						}
					}
				}
				num2++;
			}
		}

		// Token: 0x0600B0CF RID: 45263 RVA: 0x003270BC File Offset: 0x003252BC
		[return: TupleElementNames(new string[]
		{
			"rot",
			"wall"
		})]
		private static Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> GetEdgeSpawnOptions(LayoutRoom room, Map map, int contractedBy)
		{
			Dictionary<IntVec3, ValueTuple<Rot4, CellRect>> dictionary = new Dictionary<IntVec3, ValueTuple<Rot4, CellRect>>();
			foreach (CellRect rhs in room.rects)
			{
				CellRect cellRect = rhs.ContractedBy(contractedBy);
				for (int i = 0; i < 4; i++)
				{
					Rot4 rot = new Rot4(i);
					CellRect edgeRect = rhs.GetEdgeRect(rot);
					foreach (IntVec3 intVec in cellRect.GetEdgeCells(rot))
					{
						bool flag = true;
						if (!dictionary.ContainsKey(intVec) && (intVec + rot.FacingCell).GetEdifice(map) != null)
						{
							foreach (CellRect lhs in room.rects)
							{
								if (!(lhs == rhs) && lhs.Contains(intVec))
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								dictionary[intVec] = new ValueTuple<Rot4, CellRect>(rot, edgeRect);
							}
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x0600B0D0 RID: 45264 RVA: 0x00327240 File Offset: 0x00325440
		public static bool IsDoorAdjacentTo(IntVec3 cell, Map map, bool cardinalOnly = true)
		{
			return RoomGenUtility.IsAdjacentTo(cell, (IntVec3 p) => p.GetDoor(map) != null, cardinalOnly);
		}

		// Token: 0x0600B0D1 RID: 45265 RVA: 0x00327270 File Offset: 0x00325470
		public static bool IsAdjacentTo(IntVec3 cell, Predicate<IntVec3> predicate, bool cardinalOnly = false)
		{
			IntVec3[] array = cardinalOnly ? GenAdj.CardinalDirections : GenAdj.AdjacentCellsAround;
			for (int i = 0; i < array.Length; i++)
			{
				if (predicate(cell + array[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B0D2 RID: 45266 RVA: 0x003272B4 File Offset: 0x003254B4
		public static Building_AncientCryptosleepCasket SpawnCryptoCasket(IntVec3 cell, Map map, Rot4 rot, int groupID, PodContentsType type, ThingSetMakerDef thingSetMakerDef)
		{
			Building_AncientCryptosleepCasket building_AncientCryptosleepCasket = (Building_AncientCryptosleepCasket)ThingMaker.MakeThing(ThingDefOf.AncientCryptosleepCasket, null);
			building_AncientCryptosleepCasket.groupID = groupID;
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.podContentsType = new PodContentsType?(type);
			List<Thing> list = thingSetMakerDef.root.Generate(parms);
			for (int i = 0; i < list.Count; i++)
			{
				if (!building_AncientCryptosleepCasket.TryAcceptThing(list[i], false))
				{
					Pawn pawn = list[i] as Pawn;
					if (pawn != null)
					{
						Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
					}
					else
					{
						list[i].Destroy(DestroyMode.Vanish);
					}
				}
			}
			GenSpawn.Spawn(building_AncientCryptosleepCasket, cell, map, rot, WipeMode.Vanish, false, false);
			return building_AncientCryptosleepCasket;
		}

		// Token: 0x0600B0D3 RID: 45267 RVA: 0x0032735A File Offset: 0x0032555A
		public static SignalAction_OpenCasket SpawnOpenCryptoCasketSignal(Building_AncientCryptosleepCasket pod, Map map, string signalTag)
		{
			SignalAction_OpenCasket signalAction_OpenCasket = (SignalAction_OpenCasket)ThingMaker.MakeThing(ThingDefOf.SignalAction_OpenCasket, null);
			signalAction_OpenCasket.signalTag = signalTag;
			signalAction_OpenCasket.caskets.Add(pod);
			GenSpawn.Spawn(signalAction_OpenCasket, pod.Position, map, WipeMode.Vanish);
			return signalAction_OpenCasket;
		}

		// Token: 0x0600B0D4 RID: 45268 RVA: 0x0032738E File Offset: 0x0032558E
		public static void SpawnDormantThreatCluster(LayoutRoom room, Map map, float threatPoints, string signalTag = null)
		{
			if (Rand.Chance(0.5f) && Faction.OfInsects != null)
			{
				RoomGenUtility.SpawnDormantInsectCluster(room, map, threatPoints, signalTag);
				return;
			}
			if (Faction.OfMechanoids != null)
			{
				RoomGenUtility.SpawnDormantMechCluster(room, map, threatPoints, signalTag);
			}
		}

		// Token: 0x0600B0D5 RID: 45269 RVA: 0x003273BD File Offset: 0x003255BD
		public static void SpawnDormantInsectCluster(LayoutRoom room, Map map, float threatPoints, string signalTag = null)
		{
			RoomGenUtility.SpawnDormantThreatCluster(room, map, Faction.OfInsects, (PawnKindDef kind) => kind.RaceProps.Insect, threatPoints, signalTag);
		}

		// Token: 0x0600B0D6 RID: 45270 RVA: 0x003273EC File Offset: 0x003255EC
		public static void SpawnDormantMechCluster(LayoutRoom room, Map map, float threatPoints, string signalTag = null)
		{
			RoomGenUtility.SpawnDormantThreatCluster(room, map, Faction.OfMechanoids, new Func<PawnKindDef, bool>(MechClusterGenerator.MechKindSuitableForCluster), threatPoints, signalTag);
		}

		// Token: 0x0600B0D7 RID: 45271 RVA: 0x00327408 File Offset: 0x00325608
		public static void SpawnDormantThreatCluster(LayoutRoom room, Map map, Faction faction, Func<PawnKindDef, bool> selector, float threatPoints, string signalTag = null)
		{
			RoomGenUtility.<>c__DisplayClass18_0 CS$<>8__locals1 = new RoomGenUtility.<>c__DisplayClass18_0();
			CS$<>8__locals1.map = map;
			if (faction == null)
			{
				Log.Warning("Tried to spawn dormant threat cluster with null faction.");
				return;
			}
			int num = room.rects.Max((CellRect r) => Mathf.Max(r.Width, r.Height));
			LordJob_StructureThreatCluster lordJob = new LordJob_StructureThreatCluster(faction, room.rects[0].CenterCell, Mathf.Min(12f, (float)(num * 2)), true, true);
			Lord lord = LordMaker.MakeNewLord(faction, lordJob, CS$<>8__locals1.map, null);
			foreach (PawnKindDef kindDef in PawnUtility.GetCombatPawnKindsForPoints(selector, threatPoints, (PawnKindDef pk) => 1f / pk.combatPower))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kindDef, faction, null);
				IntVec3 loc;
				if (room.TryGetRandomCellInRoom(out loc, 1, new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnDormantThreatCluster>g__Validator|2)))
				{
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					lord.AddPawn(pawn);
				}
			}
			if (signalTag != null)
			{
				SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
				signalAction_DormancyWakeUp.signalTag = signalTag;
				signalAction_DormancyWakeUp.lord = lord;
				GenSpawn.Spawn(signalAction_DormancyWakeUp, CS$<>8__locals1.map.Center, CS$<>8__locals1.map, WipeMode.Vanish);
			}
		}

		// Token: 0x0600B0D8 RID: 45272 RVA: 0x00327574 File Offset: 0x00325774
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, LayoutRoom room, float chance = 1f, float thingsPerSide10Cells = 1f, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			RoomGenUtility.SpawnWallAttatchments(thingDef, map, room.rects, chance, thingsPerSide10Cells, validator, stuff, faction);
		}

		// Token: 0x0600B0D9 RID: 45273 RVA: 0x0032758C File Offset: 0x0032578C
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, List<CellRect> rects, float chance = 1f, float thingsPerSide10Cells = 1f, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			foreach (IntVec3 cell in RoomGenUtility.IdealWallAttatchmentPositions(thingDef, map, rects, thingsPerSide10Cells, validator))
			{
				if (Rand.Chance(chance))
				{
					Thing thing;
					RoomGenUtility.TrySpawnWallAttatchment(thingDef, cell, map, out thing, true, validator, stuff, faction);
				}
			}
		}

		// Token: 0x0600B0DA RID: 45274 RVA: 0x00327600 File Offset: 0x00325800
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, LayoutRoom room, IntRange count, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			RoomGenUtility.SpawnWallAttatchments(thingDef, map, room.rects, count, validator, stuff, faction);
		}

		// Token: 0x0600B0DB RID: 45275 RVA: 0x00327618 File Offset: 0x00325818
		public static void SpawnWallAttatchments(ThingDef thingDef, Map map, List<CellRect> rects, IntRange count, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			List<IntVec3> list = RoomGenUtility.IdealWallAttatchmentPositions(thingDef, map, rects, 1f, validator);
			int randomInRange = count.RandomInRange;
			int num = 0;
			int num2 = 0;
			while (num2 < randomInRange && list.Any<IntVec3>())
			{
				IntVec3 intVec = list.RandomElement<IntVec3>();
				list.Remove(intVec);
				Thing thing;
				if (RoomGenUtility.TrySpawnWallAttatchment(thingDef, intVec, map, out thing, true, validator, stuff, faction))
				{
					num++;
				}
				num2++;
			}
			int num3 = num;
			while (num3 < count.TrueMin && num < count.TrueMin)
			{
				foreach (CellRect cellRect in rects)
				{
					foreach (IntVec3 cell in cellRect.ContractedBy(1).EdgeCells.InRandomOrder(null))
					{
						Thing thing;
						if (RoomGenUtility.TrySpawnWallAttatchment(thingDef, cell, map, out thing, true, validator, stuff, faction))
						{
							num++;
						}
						if (num >= count.TrueMin)
						{
							break;
						}
					}
					if (num >= count.TrueMin)
					{
						break;
					}
				}
				num3++;
			}
		}

		// Token: 0x0600B0DC RID: 45276 RVA: 0x00327760 File Offset: 0x00325960
		private static List<IntVec3> IdealWallAttatchmentPositions(ThingDef thingDef, Map map, List<CellRect> rects, float thingsPerSide10Cells = 1f, Predicate<IntVec3, Rot4> validator = null)
		{
			List<IntVec3> list = new List<IntVec3>();
			foreach (CellRect cellRect in rects)
			{
				CellRect cellRect2 = cellRect.ContractedBy(1);
				for (int i = 0; i < 2; i++)
				{
					Rot4 rot = (i == 0) ? Rot4.North : Rot4.East;
					float num = (float)cellRect2.GetSideLength(rot) / 2f;
					int b = Mathf.RoundToInt(num / 3f);
					int num2 = Mathf.Min(Mathf.RoundToInt(num / 10f * thingsPerSide10Cells), b);
					if (num2 > 0)
					{
						int num3 = Mathf.Max(3, Mathf.RoundToInt(num / (float)(num2 + 1)));
						int num4 = num3;
						for (int j = 0; j < num2; j++)
						{
							for (int k = 0; k < 2; k++)
							{
								int num5 = num4;
								if (cellRect2.Width % 2 == 0 && k == 0 && i == 0)
								{
									num5--;
								}
								else if (cellRect2.Height % 2 == 0 && k == 0 && i == 1)
								{
									num5--;
								}
								for (int l = 0; l < 2; l++)
								{
									Rot4 rot2 = (l == 0) ? rot : rot.Opposite;
									IntVec3 centerCellOnEdge = cellRect2.GetCenterCellOnEdge(rot2, (k == 0) ? num5 : (-num5));
									Rot4 rot3;
									if (cellRect2.Contains(centerCellOnEdge) && !list.Contains(centerCellOnEdge) && RoomGenUtility.CanPlaceWallAttachment(thingDef, centerCellOnEdge, map, out rot3, true, validator))
									{
										list.Add(centerCellOnEdge);
									}
								}
							}
							num4 += num3;
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600B0DD RID: 45277 RVA: 0x0032791C File Offset: 0x00325B1C
		public static bool TrySpawnWallAttatchment(ThingDef thingDef, IntVec3 cell, Map map, out Thing spawned, bool avoidEdifices = true, Predicate<IntVec3, Rot4> validator = null, ThingDef stuff = null, Faction faction = null)
		{
			Rot4 rot;
			if (RoomGenUtility.CanPlaceWallAttachment(thingDef, cell, map, out rot, avoidEdifices, validator))
			{
				spawned = ThingMaker.MakeThing(thingDef, stuff);
				spawned.SetFactionDirect(faction);
				GenSpawn.Spawn(spawned, cell, map, rot, WipeMode.Vanish, false, false);
				return true;
			}
			spawned = null;
			return false;
		}

		// Token: 0x0600B0DE RID: 45278 RVA: 0x00327960 File Offset: 0x00325B60
		public static bool CanPlaceWallAttachment(ThingDef thingDef, IntVec3 cell, Map map, out Rot4 rotation, bool avoidEdifices = true, Predicate<IntVec3, Rot4> validator = null)
		{
			rotation = default(Rot4);
			if (avoidEdifices && cell.GetEdifice(map) != null)
			{
				return false;
			}
			foreach (Rot4 rot in Rot4.AllRotations)
			{
				if (RoomGenUtility.CanPlace(thingDef, cell, map, rot) && (validator == null || validator(cell, rot)))
				{
					rotation = rot;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B0DF RID: 45279 RVA: 0x003279E4 File Offset: 0x00325BE4
		public static bool CanPlace(ThingDef thingDef, IntVec3 cell, Map map, Rot4 rot)
		{
			if (thingDef.IsEdifice() && cell.GetEdifice(map) != null)
			{
				return false;
			}
			using (List<PlaceWorker>.Enumerator enumerator = thingDef.PlaceWorkers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowsPlacing(thingDef, cell, rot, map, null, null))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B0E0 RID: 45280 RVA: 0x00327A5C File Offset: 0x00325C5C
		public static void SpawnCorpses(LayoutRoom room, Map map, IntRange countRange, PawnKindDef kind, DamageDef damageType, ThingDef killerThing = null, Tool toolUsed = null, FloatRange? ageDaysRange = null, IntRange? bloodFilthRange = null, int? fixedAge = null, bool forceNoGear = false)
		{
			FloatRange floatRange = ageDaysRange ?? new FloatRange(3f, 600f);
			int randomInRange = countRange.RandomInRange;
			for (int i = 0; i < randomInRange; i++)
			{
				int num = Mathf.RoundToInt(floatRange.RandomInRange * 60000f);
				IntVec3 intVec;
				if (room.TryGetRandomCellInRoom(out intVec, 1, null))
				{
					IntVec3 cell = intVec;
					int deadTicks = num;
					int? num2 = fixedAge;
					RoomGenUtility.SpawnCorpse(cell, kind, deadTicks, map, damageType, (num2 != null) ? new float?((float)num2.GetValueOrDefault()) : null, forceNoGear, killerThing, toolUsed, bloodFilthRange, null);
				}
			}
		}

		// Token: 0x0600B0E1 RID: 45281 RVA: 0x00327B00 File Offset: 0x00325D00
		public static Corpse SpawnCorpse(IntVec3 cell, PawnKindDef kind, int deadTicks, Map map, DamageDef damageType, float? fixedAge = null, bool forceNoGear = false, ThingDef killerThing = null, Tool toolUsed = null, IntRange? bloodFilthRange = null, BodyPartDef idealPart = null)
		{
			IntRange countRange = bloodFilthRange ?? new IntRange(1, 5);
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(kind, null, PawnGenerationContext.NonPlayer, null, false, true, true, true, false, 1f, false, true, false, false, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, fixedAge, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, forceNoGear));
			if (!pawn.Dead)
			{
				HealthUtility.SimulateKilled(pawn, damageType, killerThing, toolUsed, idealPart);
			}
			pawn.Corpse.Age = deadTicks;
			pawn.relations.hidePawnRelations = true;
			pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
			Corpse corpse = (Corpse)GenSpawn.Spawn(pawn.Corpse, cell, map, WipeMode.Vanish);
			if (pawn.RaceProps.BloodDef != null)
			{
				GenSpawn.SpawnIrregularLump(pawn.RaceProps.BloodDef, cell, map, countRange, new IntRange(2, 4), WipeMode.Vanish, null, null, null, null, null);
			}
			corpse.TrySetForbidden(true);
			return corpse;
		}

		// Token: 0x0600B0E2 RID: 45282 RVA: 0x00327C56 File Offset: 0x00325E56
		public static bool IsGoodForDoor(IntVec3 p, Map map)
		{
			return RoomGenUtility.IsGoodForHorizontalDoor(p, map) || RoomGenUtility.IsGoodForVerticalDoor(p, map);
		}

		// Token: 0x0600B0E3 RID: 45283 RVA: 0x00327C6C File Offset: 0x00325E6C
		public static bool IsGoodForHorizontalDoor(IntVec3 p, Map map)
		{
			return RoomGenUtility.IsWallAt(p + IntVec3.West, map) && RoomGenUtility.IsWallAt(p + IntVec3.East, map) && !RoomGenUtility.IsWallAt(p + IntVec3.North, map) && !RoomGenUtility.IsWallAt(p + IntVec3.South, map);
		}

		// Token: 0x0600B0E4 RID: 45284 RVA: 0x00327CC8 File Offset: 0x00325EC8
		public static bool IsGoodForVerticalDoor(IntVec3 p, Map map)
		{
			return RoomGenUtility.IsWallAt(p + IntVec3.North, map) && RoomGenUtility.IsWallAt(p + IntVec3.South, map) && !RoomGenUtility.IsWallAt(p + IntVec3.East, map) && !RoomGenUtility.IsWallAt(p + IntVec3.West, map);
		}

		// Token: 0x0600B0E5 RID: 45285 RVA: 0x00327D24 File Offset: 0x00325F24
		public static bool IsWallAt(IntVec3 position, Map map)
		{
			Building edifice = position.GetEdifice(map);
			return edifice != null && edifice.def.IsWall;
		}

		// Token: 0x0600B0E6 RID: 45286 RVA: 0x00327D3D File Offset: 0x00325F3D
		[Obsolete]
		public static Queue<CellRect> SubdividedIntoRooms(Map map, CellRect rect, int innerWallsContract = 3, int minRoomSize = 5, int wallMoveRange = 1, int maxRooms = 2, ThingDef wall = null, ThingDef wallStuff = null)
		{
			return RoomGenUtility.SubdividedIntoRooms_NewTemp(map, rect.ContractedBy(innerWallsContract), minRoomSize, wallMoveRange, maxRooms, wall, wallStuff);
		}

		// Token: 0x0600B0E7 RID: 45287 RVA: 0x00327D58 File Offset: 0x00325F58
		public static Queue<CellRect> SubdividedIntoRooms_NewTemp(Map map, CellRect rect, int minRoomSize = 5, int wallMoveRange = 1, int maxRooms = 2, ThingDef wall = null, ThingDef wallStuff = null)
		{
			int a = Mathf.FloorToInt(((float)rect.Width - 2f) / (float)minRoomSize);
			int a2 = Mathf.FloorToInt(((float)rect.Height - 2f) / (float)minRoomSize);
			int num = Rand.Range(0, Mathf.Min(a, maxRooms));
			int num2 = Rand.Range(0, Mathf.Min(a2, maxRooms));
			Queue<CellRect> queue = new Queue<CellRect>();
			queue.Enqueue(rect);
			int num3 = -1;
			int num4 = -1;
			for (int i = 0; i < num; i++)
			{
				ValueTuple<CellRect, CellRect> valueTuple = queue.Dequeue().SplitHorizontal(0);
				CellRect item = valueTuple.Item1;
				CellRect item2 = valueTuple.Item2;
				int num5 = Rand.RangeInclusive(-wallMoveRange, wallMoveRange);
				num3 = item.maxX + num5;
				item.maxX += num5 - 1;
				item2.minX += num5 + 1;
				queue.Enqueue(item);
				queue.Enqueue(item2);
			}
			for (int j = 0; j < num2; j++)
			{
				int count = queue.Count;
				for (int k = 0; k < count; k++)
				{
					ValueTuple<CellRect, CellRect> valueTuple2 = queue.Dequeue().SplitVertical(0);
					CellRect item3 = valueTuple2.Item1;
					CellRect item4 = valueTuple2.Item2;
					int num6 = Rand.RangeInclusive(-wallMoveRange, wallMoveRange);
					num4 = item3.maxZ + num6;
					item3.maxZ += num6 - 1;
					item4.minZ += num6 + 1;
					queue.Enqueue(item3);
					queue.Enqueue(item4);
				}
			}
			if (num3 >= 0)
			{
				for (int l = rect.minZ; l <= rect.maxZ; l++)
				{
					ThingDef thingDef = wall ?? ThingDefOf.Wall;
					GenSpawn.Spawn(ThingMaker.MakeThing(thingDef, thingDef.MadeFromStuff ? (wallStuff ?? ThingDefOf.Steel) : null), new IntVec3(num3, 0, l), map, WipeMode.Vanish);
				}
			}
			if (num4 >= 0)
			{
				for (int m = rect.minX; m <= rect.maxX; m++)
				{
					ThingDef thingDef2 = wall ?? ThingDefOf.Wall;
					GenSpawn.Spawn(ThingMaker.MakeThing(thingDef2, thingDef2.MadeFromStuff ? (wallStuff ?? ThingDefOf.Steel) : null), new IntVec3(m, 0, num4), map, WipeMode.Vanish);
				}
			}
			return queue;
		}

		// Token: 0x0600B0E8 RID: 45288 RVA: 0x00327F82 File Offset: 0x00326182
		public static Building_Crate SpawnHermeticCrate(IntVec3 cell, Map map, ThingSetMakerDef setMaker, bool addRewards = true, string signal = null)
		{
			return RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, map, Rot4.South, setMaker, addRewards, signal);
		}

		// Token: 0x0600B0E9 RID: 45289 RVA: 0x00327F99 File Offset: 0x00326199
		public static Building_Crate SpawnHermeticCrate(IntVec3 cell, Map map, Rot4 rot, ThingSetMakerDef setMaker, bool addRewards = true, string signal = null)
		{
			return RoomGenUtility.SpawnCrate(ThingDefOf.AncientHermeticCrate, cell, map, rot, setMaker, addRewards, signal);
		}

		// Token: 0x0600B0EA RID: 45290 RVA: 0x00327FAD File Offset: 0x003261AD
		public static Building_Crate SpawnCrate(ThingDef crateDef, IntVec3 cell, Map map, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null)
		{
			return RoomGenUtility.SpawnCrate(crateDef, cell, map, Rot4.South, rewardMaker, addRewards, signal);
		}

		// Token: 0x0600B0EB RID: 45291 RVA: 0x00327FC4 File Offset: 0x003261C4
		public static Building_Crate SpawnCrate(ThingDef crateDef, IntVec3 cell, Map map, Rot4 rot, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null)
		{
			Building_Crate building_Crate = (Building_Crate)GenSpawn.Spawn(ThingMaker.MakeThing(crateDef, null), cell, map, rot, WipeMode.Vanish, false, false);
			if (addRewards)
			{
				List<Thing> list = rewardMaker.root.Generate(default(ThingSetMakerParams));
				for (int i = list.Count - 1; i >= 0; i--)
				{
					Thing thing = list[i];
					if (!building_Crate.TryAcceptThing(thing, false))
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
				if (!string.IsNullOrEmpty(signal))
				{
					building_Crate.openedSignal = signal;
				}
			}
			else
			{
				building_Crate.Open();
			}
			return building_Crate;
		}

		// Token: 0x0600B0EC RID: 45292 RVA: 0x0032804C File Offset: 0x0032624C
		public static void SpawnHermeticCrateInRoom(LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.SpawnCratesInRoom(ThingDefOf.AncientHermeticCrate, room, map, rewardMaker, IntRange.One, addRewards, signal, rot);
		}

		// Token: 0x0600B0ED RID: 45293 RVA: 0x00328065 File Offset: 0x00326265
		public static void SpawnHermeticCratesInRoom(LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, IntRange countRange, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.SpawnCratesInRoom(ThingDefOf.AncientHermeticCrate, room, map, rewardMaker, countRange, addRewards, signal, rot);
		}

		// Token: 0x0600B0EE RID: 45294 RVA: 0x0032807B File Offset: 0x0032627B
		public static void SpawnCrateInRoom(ThingDef crateDef, LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.SpawnCratesInRoom(crateDef, room, map, rewardMaker, IntRange.One, addRewards, signal, rot);
		}

		// Token: 0x0600B0EF RID: 45295 RVA: 0x00328094 File Offset: 0x00326294
		public static void SpawnCratesInRoom(ThingDef crateDef, LayoutRoom room, Map map, ThingSetMakerDef rewardMaker, IntRange countRange, bool addRewards = true, string signal = null, Rot4? rot = null)
		{
			RoomGenUtility.<>c__DisplayClass42_0 CS$<>8__locals1 = new RoomGenUtility.<>c__DisplayClass42_0();
			CS$<>8__locals1.crateDef = crateDef;
			CS$<>8__locals1.map = map;
			int num = countRange.RandomInRange;
			CS$<>8__locals1.rotation = (rot ?? Rot4.South);
			IntVec3 cell;
			while (num > 0 && room.TryGetRandomCellInRoom(out cell, 1, new Func<IntVec3, bool>(CS$<>8__locals1.<SpawnCratesInRoom>g__Validator|0)))
			{
				RoomGenUtility.SpawnCrate(CS$<>8__locals1.crateDef, cell, CS$<>8__locals1.map, CS$<>8__locals1.rotation, rewardMaker, addRewards, signal);
				num--;
			}
		}

		// Token: 0x0600B0F0 RID: 45296 RVA: 0x0032811C File Offset: 0x0032631C
		public static bool IsValidCrateCell(ThingDef crate, IntVec3 pos, Map map, Rot4 rot)
		{
			CellRect cellRect = pos.RectAbout(crate.Size, rot);
			using (IEnumerator<IntVec3> enumerator = cellRect.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			foreach (IntVec3 root in cellRect.EdgeCells)
			{
				foreach (IntVec3 c in GenAdjFast.AdjacentCells8Way(root))
				{
					if (!cellRect.Contains(c) && c.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B0F1 RID: 45297 RVA: 0x0032820C File Offset: 0x0032640C
		public static bool IsClearAndNotAdjacentToDoor(ThingDef def, IntVec3 pos, Map map, Rot4 rot)
		{
			return RoomGenUtility.IsClearAndNotAdjacentToDoor(pos.RectAbout(def.Size, rot), map);
		}

		// Token: 0x0600B0F2 RID: 45298 RVA: 0x00328224 File Offset: 0x00326424
		public static bool IsClearAndNotAdjacentToDoor(CellRect rect, Map map)
		{
			using (IEnumerator<IntVec3> enumerator = rect.Cells.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			foreach (IntVec3 root in rect.EdgeCells)
			{
				foreach (IntVec3 c in GenAdjFast.AdjacentCellsCardinal(root))
				{
					if (!rect.Contains(c) && c.GetDoor(map) != null)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600B0F3 RID: 45299 RVA: 0x00328304 File Offset: 0x00326504
		public static void SpawnDoorBarricades(ThingDef def, LayoutRoom room, Map map, float chancePerDoor = 0.75f, ThingDef stuff = null, int steps = 2, int offset = 2)
		{
			RoomGenUtility.<>c__DisplayClass46_0 CS$<>8__locals1;
			CS$<>8__locals1.def = def;
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.map = map;
			List<ValueTuple<IntVec3, Rot4>> list = new List<ValueTuple<IntVec3, Rot4>>();
			foreach (CellRect cellRect in room.rects)
			{
				foreach (IntVec3 intVec in cellRect.EdgeCells)
				{
					if (intVec.GetDoor(CS$<>8__locals1.map) != null)
					{
						list.Add(new ValueTuple<IntVec3, Rot4>(intVec, cellRect.GetEdgeCellRot(intVec)));
					}
				}
			}
			foreach (ValueTuple<IntVec3, Rot4> valueTuple in list)
			{
				IntVec3 item = valueTuple.Item1;
				Rot4 item2 = valueTuple.Item2;
				if (Rand.Chance(chancePerDoor))
				{
					IntVec3 intVec2 = item + item2.Opposite.FacingCell * offset;
					RoomGenUtility.<SpawnDoorBarricades>g__SpawnBarricade|46_0(intVec2, ref CS$<>8__locals1);
					for (int i = 1; i < steps; i++)
					{
						Rot4 rot = item2.Rotated(RotationDirection.Clockwise);
						RoomGenUtility.<SpawnDoorBarricades>g__SpawnBarricade|46_0(intVec2 + rot.FacingCell * i, ref CS$<>8__locals1);
						RoomGenUtility.<SpawnDoorBarricades>g__SpawnBarricade|46_0(intVec2 + rot.FacingCell * -i, ref CS$<>8__locals1);
					}
				}
			}
		}

		// Token: 0x0600B0F4 RID: 45300 RVA: 0x003284A0 File Offset: 0x003266A0
		public static void TryPlaceInRandomCorner(Map map, LayoutRoom room, ThingDef thingDef, Faction faction, ThingDef stuff = null)
		{
			RoomGenUtility.<>c__DisplayClass47_0 CS$<>8__locals1 = new RoomGenUtility.<>c__DisplayClass47_0();
			CS$<>8__locals1.map = map;
			List<IntVec3> list = room.rects.SelectMany((CellRect r) => r.ContractedBy(1).Corners).Where(new Func<IntVec3, bool>(CS$<>8__locals1.<TryPlaceInRandomCorner>g__Predicate|0)).ToList<IntVec3>();
			if (list.Count == 0)
			{
				return;
			}
			IntVec3 loc = list.RandomElement<IntVec3>();
			Thing thing = ThingMaker.MakeThing(thingDef, stuff);
			thing.SetFactionDirect(faction);
			GenSpawn.Spawn(thing, loc, CS$<>8__locals1.map, WipeMode.Vanish);
		}

		// Token: 0x0600B0F5 RID: 45301 RVA: 0x00328528 File Offset: 0x00326728
		[CompilerGenerated]
		internal static void <SpawnDoorBarricades>g__SpawnBarricade|46_0(IntVec3 pos, ref RoomGenUtility.<>c__DisplayClass46_0 A_1)
		{
			GenSpawn.Spawn(ThingMaker.MakeThing(A_1.def, A_1.stuff), pos, A_1.map, WipeMode.Vanish);
		}
	}
}
