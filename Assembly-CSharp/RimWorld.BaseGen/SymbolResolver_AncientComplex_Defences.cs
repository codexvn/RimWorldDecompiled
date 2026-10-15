using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld.BaseGen;

public class SymbolResolver_AncientComplex_Defences : SymbolResolver
{
	[CompilerGenerated]
	private sealed class _003Cget_AncientVehicles_003Ed__12 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
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
		public _003Cget_AncientVehicles_003Ed__12(int _003C_003E1__state)
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
				_003C_003E2__current = ThingDefOf.AncientRustedJeep;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientRustedCarFrame;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = ThingDefOf.AncientTank;
				_003C_003E1__state = 3;
				return true;
			case 3:
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
			return new _003Cget_AncientVehicles_003Ed__12(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private static List<IntVec3> doorCells = new List<IntVec3>();

	public const int FenceExpansionDistance = 5;

	private const int RazorWireExpansionDistance = 7;

	private const int TankTrapExpansionDistance = 10;

	private const int VehicleExpansionDistance = 14;

	private const int TripodExpansionRect = 1;

	private const float ChanceToPlaceFence = 0.8f;

	private const float ChanceToPlaceRazorWire = 0.6f;

	private const float ChanceToPlaceTankTrap = 0.05f;

	private const float ChanceToPlaceGunTripod = 0.05f;

	private const float ChanceToPlaceVehicle = 0.015f;

	private IEnumerable<ThingDef> AncientVehicles
	{
		[IteratorStateMachine(typeof(_003Cget_AncientVehicles_003Ed__12))]
		get
		{
			//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
			return new _003Cget_AncientVehicles_003Ed__12(-2);
		}
	}

	public override bool CanResolve(ResolveParams rp)
	{
		return base.CanResolve(rp);
	}

	public override void Resolve(ResolveParams rp)
	{
		doorCells.Clear();
		Map map = BaseGen.globalSettings.map;
		foreach (IntVec3 item in rp.rect)
		{
			Building_Door door = item.GetDoor(BaseGen.globalSettings.map);
			if (door != null && map.reachability.CanReachMapEdge(item, TraverseParms.For(TraverseMode.NoPassClosedDoors)))
			{
				doorCells.Add(door.Position);
			}
		}
		Thing placedThing;
		foreach (IntVec3 item2 in rp.rect.ExpandedBy(5).EdgeCells.InRandomOrder())
		{
			if (Rand.Chance(0.8f))
			{
				TryPlaceThing(ThingDefOf.AncientFence, item2, out placedThing, null);
			}
		}
		foreach (IntVec3 item3 in rp.rect.ExpandedBy(7).EdgeCells.InRandomOrder())
		{
			if (Rand.Chance(0.6f))
			{
				TryPlaceThing(ThingDefOf.AncientRazorWire, item3, out placedThing, null);
			}
		}
		foreach (IntVec3 item4 in rp.rect.ExpandedBy(10).EdgeCells.InRandomOrder())
		{
			if (Rand.Chance(0.05f) && TryPlaceThing(ThingDefOf.AncientTankTrap, item4, out var placedThing2, null))
			{
				ScatterDebrisUtility.ScatterFilthAroundThing(placedThing2, map, ThingDefOf.Filth_RubbleBuilding, 0.5f, 0);
			}
		}
		foreach (IntVec3 item5 in rp.rect.ExpandedBy(14).EdgeCells.InRandomOrder())
		{
			ThingDef thingDef = AncientVehicles.RandomElement();
			if (Rand.Chance(0.015f) && TryPlaceThing(thingDef, item5, out var placedThing3, thingDef.rotatable ? Rot4.Random : thingDef.defaultPlacingRot))
			{
				ScatterDebrisUtility.ScatterFilthAroundThing(placedThing3, map, ThingDefOf.Filth_MachineBits);
			}
		}
		foreach (IntVec3 item6 in rp.rect.ExpandedBy(1).EdgeCells.InRandomOrder())
		{
			if (!Rand.Chance(0.05f) || !TryPlaceThing(ThingDefOf.AncientMegaCannonTripod, item6, out var placedThing4, null) || !Rand.Bool)
			{
				continue;
			}
			ScatterDebrisUtility.ScatterFilthAroundThing(placedThing4, map, ThingDefOf.Filth_MachineBits);
			foreach (IntVec3 item7 in GenAdj.OccupiedRect(item6, ThingDefOf.AncientMegaCannonTripod.defaultPlacingRot, ThingDefOf.AncientMegaCannonTripod.Size).ExpandedBy(ThingDefOf.AncientMegaCannonBarrel.Size.MagnitudeManhattan).EdgeCells.InRandomOrder())
			{
				Rot4 random = Rot4.Random;
				if (TryPlaceThing(ThingDefOf.AncientMegaCannonBarrel, item7, out placedThing, random))
				{
					break;
				}
			}
		}
		doorCells.Clear();
	}

	private bool CanReachEntrance(IntVec3 cell, List<IntVec3> entrancePositions)
	{
		Map map = BaseGen.globalSettings.map;
		for (int i = 0; i < entrancePositions.Count; i++)
		{
			if (map.reachability.CanReach(cell, entrancePositions[i], PathEndMode.OnCell, TraverseParms.For(TraverseMode.PassDoors)))
			{
				return true;
			}
		}
		return false;
	}

	private bool TryPlaceThing(ThingDef thingDef, IntVec3 position, out Thing placedThing, Rot4? rot = null)
	{
		Map map = BaseGen.globalSettings.map;
		CellRect rect = GenAdj.OccupiedRect(position, rot ?? thingDef.defaultPlacingRot, thingDef.size);
		if (!rect.InBounds(map))
		{
			placedThing = null;
			return false;
		}
		if (!GenConstruct.TerrainCanSupport(rect, map, thingDef))
		{
			placedThing = null;
			return false;
		}
		foreach (IntVec3 item in rect)
		{
			if (item.Roofed(map))
			{
				placedThing = null;
				return false;
			}
			List<Thing> thingList = item.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category != ThingCategory.Plant)
				{
					placedThing = null;
					return false;
				}
			}
		}
		placedThing = GenSpawn.Spawn(ThingMaker.MakeThing(thingDef), position, map, rot ?? thingDef.defaultPlacingRot);
		return true;
	}
}
