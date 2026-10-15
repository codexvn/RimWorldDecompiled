using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_CarryGenepackToContainer : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__10 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CarryGenepackToContainer _003C_003E4__this;

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
			JobDriver_CarryGenepackToContainer CS_0024_003C_003E8__locals12 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!ModLister.CheckBiotech("Genepack"))
				{
					return false;
				}
				CS_0024_003C_003E8__locals12.FailOn(delegate
				{
					CompGenepackContainer containerComp2 = CS_0024_003C_003E8__locals12.ContainerComp;
					if (containerComp2 == null || containerComp2.Full)
					{
						return true;
					}
					return (!containerComp2.autoLoad && (!containerComp2.leftToLoad.Contains(CS_0024_003C_003E8__locals12.Genepack) || CS_0024_003C_003E8__locals12.Genepack.targetContainer != CS_0024_003C_003E8__locals12.Container)) ? true : false;
				});
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A, putRemainderInQueue: false, subtractNumTakenFromJobCount: true);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.Goto(TargetIndex.B, PathEndMode.Touch);
				_003C_003E1__state = 3;
				return true;
			case 3:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(30, TargetIndex.B).WithProgressBarToilDelay(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.B);
				toil.handlingFacing = true;
				_003C_003E2__current = toil;
				_003C_003E1__state = 4;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.A, delegate
				{
					CS_0024_003C_003E8__locals12.Genepack.def.soundDrop.PlayOneShot(SoundInfo.InMap(CS_0024_003C_003E8__locals12.Container));
					CS_0024_003C_003E8__locals12.Genepack.targetContainer = null;
					CompGenepackContainer containerComp = CS_0024_003C_003E8__locals12.ContainerComp;
					containerComp.leftToLoad.Remove(CS_0024_003C_003E8__locals12.Genepack);
					MoteMaker.ThrowText(CS_0024_003C_003E8__locals12.Container.DrawPos, CS_0024_003C_003E8__locals12.pawn.Map, "InsertedThing".Translate($"{containerComp.innerContainer.Count} / {containerComp.Props.maxCapacity}"));
				});
				_003C_003E1__state = 5;
				return true;
			case 5:
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

	private const TargetIndex GenepackInd = TargetIndex.A;

	private const TargetIndex ContainerInd = TargetIndex.B;

	private const int InsertTicks = 30;

	private Thing Container => job.GetTarget(TargetIndex.B).Thing;

	private CompGenepackContainer ContainerComp => Container.TryGetComp<CompGenepackContainer>();

	private Genepack Genepack => (Genepack)job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
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
