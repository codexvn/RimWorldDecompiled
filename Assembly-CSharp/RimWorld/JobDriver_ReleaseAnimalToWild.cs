using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_ReleaseAnimalToWild : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public Toil toil;

		public JobDriver_ReleaseAnimalToWild _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__6()
		{
			Pawn actor = toil.actor;
			if (!TryFindClosestOutsideCell(actor.Position, actor.Map, TraverseParms.For(actor), actor, out var cell))
			{
				_003C_003E4__this.EndJobWith(JobCondition.Incompletable);
			}
			else
			{
				actor.pather.StartPath(cell, PathEndMode.OnCell);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ReleaseAnimalToWild _003C_003E4__this;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private Toil _003CwaitToil_003E5__2;

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
		public _003CMakeNewToils_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CwaitToil_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ReleaseAnimalToWild CS_0024_003C_003E8__locals14 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals14.FailOn(() => CS_0024_003C_003E8__locals14.Animal.Dead);
				CS_0024_003C_003E8__locals14.FailOn(() => CS_0024_003C_003E8__locals14.Animal.Faction != Faction.OfPlayer);
				CS_0024_003C_003E8__locals14.FailOn(() => CS_0024_003C_003E8__locals14.Animal.InAggroMentalState);
				CS_0024_003C_003E8__locals14.FailOn(() => CS_0024_003C_003E8__locals14.Animal.MapHeld.designationManager.DesignationOn(CS_0024_003C_003E8__locals14.Animal, DesignationDefOf.ReleaseAnimalToWild) == null);
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003CwaitToil_003E5__2 = Toils_General.WaitWith(TargetIndex.A, 180).WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E2__current = Toils_Jump.JumpIf(_003CwaitToil_003E5__2, () => CS_0024_003C_003E8__locals14.Animal.Position.GetRegion(CS_0024_003C_003E8__locals14.Map).District.TouchesMapEdge);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1._003C_003E4__this = CS_0024_003C_003E8__locals14;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E8__1.toil = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.toil.initAction = delegate
				{
					Pawn actor = _003C_003E8__1.toil.actor;
					if (!TryFindClosestOutsideCell(actor.Position, actor.Map, TraverseParms.For(actor), actor, out var cell))
					{
						_003C_003E8__1._003C_003E4__this.EndJobWith(JobCondition.Incompletable);
					}
					else
					{
						actor.pather.StartPath(cell, PathEndMode.OnCell);
					}
				};
				_003C_003E8__1.toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
				_003C_003E2__current = _003C_003E8__1.toil;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.A, null, storageMode: false);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E8__1 = null;
				_003C_003E2__current = _003CwaitToil_003E5__2;
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					ReleaseAnimalToWildUtility.DoReleaseAnimal(CS_0024_003C_003E8__locals14.Animal, CS_0024_003C_003E8__locals14.pawn);
				});
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
			_003CMakeNewToils_003Ed__6 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__6(0)
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

	private const TargetIndex AnimalInd = TargetIndex.A;

	private const int WaitForTicks = 180;

	protected Pawn Animal => (Pawn)job.targetA.Thing;

	public static bool TryFindClosestOutsideCell(IntVec3 root, Map map, TraverseParms traverseParms, Pawn pawn, out IntVec3 cell)
	{
		cell = default(IntVec3);
		if (CellFinder.TryFindClosestRegionWith(root.GetRegion(map), traverseParms, (Region r) => r.District.TouchesMapEdge, 99999, out var result))
		{
			float num = float.PositiveInfinity;
			foreach (IntVec3 cell2 in result.Cells)
			{
				if (cell2.Standable(map) && (pawn == null || !cell2.IsForbidden(pawn)))
				{
					float num2 = cell2.DistanceToSquared(root);
					if (num2 < num)
					{
						num = num2;
						cell = cell2;
					}
				}
			}
			return num != float.PositiveInfinity;
		}
		return false;
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__6))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__6(-2)
		{
			_003C_003E4__this = this
		};
	}
}
