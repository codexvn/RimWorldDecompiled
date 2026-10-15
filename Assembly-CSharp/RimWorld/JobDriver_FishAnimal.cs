using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_FishAnimal : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_FishAnimal _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
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
			JobDriver_FishAnimal jobDriver_FishAnimal = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.B, PathEndMode.OnCell);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				int ticks = Mathf.RoundToInt(1500f / jobDriver_FishAnimal.pawn.GetStatValue(StatDefOf.FishingSpeed));
				Toil toil = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: false, maintainPosture: true, maintainSleep: false, TargetIndex.A);
				toil.WithProgressBarToilDelay(TargetIndex.B);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_FishAnimal.CompleteFishingToil();
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
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
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

	private const TargetIndex SpotInd = TargetIndex.A;

	private const TargetIndex StandInd = TargetIndex.B;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil CompleteFishingToil()
	{
		Toil toil = ToilMaker.MakeToil("CompleteFishingToil");
		toil.initAction = delegate
		{
			IntVec3 cell = job.GetTarget(TargetIndex.A).Cell;
			bool rare;
			List<Thing> catchesFor = FishingUtility.GetCatchesFor(pawn, cell, animalFishing: true, out rare);
			if (catchesFor.Any())
			{
				bool flag = false;
				int num = catchesFor.Sum((Thing x) => x.stackCount);
				foreach (Thing item in catchesFor)
				{
					flag |= GenPlace.TryPlaceThing(item, pawn.Position, pawn.Map, ThingPlaceMode.Near, null, null, null);
				}
				if (flag)
				{
					pawn.Map.waterBodyTracker.Notify_Fished(cell, num);
				}
			}
		};
		return toil;
	}
}
