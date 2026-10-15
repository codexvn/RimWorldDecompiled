using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Repair : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass8_0
	{
		public JobDriver_Repair _003C_003E4__this;

		public Toil repair;

		internal void _003CMakeNewToils_003Eb__0()
		{
			_003C_003E4__this.ticksToNextRepair = 80f;
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			Pawn actor = repair.actor;
			actor.skills?.Learn(SkillDefOf.Construction, 0.05f);
			if (_003C_003E4__this.IsBuildingAttachment)
			{
				actor.rotationTracker.FaceTarget(GenConstruct.GetWallAttachedTo(_003C_003E4__this.Building));
			}
			else
			{
				actor.rotationTracker.FaceTarget(actor.CurJob.GetTarget(TargetIndex.A));
			}
			float num = actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f;
			_003C_003E4__this.ticksToNextRepair -= num;
			if (_003C_003E4__this.ticksToNextRepair <= 0f)
			{
				_003C_003E4__this.ticksToNextRepair += 20f;
				_003C_003E4__this.TargetThingA.HitPoints++;
				_003C_003E4__this.TargetThingA.HitPoints = Mathf.Min(_003C_003E4__this.TargetThingA.HitPoints, _003C_003E4__this.TargetThingA.MaxHitPoints);
				_003C_003E4__this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)_003C_003E4__this.TargetThingA);
				if (_003C_003E4__this.TargetThingA.HitPoints == _003C_003E4__this.TargetThingA.MaxHitPoints)
				{
					actor.records.Increment(RecordDefOf.ThingsRepaired);
					actor.jobs.EndCurrentJob(JobCondition.Succeeded);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Repair _003C_003E4__this;

		private _003C_003Ec__DisplayClass8_0 _003C_003E8__1;

		private PathEndMode _003CpathEndMode_003E5__2;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
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
			int num = _003C_003E1__state;
			JobDriver_Repair jobDriver_Repair = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass8_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				jobDriver_Repair.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003CpathEndMode_003E5__2 = (jobDriver_Repair.IsBuildingAttachment ? PathEndMode.OnCell : PathEndMode.Touch);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, _003CpathEndMode_003E5__2);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E8__1.repair = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.repair.initAction = delegate
				{
					_003C_003E8__1._003C_003E4__this.ticksToNextRepair = 80f;
				};
				_003C_003E8__1.repair.tickAction = delegate
				{
					Pawn actor = _003C_003E8__1.repair.actor;
					actor.skills?.Learn(SkillDefOf.Construction, 0.05f);
					if (_003C_003E8__1._003C_003E4__this.IsBuildingAttachment)
					{
						actor.rotationTracker.FaceTarget(GenConstruct.GetWallAttachedTo(_003C_003E8__1._003C_003E4__this.Building));
					}
					else
					{
						actor.rotationTracker.FaceTarget(actor.CurJob.GetTarget(TargetIndex.A));
					}
					float num2 = actor.GetStatValue(StatDefOf.ConstructionSpeed) * 1.7f;
					_003C_003E8__1._003C_003E4__this.ticksToNextRepair = _003C_003E8__1._003C_003E4__this.ticksToNextRepair - num2;
					if (_003C_003E8__1._003C_003E4__this.ticksToNextRepair <= 0f)
					{
						_003C_003E8__1._003C_003E4__this.ticksToNextRepair = _003C_003E8__1._003C_003E4__this.ticksToNextRepair + 20f;
						_003C_003E8__1._003C_003E4__this.TargetThingA.HitPoints++;
						_003C_003E8__1._003C_003E4__this.TargetThingA.HitPoints = Mathf.Min(_003C_003E8__1._003C_003E4__this.TargetThingA.HitPoints, _003C_003E8__1._003C_003E4__this.TargetThingA.MaxHitPoints);
						_003C_003E8__1._003C_003E4__this.Map.listerBuildingsRepairable.Notify_BuildingRepaired((Building)_003C_003E8__1._003C_003E4__this.TargetThingA);
						if (_003C_003E8__1._003C_003E4__this.TargetThingA.HitPoints == _003C_003E8__1._003C_003E4__this.TargetThingA.MaxHitPoints)
						{
							actor.records.Increment(RecordDefOf.ThingsRepaired);
							actor.jobs.EndCurrentJob(JobCondition.Succeeded);
						}
					}
				};
				_003C_003E8__1.repair.FailOnCannotTouch(TargetIndex.A, _003CpathEndMode_003E5__2);
				_003C_003E8__1.repair.WithEffect(jobDriver_Repair.TargetThingA.def.repairEffect, TargetIndex.A, null);
				_003C_003E8__1.repair.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.repair.activeSkill = () => SkillDefOf.Construction;
				_003C_003E8__1.repair.handlingFacing = true;
				_003C_003E2__current = _003C_003E8__1.repair;
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	protected float ticksToNextRepair;

	private const float WarmupTicks = 80f;

	private const float TicksBetweenRepairs = 20f;

	private Building Building => (Building)job.GetTarget(TargetIndex.A).Thing;

	private bool IsBuildingAttachment
	{
		get
		{
			ThingDef thingDef = GenConstruct.BuiltDefOf(Building.def) as ThingDef;
			if (thingDef?.building != null)
			{
				return thingDef.building.isAttachment;
			}
			return false;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}
}
