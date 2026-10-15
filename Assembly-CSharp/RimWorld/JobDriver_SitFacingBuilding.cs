using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_SitFacingBuilding : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_SitFacingBuilding _003C_003E4__this;

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
			JobDriver_SitFacingBuilding CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals14.EndOnDespawnedOrNull(CS_0024_003C_003E8__locals14.BuildingIndex);
				_003C_003E2__current = Toils_Goto.Goto(CS_0024_003C_003E8__locals14.SeatIndex, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals14.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals14.Building);
					CS_0024_003C_003E8__locals14.pawn.GainComfortFromCellIfPossible(delta);
					JoyUtility.JoyTickCheckEnd(CS_0024_003C_003E8__locals14.pawn, delta, joySource: CS_0024_003C_003E8__locals14.Building, fullJoyAction: CS_0024_003C_003E8__locals14.job.doUntilGatheringEnded ? JoyTickFullJoyAction.None : JoyTickFullJoyAction.EndJob);
				};
				toil.handlingFacing = true;
				toil.defaultCompleteMode = ToilCompleteMode.Delay;
				toil.defaultDuration = (CS_0024_003C_003E8__locals14.job.doUntilGatheringEnded ? CS_0024_003C_003E8__locals14.job.expiryInterval : CS_0024_003C_003E8__locals14.job.def.joyDuration);
				toil.AddFinishAction(delegate
				{
					JoyUtility.TryGainRecRoomThought(CS_0024_003C_003E8__locals14.pawn);
				});
				CS_0024_003C_003E8__locals14.ModifyPlayToil(toil);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
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

	private readonly TargetIndex BuildingIndex = TargetIndex.A;

	private readonly TargetIndex SeatIndex = TargetIndex.B;

	private Building Building => (Building)base.TargetThingA;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(job.targetA, job, job.def.joyMaxParticipants, 0, null, errorOnFailed))
		{
			return pawn.ReserveSittableOrSpot(job.targetB.Cell, job, errorOnFailed);
		}
		return false;
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

	protected virtual void ModifyPlayToil(Toil toil)
	{
	}

	public override object[] TaleParameters()
	{
		return new object[2]
		{
			pawn,
			base.TargetA.Thing.def
		};
	}
}
