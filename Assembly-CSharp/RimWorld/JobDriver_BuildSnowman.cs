using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_BuildSnowman : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass3_0
	{
		public JobDriver_BuildSnowman _003C_003E4__this;

		public Toil doWork;

		internal void _003CMakeNewToils_003Eb__0()
		{
			_003C_003E4__this.workLeft = 2300f;
		}

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			_003C_003E4__this.workLeft -= doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
			if (_003C_003E4__this.workLeft <= 0f)
			{
				Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman);
				thing.SetFaction(_003C_003E4__this.pawn.Faction);
				GenSpawn.Spawn(thing, _003C_003E4__this.TargetLocA, _003C_003E4__this.Map);
				_003C_003E4__this.ReadyForNextToil();
			}
			else
			{
				JoyUtility.JoyTickCheckEnd(_003C_003E4__this.pawn, delta);
			}
		}

		internal bool _003CMakeNewToils_003Eb__2()
		{
			return !JoyUtility.EnjoyableOutsideNow(_003C_003E4__this.pawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BuildSnowman _003C_003E4__this;

		private _003C_003Ec__DisplayClass3_0 _003C_003E8__1;

		Toil IEnumerator<Toil>.Current
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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass3_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.doWork = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.doWork.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.workLeft = 2300f;
				};
				_003C_003E8__1.doWork.tickIntervalAction = delegate(int delta)
				{
					_003C_003E8__1._003C_003E4__this.workLeft = _003C_003E8__1._003C_003E4__this.workLeft - _003C_003E8__1.doWork.actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
					if (_003C_003E8__1._003C_003E4__this.workLeft <= 0f)
					{
						Thing thing = ThingMaker.MakeThing(ThingDefOf.Snowman);
						thing.SetFaction(_003C_003E8__1._003C_003E4__this.pawn.Faction);
						GenSpawn.Spawn(thing, _003C_003E8__1._003C_003E4__this.TargetLocA, _003C_003E8__1._003C_003E4__this.Map);
						_003C_003E8__1._003C_003E4__this.ReadyForNextToil();
					}
					else
					{
						JoyUtility.JoyTickCheckEnd(_003C_003E8__1._003C_003E4__this.pawn, delta);
					}
				};
				_003C_003E8__1.doWork.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.doWork.FailOn(() => !JoyUtility.EnjoyableOutsideNow(_003C_003E8__1._003C_003E4__this.pawn));
				_003C_003E8__1.doWork.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				_003C_003E8__1.doWork.activeSkill = () => SkillDefOf.Construction;
				_003C_003E2__current = _003C_003E8__1.doWork;
				_003C_003E1__state = 2;
				return true;
			case 2:
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private float workLeft = -1000f;

	protected const int BaseWorkAmount = 2300;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workLeft, "workLeft", 0f);
	}
}
