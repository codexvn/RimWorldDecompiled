using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ReleaseEntity : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ReleaseEntity _003C_003E4__this;

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
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ReleaseEntity CS_0024_003C_003E8__locals4 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals4.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(() => !CS_0024_003C_003E8__locals4.EntityShouldBeReleased);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 300);
				toil.PlaySustainerOrSound(SoundDefOf.ReleaseFromPlatform);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (CS_0024_003C_003E8__locals4.TargetThingB is Pawn thing)
					{
						CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
						compHoldingPlatformTarget.Escape(initiator: false);
						if (compHoldingPlatformTarget != null)
						{
							compHoldingPlatformTarget.containmentMode = EntityContainmentMode.MaintainOnly;
						}
					}
					CS_0024_003C_003E8__locals4.pawn.MentalState?.Notify_ReleasedTarget();
				});
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

	private const TargetIndex PlatformIndex = TargetIndex.A;

	private const TargetIndex EntityIndex = TargetIndex.B;

	private const int TransferTicks = 300;

	private Thing Platform => base.TargetThingA;

	private Pawn InnerPawn => (Platform as Building_HoldingPlatform)?.HeldPawn;

	private bool EntityShouldBeReleased
	{
		get
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget = InnerPawn?.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null)
			{
				if (compHoldingPlatformTarget.containmentMode != EntityContainmentMode.Release)
				{
					return job.ignoreDesignations;
				}
				return true;
			}
			return false;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Platform, job, 1, -1, null, errorOnFailed);
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
