using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_ExtractTree : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public Toil doWork;

		public JobDriver_ExtractTree _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__1(int delta)
		{
			_003C_003E4__this.workLeft -= JobDriver_PlantWork.WorkDonePerTick(_003C_003E4__this.pawn, _003C_003E4__this.Tree) * (float)delta;
			if (_003C_003E4__this.pawn.skills != null)
			{
				_003C_003E4__this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta);
			}
			if (_003C_003E4__this.workLeft <= 0f)
			{
				SoundDefOf.Finish_Wood.PlayOneShot(SoundInfo.InMap(_003C_003E4__this.Tree));
				doWork.actor.jobs.curDriver.ReadyForNextToil();
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ExtractTree _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
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
			JobDriver_ExtractTree CS_0024_003C_003E8__locals36 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals36.FailOnThingMissingDesignation(TargetIndex.A, CS_0024_003C_003E8__locals36.Designation);
				CS_0024_003C_003E8__locals36.FailOnForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass13_0 CS_0024_003C_003E8__locals33 = new _003C_003Ec__DisplayClass13_0();
				CS_0024_003C_003E8__locals33._003C_003E4__this = CS_0024_003C_003E8__locals36;
				CS_0024_003C_003E8__locals33.doWork = ToilMaker.MakeToil("MakeNewToils").FailOnDestroyedNullOrForbidden(TargetIndex.A).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
				CS_0024_003C_003E8__locals33.doWork.initAction = delegate
				{
					CS_0024_003C_003E8__locals36.totalNeededWork = CS_0024_003C_003E8__locals36.TotalNeededWork;
					CS_0024_003C_003E8__locals36.workLeft = CS_0024_003C_003E8__locals36.totalNeededWork;
				};
				CS_0024_003C_003E8__locals33.doWork.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals33._003C_003E4__this.workLeft = CS_0024_003C_003E8__locals33._003C_003E4__this.workLeft - JobDriver_PlantWork.WorkDonePerTick(CS_0024_003C_003E8__locals33._003C_003E4__this.pawn, CS_0024_003C_003E8__locals33._003C_003E4__this.Tree) * (float)delta;
					if (CS_0024_003C_003E8__locals33._003C_003E4__this.pawn.skills != null)
					{
						CS_0024_003C_003E8__locals33._003C_003E4__this.pawn.skills.Learn(SkillDefOf.Plants, 0.085f * (float)delta);
					}
					if (CS_0024_003C_003E8__locals33._003C_003E4__this.workLeft <= 0f)
					{
						SoundDefOf.Finish_Wood.PlayOneShot(SoundInfo.InMap(CS_0024_003C_003E8__locals33._003C_003E4__this.Tree));
						CS_0024_003C_003E8__locals33.doWork.actor.jobs.curDriver.ReadyForNextToil();
					}
				};
				CS_0024_003C_003E8__locals33.doWork.defaultCompleteMode = ToilCompleteMode.Never;
				CS_0024_003C_003E8__locals33.doWork.WithProgressBar(TargetIndex.A, () => 1f - CS_0024_003C_003E8__locals36.workLeft / CS_0024_003C_003E8__locals36.totalNeededWork);
				CS_0024_003C_003E8__locals33.doWork.WithEffect(EffecterDefOf.Harvest_Plant, TargetIndex.A, null);
				CS_0024_003C_003E8__locals33.doWork.PlaySustainerOrSound(() => SoundDefOf.Interact_ConstructDirt);
				CS_0024_003C_003E8__locals33.doWork.activeSkill = () => SkillDefOf.Plants;
				_003C_003E2__current = CS_0024_003C_003E8__locals33.doWork;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					IntVec3 position = CS_0024_003C_003E8__locals36.Tree.Position;
					bool num2 = Find.Selector.IsSelected(CS_0024_003C_003E8__locals36.Tree);
					Thing thing = GenSpawn.Spawn(CS_0024_003C_003E8__locals36.Tree.MakeMinified(), position, CS_0024_003C_003E8__locals36.pawn.Map);
					if (num2 && thing != null)
					{
						Find.Selector.Select(thing, playSound: false, forceDesignatorDeselect: false);
					}
					CS_0024_003C_003E8__locals36.Map.designationManager.RemoveAllDesignationsOn(CS_0024_003C_003E8__locals36.Target);
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
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
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

	public const TargetIndex TreeInd = TargetIndex.A;

	protected Thing Target => job.GetTarget(TargetIndex.A).Thing;

	protected Plant Tree => (Plant)Target.GetInnerIfMinified();

	protected DesignationDef Designation => DesignationDefOf.ExtractTree;

	protected float TotalNeededWork => Tree.def.plant.harvestWork;

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

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}
}
