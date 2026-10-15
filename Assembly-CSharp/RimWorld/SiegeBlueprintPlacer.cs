using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public static class SiegeBlueprintPlacer
{
	[CompilerGenerated]
	private sealed class _003CMakeCoverLine_003Ed__13 : IEnumerable<Blueprint_Build>, IEnumerable, IEnumerator<Blueprint_Build>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Blueprint_Build _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 root;

		public IntVec3 _003C_003E3__root;

		private ThingDef coverThing;

		public ThingDef _003C_003E3__coverThing;

		private Map map;

		public Map _003C_003E3__map;

		private ThingDef coverStuff;

		public ThingDef _003C_003E3__coverStuff;

		private Rot4 growDir;

		public Rot4 _003C_003E3__growDir;

		private int maxLength;

		public int _003C_003E3__maxLength;

		private IntVec3 _003Ccur_003E5__2;

		private int _003Cj_003E5__3;

		Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
		public _003CMakeCoverLine_003Ed__13(int _003C_003E1__state)
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
				_003Ccur_003E5__2 = root;
				_003Cj_003E5__3 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				placedCoverLocs.Add(_003Ccur_003E5__2);
				_003Ccur_003E5__2 += growDir.FacingCell;
				_003Cj_003E5__3++;
				break;
			}
			if (_003Cj_003E5__3 < maxLength && CanPlaceBlueprintAt(_003Ccur_003E5__2, Rot4.North, coverThing, map, coverStuff))
			{
				ClearHostilePlansAt(_003Ccur_003E5__2, Rot4.North, coverThing, coverStuff, map);
				_003C_003E2__current = GenConstruct.PlaceBlueprintForBuild(coverThing, _003Ccur_003E5__2, map, Rot4.North, faction, coverStuff);
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
		{
			_003CMakeCoverLine_003Ed__13 _003CMakeCoverLine_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CMakeCoverLine_003Ed__ = this;
			}
			else
			{
				_003CMakeCoverLine_003Ed__ = new _003CMakeCoverLine_003Ed__13(0);
			}
			_003CMakeCoverLine_003Ed__.root = _003C_003E3__root;
			_003CMakeCoverLine_003Ed__.map = _003C_003E3__map;
			_003CMakeCoverLine_003Ed__.growDir = _003C_003E3__growDir;
			_003CMakeCoverLine_003Ed__.maxLength = _003C_003E3__maxLength;
			_003CMakeCoverLine_003Ed__.coverThing = _003C_003E3__coverThing;
			_003CMakeCoverLine_003Ed__.coverStuff = _003C_003E3__coverStuff;
			return _003CMakeCoverLine_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Blueprint_Build>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPlaceArtilleryBlueprints_003Ed__14 : IEnumerable<Blueprint_Build>, IEnumerable, IEnumerator<Blueprint_Build>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Blueprint_Build _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private float points;

		public float _003C_003E3__points;

		private Map map;

		public Map _003C_003E3__map;

		private IEnumerable<ThingDef> _003CartyDefs_003E5__2;

		private int _003CnumArtillery_003E5__3;

		private int _003Ci_003E5__4;

		Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
		public _003CPlaceArtilleryBlueprints_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CartyDefs_003E5__2 = null;
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
				_003CartyDefs_003E5__2 = DefDatabase<ThingDef>.AllDefs.Where((ThingDef def) => def.building != null && def.building.buildingTags.Contains("Artillery_BaseDestroyer"));
				_003CnumArtillery_003E5__3 = Mathf.RoundToInt(points / 60f);
				_003CnumArtillery_003E5__3 = Mathf.Clamp(_003CnumArtillery_003E5__3, 1, 2);
				_003Ci_003E5__4 = 0;
				break;
			case 1:
				_003C_003E1__state = -1;
				points -= 60f;
				_003Ci_003E5__4++;
				break;
			}
			if (_003Ci_003E5__4 < _003CnumArtillery_003E5__3)
			{
				Rot4 random = Rot4.Random;
				ThingDef thingDef = _003CartyDefs_003E5__2.RandomElement();
				IntVec3 intVec = FindArtySpot(thingDef, random, map);
				if (!intVec.IsValid)
				{
					return false;
				}
				ClearHostilePlansAt(intVec, random, thingDef, ThingDefOf.Steel, map);
				_003C_003E2__current = GenConstruct.PlaceBlueprintForBuild(thingDef, intVec, map, random, faction, ThingDefOf.Steel);
				_003C_003E1__state = 1;
				return true;
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
		IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
		{
			_003CPlaceArtilleryBlueprints_003Ed__14 _003CPlaceArtilleryBlueprints_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPlaceArtilleryBlueprints_003Ed__ = this;
			}
			else
			{
				_003CPlaceArtilleryBlueprints_003Ed__ = new _003CPlaceArtilleryBlueprints_003Ed__14(0);
			}
			_003CPlaceArtilleryBlueprints_003Ed__.points = _003C_003E3__points;
			_003CPlaceArtilleryBlueprints_003Ed__.map = _003C_003E3__map;
			return _003CPlaceArtilleryBlueprints_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Blueprint_Build>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPlaceBlueprints_003Ed__8 : IEnumerable<Blueprint_Build>, IEnumerable, IEnumerator<Blueprint_Build>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Blueprint_Build _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private IntVec3 placeCenter;

		public IntVec3 _003C_003E3__placeCenter;

		private Faction placeFaction;

		public Faction _003C_003E3__placeFaction;

		private Map map;

		public Map _003C_003E3__map;

		private float points;

		public float _003C_003E3__points;

		private IEnumerator<Blueprint_Build> _003C_003E7__wrap1;

		Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
		public _003CPlaceBlueprints_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
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
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
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
					center = placeCenter;
					faction = placeFaction;
					_003C_003E7__wrap1 = PlaceCoverBlueprints(map).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0086;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0086;
				case 2:
					{
						_003C_003E1__state = -4;
						break;
					}
					IL_0086:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Blueprint_Build current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = PlaceArtilleryBlueprints(points, map).GetEnumerator();
					_003C_003E1__state = -4;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Blueprint_Build current2 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current2;
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003Em__Finally2();
				_003C_003E7__wrap1 = null;
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
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
		{
			_003CPlaceBlueprints_003Ed__8 _003CPlaceBlueprints_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPlaceBlueprints_003Ed__ = this;
			}
			else
			{
				_003CPlaceBlueprints_003Ed__ = new _003CPlaceBlueprints_003Ed__8(0);
			}
			_003CPlaceBlueprints_003Ed__.placeCenter = _003C_003E3__placeCenter;
			_003CPlaceBlueprints_003Ed__.map = _003C_003E3__map;
			_003CPlaceBlueprints_003Ed__.placeFaction = _003C_003E3__placeFaction;
			_003CPlaceBlueprints_003Ed__.points = _003C_003E3__points;
			return _003CPlaceBlueprints_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Blueprint_Build>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CPlaceCoverBlueprints_003Ed__12 : IEnumerable<Blueprint_Build>, IEnumerable, IEnumerator<Blueprint_Build>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Blueprint_Build _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Map map;

		public Map _003C_003E3__map;

		private ThingDef _003CcoverThing_003E5__2;

		private ThingDef _003CcoverStuff_003E5__3;

		private int _003CnumCover_003E5__4;

		private int _003Ci_003E5__5;

		private IntVec3 _003CbagRoot_003E5__6;

		private Rot4 _003CgrowDirB_003E5__7;

		private IEnumerator<Blueprint_Build> _003C_003E7__wrap7;

		Blueprint_Build IEnumerator<Blueprint_Build>.Current
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
		public _003CPlaceCoverBlueprints_003Ed__12(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
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
			case -4:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003CcoverThing_003E5__2 = null;
			_003CcoverStuff_003E5__3 = null;
			_003C_003E7__wrap7 = null;
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
					placedCoverLocs.Clear();
					if (Rand.Chance(0.5f))
					{
						_003CcoverThing_003E5__2 = ThingDefOf.Sandbags;
						_003CcoverStuff_003E5__3 = ThingDefOf.Cloth;
					}
					else
					{
						_003CcoverThing_003E5__2 = ThingDefOf.Barricade;
						_003CcoverStuff_003E5__3 = (Rand.Chance(0.5f) ? ThingDefOf.Steel : ThingDefOf.WoodLog);
					}
					_003CnumCover_003E5__4 = NumCoverRange.RandomInRange;
					_003Ci_003E5__5 = 0;
					goto IL_025c;
				case 1:
					_003C_003E1__state = -3;
					goto IL_018d;
				case 2:
					{
						_003C_003E1__state = -4;
						goto IL_0230;
					}
					IL_025c:
					if (_003Ci_003E5__5 < _003CnumCover_003E5__4)
					{
						_003CbagRoot_003E5__6 = FindCoverRoot(map, _003CcoverThing_003E5__2, _003CcoverStuff_003E5__3);
						if (!_003CbagRoot_003E5__6.IsValid)
						{
							return false;
						}
						Rot4 growDir = ((_003CbagRoot_003E5__6.x <= center.x) ? Rot4.East : Rot4.West);
						if (_003CbagRoot_003E5__6.z > center.z)
						{
							_003CgrowDirB_003E5__7 = Rot4.South;
						}
						else
						{
							_003CgrowDirB_003E5__7 = Rot4.North;
						}
						_003C_003E7__wrap7 = MakeCoverLine(_003CbagRoot_003E5__6, map, growDir, CoverLengthRange.RandomInRange, _003CcoverThing_003E5__2, _003CcoverStuff_003E5__3).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_018d;
					}
					return false;
					IL_0230:
					if (_003C_003E7__wrap7.MoveNext())
					{
						Blueprint_Build current = _003C_003E7__wrap7.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap7 = null;
					_003Ci_003E5__5++;
					goto IL_025c;
					IL_018d:
					if (_003C_003E7__wrap7.MoveNext())
					{
						Blueprint_Build current2 = _003C_003E7__wrap7.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap7 = null;
					_003CbagRoot_003E5__6 += _003CgrowDirB_003E5__7.FacingCell;
					_003C_003E7__wrap7 = MakeCoverLine(_003CbagRoot_003E5__6, map, _003CgrowDirB_003E5__7, CoverLengthRange.RandomInRange, _003CcoverThing_003E5__2, _003CcoverStuff_003E5__3).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0230;
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
			if (_003C_003E7__wrap7 != null)
			{
				_003C_003E7__wrap7.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap7 != null)
			{
				_003C_003E7__wrap7.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Blueprint_Build> IEnumerable<Blueprint_Build>.GetEnumerator()
		{
			_003CPlaceCoverBlueprints_003Ed__12 _003CPlaceCoverBlueprints_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPlaceCoverBlueprints_003Ed__ = this;
			}
			else
			{
				_003CPlaceCoverBlueprints_003Ed__ = new _003CPlaceCoverBlueprints_003Ed__12(0);
			}
			_003CPlaceCoverBlueprints_003Ed__.map = _003C_003E3__map;
			return _003CPlaceCoverBlueprints_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Blueprint_Build>)this).GetEnumerator();
		}
	}

	private static IntVec3 center;

	private static Faction faction;

	private static List<IntVec3> placedCoverLocs = new List<IntVec3>();

	private const int MaxArtyCount = 2;

	public const float ArtyCost = 60f;

	private const int MinCoverDistSquared = 36;

	private static readonly IntRange NumCoverRange = new IntRange(2, 4);

	private static readonly IntRange CoverLengthRange = new IntRange(2, 7);

	[IteratorStateMachine(typeof(_003CPlaceBlueprints_003Ed__8))]
	public static IEnumerable<Blueprint_Build> PlaceBlueprints(IntVec3 placeCenter, Map map, Faction placeFaction, float points)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlaceBlueprints_003Ed__8(-2)
		{
			_003C_003E3__placeCenter = placeCenter,
			_003C_003E3__map = map,
			_003C_003E3__placeFaction = placeFaction,
			_003C_003E3__points = points
		};
	}

	private static bool CanPlaceBlueprintAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, Map map, ThingDef stuffDef)
	{
		return GenConstruct.CanPlaceBlueprintAt_NewTemp(buildingDef, root, rot, map, godMode: false, null, null, stuffDef, ignoreEdgeArea: true, ignoreInteractionSpots: true, ignoreClearableFreeBuildings: true, (Thing t) => IsHostilePlanToIgnore(t, faction)).Accepted;
	}

	private static bool IsHostilePlanToIgnore(Thing t, Faction placingFaction)
	{
		if (placingFaction == null || t == null)
		{
			return false;
		}
		if (!(t is Blueprint) && !(t is Frame))
		{
			return false;
		}
		if (t.Faction != null && t.Faction != placingFaction)
		{
			return t.Faction.HostileTo(placingFaction);
		}
		return false;
	}

	private static void ClearHostilePlansAt(IntVec3 root, Rot4 rot, ThingDef buildingDef, ThingDef stuffDef, Map map)
	{
		foreach (IntVec3 item in GenAdj.OccupiedRect(root, rot, buildingDef.Size))
		{
			List<Thing> thingList = item.GetThingList(map);
			for (int num = thingList.Count - 1; num >= 0; num--)
			{
				Thing thing = thingList[num];
				if (IsHostilePlanToIgnore(thing, faction) && !GenConstruct.CanPlaceBlueprintOver(buildingDef, thing.def, stuffDef, thing.Stuff))
				{
					thing.Destroy(DestroyMode.Cancel);
				}
			}
		}
	}

	[IteratorStateMachine(typeof(_003CPlaceCoverBlueprints_003Ed__12))]
	private static IEnumerable<Blueprint_Build> PlaceCoverBlueprints(Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlaceCoverBlueprints_003Ed__12(-2)
		{
			_003C_003E3__map = map
		};
	}

	[IteratorStateMachine(typeof(_003CMakeCoverLine_003Ed__13))]
	private static IEnumerable<Blueprint_Build> MakeCoverLine(IntVec3 root, Map map, Rot4 growDir, int maxLength, ThingDef coverThing, ThingDef coverStuff)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeCoverLine_003Ed__13(-2)
		{
			_003C_003E3__root = root,
			_003C_003E3__map = map,
			_003C_003E3__growDir = growDir,
			_003C_003E3__maxLength = maxLength,
			_003C_003E3__coverThing = coverThing,
			_003C_003E3__coverStuff = coverStuff
		};
	}

	[IteratorStateMachine(typeof(_003CPlaceArtilleryBlueprints_003Ed__14))]
	private static IEnumerable<Blueprint_Build> PlaceArtilleryBlueprints(float points, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlaceArtilleryBlueprints_003Ed__14(-2)
		{
			_003C_003E3__points = points,
			_003C_003E3__map = map
		};
	}

	private static IntVec3 FindCoverRoot(Map map, ThingDef coverThing, ThingDef coverStuff)
	{
		CellRect cellRect = CellRect.CenteredOn(center, 13);
		cellRect.ClipInsideMap(map);
		CellRect cellRect2 = CellRect.CenteredOn(center, 8);
		cellRect2.ClipInsideMap(map);
		int num = 0;
		IntVec3 randomCell;
		while (true)
		{
			num++;
			if (num > 200)
			{
				return IntVec3.Invalid;
			}
			randomCell = cellRect.RandomCell;
			if (cellRect2.Contains(randomCell) || !map.reachability.CanReach(randomCell, center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) || !CanPlaceBlueprintAt(randomCell, Rot4.North, coverThing, map, coverStuff))
			{
				continue;
			}
			bool flag = false;
			for (int i = 0; i < placedCoverLocs.Count; i++)
			{
				if ((float)(placedCoverLocs[i] - randomCell).LengthHorizontalSquared < 36f)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				break;
			}
		}
		return randomCell;
	}

	private static IntVec3 FindArtySpot(ThingDef artyDef, Rot4 rot, Map map)
	{
		CellRect cellRect = CellRect.CenteredOn(center, 8);
		cellRect.ClipInsideMap(map);
		int num = 0;
		IntVec3 randomCell;
		do
		{
			num++;
			if (num > 200)
			{
				return IntVec3.Invalid;
			}
			randomCell = cellRect.RandomCell;
		}
		while (!map.reachability.CanReach(randomCell, center, PathEndMode.OnCell, TraverseMode.NoPassClosedDoors, Danger.Deadly) || randomCell.Roofed(map) || !CanPlaceBlueprintAt(randomCell, rot, artyDef, map, ThingDefOf.Steel));
		return randomCell;
	}
}
