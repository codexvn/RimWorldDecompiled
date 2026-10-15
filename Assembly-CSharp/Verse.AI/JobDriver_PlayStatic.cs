using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public class JobDriver_PlayStatic : JobDriver_BabyPlay
{
	[CompilerGenerated]
	private sealed class _003CPlay_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_PlayStatic _003C_003E4__this;

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
		public _003CPlay_003Ed__3(int _003C_003E1__state)
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
			JobDriver_PlayStatic CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("Play");
				toil.WithEffect(EffecterDefOf.PlayStatic, TargetIndex.A, null);
				toil.handlingFacing = true;
				toil.tickIntervalAction = delegate(int delta)
				{
					CS_0024_003C_003E8__locals12.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals12.Baby);
					if (CS_0024_003C_003E8__locals12.pawn.IsHashIntervalTick(1250, delta))
					{
						CS_0024_003C_003E8__locals12.pawn.interactions.TryInteractWith(CS_0024_003C_003E8__locals12.Baby, InteractionDefOf.BabyPlay);
					}
					if (CS_0024_003C_003E8__locals12.roomPlayGainFactor < 0f)
					{
						CS_0024_003C_003E8__locals12.roomPlayGainFactor = BabyPlayUtility.GetRoomPlayGainFactors(CS_0024_003C_003E8__locals12.Baby);
					}
					if (BabyPlayUtility.PlayTickCheckEnd(CS_0024_003C_003E8__locals12.Baby, CS_0024_003C_003E8__locals12.pawn, CS_0024_003C_003E8__locals12.roomPlayGainFactor, delta))
					{
						CS_0024_003C_003E8__locals12.pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					}
				};
				toil.defaultCompleteMode = ToilCompleteMode.Never;
				ChildcareUtility.MakeBabyPlayAsLongAsToilIsActive(toil, TargetIndex.A);
				_003C_003E2__current = toil;
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
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CPlay_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPlay_003Ed__3(0)
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

	private const int InteractionIntervalTicks = 1250;

	protected override StartingConditions StartingCondition => StartingConditions.GotoBaby;

	[IteratorStateMachine(typeof(_003CPlay_003Ed__3))]
	protected override IEnumerable<Toil> Play()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPlay_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}
}
