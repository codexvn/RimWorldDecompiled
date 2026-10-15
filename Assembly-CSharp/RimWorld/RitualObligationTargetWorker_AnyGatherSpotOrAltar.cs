using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_AnyGatherSpotOrAltar : RitualObligationTargetWorker_AnyGatherSpot
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public RitualObligationTargetWorker_AnyGatherSpotOrAltar _003C_003E4__this;

		private IEnumerator<string> _003C_003E7__wrap1;

		string IEnumerator<string>.Current
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
		public _003CGetTargetInfos_003Ed__4(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 2)
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
				int num = _003C_003E1__state;
				RitualObligationTargetWorker_AnyGatherSpotOrAltar ritualObligationTargetWorker_AnyGatherSpotOrAltar = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E2__current = "RitualTargetGatherSpotInfo".Translate();
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = RitualObligationTargetWorker_Altar.GetTargetInfosWorker(ritualObligationTargetWorker_AnyGatherSpotOrAltar.parent.ideo).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00a7;
				case 2:
					_003C_003E1__state = -3;
					goto IL_00a7;
				case 3:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00a7:
					if (_003C_003E7__wrap1.MoveNext())
					{
						string current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = ThingDefOf.RitualSpot.LabelCap;
					_003C_003E1__state = 3;
					return true;
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			_003CGetTargetInfos_003Ed__4 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetTargetInfos_003Ed__4(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<string>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargets_003Ed__2 : IEnumerable<TargetInfo>, IEnumerable, IEnumerator<TargetInfo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private TargetInfo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RitualObligation obligation;

		public RitualObligation _003C_003E3__obligation;

		private Map map;

		public Map _003C_003E3__map;

		public RitualObligationTargetWorker_AnyGatherSpotOrAltar _003C_003E4__this;

		private List<Thing> _003CritualSpots_003E5__2;

		private List<Thing> _003CpartySpot_003E5__3;

		private IEnumerator<TargetInfo> _003C_003E7__wrap3;

		private int _003Cj_003E5__5;

		TargetInfo IEnumerator<TargetInfo>.Current
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
		public _003CGetTargets_003Ed__2(int _003C_003E1__state)
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
			_003CritualSpots_003E5__2 = null;
			_003CpartySpot_003E5__3 = null;
			_003C_003E7__wrap3 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				RitualObligationTargetWorker_AnyGatherSpotOrAltar ritualObligationTargetWorker_AnyGatherSpotOrAltar = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!ModLister.CheckIdeology("Altar target"))
					{
						return false;
					}
					_003C_003E7__wrap3 = RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, ritualObligationTargetWorker_AnyGatherSpotOrAltar.parent.ideo).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00a3;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00a3;
				case 2:
					_003C_003E1__state = -1;
					_003Cj_003E5__5++;
					goto IL_0124;
				case 3:
					_003C_003E1__state = -1;
					_003Cj_003E5__5++;
					goto IL_019e;
				case 4:
					{
						_003C_003E1__state = -1;
						_003Cj_003E5__5++;
						break;
					}
					IL_0124:
					if (_003Cj_003E5__5 < _003CritualSpots_003E5__2.Count)
					{
						_003C_003E2__current = _003CritualSpots_003E5__2[_003Cj_003E5__5];
						_003C_003E1__state = 2;
						return true;
					}
					_003CpartySpot_003E5__3 = map.listerThings.ThingsOfDef(ThingDefOf.PartySpot);
					_003Cj_003E5__5 = 0;
					goto IL_019e;
					IL_00a3:
					if (_003C_003E7__wrap3.MoveNext())
					{
						TargetInfo current = _003C_003E7__wrap3.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap3 = null;
					_003CritualSpots_003E5__2 = map.listerThings.ThingsOfDef(ThingDefOf.RitualSpot);
					_003Cj_003E5__5 = 0;
					goto IL_0124;
					IL_019e:
					if (_003Cj_003E5__5 < _003CpartySpot_003E5__3.Count)
					{
						_003C_003E2__current = _003CpartySpot_003E5__3[_003Cj_003E5__5];
						_003C_003E1__state = 3;
						return true;
					}
					_003Cj_003E5__5 = 0;
					break;
				}
				if (_003Cj_003E5__5 < map.gatherSpotLister.activeSpots.Count)
				{
					_003C_003E2__current = map.gatherSpotLister.activeSpots[_003Cj_003E5__5].parent;
					_003C_003E1__state = 4;
					return true;
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
		IEnumerator<TargetInfo> IEnumerable<TargetInfo>.GetEnumerator()
		{
			_003CGetTargets_003Ed__2 _003CGetTargets_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargets_003Ed__ = this;
			}
			else
			{
				_003CGetTargets_003Ed__ = new _003CGetTargets_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTargets_003Ed__.obligation = _003C_003E3__obligation;
			_003CGetTargets_003Ed__.map = _003C_003E3__map;
			return _003CGetTargets_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TargetInfo>)this).GetEnumerator();
		}
	}

	public RitualObligationTargetWorker_AnyGatherSpotOrAltar()
	{
	}

	public RitualObligationTargetWorker_AnyGatherSpotOrAltar(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	[IteratorStateMachine(typeof(_003CGetTargets_003Ed__2))]
	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargets_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__obligation = obligation,
			_003C_003E3__map = map
		};
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		if (!target.HasThing)
		{
			return false;
		}
		Thing thing = target.Thing;
		if (def.colonistThingsOnly && (thing.Faction == null || !thing.Faction.IsPlayer))
		{
			return false;
		}
		if (thing.def == ThingDefOf.PartySpot)
		{
			return true;
		}
		if (thing.def == ThingDefOf.RitualSpot)
		{
			return true;
		}
		CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
		if (compGatherSpot != null && compGatherSpot.Active)
		{
			return true;
		}
		return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, parent.ideo);
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__4))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__4(-2)
		{
			_003C_003E4__this = this
		};
	}
}
