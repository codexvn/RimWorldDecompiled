using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_EjectFuel : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__7 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_EjectFuel _003C_003E4__this;

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
			JobDriver_EjectFuel CS_0024_003C_003E8__locals8 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
				toil2.initAction = delegate
				{
					if (!CS_0024_003C_003E8__locals8.Refuelable.CanEjectFuel())
					{
						CS_0024_003C_003E8__locals8.Map.designationManager.DesignationOn(CS_0024_003C_003E8__locals8.job.targetA.Thing, DesignationDefOf.EjectFuel)?.Delete();
					}
				};
				_003C_003E2__current = toil2.FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnThingMissingDesignation(TargetIndex.A, DesignationDefOf.EjectFuel).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.Wait(120, TargetIndex.A).WithProgressBarToilDelay(TargetIndex.A).FailOnDespawnedOrNull(TargetIndex.A)
					.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				BuildingProperties building = CS_0024_003C_003E8__locals8.Target.def.building;
				if (building != null && building.openingStartedSound != null)
				{
					toil.PlaySoundAtStart(CS_0024_003C_003E8__locals8.Target.def.building.openingStartedSound);
				}
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					CS_0024_003C_003E8__locals8.Map.designationManager.DesignationOn(CS_0024_003C_003E8__locals8.job.targetA.Thing, DesignationDefOf.EjectFuel)?.Delete();
					CS_0024_003C_003E8__locals8.Refuelable.EjectFuel();
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

	public const int EjectTicks = 120;

	private CompRefuelable compRefuelable;

	private Thing Target => job.targetA.Thing;

	private CompRefuelable Refuelable => compRefuelable ?? (compRefuelable = job.targetA.Thing.TryGetComp<CompRefuelable>());

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
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
