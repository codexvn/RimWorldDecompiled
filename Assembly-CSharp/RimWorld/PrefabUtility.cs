using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001EC4 RID: 7876
	public static class PrefabUtility
	{
		// Token: 0x0600AEA0 RID: 44704 RVA: 0x0031CE14 File Offset: 0x0031B014
		public static bool CanSpawnPrefab(PrefabDef prefab, Map map, IntVec3 pos, Rot4 rot, bool canWipeEdifices = true, int recursionDepth = 0)
		{
			if (recursionDepth >= 15)
			{
				return false;
			}
			rot = PrefabUtility.ValidateRotation(prefab, rot);
			IntVec3 root = PrefabUtility.GetRoot(prefab, pos, rot);
			foreach (ValueTuple<PrefabThingData, IntVec3> valueTuple in prefab.GetThings())
			{
				PrefabThingData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				IntVec3 adjustedThingLocalPosition = PrefabUtility.GetAdjustedThingLocalPosition(item, rot, item2);
				if ((item.def.building == null || !item.def.building.isAttachment) && !PrefabUtility.CanSpawnThing(item, map, root + adjustedThingLocalPosition, rot.Rotated(item.relativeRotation), canWipeEdifices))
				{
					return false;
				}
			}
			foreach (ValueTuple<SubPrefabData, IntVec3> valueTuple2 in prefab.GetPrefabs())
			{
				SubPrefabData item3 = valueTuple2.Item1;
				IntVec3 item4 = valueTuple2.Item2;
				IntVec3 adjustedPrefabLocalPosition = PrefabUtility.GetAdjustedPrefabLocalPosition(item3, rot, item4);
				if (!PrefabUtility.CanSpawnPrefab(item3.def, map, root + adjustedPrefabLocalPosition, rot.Rotated(item3.relativeRotation), canWipeEdifices, ++recursionDepth))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600AEA1 RID: 44705 RVA: 0x0031CF5C File Offset: 0x0031B15C
		public static void SpawnPrefab(PrefabDef prefab, Map map, IntVec3 pos, Rot4 rot, Faction faction = null, List<Thing> spawned = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null, Action<Thing> onSpawned = null, bool blueprint = false)
		{
			rot = PrefabUtility.ValidateRotation(prefab, rot);
			IntVec3 root = PrefabUtility.GetRoot(prefab, pos, rot);
			foreach (ValueTuple<PrefabTerrainData, IntVec3> valueTuple in prefab.GetTerrain())
			{
				PrefabTerrainData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				if (Rand.Chance(item.chance))
				{
					IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(item2, rot);
					IntVec3 c = root + adjustedLocalPosition;
					map.terrainGrid.SetTerrain(c, item.def);
					if (item.color != null)
					{
						map.terrainGrid.SetTerrainColor(c, item.color);
					}
				}
			}
			foreach (ValueTuple<PrefabThingData, IntVec3> valueTuple2 in prefab.GetThings())
			{
				PrefabThingData item3 = valueTuple2.Item1;
				IntVec3 item4 = valueTuple2.Item2;
				if (Rand.Chance(item3.chance))
				{
					IntVec3 adjustedThingLocalPosition = PrefabUtility.GetAdjustedThingLocalPosition(item3, rot, item4);
					IntVec3 intVec = root + adjustedThingLocalPosition;
					BuildingProperties building = item3.def.building;
					if (building != null && building.isAttachment)
					{
						PrefabUtility.tmpAttachments.Add(new ValueTuple<PrefabThingData, IntVec3>(item3, intVec));
					}
					else
					{
						PrefabUtility.SpawnThing(item3, map, intVec, rot.Rotated(item3.relativeRotation), faction, spawned, overrideSpawnData, onSpawned, blueprint);
					}
				}
			}
			foreach (ValueTuple<PrefabThingData, IntVec3> valueTuple3 in PrefabUtility.tmpAttachments)
			{
				PrefabThingData item5 = valueTuple3.Item1;
				IntVec3 item6 = valueTuple3.Item2;
				Rot4 rot2 = rot.Rotated(item5.relativeRotation);
				if (item6.InBounds(map) && (RoomGenUtility.CanPlace(item5.def, item6, map, rot2) || RoomGenUtility.CanPlaceWallAttachment(item5.def, item6, map, out rot2, false, null)))
				{
					PrefabUtility.SpawnThing(item5, map, item6, rot2, faction, spawned, overrideSpawnData, onSpawned, false);
				}
			}
			PrefabUtility.tmpAttachments.Clear();
			foreach (ValueTuple<SubPrefabData, IntVec3> valueTuple4 in prefab.GetPrefabs())
			{
				SubPrefabData item7 = valueTuple4.Item1;
				IntVec3 item8 = valueTuple4.Item2;
				if (Rand.Chance(item7.chance))
				{
					IntVec3 adjustedPrefabLocalPosition = PrefabUtility.GetAdjustedPrefabLocalPosition(item7, rot, item8);
					PrefabUtility.SpawnPrefab(item7.def, map, root + adjustedPrefabLocalPosition, rot.Rotated(item7.relativeRotation), faction, spawned, overrideSpawnData, onSpawned, blueprint);
				}
			}
		}

		// Token: 0x0600AEA2 RID: 44706 RVA: 0x0031D20C File Offset: 0x0031B40C
		public static IntVec3 GetAdjustedThingLocalPosition(PrefabThingData data, Rot4 rot, IntVec3 cell)
		{
			IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(cell, rot);
			IntVec2 size = data.def.size;
			if (!data.def.rotatable)
			{
				GenAdj.AdjustForRotation(ref adjustedLocalPosition, ref size, rot);
			}
			return adjustedLocalPosition;
		}

		// Token: 0x0600AEA3 RID: 44707 RVA: 0x0031D248 File Offset: 0x0031B448
		public static IntVec3 GetAdjustedPrefabLocalPosition(SubPrefabData data, Rot4 rot, IntVec3 cell)
		{
			IntVec3 adjustedLocalPosition = PrefabUtility.GetAdjustedLocalPosition(cell, rot);
			IntVec2 size = data.def.size;
			GenAdj.AdjustForRotation(ref adjustedLocalPosition, ref size, rot);
			return adjustedLocalPosition;
		}

		// Token: 0x0600AEA4 RID: 44708 RVA: 0x0031D274 File Offset: 0x0031B474
		[return: TupleElementNames(new string[]
		{
			"data",
			null,
			null
		})]
		public static IEnumerable<ValueTuple<PrefabThingData, IntVec3, Rot4>> GetThings(PrefabDef prefab, IntVec3 pos, Rot4 rot)
		{
			rot = PrefabUtility.ValidateRotation(prefab, rot);
			IntVec3 root = PrefabUtility.GetRoot(prefab, pos, rot);
			foreach (ValueTuple<PrefabThingData, IntVec3> valueTuple in prefab.GetThings())
			{
				PrefabThingData item = valueTuple.Item1;
				IntVec3 item2 = valueTuple.Item2;
				IntVec3 adjustedThingLocalPosition = PrefabUtility.GetAdjustedThingLocalPosition(item, rot, item2);
				yield return new ValueTuple<PrefabThingData, IntVec3, Rot4>(item, root + adjustedThingLocalPosition, (!item.def.rotatable) ? Rot4.North : rot.Rotated(item.relativeRotation));
			}
			IEnumerator<ValueTuple<PrefabThingData, IntVec3>> enumerator = null;
			foreach (ValueTuple<SubPrefabData, IntVec3> valueTuple2 in prefab.GetPrefabs())
			{
				SubPrefabData item3 = valueTuple2.Item1;
				IntVec3 item4 = valueTuple2.Item2;
				IntVec3 adjustedPrefabLocalPosition = PrefabUtility.GetAdjustedPrefabLocalPosition(item3, rot, item4);
				foreach (ValueTuple<PrefabThingData, IntVec3, Rot4> valueTuple3 in PrefabUtility.GetThings(item3.def, root + adjustedPrefabLocalPosition, rot.Rotated(item3.relativeRotation)))
				{
					yield return valueTuple3;
				}
				IEnumerator<ValueTuple<PrefabThingData, IntVec3, Rot4>> enumerator3 = null;
			}
			IEnumerator<ValueTuple<SubPrefabData, IntVec3>> enumerator2 = null;
			yield break;
			yield break;
		}

		// Token: 0x0600AEA5 RID: 44709 RVA: 0x0031D292 File Offset: 0x0031B492
		public static IntVec3 GetRoot(PrefabDef prefab, IntVec3 pos, Rot4 rot)
		{
			return PrefabUtility.GetRoot(pos, prefab.size, rot);
		}

		// Token: 0x0600AEA6 RID: 44710 RVA: 0x0031D2A1 File Offset: 0x0031B4A1
		public static IntVec3 GetRoot(IntVec3 pos, IntVec2 size, Rot4 rot)
		{
			return PrefabUtility.GetRoot(GenAdj.OccupiedRect(pos, rot, size), rot);
		}

		// Token: 0x0600AEA7 RID: 44711 RVA: 0x0031D2B4 File Offset: 0x0031B4B4
		public static IntVec3 GetRoot(CellRect rect, Rot4 rot)
		{
			IntVec3 intVec;
			IntVec3 intVec2;
			IntVec3 intVec3;
			IntVec3 intVec4;
			rect.GetInternalCorners(out intVec, out intVec2, out intVec3, out intVec4);
			IntVec3 result = intVec;
			if (rot == Rot4.East)
			{
				result = intVec2;
			}
			else if (rot == Rot4.South)
			{
				result = intVec3;
			}
			else if (rot == Rot4.West)
			{
				result = intVec4;
			}
			return result;
		}

		// Token: 0x0600AEA8 RID: 44712 RVA: 0x0031D30C File Offset: 0x0031B50C
		private static void SpawnThing(PrefabThingData data, Map map, IntVec3 cell, Rot4 rot, Faction faction = null, List<Thing> spawned = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null, Action<Thing> onSpawned = null, bool blueprint = false)
		{
			bool flag = data.def.IsBlueprint || blueprint;
			ThingDef thingDef = data.def.IsBlueprint ? ((ThingDef)data.def.entityDefToBuild) : data.def;
			ThingDef thingDef2 = data.def.IsBlueprint ? data.def : data.def.blueprintDef;
			rot = (thingDef.rotatable ? rot : Rot4.North);
			if (flag && thingDef2 == null)
			{
				return;
			}
			if (flag && !GenConstruct.CanPlaceBlueprintAt(thingDef, cell, rot, map, false, null, null, data.stuff, false, false, false))
			{
				return;
			}
			if (!flag && !GenSpawn.CanSpawnAt(thingDef, cell, map, new Rot4?(rot), true))
			{
				return;
			}
			ThingDef stuff = data.stuff;
			if (data.canOverrideData && overrideSpawnData != null)
			{
				Tuple<ThingDef, ThingDef> tuple = overrideSpawnData(data);
				if (tuple != null)
				{
					thingDef = tuple.Item1;
					stuff = tuple.Item2;
				}
			}
			Thing thing = null;
			if (flag)
			{
				if (GenSpawn.CanSpawnAt(thingDef.blueprintDef, cell, map, new Rot4?(rot), false))
				{
					thing = GenConstruct.PlaceBlueprintForBuild(thingDef, cell, map, rot, Faction.OfPlayer, stuff, null, null, true);
				}
			}
			else
			{
				thing = ThingMaker.MakeThing(thingDef, stuff);
				if (thing.def.CanHaveFaction)
				{
					thing.SetFaction(faction, null);
				}
				thing.stackCount = data.stackCountRange.RandomInRange;
				if (data.hp > 0)
				{
					thing.HitPoints = data.hp;
				}
				if (data.colorDef != null)
				{
					Building building = thing as Building;
					if (building != null)
					{
						building.ChangePaint(data.colorDef);
						goto IL_1A9;
					}
				}
				CompColorable compColorable;
				if (data.color != default(Color) && thing.TryGetComp(out compColorable))
				{
					compColorable.SetColor(data.color);
				}
				IL_1A9:
				CompQuality compQuality;
				if (data.quality != null && thing.TryGetComp(out compQuality))
				{
					compQuality.SetQuality(data.quality.Value, new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
				GenSpawn.Spawn(thing, cell, map, rot, WipeMode.Vanish, false, false);
			}
			if (thing != null)
			{
				if (spawned != null)
				{
					spawned.Add(thing);
				}
				if (onSpawned != null)
				{
					onSpawned(thing);
				}
			}
		}

		// Token: 0x0600AEA9 RID: 44713 RVA: 0x0031D51E File Offset: 0x0031B71E
		private static bool CanSpawnThing(PrefabThingData data, Map map, IntVec3 cell, Rot4 rot, bool canWipeEdifices = true)
		{
			rot = (data.def.rotatable ? rot : Rot4.North);
			return GenSpawn.CanSpawnAt(data.def, cell, map, new Rot4?(rot), canWipeEdifices);
		}

		// Token: 0x0600AEAA RID: 44714 RVA: 0x0031D54C File Offset: 0x0031B74C
		public static IntVec3 GetAdjustedLocalPosition(IntVec3 local, Rot4 rot)
		{
			if (rot == Rot4.East)
			{
				ref int ptr = ref local.x;
				int num = local.z;
				int num2 = -local.x;
				ptr = num;
				local.z = num2;
			}
			else if (rot == Rot4.South)
			{
				ref int ptr = ref local.x;
				int num2 = -local.x;
				int num = -local.z;
				ptr = num2;
				local.z = num;
			}
			else if (rot == Rot4.West)
			{
				ref int ptr = ref local.x;
				int num = -local.z;
				int num2 = local.x;
				ptr = num;
				local.z = num2;
			}
			return local;
		}

		// Token: 0x0600AEAB RID: 44715 RVA: 0x0031D5F0 File Offset: 0x0031B7F0
		public static Rot4 ValidateRotation(PrefabDef prefab, Rot4 rot)
		{
			if (!prefab.rotations.HasFlag(rot))
			{
				for (int i = 0; i < 4; i++)
				{
					if (prefab.rotations.HasFlag((RotEnum)i))
					{
						rot = new Rot4(i);
						break;
					}
				}
			}
			return rot;
		}

		// Token: 0x0600AEAC RID: 44716 RVA: 0x0031D64C File Offset: 0x0031B84C
		public static PrefabDef CreatePrefab(CellRect rect, bool copyAllThings = false, bool copyTerrain = false)
		{
			PrefabUtility.<>c__DisplayClass14_0 CS$<>8__locals1 = new PrefabUtility.<>c__DisplayClass14_0();
			CS$<>8__locals1.copyAllThings = copyAllThings;
			HashSet<Thing> hashSet = new HashSet<Thing>();
			CS$<>8__locals1.group = new HashSet<Thing>();
			PrefabDef prefabDef = new PrefabDef();
			Thing thing;
			List<Thing> list = (from t in rect.Cells.SelectMany(delegate(IntVec3 c)
			{
				IEnumerable<Thing> thingList = c.GetThingList(Find.CurrentMap);
				Func<Thing, bool> predicate;
				if ((predicate = CS$<>8__locals1.<>9__3) == null)
				{
					predicate = (CS$<>8__locals1.<>9__3 = ((Thing thing) => thing is Building | CS$<>8__locals1.copyAllThings));
				}
				return thingList.Where(predicate);
			})
			orderby t is Building descending, t.def.shortHash descending
			select t).ToList<Thing>();
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					thing = list[i];
					CS$<>8__locals1.group.Clear();
					if (hashSet.Add(thing))
					{
						for (int j = i + 1; j < list.Count; j++)
						{
							Thing thing2 = list[j];
							if (!hashSet.Contains(thing2) && !CS$<>8__locals1.group.Contains(thing2) && thing2.def == thing.def && thing2.stackCount == thing.stackCount && thing2.Rotation == thing.Rotation && thing2.HitPoints == thing.HitPoints && thing2.Stuff == thing.Stuff)
							{
								Building building = thing2 as Building;
								if (building != null)
								{
									Building building2 = thing as Building;
									if (building2 != null && building.PaintColorDef != building2.PaintColorDef)
									{
										goto IL_1EC;
									}
								}
								CompColorable compColorable;
								CompColorable compColorable2;
								QualityCategory qualityCategory;
								QualityCategory qualityCategory2;
								if ((!thing2.TryGetComp(out compColorable) || !thing.TryGetComp(out compColorable2) || compColorable == compColorable2) && (!thing2.TryGetQuality(out qualityCategory) || !thing.TryGetQuality(out qualityCategory2) || qualityCategory == qualityCategory2))
								{
									if (!CS$<>8__locals1.group.Any<Thing>())
									{
										CS$<>8__locals1.group.Add(thing);
									}
									hashSet.Add(thing2);
									CS$<>8__locals1.group.Add(thing2);
								}
							}
							IL_1EC:;
						}
						IntVec3 intVec = thing.Position - rect.Min;
						RotationDirection relativeRotation = RotationDirection.None;
						if (thing.Rotation == Rot4.East)
						{
							relativeRotation = RotationDirection.Clockwise;
						}
						else if (thing.Rotation == Rot4.South)
						{
							relativeRotation = RotationDirection.Opposite;
						}
						else if (thing.Rotation == Rot4.West)
						{
							relativeRotation = RotationDirection.Counterclockwise;
						}
						PrefabThingData prefabThingData = new PrefabThingData
						{
							def = thing.def,
							relativeRotation = relativeRotation,
							stuff = thing.Stuff
						};
						CompQuality compQuality;
						if (thing.TryGetComp(out compQuality))
						{
							prefabThingData.quality = new QualityCategory?(compQuality.Quality);
						}
						Building building3 = thing as Building;
						CompColorable compColorable3;
						if (building3 != null)
						{
							prefabThingData.colorDef = building3.PaintColorDef;
						}
						else if (thing.TryGetComp(out compColorable3) && prefabThingData.color != Color.white)
						{
							prefabThingData.color = compColorable3.Color;
						}
						if (CS$<>8__locals1.group.Any<Thing>())
						{
							if (thing.def.Size == IntVec2.One)
							{
								prefabThingData.rects = new List<CellRect>();
								using (IEnumerator<CellRect> enumerator = rect.EnumerateRectanglesCovering(new Predicate<IntVec3>(CS$<>8__locals1.<CreatePrefab>g__Validator|4)).GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										CellRect cellRect = enumerator.Current;
										prefabThingData.rects.Add(cellRect.MovedBy(-rect.Min));
									}
									goto IL_3EF;
								}
							}
							prefabThingData.positions = new List<IntVec3>();
							using (HashSet<Thing>.Enumerator enumerator2 = CS$<>8__locals1.group.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									Thing thing3 = enumerator2.Current;
									prefabThingData.positions.Add(thing3.Position - rect.Min);
								}
								goto IL_3EF;
							}
							goto IL_3D8;
						}
						goto IL_3D8;
						IL_3EF:
						if (thing.def.useHitPoints && thing.HitPoints != thing.MaxHitPoints)
						{
							prefabThingData.hp = thing.HitPoints;
						}
						if (thing.stackCount != 1)
						{
							prefabThingData.stackCountRange = new IntRange(thing.stackCount, thing.stackCount);
						}
						prefabDef.things.Add(prefabThingData);
						goto IL_44C;
						IL_3D8:
						if (intVec != IntVec3.Zero)
						{
							prefabThingData.position = intVec;
							goto IL_3EF;
						}
						goto IL_3EF;
					}
					IL_44C:;
				}
			}
			if (copyTerrain)
			{
				Dictionary<ValueTuple<TerrainDef, ColorDef>, List<IntVec3>> dictionary = new Dictionary<ValueTuple<TerrainDef, ColorDef>, List<IntVec3>>();
				TerrainGrid terrainGrid = Find.CurrentMap.terrainGrid;
				foreach (IntVec3 intVec2 in rect.Cells)
				{
					TerrainDef terrain = intVec2.GetTerrain(Find.CurrentMap);
					ValueTuple<TerrainDef, ColorDef> key = new ValueTuple<TerrainDef, ColorDef>(terrain, terrainGrid.ColorAt(intVec2));
					List<IntVec3> list2;
					if (!dictionary.TryGetValue(key, out list2))
					{
						list2 = (dictionary[key] = new List<IntVec3>());
					}
					list2.Add(intVec2);
				}
				using (Dictionary<ValueTuple<TerrainDef, ColorDef>, List<IntVec3>>.Enumerator enumerator4 = dictionary.GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						PrefabUtility.<>c__DisplayClass14_2 CS$<>8__locals2 = new PrefabUtility.<>c__DisplayClass14_2();
						KeyValuePair<ValueTuple<TerrainDef, ColorDef>, List<IntVec3>> keyValuePair = enumerator4.Current;
						ValueTuple<TerrainDef, ColorDef> valueTuple;
						List<IntVec3> list3;
						keyValuePair.Deconstruct(out valueTuple, out list3);
						ValueTuple<TerrainDef, ColorDef> valueTuple2 = valueTuple;
						TerrainDef item = valueTuple2.Item1;
						ColorDef item2 = valueTuple2.Item2;
						CS$<>8__locals2.list = list3;
						PrefabTerrainData prefabTerrainData = new PrefabTerrainData
						{
							def = item,
							color = item2,
							rects = new List<CellRect>()
						};
						foreach (CellRect cellRect2 in rect.EnumerateRectanglesCovering(new Predicate<IntVec3>(CS$<>8__locals2.<CreatePrefab>g__Validator|6)))
						{
							prefabTerrainData.rects.Add(cellRect2.MovedBy(-rect.Min));
						}
						prefabDef.terrain.Add(prefabTerrainData);
					}
				}
			}
			return prefabDef;
		}

		// Token: 0x0400780B RID: 30731
		private const int MaximumDepth = 15;

		// Token: 0x0400780C RID: 30732
		private static readonly List<ValueTuple<PrefabThingData, IntVec3>> tmpAttachments = new List<ValueTuple<PrefabThingData, IntVec3>>();
	}
}
