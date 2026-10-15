using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ActivitySuppression : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ActivitySuppression _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ActivitySuppression CS_0024_003C_003E8__locals15 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals15.job.targetC = CS_0024_003C_003E8__locals15.ThingToSuppress;
				CS_0024_003C_003E8__locals15.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				CS_0024_003C_003E8__locals15.FailOn(() => !ActivitySuppressionUtility.CanBeSuppressed(CS_0024_003C_003E8__locals15.ThingToSuppress, considerMinActivity: false, CS_0024_003C_003E8__locals15.job.playerForced));
				_003C_003E2__current = Toils_General.Do(delegate
				{
					IntVec3 adjacentInteractionCell = SocialInteractionUtility.GetAdjacentInteractionCell(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.TargetThingA, CS_0024_003C_003E8__locals15.job.playerForced);
					CS_0024_003C_003E8__locals15.pawn.Map.pawnDestinationReservationManager.Reserve(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.job, adjacentInteractionCell);
					CS_0024_003C_003E8__locals15.job.SetTarget(TargetIndex.B, adjacentInteractionCell);
				});
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = CS_0024_003C_003E8__locals15.TrySuppress(CS_0024_003C_003E8__locals15.ThingToSuppress);
				_003C_003E1__state = 3;
				return true;
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

	private const TargetIndex ThingToSuppressOrPlatformIndex = TargetIndex.A;

	private const TargetIndex AdjacentCellIndex = TargetIndex.B;

	private Building_HoldingPlatform Platform => base.TargetThingA as Building_HoldingPlatform;

	private Thing ThingToSuppress => Platform?.HeldPawn ?? base.TargetThingA;

	protected override string ReportStringProcessed(string str)
	{
		return JobUtility.GetResolvedJobReport(str, LocalTargetInfo.Invalid, LocalTargetInfo.Invalid, ThingToSuppress);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (Platform != null)
		{
			if (pawn.Reserve(Platform, job, 1, -1, null, errorOnFailed))
			{
				return pawn.Reserve(ThingToSuppress, job, 1, -1, null, errorOnFailed);
			}
			return false;
		}
		return pawn.Reserve(ThingToSuppress, job, 1, -1, null, errorOnFailed);
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

	private Toil TrySuppress(Thing thingToSuppress)
	{
		Toil toil = ToilMaker.MakeToil("TrySuppress");
		toil.initAction = delegate
		{
			toil.actor.pather.StopDead();
		};
		toil.tickIntervalAction = delegate(int delta)
		{
			Pawn actor = toil.actor;
			if (thingToSuppress == null)
			{
				Log.Error("Tried to execute activity suppression toil but thingToSuppress was null");
				actor.jobs.EndCurrentJob(JobCondition.Errored);
			}
			else
			{
				CompActivity compActivity = thingToSuppress.TryGetComp<CompActivity>();
				float num = actor.GetStatValue(StatDefOf.ActivitySuppressionRate) / 2500f * (float)delta;
				compActivity.AdjustActivity(0f - num);
				pawn.skills.Learn(SkillDefOf.Intellectual, 0.1f);
				if (compActivity.ActivityLevel < thingToSuppress.TryGetComp<CompActivity>().suppressIfAbove - 0.1f && actor.IsHashIntervalTick(4000, delta))
				{
					actor.jobs.CheckForJobOverride();
				}
				ThingToSuppress.TryGetComp<CompObelisk>()?.Notify_InteractedTick(pawn, delta);
			}
		};
		toil.WithProgressBar(TargetIndex.A, () => (thingToSuppress == null) ? 1f : (1f - thingToSuppress.TryGetComp<CompActivity>().ActivityLevel));
		toil.WithEffect(EffecterDefOf.ActivitySuppression, () => ThingToSuppress.SpawnedParentOrMe, null);
		toil.AddEndCondition(delegate
		{
			if (thingToSuppress == null)
			{
				Log.Error("Tried to execute activity suppression toil but thingToSuppress was null");
				return JobCondition.Errored;
			}
			return (!(thingToSuppress.TryGetComp<CompActivity>().ActivityLevel < 0.01f)) ? JobCondition.Ongoing : JobCondition.Succeeded;
		});
		toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
		toil.activeSkill = () => SkillDefOf.Intellectual;
		toil.socialMode = RandomSocialMode.Off;
		toil.defaultCompleteMode = ToilCompleteMode.Never;
		return toil;
	}
}
