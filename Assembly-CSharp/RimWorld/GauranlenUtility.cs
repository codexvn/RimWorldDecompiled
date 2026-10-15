using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public static class GauranlenUtility
{
	[CompilerGenerated]
	private sealed class _003CGetConnectionsAffectedByBuilding_003Ed__2 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private Faction faction;

		public Faction _003C_003E3__faction;

		private Map map;

		public Map _003C_003E3__map;

		private IntVec3 pos;

		public IntVec3 _003C_003E3__pos;

		private Rot4 rotation;

		public Rot4 _003C_003E3__rotation;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

		Thing IEnumerator<Thing>.Current
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
		public _003CGetConnectionsAffectedByBuilding_003Ed__2(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
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
					if (!MeditationUtility.CountsAsArtificialBuilding(def, faction))
					{
						return false;
					}
					_003C_003E7__wrap1 = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					CompTreeConnection compTreeConnection = current.TryGetComp<CompTreeConnection>();
					if (compTreeConnection != null && compTreeConnection.WillBeAffectedBy(def, faction, pos, rotation))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
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
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetConnectionsAffectedByBuilding_003Ed__2 _003CGetConnectionsAffectedByBuilding_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetConnectionsAffectedByBuilding_003Ed__ = this;
			}
			else
			{
				_003CGetConnectionsAffectedByBuilding_003Ed__ = new _003CGetConnectionsAffectedByBuilding_003Ed__2(0);
			}
			_003CGetConnectionsAffectedByBuilding_003Ed__.map = _003C_003E3__map;
			_003CGetConnectionsAffectedByBuilding_003Ed__.def = _003C_003E3__def;
			_003CGetConnectionsAffectedByBuilding_003Ed__.faction = _003C_003E3__faction;
			_003CGetConnectionsAffectedByBuilding_003Ed__.pos = _003C_003E3__pos;
			_003CGetConnectionsAffectedByBuilding_003Ed__.rotation = _003C_003E3__rotation;
			return _003CGetConnectionsAffectedByBuilding_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	public static bool CocoonAndPodCellValidator(IntVec3 c, Map map, ThingDef thingDefToSpawn = null)
	{
		if (thingDefToSpawn?.plant != null && thingDefToSpawn.CanEverPlantAt(c, map, out var _).Accepted)
		{
			return false;
		}
		if (!c.InBounds(map) || !c.Standable(map))
		{
			return false;
		}
		List<Thing> thingList = c.GetThingList(map);
		for (int i = 0; i < thingList.Count; i++)
		{
			if (thingList[i].def.category == ThingCategory.Building || thingList[i].def.IsFrame)
			{
				return false;
			}
			if (thingList[i].def == ThingDefOf.Plant_TreeGauranlen || thingList[i].def == ThingDefOf.GaumakerCocoon || thingList[i].def == ThingDefOf.Plant_PodGauranlen)
			{
				return false;
			}
		}
		return true;
	}

	public static List<Thing> BuildingsAffectingConnectionStrengthAt(IntVec3 pos, Map map, CompProperties_TreeConnection props)
	{
		return map.listerArtificialBuildingsForMeditation.GetForCell(pos, props.radiusToBuildingForConnectionStrengthLoss);
	}

	[IteratorStateMachine(typeof(_003CGetConnectionsAffectedByBuilding_003Ed__2))]
	public static IEnumerable<Thing> GetConnectionsAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetConnectionsAffectedByBuilding_003Ed__2(-2)
		{
			_003C_003E3__map = map,
			_003C_003E3__def = def,
			_003C_003E3__faction = faction,
			_003C_003E3__pos = pos,
			_003C_003E3__rotation = rotation
		};
	}

	public static void DrawConnectionsAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
	{
		_ = (CompProperties_TreeConnection)def.CompDefFor<CompTreeConnection>();
		int num = 0;
		foreach (Thing item in GetConnectionsAffectedByBuilding(map, def, faction, pos, rotation))
		{
			if (num++ > 10)
			{
				break;
			}
			GenAdj.OccupiedRect(pos, rotation, def.size);
			GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), item.TrueCenter(), SimpleColor.Red);
		}
	}
}
