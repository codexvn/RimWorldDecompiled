using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;

namespace RimWorld;

public class GenStep_ScatterRoadDebris : GenStep_Scatterer
{
	[CompilerGenerated]
	private sealed class _003Cget_Debris_003Ed__11 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		ThingDef IEnumerator<ThingDef>.Current
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
		public _003Cget_Debris_003Ed__11(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientRustedCarFrame;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientRustedJeep;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientRustedCar;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientTank;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientPodCar;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientHydrant;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<ThingDef> IEnumerable<ThingDef>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003Cget_Debris_003Ed__11(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private const int EngineBlockMinDistance = 10;

	private const float EngineBlockChance = 0.5f;

	private const float OilSmearChance = 0.15f;

	private static readonly IntRange VehicleRangeNonRoadMap = new IntRange(1, 2);

	private static readonly IntRange VehicleRangeRoadMap = new IntRange(2, 3);

	private ThingDef thingToPlace;

	private Rot4 rotation;

	private bool mapHasRoads;

	public override int SeedPart => 765346456;

	public IEnumerable<ThingDef> Debris
	{
		[IteratorStateMachine(typeof(_003Cget_Debris_003Ed__11))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_Debris_003Ed__11(-2);
		}
	}

	protected override float GetPlacementFactor(Map map)
	{
		float num = 1f;
		foreach (TileMutatorDef mutator in map.TileInfo.Mutators)
		{
			num *= mutator.junkDensityFactor;
		}
		return num;
	}

	public override void Generate(Map map, GenStepParams parms)
	{
		if (ModLister.CheckIdeology("Scatter road debris"))
		{
			allowInWaterBiome = false;
			mapHasRoads = HasAncientRoads(map);
			count = (mapHasRoads ? VehicleRangeRoadMap : VehicleRangeNonRoadMap).RandomInRange;
			thingToPlace = Debris.RandomElement();
			base.Generate(map, parms);
		}
	}

	private bool HasAncientRoads(Map map)
	{
		if (map.TileInfo.Isnt<SurfaceTile>(out var casted) || casted.Roads == null)
		{
			return false;
		}
		for (int i = 0; i < casted.Roads.Count; i++)
		{
			if (casted.Roads[i].road == RoadDefOf.AncientAsphaltHighway || casted.Roads[i].road == RoadDefOf.AncientAsphaltRoad)
			{
				return true;
			}
		}
		return false;
	}

	protected override bool CanScatterAt(IntVec3 c, Map map)
	{
		if (!base.CanScatterAt(c, map))
		{
			return false;
		}
		if (mapHasRoads && !c.GetTerrain(map).IsRoad)
		{
			return false;
		}
		if (thingToPlace.rotatable)
		{
			int num = Rand.RangeInclusive(1, 4);
			for (int i = 0; i < 4; i++)
			{
				rotation = new Rot4((i + num) % 4);
				if (CanPlaceThingAt(c, rotation, map, thingToPlace))
				{
					return true;
				}
			}
			return false;
		}
		rotation = thingToPlace.defaultPlacingRot;
		return CanPlaceThingAt(c, rotation, map, thingToPlace);
	}

	private bool CanPlaceThingAt(IntVec3 c, Rot4 rot, Map map, ThingDef thingDef)
	{
		return ScatterDebrisUtility.CanPlaceThingAt(c, rot, map, thingDef);
	}

	protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
	{
		Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(thingToPlace), loc, map, rotation);
		ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_MachineBits, 0.75f);
		ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.25f, 0);
		ThingDef engineBlockDef = (Rand.Bool ? ThingDefOf.AncientRustedEngineBlock : ThingDefOf.AncientLargeRustedEngineBlock);
		if (Rand.Chance(0.5f) && RCellFinder.TryFindRandomCellNearWith(loc, (IntVec3 c) => CanPlaceThingAt(c, Rot4.North, map, engineBlockDef), map, out var result, 10))
		{
			ScatterDebrisUtility.ScatterFilthAroundThing(GenSpawn.Spawn(ThingMaker.MakeThing(engineBlockDef), result, map, Rot4.North), map, ThingDefOf.Filth_MachineBits);
			ScatterDebrisUtility.ScatterFilthAroundThing(thing, map, ThingDefOf.Filth_OilSmear, 0.15f, 0);
		}
		thingToPlace = Debris.RandomElement();
	}
}
