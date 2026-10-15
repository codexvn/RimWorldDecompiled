using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public abstract class RitualPosition_ThingDef : RitualPosition_NearbyThing
{
	[CompilerGenerated]
	private sealed class _003CCandidateThings_003Ed__2 : IEnumerable<Thing>, IEnumerable, IEnumerator<Thing>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Thing _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private LordJob_Ritual ritual;

		public LordJob_Ritual _003C_003E3__ritual;

		public RitualPosition_ThingDef _003C_003E4__this;

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
		public _003CCandidateThings_003Ed__2(int _003C_003E1__state)
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
				int num = _003C_003E1__state;
				RitualPosition_ThingDef ritualPosition_ThingDef = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ritual.Map.listerThings.ThingsOfDef(ritualPosition_ThingDef.ThingDef).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 1:
					_003C_003E1__state = -3;
					break;
				}
				while (_003C_003E7__wrap1.MoveNext())
				{
					Thing current = _003C_003E7__wrap1.Current;
					if (current.Position.InHorDistOf(ritual.selectedTarget.Cell, 50f))
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
			_003CCandidateThings_003Ed__2 _003CCandidateThings_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCandidateThings_003Ed__ = this;
			}
			else
			{
				_003CCandidateThings_003Ed__ = new _003CCandidateThings_003Ed__2(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCandidateThings_003Ed__.ritual = _003C_003E3__ritual;
			return _003CCandidateThings_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Thing>)this).GetEnumerator();
		}
	}

	protected abstract ThingDef ThingDef { get; }

	[IteratorStateMachine(typeof(_003CCandidateThings_003Ed__2))]
	public override IEnumerable<Thing> CandidateThings(LordJob_Ritual ritual)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCandidateThings_003Ed__2(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__ritual = ritual
		};
	}

	public override IntVec3 PositionForThing(Thing t)
	{
		return t.Position + t.Rotation.Opposite.FacingCell;
	}

	public override bool IsUsableThing(Thing thing, IntVec3 spot, TargetInfo ritualTarget)
	{
		IntVec3 c = PositionForThing(thing);
		Map map = ritualTarget.Map;
		Building edifice = c.GetEdifice(map);
		if (!c.Standable(map) || (edifice != null && edifice.def.Fillage != 0))
		{
			return false;
		}
		return base.IsUsableThing(thing, spot, ritualTarget);
	}

	protected override Rot4 FacingDir(Thing t)
	{
		return t.Rotation;
	}
}
