using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class PrefabUtility
{
	[CompilerGenerated]
	private sealed class _003CGetThings_003Ed__7 : IEnumerable<(PrefabThingData data, IntVec3, Rot4)>, IEnumerable, IEnumerator<(PrefabThingData data, IntVec3, Rot4)>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private (PrefabThingData data, IntVec3, Rot4) _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Rot4 rot;

		public Rot4 _003C_003E3__rot;

		private PrefabDef prefab;

		public PrefabDef _003C_003E3__prefab;

		private IntVec3 pos;

		public IntVec3 _003C_003E3__pos;

		private IntVec3 _003Croot_003E5__2;

		private IEnumerator<(PrefabThingData data, IntVec3 cell)> _003C_003E7__wrap2;

		private IEnumerator<(SubPrefabData data, IntVec3 cell)> _003C_003E7__wrap3;

		private IEnumerator<(PrefabThingData data, IntVec3, Rot4)> _003C_003E7__wrap4;

		(PrefabThingData data, IntVec3, Rot4) IEnumerator<(PrefabThingData, IntVec3, Rot4)>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetThings_003Ed__7(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			switch (num)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -5:
			case -4:
			case 2:
				try
				{
					if (num == -5 || num == 2)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally3();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E7__wrap4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					rot = ValidateRotation(prefab, rot);
					_003Croot_003E5__2 = GetRoot(prefab, pos, rot);
					_003C_003E7__wrap2 = prefab.GetThings().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00f6;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00f6;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_01c9;
					}
					IL_00f6:
					if (_003C_003E7__wrap2.MoveNext())
					{
						(PrefabThingData data, IntVec3 cell) current = _003C_003E7__wrap2.Current;
						PrefabThingData item = current.data;
						IntVec3 item2 = current.cell;
						IntVec3 adjustedThingLocalPosition = GetAdjustedThingLocalPosition(item, rot, item2);
						_003C_003E2__current = (data: item, _003Croot_003E5__2 + adjustedThingLocalPosition, (!item.def.rotatable) ? Rot4.North : rot.Rotated(item.relativeRotation));
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003C_003E7__wrap3 = prefab.GetPrefabs().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_01e3;
					IL_01c9:
					if (_003C_003E7__wrap4.MoveNext())
					{
						(PrefabThingData, IntVec3, Rot4) current2 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = null;
					goto IL_01e3;
					IL_01e3:
					if (_003C_003E7__wrap3.MoveNext())
					{
						(SubPrefabData data, IntVec3 cell) current3 = _003C_003E7__wrap3.Current;
						SubPrefabData item3 = current3.data;
						IntVec3 item4 = current3.cell;
						IntVec3 adjustedPrefabLocalPosition = GetAdjustedPrefabLocalPosition(item3, rot, item4);
						_003C_003E7__wrap4 = GetThings(item3.def, _003Croot_003E5__2 + adjustedPrefabLocalPosition, rot.Rotated(item3.relativeRotation)).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_01c9;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = null;
					return false;
				}
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<(PrefabThingData data, IntVec3, Rot4)> IEnumerable<(PrefabThingData, IntVec3, Rot4)>.GetEnumerator()
		{
			_003CGetThings_003Ed__7 _003CGetThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetThings_003Ed__ = this;
			}
			else
			{
				_003CGetThings_003Ed__ = new _003CGetThings_003Ed__7(0);
			}
			_003CGetThings_003Ed__.prefab = _003C_003E3__prefab;
			_003CGetThings_003Ed__.pos = _003C_003E3__pos;
			_003CGetThings_003Ed__.rot = _003C_003E3__rot;
			return _003CGetThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<(PrefabThingData, IntVec3, Rot4)>)this).GetEnumerator();
		}
	}

	private const int MaximumDepth = 15;

	private static readonly List<(PrefabThingData, IntVec3)> tmpAttachments = new List<(PrefabThingData, IntVec3)>();

	public static bool CanSpawnPrefab(PrefabDef prefab, Map map, IntVec3 pos, Rot4 rot, bool canWipeEdifices = true, int recursionDepth = 0)
	{
		if (recursionDepth >= 15)
		{
			return false;
		}
		rot = ValidateRotation(prefab, rot);
		IntVec3 root = GetRoot(prefab, pos, rot);
		foreach (var thing in prefab.GetThings())
		{
			PrefabThingData item = thing.data;
			IntVec3 item2 = thing.cell;
			IntVec3 adjustedThingLocalPosition = GetAdjustedThingLocalPosition(item, rot, item2);
			if ((item.def.building == null || !item.def.building.isAttachment) && !CanSpawnThing(item, map, root + adjustedThingLocalPosition, rot.Rotated(item.relativeRotation), canWipeEdifices))
			{
				return false;
			}
		}
		foreach (var prefab2 in prefab.GetPrefabs())
		{
			SubPrefabData item3 = prefab2.data;
			IntVec3 item4 = prefab2.cell;
			IntVec3 adjustedPrefabLocalPosition = GetAdjustedPrefabLocalPosition(item3, rot, item4);
			if (!CanSpawnPrefab(item3.def, map, root + adjustedPrefabLocalPosition, rot.Rotated(item3.relativeRotation), canWipeEdifices, ++recursionDepth))
			{
				return false;
			}
		}
		return true;
	}

	public static void SpawnPrefab(PrefabDef prefab, Map map, IntVec3 pos, Rot4 rot, Faction faction = null, List<Thing> spawned = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null, Action<Thing> onSpawned = null, bool blueprint = false)
	{
		rot = ValidateRotation(prefab, rot);
		IntVec3 root = GetRoot(prefab, pos, rot);
		foreach (var (prefabTerrainData, local) in prefab.GetTerrain())
		{
			if (Rand.Chance(prefabTerrainData.chance))
			{
				IntVec3 adjustedLocalPosition = GetAdjustedLocalPosition(local, rot);
				IntVec3 c = root + adjustedLocalPosition;
				map.terrainGrid.SetTerrain(c, prefabTerrainData.def);
				if (prefabTerrainData.color != null)
				{
					map.terrainGrid.SetTerrainColor(c, prefabTerrainData.color);
				}
			}
		}
		foreach (var (prefabThingData, cell) in prefab.GetThings())
		{
			if (Rand.Chance(prefabThingData.chance))
			{
				IntVec3 adjustedThingLocalPosition = GetAdjustedThingLocalPosition(prefabThingData, rot, cell);
				IntVec3 intVec = root + adjustedThingLocalPosition;
				BuildingProperties building = prefabThingData.def.building;
				if (building != null && building.isAttachment)
				{
					tmpAttachments.Add((prefabThingData, intVec));
				}
				else
				{
					SpawnThing(prefabThingData, map, intVec, rot.Rotated(prefabThingData.relativeRotation), faction, spawned, overrideSpawnData, onSpawned, blueprint);
				}
			}
		}
		foreach (var tmpAttachment in tmpAttachments)
		{
			PrefabThingData item = tmpAttachment.Item1;
			IntVec3 item2 = tmpAttachment.Item2;
			Rot4 rotation = rot.Rotated(item.relativeRotation);
			if (item2.InBounds(map) && (RoomGenUtility.CanPlace(item.def, item2, map, rotation) || RoomGenUtility.CanPlaceWallAttachment(item.def, item2, map, out rotation, avoidEdifices: false)))
			{
				SpawnThing(item, map, item2, rotation, faction, spawned, overrideSpawnData, onSpawned);
			}
		}
		tmpAttachments.Clear();
		foreach (var (subPrefabData, cell2) in prefab.GetPrefabs())
		{
			if (Rand.Chance(subPrefabData.chance))
			{
				IntVec3 adjustedPrefabLocalPosition = GetAdjustedPrefabLocalPosition(subPrefabData, rot, cell2);
				SpawnPrefab(subPrefabData.def, map, root + adjustedPrefabLocalPosition, rot.Rotated(subPrefabData.relativeRotation), faction, spawned, overrideSpawnData, onSpawned, blueprint);
			}
		}
	}

	public static IntVec3 GetAdjustedThingLocalPosition(ThingDef def, Rot4 rot, IntVec3 cell)
	{
		IntVec3 center = GetAdjustedLocalPosition(cell, rot);
		IntVec2 size = def.size;
		if (!def.rotatable)
		{
			GenAdj.AdjustForRotation(ref center, ref size, def.defaultPlacingRot, rot);
		}
		return center;
	}

	public static IntVec3 GetAdjustedThingLocalPosition(PrefabThingData data, Rot4 rot, IntVec3 cell)
	{
		return GetAdjustedThingLocalPosition(data.def, rot, cell);
	}

	public static IntVec3 GetAdjustedPrefabLocalPosition(SubPrefabData data, Rot4 rot, IntVec3 cell)
	{
		IntVec3 center = GetAdjustedLocalPosition(cell, rot);
		IntVec2 size = data.def.size;
		GenAdj.AdjustForRotation(ref center, ref size, rot);
		return center;
	}

	[IteratorStateMachine(typeof(_003CGetThings_003Ed__7))]
	public static IEnumerable<(PrefabThingData data, IntVec3, Rot4)> GetThings(PrefabDef prefab, IntVec3 pos, Rot4 rot)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetThings_003Ed__7(-2)
		{
			_003C_003E3__prefab = prefab,
			_003C_003E3__pos = pos,
			_003C_003E3__rot = rot
		};
	}

	public static IntVec3 GetRoot(PrefabDef prefab, IntVec3 pos, Rot4 rot)
	{
		return GetRoot(pos, prefab.size, rot);
	}

	public static IntVec3 GetRoot(IntVec3 pos, IntVec2 size, Rot4 rot)
	{
		return GetRoot(GenAdj.OccupiedRect(pos, rot, size), rot);
	}

	public static IntVec3 GetRoot(CellRect rect, Rot4 rot)
	{
		rect.GetInternalCorners(out var BL, out var TL, out var TR, out var BR);
		IntVec3 result = BL;
		if (rot == Rot4.East)
		{
			result = TL;
		}
		else if (rot == Rot4.South)
		{
			result = TR;
		}
		else if (rot == Rot4.West)
		{
			result = BR;
		}
		return result;
	}

	private static void SpawnThing(PrefabThingData data, Map map, IntVec3 cell, Rot4 rot, Faction faction = null, List<Thing> spawned = null, Func<PrefabThingData, Tuple<ThingDef, ThingDef>> overrideSpawnData = null, Action<Thing> onSpawned = null, bool blueprint = false)
	{
		bool flag = data.def.IsBlueprint || blueprint;
		ThingDef thingDef = (data.def.IsBlueprint ? ((ThingDef)data.def.entityDefToBuild) : data.def);
		ThingDef thingDef2 = (data.def.IsBlueprint ? data.def : data.def.blueprintDef);
		rot = (thingDef.rotatable ? rot : thingDef.defaultPlacingRot);
		if ((flag && thingDef2 == null) || (flag && !GenConstruct.CanPlaceBlueprintAt(thingDef, cell, rot, map, godMode: false, null, null, data.stuff)) || (!flag && !GenSpawn.CanSpawnAt(thingDef, cell, map, rot)))
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
			if (GenSpawn.CanSpawnAt(thingDef.blueprintDef, cell, map, rot, canWipeEdifices: false))
			{
				thing = GenConstruct.PlaceBlueprintForBuild(thingDef, cell, map, rot, Faction.OfPlayer, stuff);
			}
		}
		else
		{
			thing = ThingMaker.MakeThing(thingDef, stuff);
			if (thing.def.CanHaveFaction)
			{
				thing.SetFaction(faction);
			}
			thing.stackCount = data.stackCountRange.RandomInRange;
			if (data.hp > 0)
			{
				thing.HitPoints = data.hp;
			}
			CompColorable comp;
			if (data.colorDef != null && thing is Building building)
			{
				building.ChangePaint(data.colorDef);
			}
			else if (data.color != default(Color) && thing.TryGetComp(out comp))
			{
				comp.SetColor(data.color);
			}
			if (data.quality.HasValue && thing.TryGetComp(out CompQuality comp2))
			{
				comp2.SetQuality(data.quality.Value, ArtGenerationContext.Outsider);
			}
			GenSpawn.Spawn(thing, cell, map, rot);
		}
		if (thing != null)
		{
			spawned?.Add(thing);
			onSpawned?.Invoke(thing);
		}
	}

	private static bool CanSpawnThing(PrefabThingData data, Map map, IntVec3 cell, Rot4 rot, bool canWipeEdifices = true)
	{
		rot = (data.def.rotatable ? rot : Rot4.North);
		return GenSpawn.CanSpawnAt(data.def, cell, map, rot, canWipeEdifices);
	}

	public static IntVec3 GetAdjustedLocalPosition(IntVec3 local, Rot4 rot)
	{
		if (rot == Rot4.East)
		{
			ref int x = ref local.x;
			ref int z = ref local.z;
			int z2 = local.z;
			int num = -local.x;
			x = z2;
			z = num;
		}
		else if (rot == Rot4.South)
		{
			ref int x = ref local.x;
			ref int z3 = ref local.z;
			int num = -local.x;
			int z2 = -local.z;
			x = num;
			z3 = z2;
		}
		else if (rot == Rot4.West)
		{
			ref int x = ref local.x;
			ref int z4 = ref local.z;
			int z2 = -local.z;
			int num = local.x;
			x = z2;
			z4 = num;
		}
		return local;
	}

	public static Rot4 ValidateRotation(PrefabDef prefab, Rot4 rot)
	{
		if (!prefab.rotations.HasFlag((RotEnum)rot))
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

	public static PrefabDef CreatePrefab(CellRect rect, bool copyAllThings = false, bool copyTerrain = false)
	{
		HashSet<Thing> hashSet = new HashSet<Thing>();
		HashSet<Thing> group = new HashSet<Thing>();
		PrefabDef prefabDef = new PrefabDef();
		List<Thing> list = (from t in rect.Cells.SelectMany((IntVec3 c) => from thing in c.GetThingList(Find.CurrentMap)
				where thing is Building || copyAllThings
				select thing)
			orderby t is Building descending, t.def.shortHash descending
			select t).ToList();
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing2 = list[i];
				group.Clear();
				if (!hashSet.Add(thing2))
				{
					continue;
				}
				for (int j = i + 1; j < list.Count; j++)
				{
					Thing thing3 = list[j];
					if (!hashSet.Contains(thing3) && !group.Contains(thing3) && thing3.def == thing2.def && thing3.stackCount == thing2.stackCount && thing3.Rotation == thing2.Rotation && thing3.HitPoints == thing2.HitPoints && thing3.Stuff == thing2.Stuff && (!(thing3 is Building building) || !(thing2 is Building building2) || building.PaintColorDef == building2.PaintColorDef) && (!thing3.TryGetComp(out CompColorable comp) || !thing2.TryGetComp(out CompColorable comp2) || comp == comp2) && (!thing3.TryGetQuality(out var qc) || !thing2.TryGetQuality(out var qc2) || qc == qc2))
					{
						if (!group.Any())
						{
							group.Add(thing2);
						}
						hashSet.Add(thing3);
						group.Add(thing3);
					}
				}
				IntVec3 intVec = thing2.Position - rect.Min;
				RotationDirection relativeRotation = RotationDirection.None;
				if (thing2.Rotation == Rot4.East)
				{
					relativeRotation = RotationDirection.Clockwise;
				}
				else if (thing2.Rotation == Rot4.South)
				{
					relativeRotation = RotationDirection.Opposite;
				}
				else if (thing2.Rotation == Rot4.West)
				{
					relativeRotation = RotationDirection.Counterclockwise;
				}
				PrefabThingData prefabThingData = new PrefabThingData
				{
					def = thing2.def,
					relativeRotation = relativeRotation,
					stuff = thing2.Stuff
				};
				if (thing2.TryGetComp(out CompQuality comp3))
				{
					prefabThingData.quality = comp3.Quality;
				}
				CompColorable comp4;
				if (thing2 is Building building3)
				{
					prefabThingData.colorDef = building3.PaintColorDef;
				}
				else if (thing2.TryGetComp(out comp4) && prefabThingData.color != Color.white)
				{
					prefabThingData.color = comp4.Color;
				}
				if (group.Any())
				{
					if (thing2.def.Size == IntVec2.One)
					{
						prefabThingData.rects = new List<CellRect>();
						foreach (CellRect item3 in rect.EnumerateRectanglesCovering(Validator))
						{
							prefabThingData.rects.Add(item3.MovedBy(-rect.Min));
						}
					}
					else
					{
						prefabThingData.positions = new List<IntVec3>();
						foreach (Thing item4 in group)
						{
							prefabThingData.positions.Add(item4.Position - rect.Min);
						}
					}
				}
				else if (intVec != IntVec3.Zero)
				{
					prefabThingData.position = intVec;
				}
				if (thing2.def.useHitPoints && thing2.HitPoints != thing2.MaxHitPoints)
				{
					prefabThingData.hp = thing2.HitPoints;
				}
				if (thing2.stackCount != 1)
				{
					prefabThingData.stackCountRange = new IntRange(thing2.stackCount, thing2.stackCount);
				}
				prefabDef.things.Add(prefabThingData);
			}
		}
		if (copyTerrain)
		{
			Dictionary<(TerrainDef, ColorDef), List<IntVec3>> dictionary = new Dictionary<(TerrainDef, ColorDef), List<IntVec3>>();
			TerrainGrid terrainGrid = Find.CurrentMap.terrainGrid;
			foreach (IntVec3 cell in rect.Cells)
			{
				TerrainDef terrain = cell.GetTerrain(Find.CurrentMap);
				(TerrainDef, ColorDef) key = (terrain, terrainGrid.ColorAt(cell));
				if (!dictionary.TryGetValue(key, out var value))
				{
					value = (dictionary[key] = new List<IntVec3>());
				}
				value.Add(cell);
			}
			foreach (KeyValuePair<(TerrainDef, ColorDef), List<IntVec3>> item5 in dictionary)
			{
				item5.Deconstruct(out var key2, out var value2);
				(TerrainDef, ColorDef) tuple = key2;
				TerrainDef item = tuple.Item1;
				ColorDef item2 = tuple.Item2;
				List<IntVec3> list3 = value2;
				PrefabTerrainData prefabTerrainData = new PrefabTerrainData
				{
					def = item,
					color = item2,
					rects = new List<CellRect>()
				};
				foreach (CellRect item6 in rect.EnumerateRectanglesCovering(Validator))
				{
					prefabTerrainData.rects.Add(item6.MovedBy(-rect.Min));
				}
				prefabDef.terrain.Add(prefabTerrainData);
				bool Validator(IntVec3 index)
				{
					return list3.Contains(index);
				}
			}
		}
		return prefabDef;
		bool Validator(IntVec3 index)
		{
			return group.Any((Thing t) => t.Position == index);
		}
	}
}
