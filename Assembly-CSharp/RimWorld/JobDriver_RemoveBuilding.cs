using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public abstract class JobDriver_RemoveBuilding : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass14_0
	{
		public Toil doWork;

		public JobDriver_RemoveBuilding _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__2(int delta)
		{
			_003C_003E4__this.workLeft -= _003C_003E4__this.pawn.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
			_003C_003E4__this.TickActionInterval(delta);
			if (_003C_003E4__this.workLeft <= 0f)
			{
				doWork.actor.jobs.curDriver.ReadyForNextToil();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__14 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_RemoveBuilding _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__14(int _003C_003E1__state)
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
			JobDriver_RemoveBuilding CS_0024_003C_003E8__locals39 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals39.Designation != null)
				{
					CS_0024_003C_003E8__locals39.FailOnThingMissingDesignation(TargetIndex.A, CS_0024_003C_003E8__locals39.Designation);
				}
				CS_0024_003C_003E8__locals39.FailOnForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals39.FailOn(() => CS_0024_003C_003E8__locals39.Building.TryGetComp<CompExplosive>(out var comp) && comp.wickStarted);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, (CS_0024_003C_003E8__locals39.Target is Building_Trap) ? PathEndMode.OnCell : PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass14_0 CS_0024_003C_003E8__locals32 = new _003C_003Ec__DisplayClass14_0();
				CS_0024_003C_003E8__locals32._003C_003E4__this = CS_0024_003C_003E8__locals39;
				CS_0024_003C_003E8__locals32.doWork = ToilMaker.MakeToil("MakeNewToils").FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				CS_0024_003C_003E8__locals32.doWork.initAction = delegate
				{
					CS_0024_003C_003E8__locals39.totalNeededWork = CS_0024_003C_003E8__locals39.TotalNeededWork;
					CS_0024_003C_003E8__locals39.workLeft = CS_0024_003C_003E8__locals39.totalNeededWork;
				};
				CS_0024_003C_003E8__locals32.doWork.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals32._003C_003E4__this.workLeft = CS_0024_003C_003E8__locals32._003C_003E4__this.workLeft - CS_0024_003C_003E8__locals32._003C_003E4__this.pawn.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f * (float)delta;
					CS_0024_003C_003E8__locals32._003C_003E4__this.TickActionInterval(delta);
					if (CS_0024_003C_003E8__locals32._003C_003E4__this.workLeft <= 0f)
					{
						CS_0024_003C_003E8__locals32.doWork.actor.jobs.curDriver.ReadyForNextToil();
					}
				};
				CS_0024_003C_003E8__locals32.doWork.defaultCompleteMode = ToilCompleteMode.Never;
				if (CS_0024_003C_003E8__locals39.WorkEffecter != null)
				{
					CS_0024_003C_003E8__locals32.doWork.WithEffect(CS_0024_003C_003E8__locals39.WorkEffecter, TargetIndex.A, null);
				}
				CS_0024_003C_003E8__locals32.doWork.WithProgressBar(TargetIndex.A, () => 1f - CS_0024_003C_003E8__locals39.workLeft / CS_0024_003C_003E8__locals39.totalNeededWork);
				CS_0024_003C_003E8__locals32.doWork.activeSkill = () => SkillDefOf.Construction;
				_003C_003E2__current = CS_0024_003C_003E8__locals32.doWork;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					if (CS_0024_003C_003E8__locals39.Target.Faction != null)
					{
						CS_0024_003C_003E8__locals39.Target.Faction.Notify_BuildingRemoved(CS_0024_003C_003E8__locals39.Building, CS_0024_003C_003E8__locals39.pawn);
					}
					CS_0024_003C_003E8__locals39.FinishedRemoving();
					CS_0024_003C_003E8__locals39.Map.designationManager.RemoveAllDesignationsOn(CS_0024_003C_003E8__locals39.Target);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
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
			_003CMakeNewToils_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__14(0)
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

	private float workLeft;

	private float totalNeededWork;

	protected Thing Target => job.targetA.Thing;

	protected Building Building => (Building)Target.GetInnerIfMinified();

	protected abstract DesignationDef Designation { get; }

	protected abstract float TotalNeededWork { get; }

	protected abstract EffecterDef WorkEffecter { get; }

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref workLeft, "workLeft", 0f);
		Scribe_Values.Look(ref totalNeededWork, "totalNeededWork", 0f);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__14))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	protected virtual void FinishedRemoving()
	{
	}

	protected virtual void TickActionInterval(int delta)
	{
	}
}
