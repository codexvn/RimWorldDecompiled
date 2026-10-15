using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_TransferBetweenEntityHolders : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_TransferBetweenEntityHolders _003C_003E4__this;

		private IEnumerator<Toil> _003C_003E7__wrap1;

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
		public _003CMakeNewToils_003Ed__10(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 5)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_TransferBetweenEntityHolders CS_0024_003C_003E8__locals13 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals13.FailOnDestroyedOrNull(TargetIndex.C);
					CS_0024_003C_003E8__locals13.FailOnDespawnedNullOrForbidden(TargetIndex.B);
					CS_0024_003C_003E8__locals13.FailOn(() => !CS_0024_003C_003E8__locals13.DestHolder.Available);
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_General.Do(delegate
					{
						CS_0024_003C_003E8__locals13.Takee.TryGetComp<CompHoldingPlatformTarget>()?.Notify_ReleasedFromPlatform();
						CS_0024_003C_003E8__locals13.SourceHolder.EjectContents();
						CS_0024_003C_003E8__locals13.pawn.Map.reservationManager.Release(CS_0024_003C_003E8__locals13.job.GetTarget(TargetIndex.A), CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.job);
					}).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.C);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = JobDriver_CarryToEntityHolder.ChainTakeeToPlatformToils(CS_0024_003C_003E8__locals13.pawn, CS_0024_003C_003E8__locals13.Takee, CS_0024_003C_003E8__locals13.DestHolder, TargetIndex.B).GetEnumerator();
					_003C_003E1__state = -3;
					break;
				case 5:
					_003C_003E1__state = -3;
					break;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Toil current = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current;
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003Em__Finally1();
				_003C_003E7__wrap1 = null;
				return false;
			}
			catch
			{
				//try-fault
				((IDisposable)this).Dispose();
				throw;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		private void _003C_003Em__Finally1()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__10 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__10(0)
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

	private const TargetIndex SourceHolderIndex = TargetIndex.A;

	private const TargetIndex DestHolderIndex = TargetIndex.B;

	private const TargetIndex TakeeIndex = TargetIndex.C;

	private Thing Takee => job.GetTarget(TargetIndex.C).Thing;

	private CompEntityHolder SourceHolder => job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();

	private CompEntityHolder DestHolder => job.GetTarget(TargetIndex.B).Thing.TryGetComp<CompEntityHolder>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed) && pawn.Reserve(SourceHolder.parent, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(DestHolder.parent, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__10))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__10(-2)
		{
			_003C_003E4__this = this
		};
	}
}
