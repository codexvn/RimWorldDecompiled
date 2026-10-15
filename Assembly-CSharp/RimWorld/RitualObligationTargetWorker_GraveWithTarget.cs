using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld;

public class RitualObligationTargetWorker_GraveWithTarget : RitualObligationTargetFilter
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass2_0
	{
		public RitualObligation obligation;

		internal bool _003CGetTargets_003Eb__0(Thing t)
		{
			return ((Building_Grave)t).Corpse == obligation.targetA.Thing;
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetTargetInfos_003Ed__5 : IEnumerable<string>, IEnumerable, IEnumerator<string>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private string _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private RitualObligation obligation;

		public RitualObligation _003C_003E3__obligation;

		public RitualObligationTargetWorker_GraveWithTarget _003C_003E4__this;

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
		public _003CGetTargetInfos_003Ed__5(int _003C_003E1__state)
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
			RitualObligationTargetWorker_GraveWithTarget ritualObligationTargetWorker_GraveWithTarget = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (obligation == null)
				{
					if (Find.IdeoManager.classicMode)
					{
						_003C_003E2__current = "RitualTargetGraveInfoAbstractNoIdeo".Translate();
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E2__current = "RitualTargetGraveInfoAbstract".Translate(ritualObligationTargetWorker_GraveWithTarget.parent.ideo.Named("IDEO"));
					_003C_003E1__state = 2;
					return true;
				}
				Pawn pawn = obligation.targetA.Thing as Pawn;
				if (pawn == null)
				{
					pawn = ((Corpse)obligation.targetA.Thing).InnerPawn;
				}
				_003C_003E2__current = "RitualTargetGraveInfo".Translate(pawn.Named("PAWN"));
				_003C_003E1__state = 3;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00a2;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a2;
			case 3:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00a2:
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
			_003CGetTargetInfos_003Ed__5 _003CGetTargetInfos_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetTargetInfos_003Ed__ = this;
			}
			else
			{
				_003CGetTargetInfos_003Ed__ = new _003CGetTargetInfos_003Ed__5(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetTargetInfos_003Ed__.obligation = _003C_003E3__obligation;
			return _003CGetTargetInfos_003Ed__;
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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass2_0 CS_0024_003C_003E8__locals1 = new _003C_003Ec__DisplayClass2_0
				{
					obligation = obligation
				};
				Thing thing = map.listerThings.ThingsInGroup(ThingRequestGroup.Grave).FirstOrDefault((Thing t) => ((Building_Grave)t).Corpse == CS_0024_003C_003E8__locals1.obligation.targetA.Thing);
				if (thing != null)
				{
					_003C_003E2__current = thing;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
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

	public RitualObligationTargetWorker_GraveWithTarget()
	{
	}

	public RitualObligationTargetWorker_GraveWithTarget(RitualObligationTargetFilterDef def)
		: base(def)
	{
	}

	[IteratorStateMachine(typeof(_003CGetTargets_003Ed__2))]
	public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargets_003Ed__2(-2)
		{
			_003C_003E3__obligation = obligation,
			_003C_003E3__map = map
		};
	}

	protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
	{
		return target.HasThing && target.Thing is Building_Grave building_Grave && (building_Grave.Corpse == obligation.targetA.Thing || building_Grave.Corpse?.InnerPawn == obligation.targetA.Thing);
	}

	public override bool ObligationTargetsValid(RitualObligation obligation)
	{
		Corpse corpse = (obligation.targetA.Thing as Pawn)?.Corpse;
		if (obligation.targetA.HasThing)
		{
			if (corpse != null)
			{
				return !corpse.Destroyed;
			}
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CGetTargetInfos_003Ed__5))]
	public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetTargetInfos_003Ed__5(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__obligation = obligation
		};
	}

	public override string LabelExtraPart(RitualObligation obligation)
	{
		if (obligation.targetA.Thing is Pawn pawn)
		{
			return pawn.LabelShort;
		}
		return ((Corpse)obligation.targetA.Thing).InnerPawn.LabelShort;
	}
}
