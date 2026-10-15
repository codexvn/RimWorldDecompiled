using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_AnyGatherSpot : RitualObligationTargetFilter
{
	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__4 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

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
				_003C_003E2__current = "RitualTargetGatherSpotInfo".Translate();
				_003C_003E1__state = 1;
				return true;
			case 1:
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
		IEnumerator<string> IEnumerable<string>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CGetTargetInfos_003Ed__4(0);
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

		private Map map;

		public Map _003C_003E3__map;

		private List<Thing> _003CpartySpot_003E5__2;

		private int _003Cj_003E5__3;

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
			_003CpartySpot_003E5__2 = null;
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
				_003CpartySpot_003E5__2 = map.listerThings.ThingsOfDef(ThingDefOf.PartySpot);
				_003Cj_003E5__3 = 0;
				goto IL_0082;
			case 1:
				_003C_003E1__state = -1;
				_003Cj_003E5__3++;
				goto IL_0082;
			case 2:
				{
					_003C_003E1__state = -1;
					_003Cj_003E5__3++;
					break;
				}
				IL_0082:
				if (_003Cj_003E5__3 < _003CpartySpot_003E5__2.Count)
				{
					_003C_003E2__current = _003CpartySpot_003E5__2[_003Cj_003E5__3];
					_003C_003E1__state = 1;
					return true;
				}
				_003Cj_003E5__3 = 0;
				break;
			}
			if (_003Cj_003E5__3 < map.gatherSpotLister.activeSpots.Count)
			{
				_003C_003E2__current = map.gatherSpotLister.activeSpots[_003Cj_003E5__3].parent;
				_003C_003E1__state = 2;
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
				_003CGetTargets_003Ed__ = new _003CGetTargets_003Ed__2(0);
			}
			_003CGetTargets_003Ed__.map = _003C_003E3__map;
			return _003CGetTargets_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<TargetInfo>)this).GetEnumerator();
		}
	}

	public RitualObligationTargetWorker_AnyGatherSpot()
	{
	}

	public RitualObligationTargetWorker_AnyGatherSpot(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	[IteratorStateMachine(typeof(_003CGetTargets_003Ed__2))]
	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargets_003Ed__2(-2)
		{
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
		CompGatherSpot compGatherSpot = thing.TryGetComp<CompGatherSpot>();
		if (compGatherSpot != null && compGatherSpot.Active)
		{
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__4))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__4(-2);
	}
}
