using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse;

public sealed class ListerThings
{
	[CompilerGenerated]
	private sealed class _003CGetAllThings_003Ed__19 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ListerThings _003C_003E4__this;

		private Predicate<Thing> validator;

		public Predicate<Thing> _003C_003E3__validator;

		private bool lookInHaulSources;

		public bool _003C_003E3__lookInHaulSources;

		private List<Thing>.Enumerator _003C_003E7__wrap1;

		private List<IHaulSource>.Enumerator _003C_003E7__wrap2;

		private IEnumerator<Thing> _003C_003E7__wrap3;

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
		public _003CGetAllThings_003Ed__19(int _003C_003E1__state)
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
			_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
			_003C_003E7__wrap2 = default(List<IHaulSource>.Enumerator);
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				ListerThings listerThings = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = listerThings.AllThings.GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0088;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0088;
				case 2:
					{
						_003C_003E1__state = -5;
						goto IL_013c;
					}
					IL_0088:
					while (_003C_003E7__wrap1.MoveNext())
					{
						Thing current = _003C_003E7__wrap1.Current;
						if (validator == null || validator(current))
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 1;
							return true;
						}
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = default(List<Thing>.Enumerator);
					if (!lookInHaulSources)
					{
						return false;
					}
					_003C_003E7__wrap2 = listerThings.haulSources.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0156;
					IL_0156:
					if (_003C_003E7__wrap2.MoveNext())
					{
						IHaulSource current2 = _003C_003E7__wrap2.Current;
						_003C_003E7__wrap3 = ((IEnumerable<Thing>)current2.GetDirectlyHeldThings()).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_013c;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = default(List<IHaulSource>.Enumerator);
					return false;
					IL_013c:
					while (_003C_003E7__wrap3.MoveNext())
					{
						Thing current3 = _003C_003E7__wrap3.Current;
						if (validator == null || validator(current3))
						{
							_003C_003E2__current = current3;
							_003C_003E1__state = 2;
							return true;
						}
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap3 = null;
					goto IL_0156;
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
			((IDisposable)_003C_003E7__wrap1/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap2/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -4;
			if (_003C_003E7__wrap3 != null)
			{
				_003C_003E7__wrap3.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Thing> IEnumerable<Thing>.GetEnumerator()
		{
			_003CGetAllThings_003Ed__19 _003CGetAllThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetAllThings_003Ed__ = this;
			}
			else
			{
				_003CGetAllThings_003Ed__ = new _003CGetAllThings_003Ed__19(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetAllThings_003Ed__.validator = _003C_003E3__validator;
			_003CGetAllThings_003Ed__.lookInHaulSources = _003C_003E3__lookInHaulSources;
			return _003CGetAllThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetThingsOfType_003Ed__18<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : Thing
	{
		private int _003C_003E1__state;

		private T _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public ListerThings _003C_003E4__this;

		private List<Thing> _003Cthings_003E5__2;

		private int _003Ci_003E5__3;

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
		public _003CGetThingsOfType_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Cthings_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			ListerThings listerThings = _003C_003E4__this;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0095;
			}
			_003C_003E1__state = -1;
			if (typeof(T) == typeof(Thing))
			{
				Log.Error("Do not call this method with type 'Thing' directly, as it will return all things currently registered.");
				return false;
			}
			_003Cthings_003E5__2 = listerThings.AllThings;
			_003Ci_003E5__3 = 0;
			goto IL_00a5;
			IL_00a5:
			if (_003Ci_003E5__3 < listerThings.AllThings.Count)
			{
				if (_003Cthings_003E5__2[_003Ci_003E5__3] is T val)
				{
					_003C_003E2__current = val;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0095;
			}
			return false;
			IL_0095:
			_003Ci_003E5__3++;
			goto IL_00a5;
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
			_003CGetThingsOfType_003Ed__18<T> result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetThingsOfType_003Ed__18<T>(0)
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

	private readonly Dictionary<ThingDef, List<Thing>> listsByDef = new Dictionary<ThingDef, List<Thing>>(ThingDefComparer.Instance);

	private readonly List<Thing>[] listsByGroup;

	private readonly List<IHaulSource> haulSources;

	private readonly int[] stateHashByGroup;

	public ListerThingsUse use;

	public ThingListChangedCallbacks thingListChangedCallbacks;

	private static readonly List<Thing> EmptyList = new List<Thing>();

	private static readonly List<Thing> tmpThingsMatchingFilter = new List<Thing>(1024);

	public List<Thing> AllThings => listsByGroup[2];

	public ListerThings(ListerThingsUse use, ThingListChangedCallbacks thingListChangedCallbacks = null)
	{
		this.use = use;
		this.thingListChangedCallbacks = thingListChangedCallbacks;
		listsByGroup = new List<Thing>[ThingListGroupHelper.AllGroups.Length];
		stateHashByGroup = new int[ThingListGroupHelper.AllGroups.Length];
		haulSources = new List<IHaulSource>();
		listsByGroup[2] = new List<Thing>();
	}

	public List<Thing> ThingsInGroup(ThingRequestGroup group)
	{
		return ThingsMatching(ThingRequest.ForGroup(group));
	}

	public int StateHashOfGroup(ThingRequestGroup group)
	{
		if (use == ListerThingsUse.Region && !group.StoreInRegion())
		{
			Log.ErrorOnce("Tried to get state hash of group " + group.ToString() + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968738832);
			return -1;
		}
		return Gen.HashCombineInt(85693994, stateHashByGroup[(uint)group]);
	}

	public List<Thing> ThingsOfDef(ThingDef def)
	{
		if (def == ThingDefOf.MinifiedThing)
		{
			Log.ErrorOnce("Tried to get ThingsOfDef of MinifiedThing, use ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MinifiedThing)) instead", 1927834012);
		}
		return ThingsMatching(ThingRequest.ForDef(def));
	}

	public bool AnyThingWithDef(ThingDef def)
	{
		if (listsByDef.ContainsKey(def))
		{
			return listsByDef[def].Count > 0;
		}
		return false;
	}

	public List<Thing> ThingsMatching(ThingRequest req)
	{
		if (req.singleDef != null)
		{
			return listsByDef.GetValueOrDefault(req.singleDef, EmptyList);
		}
		if (req.group != 0)
		{
			if (use == ListerThingsUse.Region && !req.group.StoreInRegion())
			{
				Log.ErrorOnce("Tried to get things in group " + req.group.ToString() + " in a region, but this group is never stored in regions. Most likely a global query should have been used.", 1968735132);
				return EmptyList;
			}
			return listsByGroup[(uint)req.group] ?? EmptyList;
		}
		ThingRequest thingRequest = req;
		throw new InvalidOperationException("Invalid ThingRequest " + thingRequest.ToString());
	}

	public List<Thing> ThingsMatchingFilter(ThingFilter filter)
	{
		tmpThingsMatchingFilter.Clear();
		foreach (ThingDef allowedThingDef in filter.AllowedThingDefs)
		{
			tmpThingsMatchingFilter.AddRange(ThingsOfDef(allowedThingDef));
		}
		return tmpThingsMatchingFilter;
	}

	public void GetThingsOfType<T>(List<T> list) where T : Thing
	{
		if (typeof(T) == typeof(Thing))
		{
			Log.Error("Do not call this method with type 'Thing' directly, as it will return all things currently registered.");
			return;
		}
		List<Thing> allThings = AllThings;
		for (int i = 0; i < AllThings.Count; i++)
		{
			if (allThings[i] is T item)
			{
				list.Add(item);
			}
		}
	}

	[IteratorStateMachine(typeof(_003CGetThingsOfType_003Ed__18<>))]
	public IEnumerable<T> GetThingsOfType<T>() where T : Thing
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetThingsOfType_003Ed__18<T>(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CGetAllThings_003Ed__19))]
	public IEnumerable<Thing> GetAllThings(Predicate<Thing> validator = null, bool lookInHaulSources = false)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetAllThings_003Ed__19(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__validator = validator,
			_003C_003E3__lookInHaulSources = lookInHaulSources
		};
	}

	public void GetAllThings(in List<Thing> list, Predicate<Thing> validator = null, bool lookInHaulSources = false)
	{
		if (validator != null)
		{
			foreach (Thing allThing in AllThings)
			{
				if (validator(allThing))
				{
					list.Add(allThing);
				}
			}
		}
		else
		{
			list.AddRange(AllThings);
		}
		if (!lookInHaulSources)
		{
			return;
		}
		foreach (IHaulSource haulSource in haulSources)
		{
			foreach (Thing item in (IEnumerable<Thing>)haulSource.GetDirectlyHeldThings())
			{
				if (validator == null || validator(item))
				{
					list.Add(item);
				}
			}
		}
	}

	public void GetAllThings(in List<Thing> list, ThingRequestGroup group, Predicate<Thing> validator = null, bool lookInHaulSources = false)
	{
		if (validator != null)
		{
			foreach (Thing item in ThingsInGroup(group))
			{
				if (validator(item))
				{
					list.Add(item);
				}
			}
		}
		else
		{
			list.AddRange(ThingsInGroup(group));
		}
		if (!lookInHaulSources)
		{
			return;
		}
		foreach (IHaulSource haulSource in haulSources)
		{
			foreach (Thing item2 in (IEnumerable<Thing>)haulSource.GetDirectlyHeldThings())
			{
				if (GroupIncludes(item2, group) && (validator == null || validator(item2)))
				{
					list.Add(item2);
				}
			}
		}
	}

	public bool Contains(Thing t)
	{
		return AllThings.Contains(t);
	}

	public void Add(Thing t)
	{
		if (!EverListable(t.def, use))
		{
			return;
		}
		if (!listsByDef.TryGetValue(t.def, out var value))
		{
			value = new List<Thing>();
			listsByDef.Add(t.def, value);
		}
		value.Add(t);
		if (t is IHaulSource item)
		{
			haulSources.Add(item);
		}
		ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
		foreach (ThingRequestGroup thingRequestGroup in allGroups)
		{
			if (GroupIncludes(t, thingRequestGroup))
			{
				List<Thing> list = listsByGroup[(uint)thingRequestGroup];
				if (list == null)
				{
					list = new List<Thing>();
					listsByGroup[(uint)thingRequestGroup] = list;
					stateHashByGroup[(uint)thingRequestGroup] = 0;
				}
				list.Add(t);
				stateHashByGroup[(uint)thingRequestGroup]++;
			}
		}
		thingListChangedCallbacks?.onThingAdded?.Invoke(t);
	}

	private bool GroupIncludes(Thing thing, ThingRequestGroup group)
	{
		if (use == ListerThingsUse.Region && !group.StoreInRegion())
		{
			return false;
		}
		return group.Includes(thing.def);
	}

	public void Remove(Thing t)
	{
		if (!EverListable(t.def, use))
		{
			return;
		}
		if (listsByDef.TryGetValue(t.def, out var value))
		{
			RemoveSwap(value, t);
		}
		if (t is IHaulSource item)
		{
			haulSources.Remove(item);
		}
		ThingRequestGroup[] allGroups = ThingListGroupHelper.AllGroups;
		for (int i = 0; i < allGroups.Length; i++)
		{
			ThingRequestGroup thingRequestGroup = allGroups[i];
			if ((use != ListerThingsUse.Region || thingRequestGroup.StoreInRegion()) && thingRequestGroup.Includes(t.def))
			{
				RemoveSwap(listsByGroup[i], t);
				stateHashByGroup[(uint)thingRequestGroup]++;
			}
		}
		thingListChangedCallbacks?.onThingRemoved?.Invoke(t);
	}

	private static void RemoveSwap(List<Thing> list, Thing t)
	{
		int num = list.IndexOf(t);
		if (num >= 0)
		{
			int index = list.Count - 1;
			list[num] = list[index];
			list.RemoveAt(index);
		}
	}

	public static bool EverListable(ThingDef def, ListerThingsUse use)
	{
		if (def.category == ThingCategory.Mote && (!def.drawGUIOverlay || use == ListerThingsUse.Region))
		{
			return false;
		}
		if (def.category == ThingCategory.Projectile && use == ListerThingsUse.Region)
		{
			return false;
		}
		return true;
	}

	public void Clear()
	{
		listsByDef.Clear();
		for (int i = 0; i < listsByGroup.Length; i++)
		{
			if (listsByGroup[i] != null)
			{
				listsByGroup[i].Clear();
			}
			stateHashByGroup[i] = 0;
		}
	}
}
