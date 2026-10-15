using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_FertilizeOvum : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FertilizeOvum _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
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
			JobDriver_FertilizeOvum CS_0024_003C_003E8__locals11 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModsConfig.BiotechActive)
				{
					return false;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedOrNull(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A, putRemainderInQueue: false, subtractNumTakenFromJobCount: false, failIfStackCountLessThanJobCount: true);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(180).WithProgressBarToilDelay(TargetIndex.A);
				toil.tickIntervalAction = delegate(int delta)
				{
					if (CS_0024_003C_003E8__locals11.pawn.IsHashIntervalTick(100, delta))
					{
						FleckMaker.ThrowMetaIcon(CS_0024_003C_003E8__locals11.pawn.Position, CS_0024_003C_003E8__locals11.pawn.Map, FleckDefOf.Heart);
					}
				};
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.DoAtomic(delegate
				{
					Thing thing = CS_0024_003C_003E8__locals11.ovum.ProduceEmbryo(CS_0024_003C_003E8__locals11.pawn);
					if (thing != null)
					{
						GenPlace.TryPlaceThing(thing, CS_0024_003C_003E8__locals11.pawn.PositionHeld, CS_0024_003C_003E8__locals11.pawn.Map, ThingPlaceMode.Near, null, null, null);
						CS_0024_003C_003E8__locals11.pawn.carryTracker.DestroyCarriedThing();
					}
					else if (PawnUtility.ShouldSendNotificationAbout(CS_0024_003C_003E8__locals11.pawn))
					{
						Messages.Message("MessageFerilizeFailed".Translate(CS_0024_003C_003E8__locals11.pawn.Named("PAWN")) + ": " + "CombinedGenesExceedMetabolismLimits".Translate(), CS_0024_003C_003E8__locals11.pawn, MessageTypeDefOf.NegativeEvent);
					}
				});
				_003C_003E1__state = 4;
				return true;
			case 4:
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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

	private const int FertilizeTicks = 180;

	public const TargetIndex OvumIndex = TargetIndex.A;

	private HumanOvum ovum => (HumanOvum)base.TargetThingA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}
}
