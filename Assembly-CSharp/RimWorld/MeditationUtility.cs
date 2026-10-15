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

public class MeditationUtility
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public Pawn pawn;

		public Func<Building, bool> _003C_003E9__2;

		internal bool _003CAllMeditationSpotCandidates_003Eb__2(Building s)
		{
			return IsValidMeditationBuildingForPawn(s, pawn);
		}

		internal bool _003CAllMeditationSpotCandidates_003Eb__1(Pawn p, IntVec3 c, IntVec3 r)
		{
			if (!c.Standable(p.Map) || c.GetDoor(p.Map) != null || !p.CanReserveAndReach(c, PathEndMode.OnCell, p.NormalMaxDanger()))
			{
				return false;
			}
			Room room = c.GetRoom(p.Map);
			if (room != null && !CanUseRoomToMeditate(room, pawn))
			{
				return false;
			}
			return true;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_1
	{
		public Room room;

		internal bool _003CAllMeditationSpotCandidates_003Eb__3(Pawn p, IntVec3 c, IntVec3 r)
		{
			if (c.GetRoom(p.Map) == room && c.Standable(p.Map) && c.GetDoor(p.Map) == null)
			{
				return WanderRoomUtility.IsValidWanderDest(p, c, r);
			}
			return false;
		}
	}

	[CompilerGenerated]
	private sealed class _003CAllMeditationSpotCandidates_003Ed__6 : IEnumerable<LocalTargetInfo>, IEnumerable, IEnumerator<LocalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LocalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private bool allowFallbackSpots;

		public bool _003C_003E3__allowFallbackSpots;

		private _003C_003Ec__DisplayClass6_1 _003C_003E8__2;

		private Building_Bed _003Cbed_003E5__2;

		private IEnumerator<Building> _003C_003E7__wrap2;

		private List<Thing>.Enumerator _003C_003E7__wrap3;

		private IEnumerator<LocalTargetInfo> _003C_003E7__wrap4;

		private IEnumerator<Room> _003C_003E7__wrap5;

		LocalTargetInfo IEnumerator<LocalTargetInfo>.Current
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
		public _003CAllMeditationSpotCandidates_003Ed__6(int _003C_003E1__state)
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
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 3:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			case -7:
			case -6:
			case 6:
			case 7:
				try
				{
					if (num == -7 || num == 6)
					{
						try
						{
						}
						finally
						{
							_003C_003Em__Finally5();
						}
					}
				}
				finally
				{
					_003C_003Em__Finally4();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003C_003E8__2 = null;
			_003Cbed_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E7__wrap3 = default(List<Thing>.Enumerator);
			_003C_003E7__wrap4 = null;
			_003C_003E7__wrap5 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				bool flag;
				IntVec3 colonyWanderRoot;
				List<Thing> list;
				IntVec3 randomCell;
				Room room3;
				IntVec3 intVec;
				switch (_003C_003E1__state)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
					_003C_003E8__1.pawn = pawn;
					flag = false;
					if (_003C_003E8__1.pawn.royalty != null && _003C_003E8__1.pawn.royalty.AllTitlesInEffectForReading.Count > 0 && !_003C_003E8__1.pawn.IsPrisonerOfColony)
					{
						Building_Throne building_Throne = RoyalTitleUtility.FindBestUsableThrone(_003C_003E8__1.pawn);
						if (building_Throne != null)
						{
							_003C_003E2__current = building_Throne;
							_003C_003E1__state = 1;
							return true;
						}
					}
					goto IL_00d8;
				case 1:
					_003C_003E1__state = -1;
					flag = true;
					goto IL_00d8;
				case 2:
					_003C_003E1__state = -3;
					flag = true;
					goto IL_0188;
				case 3:
					_003C_003E1__state = -4;
					goto IL_0285;
				case 4:
					_003C_003E1__state = -5;
					goto IL_0372;
				case 5:
					_003C_003E1__state = -1;
					goto IL_0407;
				case 6:
					_003C_003E1__state = -7;
					goto IL_04d3;
				case 7:
					_003C_003E1__state = -6;
					goto IL_0578;
				case 8:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_057f:
					if (_003C_003E7__wrap5.MoveNext())
					{
						_003C_003E8__2 = new _003C_003Ec__DisplayClass6_1();
						_003C_003E8__2.room = _003C_003E7__wrap5.Current;
						_003C_003E7__wrap4 = FocusSpotsInTheRoom(_003C_003E8__1.pawn, _003C_003E8__2.room).GetEnumerator();
						_003C_003E1__state = -7;
						goto IL_04d3;
					}
					_003C_003Em__Finally4();
					_003C_003E7__wrap5 = null;
					if (_003C_003E8__1.pawn.IsPrisonerOfColony)
					{
						return false;
					}
					colonyWanderRoot = WanderUtility.GetColonyWanderRoot(_003C_003E8__1.pawn);
					intVec = RCellFinder.RandomWanderDestFor(_003C_003E8__1.pawn, colonyWanderRoot, WanderRadius, delegate(Pawn p, IntVec3 c, IntVec3 r)
					{
						if (!c.Standable(p.Map) || c.GetDoor(p.Map) != null || !p.CanReserveAndReach(c, PathEndMode.OnCell, p.NormalMaxDanger()))
						{
							return false;
						}
						Room room = c.GetRoom(p.Map);
						return (room == null || CanUseRoomToMeditate(room, _003C_003E8__1.pawn)) ? true : false;
					}, _003C_003E8__1.pawn.NormalMaxDanger());
					if (intVec.IsValid)
					{
						_003C_003E2__current = intVec;
						_003C_003E1__state = 8;
						return true;
					}
					break;
					IL_01a2:
					if (flag || !allowFallbackSpots)
					{
						return false;
					}
					list = _003C_003E8__1.pawn.Map.listerThings.ThingsInGroup(ThingRequestGroup.MeditationFocus);
					_003C_003E7__wrap3 = list.GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0285;
					IL_04d3:
					while (_003C_003E7__wrap4.MoveNext())
					{
						LocalTargetInfo current = _003C_003E7__wrap4.Current;
						if (_003C_003E8__1.pawn.CanReach(current, PathEndMode.Touch, _003C_003E8__1.pawn.NormalMaxDanger()))
						{
							_003C_003E2__current = current;
							_003C_003E1__state = 6;
							return true;
						}
					}
					_003C_003Em__Finally5();
					_003C_003E7__wrap4 = null;
					randomCell = _003C_003E8__2.room.Districts.RandomElement().Regions.RandomElement().RandomCell;
					intVec = RCellFinder.RandomWanderDestFor(_003C_003E8__1.pawn, randomCell, WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.GetRoom(p.Map) == _003C_003E8__2.room && c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r), _003C_003E8__1.pawn.NormalMaxDanger());
					if (intVec.IsValid)
					{
						_003C_003E2__current = intVec;
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0578;
					IL_0407:
					_003C_003E7__wrap5 = UsableWorshipRooms(_003C_003E8__1.pawn).GetEnumerator();
					_003C_003E1__state = -6;
					goto IL_057f;
					IL_00d8:
					if (!_003C_003E8__1.pawn.IsPrisonerOfColony)
					{
						_003C_003E7__wrap2 = (from s in _003C_003E8__1.pawn.Map.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.MeditationSpot)
							where IsValidMeditationBuildingForPawn(s, _003C_003E8__1.pawn)
							select s).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0188;
					}
					goto IL_01a2;
					IL_0285:
					while (_003C_003E7__wrap3.MoveNext())
					{
						Thing current2 = _003C_003E7__wrap3.Current;
						if (current2.def == ThingDefOf.Wall)
						{
							continue;
						}
						Room room2 = current2.GetRoom();
						if ((room2 == null || CanUseRoomToMeditate(room2, _003C_003E8__1.pawn)) && current2.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, _003C_003E8__1.pawn) > 0f)
						{
							LocalTargetInfo localTargetInfo = MeditationSpotForFocus(current2, _003C_003E8__1.pawn);
							if (localTargetInfo.IsValid)
							{
								_003C_003E2__current = localTargetInfo;
								_003C_003E1__state = 3;
								return true;
							}
						}
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap3 = default(List<Thing>.Enumerator);
					_003Cbed_003E5__2 = _003C_003E8__1.pawn.ownership.OwnedBed;
					room3 = _003Cbed_003E5__2?.GetRoom();
					if (room3 != null && !room3.PsychologicallyOutdoors && _003C_003E8__1.pawn.CanReserveAndReach(_003Cbed_003E5__2, PathEndMode.OnCell, _003C_003E8__1.pawn.NormalMaxDanger()))
					{
						_003C_003E7__wrap4 = FocusSpotsInTheRoom(_003C_003E8__1.pawn, room3).GetEnumerator();
						_003C_003E1__state = -5;
						goto IL_0372;
					}
					goto IL_0407;
					IL_0372:
					if (_003C_003E7__wrap4.MoveNext())
					{
						LocalTargetInfo current3 = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current3;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally3();
					_003C_003E7__wrap4 = null;
					intVec = RCellFinder.RandomWanderDestFor(_003C_003E8__1.pawn, _003Cbed_003E5__2.Position, WanderRadius, (Pawn p, IntVec3 c, IntVec3 r) => c.Standable(p.Map) && c.GetDoor(p.Map) == null && WanderRoomUtility.IsValidWanderDest(p, c, r) && CanUseRoomToMeditate(c.GetRoom(p.Map), p), _003C_003E8__1.pawn.NormalMaxDanger());
					if (intVec.IsValid)
					{
						_003C_003E2__current = intVec;
						_003C_003E1__state = 5;
						return true;
					}
					goto IL_0407;
					IL_0188:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Building current4 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current4;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					goto IL_01a2;
					IL_0578:
					_003C_003E8__2 = null;
					goto IL_057f;
				}
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			((IDisposable)_003C_003E7__wrap3/*cast due to .constrained prefix*/).Dispose();
		}

		private void _003C_003Em__Finally3()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap4 != null)
			{
				_003C_003E7__wrap4.Dispose();
			}
		}

		private void _003C_003Em__Finally4()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap5 != null)
			{
				_003C_003E7__wrap5.Dispose();
			}
		}

		private void _003C_003Em__Finally5()
		{
			_003C_003E1__state = -6;
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
		IEnumerator<LocalTargetInfo> IEnumerable<LocalTargetInfo>.GetEnumerator()
		{
			_003CAllMeditationSpotCandidates_003Ed__6 _003CAllMeditationSpotCandidates_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CAllMeditationSpotCandidates_003Ed__ = this;
			}
			else
			{
				_003CAllMeditationSpotCandidates_003Ed__ = new _003CAllMeditationSpotCandidates_003Ed__6(0);
			}
			_003CAllMeditationSpotCandidates_003Ed__.pawn = _003C_003E3__pawn;
			_003CAllMeditationSpotCandidates_003Ed__.allowFallbackSpots = _003C_003E3__allowFallbackSpots;
			return _003CAllMeditationSpotCandidates_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LocalTargetInfo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CFocusObjectsForPawnHyperlinks_003Ed__18 : IEnumerable<Dialog_InfoCard.Hyperlink>, IEnumerable, IEnumerator<Dialog_InfoCard.Hyperlink>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Dialog_InfoCard.Hyperlink _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private int _003Ci_003E5__2;

		private List<Dialog_InfoCard.Hyperlink> _003Clist_003E5__3;

		private int _003Cj_003E5__4;

		Dialog_InfoCard.Hyperlink IEnumerator<Dialog_InfoCard.Hyperlink>.Current
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
		public _003CFocusObjectsForPawnHyperlinks_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003Clist_003E5__3 = null;
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
				_003Cj_003E5__4++;
				goto IL_0113;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_013f;
			IL_013f:
			if (_003Ci_003E5__2 < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[_003Ci_003E5__2];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					if (!focusObjectHyperlinksPerTypeCache.ContainsKey(meditationFocusDef))
					{
						List<Dialog_InfoCard.Hyperlink> list = new List<Dialog_InfoCard.Hyperlink>();
						foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
						{
							CompProperties_MeditationFocus compProperties = item.GetCompProperties<CompProperties_MeditationFocus>();
							if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
							{
								list.Add(new Dialog_InfoCard.Hyperlink(item));
							}
						}
						focusObjectHyperlinksPerTypeCache[meditationFocusDef] = list;
					}
					_003Clist_003E5__3 = focusObjectHyperlinksPerTypeCache[meditationFocusDef];
					_003Cj_003E5__4 = 0;
					goto IL_0113;
				}
				goto IL_012d;
			}
			return false;
			IL_0113:
			if (_003Cj_003E5__4 < _003Clist_003E5__3.Count)
			{
				_003C_003E2__current = _003Clist_003E5__3[_003Cj_003E5__4];
				_003C_003E1__state = 1;
				return true;
			}
			_003Clist_003E5__3 = null;
			goto IL_012d;
			IL_012d:
			_003Ci_003E5__2++;
			goto IL_013f;
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
		IEnumerator<Dialog_InfoCard.Hyperlink> IEnumerable<Dialog_InfoCard.Hyperlink>.GetEnumerator()
		{
			_003CFocusObjectsForPawnHyperlinks_003Ed__18 _003CFocusObjectsForPawnHyperlinks_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFocusObjectsForPawnHyperlinks_003Ed__ = this;
			}
			else
			{
				_003CFocusObjectsForPawnHyperlinks_003Ed__ = new _003CFocusObjectsForPawnHyperlinks_003Ed__18(0);
			}
			_003CFocusObjectsForPawnHyperlinks_003Ed__.pawn = _003C_003E3__pawn;
			return _003CFocusObjectsForPawnHyperlinks_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Dialog_InfoCard.Hyperlink>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CFocusSpotsInTheRoom_003Ed__13 : IEnumerable<LocalTargetInfo>, IEnumerable, IEnumerator<LocalTargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private LocalTargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Room r;

		public Room _003C_003E3__r;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private List<Thing> _003Cthings_003E5__2;

		private int _003Ci_003E5__3;

		LocalTargetInfo IEnumerator<LocalTargetInfo>.Current
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
		public _003CFocusSpotsInTheRoom_003Ed__13(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_00f4;
			}
			_003C_003E1__state = -1;
			_003Cthings_003E5__2 = r.ContainedAndAdjacentThings;
			_003Ci_003E5__3 = 0;
			goto IL_0104;
			IL_00f4:
			_003Ci_003E5__3++;
			goto IL_0104;
			IL_0104:
			if (_003Ci_003E5__3 < _003Cthings_003E5__2.Count)
			{
				CompMeditationFocus compMeditationFocus = _003Cthings_003E5__2[_003Ci_003E5__3].TryGetComp<CompMeditationFocus>();
				if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn) && !(_003Cthings_003E5__2[_003Ci_003E5__3] is Building_Throne) && _003Cthings_003E5__2[_003Ci_003E5__3].GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn) > 0f)
				{
					LocalTargetInfo localTargetInfo = MeditationSpotForFocus(_003Cthings_003E5__2[_003Ci_003E5__3], pawn, r.ContainsCell);
					if (localTargetInfo.IsValid)
					{
						_003C_003E2__current = localTargetInfo;
						_003C_003E1__state = 1;
						return true;
					}
				}
				goto IL_00f4;
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
		IEnumerator<LocalTargetInfo> IEnumerable<LocalTargetInfo>.GetEnumerator()
		{
			_003CFocusSpotsInTheRoom_003Ed__13 _003CFocusSpotsInTheRoom_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFocusSpotsInTheRoom_003Ed__ = this;
			}
			else
			{
				_003CFocusSpotsInTheRoom_003Ed__ = new _003CFocusSpotsInTheRoom_003Ed__13(0);
			}
			_003CFocusSpotsInTheRoom_003Ed__.pawn = _003C_003E3__pawn;
			_003CFocusSpotsInTheRoom_003Ed__.r = _003C_003E3__r;
			return _003CFocusSpotsInTheRoom_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<LocalTargetInfo>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CFocusTypesAvailableForPawn_003Ed__15 : IEnumerable<MeditationFocusDef>, IEnumerable, IEnumerator<MeditationFocusDef>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private MeditationFocusDef _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private int _003Ci_003E5__2;

		MeditationFocusDef IEnumerator<MeditationFocusDef>.Current
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
		public _003CFocusTypesAvailableForPawn_003Ed__15(int _003C_003E1__state)
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
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
				goto IL_0056;
			}
			_003C_003E1__state = -1;
			_003Ci_003E5__2 = 0;
			goto IL_0066;
			IL_0056:
			_003Ci_003E5__2++;
			goto IL_0066;
			IL_0066:
			if (_003Ci_003E5__2 < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count)
			{
				MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[_003Ci_003E5__2];
				if (meditationFocusDef.CanPawnUse(pawn))
				{
					_003C_003E2__current = meditationFocusDef;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0056;
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
		IEnumerator<MeditationFocusDef> IEnumerable<MeditationFocusDef>.GetEnumerator()
		{
			_003CFocusTypesAvailableForPawn_003Ed__15 _003CFocusTypesAvailableForPawn_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CFocusTypesAvailableForPawn_003Ed__ = this;
			}
			else
			{
				_003CFocusTypesAvailableForPawn_003Ed__ = new _003CFocusTypesAvailableForPawn_003Ed__15(0);
			}
			_003CFocusTypesAvailableForPawn_003Ed__.pawn = _003C_003E3__pawn;
			return _003CFocusTypesAvailableForPawn_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<MeditationFocusDef>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetMeditationFociAffectedByBuilding_003Ed__23 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
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
		public _003CGetMeditationFociAffectedByBuilding_003Ed__23(int _003C_003E1__state)
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
					if (!CountsAsArtificialBuilding(def, faction))
					{
						return false;
					}
					_003C_003E7__wrap1 = map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.MeditationFocus)).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					CompMeditationFocus compMeditationFocus = current.TryGetComp<CompMeditationFocus>();
					if (compMeditationFocus != null && compMeditationFocus.WillBeAffectedBy(def, faction, pos, rotation))
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
			_003CGetMeditationFociAffectedByBuilding_003Ed__23 _003CGetMeditationFociAffectedByBuilding_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetMeditationFociAffectedByBuilding_003Ed__ = this;
			}
			else
			{
				_003CGetMeditationFociAffectedByBuilding_003Ed__ = new _003CGetMeditationFociAffectedByBuilding_003Ed__23(0);
			}
			_003CGetMeditationFociAffectedByBuilding_003Ed__.map = _003C_003E3__map;
			_003CGetMeditationFociAffectedByBuilding_003Ed__.def = _003C_003E3__def;
			_003CGetMeditationFociAffectedByBuilding_003Ed__.faction = _003C_003E3__faction;
			_003CGetMeditationFociAffectedByBuilding_003Ed__.pos = _003C_003E3__pos;
			_003CGetMeditationFociAffectedByBuilding_003Ed__.rotation = _003C_003E3__rotation;
			return _003CGetMeditationFociAffectedByBuilding_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CUsableWorshipRooms_003Ed__7 : IEnumerable<Room>, IEnumerable, IEnumerator<Room>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Room _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Pawn pawn;

		public Pawn _003C_003E3__pawn;

		private IEnumerator<Room> _003C_003E7__wrap1;

		Room IEnumerator<Room>.Current
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
		public _003CUsableWorshipRooms_003Ed__7(int _003C_003E1__state)
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
					_003C_003E7__wrap1 = pawn.Map.regionGrid.AllRooms.GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Room current = _003C_003E7__wrap1.Current;
					if (current.Role == RoomRoleDefOf.WorshipRoom && CanUseRoomToMeditate(current, pawn))
					{
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
				}
				_003C_003Em__Finally1();
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Room> IEnumerable<Room>.GetEnumerator()
		{
			_003CUsableWorshipRooms_003Ed__7 _003CUsableWorshipRooms_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CUsableWorshipRooms_003Ed__ = this;
			}
			else
			{
				_003CUsableWorshipRooms_003Ed__ = new _003CUsableWorshipRooms_003Ed__7(0);
			}
			_003CUsableWorshipRooms_003Ed__.pawn = _003C_003E3__pawn;
			return _003CUsableWorshipRooms_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Room>)this).GetEnumerator();
		}
	}

	public static float FocusObjectSearchRadius = 3.9f;

	private static float WanderRadius = 10f;

	public static readonly Color ArtificialBuildingRingColor = new Color(0.8f, 0.49f, 0.43f);

	private static Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>> focusObjectHyperlinksPerTypeCache = new Dictionary<MeditationFocusDef, List<Dialog_InfoCard.Hyperlink>>();

	private static Dictionary<MeditationFocusDef, string> focusObjectsPerTypeCache = new Dictionary<MeditationFocusDef, string>();

	public static Job GetMeditationJob(Pawn pawn, bool forJoy = false)
	{
		MeditationSpotAndFocus meditationSpotAndFocus = FindMeditationSpot(pawn);
		if (meditationSpotAndFocus.IsValid && pawn.CanReserveAndReach(meditationSpotAndFocus.spot, PathEndMode.OnCell, pawn.NormalMaxDanger()))
		{
			Job job;
			if (meditationSpotAndFocus.focus.Thing is Building_Throne building_Throne)
			{
				job = JobMaker.MakeJob(JobDefOf.Reign, building_Throne, null, building_Throne);
			}
			else
			{
				JobDef def = JobDefOf.Meditate;
				if (forJoy && ModsConfig.IdeologyActive && pawn.Ideo != null && pawn.Ideo.foundation is IdeoFoundation_Deity ideoFoundation_Deity && ideoFoundation_Deity.DeitiesListForReading.Any())
				{
					def = JobDefOf.MeditatePray;
				}
				job = JobMaker.MakeJob(def, meditationSpotAndFocus.spot, null, meditationSpotAndFocus.focus);
			}
			job.ignoreJoyTimeAssignment = !forJoy;
			return job;
		}
		return null;
	}

	public static MeditationSpotAndFocus FindMeditationSpot(Pawn pawn)
	{
		float num = float.MinValue;
		LocalTargetInfo spot = LocalTargetInfo.Invalid;
		LocalTargetInfo focus = LocalTargetInfo.Invalid;
		if (!ModLister.CheckRoyalty("Psyfocus"))
		{
			return new MeditationSpotAndFocus(spot, focus);
		}
		Room ownedRoom = pawn.ownership.OwnedRoom;
		foreach (LocalTargetInfo item in AllMeditationSpotCandidates(pawn))
		{
			if (!SafeEnvironmentalConditions(pawn, item.Cell, pawn.Map) || !item.Cell.Standable(pawn.Map) || item.Cell.IsForbidden(pawn))
			{
				continue;
			}
			float num2 = 1f / Mathf.Max(item.Cell.DistanceToSquared(pawn.Position), 0.1f);
			LocalTargetInfo localTargetInfo = ((item.Thing is Building_Throne) ? ((LocalTargetInfo)item.Thing) : BestFocusAt(item, pawn));
			if (pawn.HasPsylink && localTargetInfo.IsValid)
			{
				num2 += localTargetInfo.Thing.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn) * 100f;
			}
			Room room = item.Cell.GetRoom(pawn.Map);
			if (room != null && ownedRoom == room)
			{
				num2 += 1f;
			}
			if (item.Thing != null && item.Thing is Building building && building.GetAssignedPawn() == pawn)
			{
				num2 += (float)((building.def == ThingDefOf.MeditationSpot) ? 200 : 100);
			}
			if (room != null && ModsConfig.IdeologyActive && room.Role == RoomRoleDefOf.WorshipRoom)
			{
				num2 += 100f;
				foreach (Thing containedAndAdjacentThing in room.ContainedAndAdjacentThings)
				{
					num2 += containedAndAdjacentThing.GetStatValue(StatDefOf.StyleDominance);
				}
			}
			if (num2 > num)
			{
				spot = item;
				focus = localTargetInfo;
				num = num2;
			}
		}
		return new MeditationSpotAndFocus(spot, focus);
	}

	public static bool IsValidMeditationBuildingForPawn(Building spot, Pawn pawn)
	{
		if (spot.DestroyedOrNull() || !spot.Spawned || spot.IsForbidden(pawn) || !spot.Position.Standable(spot.Map))
		{
			return false;
		}
		if (spot.GetAssignedPawn() != null && spot.GetAssignedPawn() != pawn)
		{
			return false;
		}
		Room room = spot.GetRoom();
		if (room != null && !CanUseRoomToMeditate(room, pawn))
		{
			return false;
		}
		if (spot is Building_Throne && RoomRoleWorker_ThroneRoom.Validate(room) != null)
		{
			return false;
		}
		return pawn.CanReserveAndReach(spot, PathEndMode.OnCell, pawn.NormalMaxDanger());
	}

	[IteratorStateMachine(typeof(_003CAllMeditationSpotCandidates_003Ed__6))]
	public static IEnumerable<LocalTargetInfo> AllMeditationSpotCandidates(Pawn pawn, bool allowFallbackSpots = true)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CAllMeditationSpotCandidates_003Ed__6(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__allowFallbackSpots = allowFallbackSpots
		};
	}

	[IteratorStateMachine(typeof(_003CUsableWorshipRooms_003Ed__7))]
	public static IEnumerable<Room> UsableWorshipRooms(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CUsableWorshipRooms_003Ed__7(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static bool SafeEnvironmentalConditions(Pawn pawn, IntVec3 cell, Map map)
	{
		if (map.gameConditionManager.ConditionIsActive(GameConditionDefOf.ToxicFallout) && !cell.Roofed(map))
		{
			return false;
		}
		if (ModsConfig.BiotechActive && NoxiousHazeUtility.IsExposedToNoxiousHaze(pawn, cell, map))
		{
			return false;
		}
		if (cell.GetDangerFor(pawn, map) != Danger.None)
		{
			return false;
		}
		if (cell.VacuumConcernTo(pawn))
		{
			return false;
		}
		return true;
	}

	public static bool CanMeditateNow(Pawn pawn)
	{
		if (pawn.needs.rest != null && (int)pawn.needs.rest.CurCategory >= 2)
		{
			return false;
		}
		if (pawn.needs.food != null && pawn.needs.food.Starving)
		{
			return false;
		}
		if (pawn.Downed || !pawn.Awake())
		{
			return false;
		}
		if (pawn.health.hediffSet.BleedRateTotal > 0f || (HealthAIUtility.ShouldSeekMedicalRest(pawn) && pawn.timetable?.CurrentAssignment != TimeAssignmentDefOf.Meditate) || HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn))
		{
			return false;
		}
		return true;
	}

	public static bool CanOnlyMeditateInBed(Pawn pawn)
	{
		return pawn.Downed;
	}

	public static bool ShouldMeditateInBed(Pawn pawn)
	{
		if (pawn.Downed)
		{
			return true;
		}
		if (pawn.health.hediffSet.AnyHediffMakesSickThought)
		{
			return true;
		}
		return false;
	}

	public static LocalTargetInfo BestFocusAt(LocalTargetInfo spot, Pawn pawn)
	{
		float num = 0f;
		LocalTargetInfo result = LocalTargetInfo.Invalid;
		foreach (Thing item in GenRadial.RadialDistinctThingsAround(spot.Cell, pawn.MapHeld, FocusObjectSearchRadius, useCenter: false))
		{
			if (!GenSight.LineOfSightToThing(spot.Cell, item, pawn.Map) || item is Building_Throne)
			{
				continue;
			}
			CompMeditationFocus compMeditationFocus = item.TryGetComp<CompMeditationFocus>();
			if (compMeditationFocus != null && compMeditationFocus.CanPawnUse(pawn))
			{
				float statValueForPawn = item.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn);
				if (statValueForPawn > num)
				{
					result = item;
					num = statValueForPawn;
				}
			}
		}
		return result;
	}

	[IteratorStateMachine(typeof(_003CFocusSpotsInTheRoom_003Ed__13))]
	public static IEnumerable<LocalTargetInfo> FocusSpotsInTheRoom(Pawn pawn, Room r)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFocusSpotsInTheRoom_003Ed__13(-2)
		{
			_003C_003E3__pawn = pawn,
			_003C_003E3__r = r
		};
	}

	public static LocalTargetInfo MeditationSpotForFocus(Thing t, Pawn p, Func<IntVec3, bool> validator = null)
	{
		return t.OccupiedRect().ExpandedBy(2).AdjacentCellsCardinal.Where((IntVec3 cell) => cell.InBounds(t.Map) && (validator == null || validator(cell)) && !cell.IsForbidden(p) && p.CanReserveAndReach(cell, PathEndMode.OnCell, p.NormalMaxDanger()) && cell.Standable(p.Map)).RandomElementWithFallback(IntVec3.Invalid);
	}

	[IteratorStateMachine(typeof(_003CFocusTypesAvailableForPawn_003Ed__15))]
	public static IEnumerable<MeditationFocusDef> FocusTypesAvailableForPawn(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFocusTypesAvailableForPawn_003Ed__15(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static string FocusTypesAvailableForPawnString(Pawn pawn)
	{
		return (from f in FocusTypesAvailableForPawn(pawn)
			select f.label).ToCommaList();
	}

	[IteratorStateMachine(typeof(_003CFocusObjectsForPawnHyperlinks_003Ed__18))]
	public static IEnumerable<Dialog_InfoCard.Hyperlink> FocusObjectsForPawnHyperlinks(Pawn pawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CFocusObjectsForPawnHyperlinks_003Ed__18(-2)
		{
			_003C_003E3__pawn = pawn
		};
	}

	public static string FocusTypeAvailableExplanation(Pawn pawn)
	{
		string text = "";
		for (int i = 0; i < DefDatabase<MeditationFocusDef>.AllDefsListForReading.Count; i++)
		{
			MeditationFocusDef meditationFocusDef = DefDatabase<MeditationFocusDef>.AllDefsListForReading[i];
			if (!meditationFocusDef.CanPawnUse(pawn))
			{
				continue;
			}
			text = text + "MeditationFocusCanUse".Translate(meditationFocusDef.label).Resolve() + ":\n" + meditationFocusDef.EnablingThingsExplanation(pawn) + "\n\n";
			if (!focusObjectsPerTypeCache.ContainsKey(meditationFocusDef))
			{
				List<string> list = new List<string>();
				foreach (ThingDef item in DefDatabase<ThingDef>.AllDefsListForReading)
				{
					CompProperties_MeditationFocus compProperties = item.GetCompProperties<CompProperties_MeditationFocus>();
					if (compProperties != null && compProperties.focusTypes.Contains(meditationFocusDef))
					{
						list.AddUnique(item.label);
					}
				}
				focusObjectsPerTypeCache[meditationFocusDef] = list.ToLineList("  - ", capitalizeItems: true);
			}
			text += "MeditationFocusObjects".Translate(meditationFocusDef.label).CapitalizeFirst() + ":\n" + focusObjectsPerTypeCache[meditationFocusDef] + "\n\n";
		}
		return text;
	}

	public static void DrawMeditationSpotOverlay(IntVec3 center, Map map)
	{
		GenDraw.DrawRadiusRing(center, FocusObjectSearchRadius);
		foreach (Thing item in GenRadial.RadialDistinctThingsAround(center, map, FocusObjectSearchRadius, useCenter: false))
		{
			if (!(item is Building_Throne) && (item.def.building == null || !item.def.building.supportsWallAttachments) && item.TryGetComp<CompMeditationFocus>() != null && GenSight.LineOfSight(center, item.Position, map))
			{
				GenDraw.DrawLineBetween(center.ToVector3() + new Vector3(0.5f, 0f, 0.5f), item.TrueCenter(), SimpleColor.White);
			}
		}
	}

	public static bool CanUseRoomToMeditate(Room r, Pawn p)
	{
		if (!r.Owners.EnumerableNullOrEmpty() && !r.Owners.Contains(p))
		{
			return false;
		}
		if (r.IsPrisonCell && !p.IsPrisoner)
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CGetMeditationFociAffectedByBuilding_003Ed__23))]
	public static IEnumerable<Thing> GetMeditationFociAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetMeditationFociAffectedByBuilding_003Ed__23(-2)
		{
			_003C_003E3__map = map,
			_003C_003E3__def = def,
			_003C_003E3__faction = faction,
			_003C_003E3__pos = pos,
			_003C_003E3__rotation = rotation
		};
	}

	public static void DrawMeditationFociAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
	{
		int num = 0;
		foreach (Thing item in GetMeditationFociAffectedByBuilding(map, def, faction, pos, rotation))
		{
			if (num++ > 10)
			{
				break;
			}
			CompToggleDrawAffectedMeditationFoci compToggleDrawAffectedMeditationFoci = item.TryGetComp<CompToggleDrawAffectedMeditationFoci>();
			if (compToggleDrawAffectedMeditationFoci == null || compToggleDrawAffectedMeditationFoci.Enabled)
			{
				GenAdj.OccupiedRect(pos, rotation, def.size);
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), item.TrueCenter(), SimpleColor.Red);
			}
		}
	}

	public static bool CountsAsArtificialBuilding(ThingDef def, Faction faction)
	{
		if ((def.category == ThingCategory.Building || def.thingCategories.NotNullAndContains(ThingCategoryDefOf.BuildingsSpecial)) && faction != null)
		{
			return def.building.artificialForMeditationPurposes;
		}
		return false;
	}

	public static bool CountsAsArtificialBuilding(Thing t)
	{
		return CountsAsArtificialBuilding(t.def, t.Faction);
	}

	public static void DrawArtificialBuildingOverlay(IntVec3 pos, ThingDef def, Map map, float radius)
	{
		GenDraw.DrawRadiusRing(pos, radius, ArtificialBuildingRingColor);
		int num = 0;
		foreach (Thing item in map.listerArtificialBuildingsForMeditation.GetForCell(pos, radius))
		{
			if (num++ > 10)
			{
				break;
			}
			GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, Rot4.North, def.size, def.Altitude), item.TrueCenter(), SimpleColor.Red);
		}
	}

	public static float PsyfocusGainPerTick(Pawn pawn, Thing focus = null)
	{
		float num = pawn.GetStatValue(StatDefOf.MeditationFocusGain);
		if (focus != null && !focus.Destroyed)
		{
			num += focus.GetStatValueForPawn(StatDefOf.MeditationFocusStrength, pawn);
		}
		return num / 60000f;
	}

	public static void CheckMeditationScheduleTeachOpportunity(Pawn pawn)
	{
		if (!pawn.Dead && pawn.Spawned && pawn.HasPsylink && pawn.Faction == Faction.OfPlayer && !pawn.IsQuestLodger())
		{
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationSchedule, pawn, OpportunityType.GoodToKnow);
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.MeditationDesiredPsyfocus, pawn, OpportunityType.GoodToKnow);
		}
	}
}
