using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;
using Verse.AI;

namespace Verse;

public sealed class ListerBuildings : IDisposable
{
	public class TrackingScope : IDisposable, IEnumerable<Building>, IEnumerable
	{
		private bool enabled;

		private readonly ListerBuildings parent;

		public readonly HashSet<Building> tracked;

		public readonly Predicate<Building> predicate;

		public bool Enabled => enabled;

		public TrackingScope(ListerBuildings parent, Predicate<Building> predicate = null)
		{
			tracked = SimplePool<HashSet<Building>>.Get();
			this.parent = parent;
			this.predicate = predicate;
			Start();
		}

		public void Dispose()
		{
			tracked.Clear();
			SimplePool<HashSet<Building>>.Return(tracked);
			Stop();
		}

		public void Start()
		{
			if (!enabled)
			{
				enabled = true;
				parent.StartTracker(this);
			}
		}

		public void Stop()
		{
			if (enabled)
			{
				enabled = false;
				parent.ReleaseTracker(this);
			}
		}

		public IEnumerator<Building> GetEnumerator()
		{
			return tracked.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return tracked.GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllBuildingsColonistOfClass_003Ed__24<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : Building
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ListerBuildings _003C_003E4__this;

		private int _003Ci_003E5__2;

		T IEnumerator<T>.Current
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
		public _003CAllBuildingsColonistOfClass_003Ed__24(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			ListerBuildings listerBuildings = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0062;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0072;
			IL_0062:
			_003Ci_003E5__2++;
			goto IL_0072;
			IL_0072:
			if (_003Ci_003E5__2 < listerBuildings.allBuildingsColonist.Count)
			{
				if (listerBuildings.allBuildingsColonist[_003Ci_003E5__2] is T val)
				{
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0062;
			}
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CAllBuildingsColonistOfClass_003Ed__24<T> result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CAllBuildingsColonistOfClass_003Ed__24<T>(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllBuildingsNonColonistOfDef_003Ed__26 : IEnumerable<Building>, IEnumerable, IEnumerator<Building>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Building _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ListerBuildings _003C_003E4__this;

		private ThingDef def;

		public ThingDef _003C_003E3__def;

		private int _003Ci_003E5__2;

		Building IEnumerator<Building>.Current
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
		public _003CAllBuildingsNonColonistOfDef_003Ed__26(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			ListerBuildings listerBuildings = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_006c;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_007c;
			IL_006c:
			_003Ci_003E5__2++;
			goto IL_007c;
			IL_007c:
			if (_003Ci_003E5__2 < listerBuildings.allBuildingsNonColonist.Count)
			{
				if (listerBuildings.allBuildingsNonColonist[_003Ci_003E5__2].def == def)
				{
					_003C_003E2__current = listerBuildings.allBuildingsNonColonist[_003Ci_003E5__2];
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_006c;
			}
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
		IEnumerator<Building> IEnumerable<Building>.GetEnumerator()
		{
			_003CAllBuildingsNonColonistOfDef_003Ed__26 _003CAllBuildingsNonColonistOfDef_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllBuildingsNonColonistOfDef_003Ed__ = this;
			}
			else
			{
				_003CAllBuildingsNonColonistOfDef_003Ed__ = new _003CAllBuildingsNonColonistOfDef_003Ed__26(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CAllBuildingsNonColonistOfDef_003Ed__.def = _003C_003E3__def;
			return _003CAllBuildingsNonColonistOfDef_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Building>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllColonistBuildingsOfType_003Ed__25<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ListerBuildings _003C_003E4__this;

		private int _003Ci_003E5__2;

		T IEnumerator<T>.Current
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
		public _003CAllColonistBuildingsOfType_003Ed__25(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			ListerBuildings listerBuildings = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0064;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0076;
			IL_0064:
			_003Ci_003E5__2++;
			goto IL_0076;
			IL_0076:
			if (_003Ci_003E5__2 < listerBuildings.allBuildingsColonist.Count)
			{
				if (listerBuildings.allBuildingsColonist[_003Ci_003E5__2] is T val)
				{
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0064;
			}
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
		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			_003CAllColonistBuildingsOfType_003Ed__25<T> result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CAllColonistBuildingsOfType_003Ed__25<T>(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}
	}

	public readonly List<Building> allBuildingsColonist = new List<Building>();

	public readonly List<Building> allBuildingsNonColonist = new List<Building>();

	public readonly HashSet<Building> allBuildingsColonistCombatTargets = new HashSet<Building>();

	public readonly HashSet<Building> allBuildingsColonistElecFire = new HashSet<Building>();

	public readonly HashSet<Building> allBuildingsAnimalPenMarkers = new HashSet<Building>();

	public readonly HashSet<Building> allBuildingsHitchingPosts = new HashSet<Building>();

	private readonly Dictionary<Thing, Blueprint_Install> reinstallationMap = new Dictionary<Thing, Blueprint_Install>();

	private readonly List<TrackingScope> scopes = new List<TrackingScope>();

	private static List<Building> allBuildingsColonistOfDefResult = new List<Building>();

	private static List<Building> allBuildingsColonistOfGroupResult = new List<Building>();

	public TrackingScope Track(Predicate<Building> predicate = null)
	{
		return new TrackingScope(this, predicate);
	}

	private void StartTracker(TrackingScope scope)
	{
		scopes.Add(scope);
	}

	private void ReleaseTracker(TrackingScope scope)
	{
		scopes.Remove(scope);
	}

	public void Add(Building b)
	{
		if (b.def.building != null && b.def.building.isNaturalRock)
		{
			return;
		}
		if (b.Faction == Faction.OfPlayer)
		{
			allBuildingsColonist.Add(b);
			if (b is IAttackTarget)
			{
				allBuildingsColonistCombatTargets.Add(b);
			}
		}
		else
		{
			allBuildingsNonColonist.Add(b);
		}
		CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
		if (compProperties != null && compProperties.shortCircuitInRain)
		{
			allBuildingsColonistElecFire.Add(b);
		}
		if (b.TryGetComp<CompAnimalPenMarker>() != null)
		{
			allBuildingsAnimalPenMarkers.Add(b);
		}
		if (b.def == ThingDefOf.CaravanPackingSpot)
		{
			allBuildingsHitchingPosts.Add(b);
		}
		foreach (TrackingScope scope in scopes)
		{
			if ((scope.Enabled && scope.predicate == null) || scope.predicate(b))
			{
				scope.tracked.Add(b);
			}
		}
	}

	public void Remove(Building b)
	{
		allBuildingsColonist.Remove(b);
		allBuildingsNonColonist.Remove(b);
		if (b is IAttackTarget)
		{
			allBuildingsColonistCombatTargets.Remove(b);
		}
		CompProperties_Power compProperties = b.def.GetCompProperties<CompProperties_Power>();
		if (compProperties != null && compProperties.shortCircuitInRain)
		{
			allBuildingsColonistElecFire.Remove(b);
		}
		allBuildingsAnimalPenMarkers.Remove(b);
		allBuildingsHitchingPosts.Remove(b);
		foreach (TrackingScope scope in scopes)
		{
			if (scope.Enabled)
			{
				scope.tracked.Remove(b);
			}
		}
	}

	public void RegisterInstallBlueprint(Blueprint_Install blueprint)
	{
		reinstallationMap.Add(blueprint.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified(), blueprint);
	}

	public void DeregisterInstallBlueprint(Blueprint_Install blueprint)
	{
		Thing thing = blueprint.MiniToInstallOrBuildingToReinstall?.GetInnerIfMinified();
		if (thing != null)
		{
			reinstallationMap.Remove(thing);
			return;
		}
		Thing thing2 = null;
		foreach (KeyValuePair<Thing, Blueprint_Install> item in reinstallationMap)
		{
			if (item.Value == blueprint)
			{
				thing2 = item.Key;
				break;
			}
		}
		if (thing2 != null)
		{
			reinstallationMap.Remove(thing2);
		}
	}

	public bool ColonistsHaveBuilding(ThingDef def)
	{
		for (int i = 0; i < allBuildingsColonist.Count; i++)
		{
			if (allBuildingsColonist[i].def == def)
			{
				return true;
			}
		}
		return false;
	}

	public bool ColonistsHaveBuilding(Func<Thing, bool> predicate)
	{
		for (int i = 0; i < allBuildingsColonist.Count; i++)
		{
			if (predicate(allBuildingsColonist[i]))
			{
				return true;
			}
		}
		return false;
	}

	public bool ColonistsHaveResearchBench()
	{
		return AllBuildingsColonistOfClass<Building_ResearchBench>().Any();
	}

	public bool ColonistsHaveBuildingWithPowerOn(ThingDef def)
	{
		List<Building> list = AllBuildingsColonistOfDef(def);
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].def == def)
			{
				CompPowerTrader compPowerTrader = list[i].TryGetComp<CompPowerTrader>();
				if (compPowerTrader == null || compPowerTrader.PowerOn)
				{
					return true;
				}
			}
		}
		return false;
	}

	public List<Building> AllBuildingsColonistOfDef(ThingDef def)
	{
		allBuildingsColonistOfDefResult.Clear();
		for (int i = 0; i < allBuildingsColonist.Count; i++)
		{
			if (allBuildingsColonist[i].def == def)
			{
				allBuildingsColonistOfDefResult.Add(allBuildingsColonist[i]);
			}
		}
		return allBuildingsColonistOfDefResult;
	}

	public List<Building> AllBuildingsColonistOfGroup(ThingRequestGroup group)
	{
		allBuildingsColonistOfGroupResult.Clear();
		for (int i = 0; i < allBuildingsColonist.Count; i++)
		{
			if (group.Includes(allBuildingsColonist[i].def))
			{
				allBuildingsColonistOfGroupResult.Add(allBuildingsColonist[i]);
			}
		}
		return allBuildingsColonistOfGroupResult;
	}

	[IteratorStateMachine(typeof(_003CAllBuildingsColonistOfClass_003Ed__24<>))]
	public IEnumerable<T> AllBuildingsColonistOfClass<T>() where T : Building
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllBuildingsColonistOfClass_003Ed__24<T>(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CAllColonistBuildingsOfType_003Ed__25<>))]
	public IEnumerable<T> AllColonistBuildingsOfType<T>()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllColonistBuildingsOfType_003Ed__25<T>(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CAllBuildingsNonColonistOfDef_003Ed__26))]
	public IEnumerable<Building> AllBuildingsNonColonistOfDef(ThingDef def)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllBuildingsNonColonistOfDef_003Ed__26(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__def = def
		};
	}

	public bool TryGetReinstallBlueprint(Thing building, out Blueprint_Install bp)
	{
		return reinstallationMap.TryGetValue(building, out bp);
	}

	public void Notify_FactionRemoved(Faction faction)
	{
		for (int i = 0; i < allBuildingsNonColonist.Count; i++)
		{
			if (allBuildingsNonColonist[i].Faction == faction)
			{
				allBuildingsNonColonist[i].SetFaction(null);
			}
		}
	}

	public void Dispose()
	{
		allBuildingsColonist.Clear();
		allBuildingsNonColonist.Clear();
		allBuildingsColonistCombatTargets.Clear();
		allBuildingsColonistElecFire.Clear();
		allBuildingsAnimalPenMarkers.Clear();
		allBuildingsHitchingPosts.Clear();
		reinstallationMap.Clear();
	}
}
