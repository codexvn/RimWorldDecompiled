using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public class GenStep_OrbitalWreck : GenStep_BaseRuins
{
	[CompilerGenerated]
	private sealed class _003CGetRects_003Ed__39 : IEnumerable<CellRect>, IEnumerable, IEnumerator<CellRect>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private CellRect _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		public GenStep_OrbitalWreck _003C_003E4__this;

		private List<CellRect>.Enumerator _003C_003E7__wrap1;

		CellRect IEnumerator<CellRect>.Current
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
		public _003CGetRects_003Ed__39(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = default(List<CellRect>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				GenStep_OrbitalWreck genStep_OrbitalWreck = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					int num2 = 0;
					int num3 = Mathf.RoundToInt(RuinCoverageRange.RandomInRange * (float)chunks.ApproximateArea);
					List<CellRect> list = new List<CellRect>();
					foreach (CellRect item in chunks.EnumeratedRects.OrderBy((CellRect r) => Mathf.Abs(0.5f - (float)r.Height / (float)r.Area)))
					{
						CellRect rect = item;
						IntVec2 size = new IntVec2(Mathf.Min(item.Size.x, MaxRuinSizeRange.RandomInRange), Mathf.Min(item.Size.z, MaxRuinSizeRange.RandomInRange));
						if (size.x < item.Size.x || size.z < item.Size.z)
						{
							item.TryFindRandomInnerRect(size, out rect);
						}
						rect = rect.ContractedBy(RuinContractRange.RandomInRange, RuinContractRange.RandomInRange, RuinContractRange.RandomInRange, RuinContractRange.RandomInRange);
						list.Add(rect);
						num2 += rect.Area;
						if (num2 >= num3)
						{
							break;
						}
					}
					MapGenUtility.TryFixInvalidRects(list, map, genStep_OrbitalWreck.MinRegionSize, genStep_OrbitalWreck.UseUsedRects, genStep_OrbitalWreck.MinAffordance, genStep_OrbitalWreck.AvoidWaterRoads);
					MapGenUtility.RemoveInvalidRects(list, map, genStep_OrbitalWreck.UseUsedRects, genStep_OrbitalWreck.MinAffordance, genStep_OrbitalWreck.AvoidWaterRoads);
					_003C_003E7__wrap1 = list.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				}
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					CellRect current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<CellRect>.Enumerator);
				return false;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<CellRect> IEnumerable<CellRect>.GetEnumerator()
		{
			_003CGetRects_003Ed__39 _003CGetRects_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetRects_003Ed__ = this;
			}
			else
			{
				_003CGetRects_003Ed__ = new _003CGetRects_003Ed__39(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetRects_003Ed__.map = _003C_003E3__map;
			return _003CGetRects_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CellRect>)this).GetEnumerator();
		}
	}

	private FactionDef factionDef;

	private List<PrefabParms> prefabs = new List<PrefabParms>();

	private List<LayoutParms> layouts = new List<LayoutParms>();

	private OrbitalDebrisDef orbitalDebrisDef;

	private float? temperature;

	private static readonly List<CellRect> importantRects = new List<CellRect>();

	private static CellRect intBoundary;

	public static Chunks chunks;

	private const int MapBorderPadding = 20;

	private static readonly FloatRange InitialPercentageRange = new FloatRange(0.65f, 0.65f);

	private static readonly FloatRange DamagedMaxPercentageRange = new FloatRange(0.5f, 0.5f);

	private static readonly IntRange PlatformPadBorderLumpLengthRange = new IntRange(8, 12);

	private static readonly IntRange PlatformPadBorderLumpOffsetRange = new IntRange(-2, 3);

	private static readonly IntRange HoleLumpOffsetRange = new IntRange(-1, 2);

	private static readonly IntRange HoleSizeRange = new IntRange(3, 6);

	private static readonly FloatRange HolesPerHundredCellsRange = new FloatRange(0.025f, 0.025f);

	private static readonly FloatRange RubblePilesPer10K = new FloatRange(15f, 18f);

	private static readonly IntRange RubblePileCountRange = new IntRange(2, 12);

	private static readonly IntRange RubblePileDistanceRange = new IntRange(3, 10);

	private static readonly FloatRange ChunksPer10K = new FloatRange(1f, 2f);

	private static readonly IntRange ChunksCountRange = new IntRange(2, 4);

	private static readonly IntRange ChunksDistanceRange = new IntRange(3, 6);

	private static readonly IntRange MaxRuinSizeRange = new IntRange(60, 80);

	private static readonly IntRange RuinContractRange = new IntRange(1, 2);

	private static readonly FloatRange RuinCoverageRange = new FloatRange(0.6f, 0.65f);

	private const int PlatformRegionSize = 22;

	protected override LayoutDef LayoutDef => layouts.RandomElementByWeight((LayoutParms parms) => parms.weight).def;

	protected override bool UseUsedRects => true;

	protected override Faction Faction
	{
		get
		{
			if (factionDef != null)
			{
				return Find.FactionManager.FirstFactionOfDef(factionDef);
			}
			return null;
		}
	}

	protected override FloatRange DefaultMapFillPercentRange => new FloatRange(1f, 1f);

	protected override FloatRange MergeRange => FloatRange.One;

	public override int SeedPart => 982581;

	protected override CellRect GetBounds(Map map)
	{
		return intBoundary.ContractedBy(2);
	}

	[IteratorStateMachine(typeof(_003CGetRects_003Ed__39))]
	protected override IEnumerable<CellRect> GetRects(CellRect area, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetRects_003Ed__39(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__map = map
		};
	}

	protected override IEnumerable<CellRect> GetRectOrder(IEnumerable<CellRect> rects, Map map)
	{
		return rects;
	}

	public override void Generate(Map map, GenStepParams parms)
	{
		if (ModLister.CheckOdyssey("Orbital Wreck"))
		{
			using (map.pathing.DisableIncrementalScope())
			{
				GenerateSpawnBasePlatform(map);
			}
			int cells = chunks.ApproximateArea;
			int maximum = Mathf.RoundToInt((float)cells * DamagedMaxPercentageRange.RandomInRange);
			Chunks obj = chunks;
			List<CellRect> spaces = importantRects;
			SpaceGenUtility.ScatterExtrusions(map, obj, null, null, null, null, spaces);
			GenerateRuins(map, parms, DefaultMapFillPercentRange);
			PostStructuresDamage(map, maximum, ref cells);
			SpaceGenUtility.ScatterPrefabs(map, chunks, Faction, prefabs, ValidRect, null);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.RubblePile, RubblePilesPer10K, RubblePileCountRange, RubblePileDistanceRange);
			MapGenUtility.SpawnExteriorLumps(map, ThingDefOf.ChunkSlagSteel, ChunksPer10K, ChunksCountRange, ChunksDistanceRange);
			importantRects.Clear();
			map.FogOfWarColor = ColorLibrary.SpaceHumanFog;
			map.OrbitalDebris = orbitalDebrisDef;
		}
	}

	private void GenerateSpawnBasePlatform(Map map)
	{
		chunks = new Chunks(map.BoundsRect(20), 22);
		intBoundary = chunks.boundary;
		SpaceGenUtility.ChunksSetSparsley(chunks, InitialPercentageRange);
		foreach (CellRect enumeratedRect in chunks.EnumeratedRects)
		{
			foreach (IntVec3 cell in enumeratedRect.Cells)
			{
				map.terrainGrid.SetTerrain(cell, TerrainDefOf.OrbitalPlatform);
			}
			CellRect rect = enumeratedRect;
			MapGenUtility.DoRectEdgeLumps(map, TerrainDefOf.OrbitalPlatform, ref rect, PlatformPadBorderLumpLengthRange, PlatformPadBorderLumpOffsetRange);
		}
	}

	protected override LayoutStructureSketch GenerateAndSpawn(CellRect rect, Map map, GenStepParams parms, LayoutDef layoutDef)
	{
		LayoutStructureSketch layoutStructureSketch = base.GenerateAndSpawn(rect, map, parms, layoutDef);
		foreach (LayoutRoom room in layoutStructureSketch.structureLayout.Rooms)
		{
			foreach (LayoutRoomDef def in room.defs)
			{
				if (!def.canRemoveBorderDoors || !def.canRemoveBorderWalls)
				{
					importantRects.AddRange(room.rects);
					break;
				}
			}
		}
		MapGenerator.SetVar("SpawnRect", rect);
		MapGenerator.SetVar("RectOfInterest", rect);
		MapGenerator.UsedRects.Add(rect);
		return layoutStructureSketch;
	}

	private void PostStructuresDamage(Map map, int maximum, ref int cells)
	{
		SpaceGenUtility.DamageHoles(chunks, map, maximum, ref cells, HoleSizeRange, HolesPerHundredCellsRange, 1f, ValidRect, importantRects);
	}

	private static bool ValidRect(CellRect rect)
	{
		return SpaceGenUtility.NoIntersection(rect, importantRects, HoleLumpOffsetRange);
	}
}
