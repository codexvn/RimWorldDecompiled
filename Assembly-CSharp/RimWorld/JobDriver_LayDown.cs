using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_LayDown : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__16 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_LayDown _003C_003E4__this;

		private bool _003ChasBed_003E5__2;

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
		public _003CMakeNewToils_003Ed__16(int _003C_003E1__state)
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
			JobDriver_LayDown CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003ChasBed_003E5__2 = CS_0024_003C_003E8__locals11.Bed != null;
				if (_003ChasBed_003E5__2)
				{
					_003C_003E2__current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				}
				Thing thing = CS_0024_003C_003E8__locals11.job.GetTarget(TargetIndex.A).Thing;
				if ((thing == null || CS_0024_003C_003E8__locals11.pawn.SpawnedParentOrMe != thing) && CS_0024_003C_003E8__locals11.canMoveOrCrawl)
				{
					_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell).FailOn(() => CS_0024_003C_003E8__locals11.pawn.Downed && !CS_0024_003C_003E8__locals11.pawn.health.CanCrawl);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00f0;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Bed.GotoBed(TargetIndex.A).FailOn(() => CS_0024_003C_003E8__locals11.pawn.Downed && !CS_0024_003C_003E8__locals11.pawn.health.CanCrawl && !CS_0024_003C_003E8__locals11.Bed.OccupiedRect().Contains(CS_0024_003C_003E8__locals11.pawn.Position));
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00f0;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00f0;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00f0:
				_003C_003E2__current = CS_0024_003C_003E8__locals11.LayDownToil(_003ChasBed_003E5__2);
				_003C_003E1__state = 4;
				return true;
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
			_003CMakeNewToils_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__16(0)
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

	private bool canMoveOrCrawl;

	private bool hasSavedValues;

	public const TargetIndex BedOrRestSpotIndex = TargetIndex.A;

	public Building_Bed Bed => job.GetTarget(TargetIndex.A).Thing as Building_Bed;

	public IntVec3 Cell => job.GetTarget(TargetIndex.A).Cell;

	public virtual bool CanSleep => true;

	public virtual bool CanRest => true;

	public virtual bool LookForOtherJobs => true;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (Bed != null)
		{
			if (!pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed))
			{
				return false;
			}
		}
		else if (job.targetA.Cell.IsValid && !pawn.ageTracker.CurLifeStage.alwaysDowned && !job.forceSleep && !pawn.Reserve(job.targetA.Cell, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	public override bool CanBeginNowWhileLyingDown()
	{
		return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		canMoveOrCrawl = !pawn.Downed || pawn.health.CanCrawl;
		hasSavedValues = true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__16))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	public virtual Toil LayDownToil(bool hasBed)
	{
		return Toils_LayDown.LayDown(TargetIndex.A, hasBed, LookForOtherJobs, CanSleep, CanRest);
	}

	public override string GetReport()
	{
		string reportStringOverride = GetReportStringOverride();
		if (!reportStringOverride.NullOrEmpty())
		{
			return reportStringOverride;
		}
		if (asleep)
		{
			return "ReportSleeping".Translate();
		}
		Thing spawnedParentOrMe = pawn.SpawnedParentOrMe;
		if (spawnedParentOrMe != null && pawn != spawnedParentOrMe)
		{
			return JobDriver_Carried.GetReport(pawn, spawnedParentOrMe);
		}
		if (pawn.health.hediffSet.InLabor(includePostpartumExhaustion: false))
		{
			return "GivingBirth".Translate();
		}
		return "ReportResting".Translate();
	}

	public override bool IsContinuation(Job j)
	{
		if (job.GetTarget(TargetIndex.A) != j.GetTarget(TargetIndex.A))
		{
			return false;
		}
		return base.IsContinuation(j);
	}

	public override void ExposeData()
	{
		Scribe_Values.Look(ref canMoveOrCrawl, "canMoveOrCrawl", defaultValue: false);
		Scribe_Values.Look(ref hasSavedValues, "hasSavedValues", defaultValue: false);
		if (Scribe.mode == LoadSaveMode.PostLoadInit && !hasSavedValues)
		{
			canMoveOrCrawl = !pawn.Downed || pawn.health.CanCrawl;
			hasSavedValues = true;
		}
		base.ExposeData();
	}
}
