using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class CompPushable : ThingComp
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public CompPushable _003C_003E4__this;

		public Pawn selPawn;

		public Action<LocalTargetInfo> _003C_003E9__1;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			Find.Targeter.BeginTargeting(TargetingParameters.ForCell(), delegate(LocalTargetInfo target)
			{
				Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, _003C_003E4__this.Pawn, target.Cell);
				job.count = 1;
				selPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
			});
		}

		internal void _003CCompFloatMenuOptions_003Eb__1(LocalTargetInfo target)
		{
			Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, _003C_003E4__this.Pawn, target.Cell);
			job.count = 1;
			selPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__8 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompPushable _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompFloatMenuOptions_003Ed__8(int _003C_003E1__state)
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
			CompPushable compPushable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass8_0 CS_0024_003C_003E8__locals3 = new _003C_003Ec__DisplayClass8_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if (!compPushable.canBePushed || !compPushable.Props.givePushOption)
				{
					return false;
				}
				FloatMenuOption floatMenuOption = new FloatMenuOption("CommandPushTo".Translate(), delegate
				{
					Find.Targeter.BeginTargeting(TargetingParameters.ForCell(), delegate(LocalTargetInfo target)
					{
						Job job = JobMaker.MakeJob(JobDefOf.HaulToCell, CS_0024_003C_003E8__locals3._003C_003E4__this.Pawn, target.Cell);
						job.count = 1;
						CS_0024_003C_003E8__locals3.selPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
					});
				});
				if (!CS_0024_003C_003E8__locals3.selPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					floatMenuOption.Label = $"{floatMenuOption.Label} ({"RequiredCapacity".Translate(PawnCapacityDefOf.Manipulation.label)})";
					floatMenuOption.Disabled = true;
				}
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 1;
				return true;
			}
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__8 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__8(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__10 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CCompGetGizmosExtra_003Ed__10(int _003C_003E1__state)
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
			if (_003C_003E1__state != 0)
			{
				return false;
			}
			_003C_003E1__state = -1;
			_ = Prefs.DevMode;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				return this;
			}
			return new _003CCompGetGizmosExtra_003Ed__10(0);
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public bool canBePushed = true;

	public Vector3 drawPos;

	public Vector3 drawVel;

	public CompProperties_Pushable Props => (CompProperties_Pushable)props;

	public Pawn Pawn => (Pawn)parent;

	public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
	{
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__8))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__8(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public void OnStartedCarrying(Pawn pawn)
	{
		Vector3 v = (pawn.Position - Pawn.Position).ToVector3();
		Vector2 vector = new Vector2(0f, 0f - Props.offsetDistance).RotatedBy(v.AngleFlat());
		drawPos = new Vector3(vector.x, 0f, 0f - vector.y);
		drawVel = Vector3.zero;
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__10))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__10(-2);
	}

	public override void PostExposeData()
	{
		base.PostExposeData();
		Scribe_Values.Look(ref canBePushed, "canBePushed", defaultValue: false);
		Scribe_Values.Look(ref drawPos, "drawPos");
		Scribe_Values.Look(ref drawVel, "drawVel");
	}
}
