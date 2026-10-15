using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_EmptyThingContainer : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EmptyThingContainer _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__7(int _003C_003E1__state)
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
			JobDriver_EmptyThingContainer CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, CS_0024_003C_003E8__locals8.ContainerPathEndMode).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A)
					.FailOn(() => CS_0024_003C_003E8__locals8.job.GetTarget(TargetIndex.A).Thing.TryGetComp(out CompThingContainer comp) && comp.Empty);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.WaitWhileExtractingContents(TargetIndex.A, TargetIndex.B, 120);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					if (CS_0024_003C_003E8__locals8.TargetThingA.TryGetInnerInteractableThingOwner().TryDropAll(CS_0024_003C_003E8__locals8.pawn.Position, CS_0024_003C_003E8__locals8.pawn.Map, ThingPlaceMode.Near))
					{
						CS_0024_003C_003E8__locals8.TargetThingA.TryGetComp<CompThingContainer>()?.Props.dropEffecterDef?.Spawn(CS_0024_003C_003E8__locals8.TargetThingA, CS_0024_003C_003E8__locals8.Map).Cleanup();
					}
				});
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true);
				_003C_003E1__state = 6;
				return true;
			case 6:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_Haul.CarryHauledThingToCell(TargetIndex.C);
				_003C_003E2__current = toil;
				_003C_003E1__state = 7;
				return true;
			}
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, storageMode: true, tryStoreInSameStorageIfSpotCantHoldWholeStack: true);
				_003C_003E1__state = 8;
				return true;
			case 8:
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
			_003CMakeNewToils_003Ed__7 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__7(0)
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

	protected const TargetIndex ContainerInd = TargetIndex.A;

	protected const TargetIndex ContentsInd = TargetIndex.B;

	protected const TargetIndex StoreCellInd = TargetIndex.C;

	private const int OpenTicks = 120;

	protected virtual PathEndMode ContainerPathEndMode
	{
		get
		{
			if (!job.GetTarget(TargetIndex.A).Thing.def.hasInteractionCell)
			{
				return PathEndMode.Touch;
			}
			return PathEndMode.InteractionCell;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		Pawn p = pawn;
		LocalTargetInfo target = job.GetTarget(TargetIndex.A);
		Job obj = job;
		bool errorOnFailed2 = errorOnFailed;
		if (p.Reserve(target, obj, 1, -1, job.def.containerReservationLayer, errorOnFailed2))
		{
			return pawn.Reserve(job.GetTarget(TargetIndex.C), job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__7))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__7(-2)
		{
			_003C_003E4__this = this
		};
	}
}
