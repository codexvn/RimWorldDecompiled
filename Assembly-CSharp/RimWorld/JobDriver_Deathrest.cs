using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Deathrest : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Deathrest _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__9(int _003C_003E1__state)
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
			JobDriver_Deathrest CS_0024_003C_003E8__locals16 = _003C_003E4__this;
			Toil toil;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003ChasBed_003E5__2 = CS_0024_003C_003E8__locals16.Bed != null;
				if (_003ChasBed_003E5__2)
				{
					_003C_003E2__current = Toils_Bed.ClaimBedIfNonMedical(TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E1__state = 3;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil2 = Toils_Bed.GotoBed(TargetIndex.A);
				toil2.AddFailCondition(delegate
				{
					if (CS_0024_003C_003E8__locals16.pawn.BeingTransportedOnGravship)
					{
						return false;
					}
					if (SanguophageUtility.ShouldBeDeathrestingOrInComaInsteadOfDead(CS_0024_003C_003E8__locals16.pawn))
					{
						return false;
					}
					foreach (IntVec3 item in CS_0024_003C_003E8__locals16.Bed.OccupiedRect())
					{
						if (item.InSunlight(CS_0024_003C_003E8__locals16.Bed.Map))
						{
							Messages.Message("MessageBedExposedToSunlight".Translate(CS_0024_003C_003E8__locals16.pawn.Named("PAWN"), CS_0024_003C_003E8__locals16.Bed.Named("BED")), CS_0024_003C_003E8__locals16.Bed, MessageTypeDefOf.RejectInput);
							return true;
						}
					}
					return false;
				});
				_003C_003E2__current = toil2;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				goto IL_00b4;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00b4;
			case 4:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00b4:
				toil = Toils_LayDown.LayDown(TargetIndex.A, _003ChasBed_003E5__2, lookForOtherJobs: false, canSleep: true, gainRestAndHealth: true, PawnPosture.LayingOnGroundNormal, deathrest: true);
				toil.initAction = (Action)Delegate.Combine(toil.initAction, (Action)delegate
				{
					if (CS_0024_003C_003E8__locals16.pawn.Drafted)
					{
						CS_0024_003C_003E8__locals16.pawn.drafter.Drafted = false;
					}
					if (!CS_0024_003C_003E8__locals16.pawn.health.hediffSet.HasHediff(HediffDefOf.Deathrest))
					{
						CS_0024_003C_003E8__locals16.pawn.health.AddHediff(HediffDefOf.Deathrest, null, null);
					}
				});
				toil.tickIntervalAction = (Action<int>)Delegate.Combine(toil.tickIntervalAction, (Action<int>)delegate(int delta)
				{
					if (CS_0024_003C_003E8__locals16.pawn.IsHashIntervalTick(160, delta))
					{
						MoteMaker.MakeAttachedOverlay(CS_0024_003C_003E8__locals16.pawn, ThingDefOf.Mote_Deathresting, new Vector3(0f, CS_0024_003C_003E8__locals16.pawn.story.bodyType.bedOffset).RotatedBy(CS_0024_003C_003E8__locals16.pawn.Rotation));
					}
				});
				_003C_003E2__current = toil;
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
			_003CMakeNewToils_003Ed__9 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__9(0)
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

	private const TargetIndex BedIndex = TargetIndex.A;

	private const int MoteIntervalTicks = 160;

	private Building_Bed Bed => job.GetTarget(TargetIndex.A).Thing as Building_Bed;

	public override bool PlayerInterruptable => !base.OnLastToil;

	public override string GetReport()
	{
		return ReportStringProcessed(SanguophageUtility.DeathrestJobReport(pawn));
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (Bed != null)
		{
			return pawn.Reserve(Bed, job, Bed.SleepingSlotsCount, 0, null, errorOnFailed);
		}
		return true;
	}

	public override bool CanBeginNowWhileLyingDown()
	{
		return JobInBedUtility.InBedOrRestSpotNow(pawn, job.GetTarget(TargetIndex.A));
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__9))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__9(-2)
		{
			_003C_003E4__this = this
		};
	}
}
