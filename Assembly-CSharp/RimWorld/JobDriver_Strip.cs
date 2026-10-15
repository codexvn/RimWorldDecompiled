using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Strip : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__2 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Strip _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__2(int _003C_003E1__state)
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
			JobDriver_Strip CS_0024_003C_003E8__locals9 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals9.FailOnDespawnedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals9.FailOnAggroMentalState(TargetIndex.A);
				CS_0024_003C_003E8__locals9.FailOn(() => !StrippableUtility.CanBeStrippedByColony(CS_0024_003C_003E8__locals9.TargetThingA));
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = delegate
				{
					CS_0024_003C_003E8__locals9.pawn.pather.StartPath(CS_0024_003C_003E8__locals9.TargetThingA, PathEndMode.ClosestTouch);
				};
				toil2.defaultCompleteMode = ToilCompleteMode.PatherArrival;
				toil2.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E2__current = toil2;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(60).WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate
				{
					Thing thing = CS_0024_003C_003E8__locals9.job.targetA.Thing;
					CS_0024_003C_003E8__locals9.Map.designationManager.DesignationOn(thing, DesignationDefOf.Strip)?.Delete();
					if (thing is IStrippable strippable)
					{
						strippable.Strip();
					}
					CS_0024_003C_003E8__locals9.pawn.records.Increment(RecordDefOf.BodiesStripped);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
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
			_003CMakeNewToils_003Ed__2 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__2(0)
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

	private const int StripTicks = 60;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__2))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__2(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override object[] TaleParameters()
	{
		Corpse corpse = base.TargetA.Thing as Corpse;
		return new object[2]
		{
			pawn,
			(corpse != null) ? corpse.InnerPawn : base.TargetA.Thing
		};
	}
}
