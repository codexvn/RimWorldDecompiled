using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Feed : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Feed _003C_003E4__this;

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
			JobDriver_Feed jobDriver_Feed = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				jobDriver_Feed.FailOnDespawnedNullOrForbidden(TargetIndex.B);
				if (jobDriver_Feed.pawn.inventory != null && jobDriver_Feed.pawn.inventory.Contains(jobDriver_Feed.TargetThingA))
				{
					_003C_003E2__current = Toils_Misc.TakeItemFromInventoryToCarrier(jobDriver_Feed.pawn, TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				}
				if (jobDriver_Feed.TargetThingA is Building_NutrientPasteDispenser)
				{
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnForbidden(TargetIndex.A);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnForbidden(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, jobDriver_Feed.pawn);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.PickupIngestible(TargetIndex.A, jobDriver_Feed.Deliveree);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				goto IL_0130;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.ChewIngestible(jobDriver_Feed.Deliveree, 1.5f, TargetIndex.A).FailOnCannotTouch(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.FinalizeIngest(jobDriver_Feed.Deliveree, TargetIndex.A);
				_003C_003E1__state = 8;
				return true;
			case 8:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_0130:
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 6;
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

	private const TargetIndex IngestableInd = TargetIndex.A;

	private const TargetIndex DelivereeInd = TargetIndex.B;

	private const float FeedDurationMultiplier = 1.5f;

	protected Thing Food => job.targetA.Thing;

	protected Pawn Deliveree => (Pawn)job.targetB.Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		return true;
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
