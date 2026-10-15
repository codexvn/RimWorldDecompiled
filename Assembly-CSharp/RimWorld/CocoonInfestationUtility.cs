using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld;

public static class CocoonInfestationUtility
{
	[CompilerGenerated]
	private sealed class _003CGetCocoonsToSpawn_003Ed__3 : IEnumerable<ThingDef>, IEnumerable, IEnumerator<ThingDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private ThingDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float points;

		public float _003C_003E3__points;

		private IEnumerable<ThingDef> _003CallCocoons_003E5__2;

		private int _003Ctries_003E5__3;

		private ThingDef _003CcocoonDef_003E5__4;

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
		public _003CGetCocoonsToSpawn_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CallCocoons_003E5__2 = null;
			_003CcocoonDef_003E5__4 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				points -= _003CcocoonDef_003E5__4.building.combatPower;
				_003Ctries_003E5__3++;
				if (_003Ctries_003E5__3 > 1000)
				{
					Log.Warning("Failed to spend all points when selecing cocoons to spawn.");
					goto IL_00db;
				}
				_003CcocoonDef_003E5__4 = null;
			}
			else
			{
				_003C_003E1__state = -1;
				_003CallCocoons_003E5__2 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef t) => t.building != null && t.building.isInsectCocoon);
				_003Ctries_003E5__3 = 0;
			}
			if (points > 0f && _003CallCocoons_003E5__2.TryRandomElement(out _003CcocoonDef_003E5__4))
			{
				_003C_003E2__current = _003CcocoonDef_003E5__4;
				_003C_003E1__state = 1;
				return true;
			}
			goto IL_00db;
			IL_00db:
			return false;
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
			_003CGetCocoonsToSpawn_003Ed__3 _003CGetCocoonsToSpawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetCocoonsToSpawn_003Ed__ = this;
			}
			else
			{
				_003CGetCocoonsToSpawn_003Ed__ = new _003CGetCocoonsToSpawn_003Ed__3(0);
			}
			_003CGetCocoonsToSpawn_003Ed__.points = _003C_003E3__points;
			return _003CGetCocoonsToSpawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ThingDef>)this).GetEnumerator();
		}
	}

	private const float MinDistanceBetweenCocoons = 2f;

	private static List<ThingDef> tmpCocoonsToSpawn = new List<ThingDef>();

	public static bool TryFindCocoonSpawnPositionNear(IntVec3 root, Map map, ThingDef cocoon, out IntVec3 result)
	{
		result = CellFinder.FindNoWipeSpawnLocNear(root, map, cocoon, Rot4.North, Mathf.FloorToInt(GenRadial.MaxRadialPatternRadius), (IntVec3 c) => CanSpawnCocoonAt(c, map));
		return result.IsValid;
	}

	public static bool CanSpawnCocoonAt(IntVec3 c, Map map)
	{
		if (!c.Walkable(map) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
		{
			return false;
		}
		foreach (IntVec3 item in GenRadial.RadialCellsAround(c, 2f, useCenter: true))
		{
			if (!item.InBounds(map))
			{
				continue;
			}
			List<Thing> thingList = item.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i] is CocoonSpawner)
				{
					return false;
				}
				if (thingList[i].def.building != null)
				{
					if (thingList[i].def.building.isInsectCocoon)
					{
						return false;
					}
					if (thingList[i].def.passability == Traversability.Impassable)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetCocoonsToSpawn_003Ed__3))]
	public static IEnumerable<ThingDef> GetCocoonsToSpawn(float points)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetCocoonsToSpawn_003Ed__3(-2)
		{
			_003C_003E3__points = points
		};
	}

	public static List<Thing> SpawnCocoonInfestation(IntVec3 root, Map map, float points)
	{
		List<Thing> list = new List<Thing>();
		if (!ModLister.CheckBiotech("Spawn cocoon infestation"))
		{
			return list;
		}
		tmpCocoonsToSpawn.Clear();
		tmpCocoonsToSpawn.AddRange(GetCocoonsToSpawn(points));
		if (tmpCocoonsToSpawn.Count == 0)
		{
			Log.Warning("Failed to find cocoons to spawn with points:" + points);
			return list;
		}
		int nextCocoonGroupID = Find.UniqueIDsManager.GetNextCocoonGroupID();
		IntVec3 result;
		for (int i = 0; i < tmpCocoonsToSpawn.Count && TryFindCocoonSpawnPositionNear(root, map, tmpCocoonsToSpawn[i], out result); i++)
		{
			list.Add(GenSpawn.Spawn(MakeCocoon(tmpCocoonsToSpawn[i], nextCocoonGroupID), result, map, WipeMode.FullRefund));
		}
		tmpCocoonsToSpawn.Clear();
		return list;
	}

	private static Thing MakeCocoon(ThingDef cocoonDef, int groupID)
	{
		CocoonSpawner obj = (CocoonSpawner)ThingMaker.MakeThing(ThingDefOf.CocoonSpawner);
		obj.cocoon = cocoonDef;
		obj.groupID = groupID;
		return obj;
	}
}
