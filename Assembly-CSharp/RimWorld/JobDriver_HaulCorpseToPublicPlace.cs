using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_HaulCorpseToPublicPlace : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_HaulCorpseToPublicPlace _003C_003E4__this;

		private Toil _003CgotoCorpse_003E5__2;

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
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgotoCorpse_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_HaulCorpseToPublicPlace jobDriver_HaulCorpseToPublicPlace = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				jobDriver_HaulCorpseToPublicPlace.FailOnDestroyedOrNull(TargetIndex.A);
				_003CgotoCorpse_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
				_003C_003E2__current = Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, _003CgotoCorpse_003E5__2);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell).FailOnDespawnedOrNull(TargetIndex.B);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(jobDriver_HaulCorpseToPublicPlace.Grave?.OpenTicks ?? 60).WithProgressBarToilDelay(TargetIndex.B).FailOnDespawnedOrNull(TargetIndex.B)
					.FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Open(TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.A);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgotoCorpse_003E5__2;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_HaulCorpseToPublicPlace.FindCellToDropCorpseToil();
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.C);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.Touch);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, storageMode: false);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_HaulCorpseToPublicPlace.ForbidAndNotifyMentalStateToil();
				_003C_003E1__state = 12;
				return true;
			case 12:
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
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
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

	private const TargetIndex CorpseInd = TargetIndex.A;

	private const TargetIndex GraveInd = TargetIndex.B;

	private const TargetIndex CellInd = TargetIndex.C;

	private static List<IntVec3> tmpCells = new List<IntVec3>();

	private Corpse Corpse => (Corpse)job.GetTarget(TargetIndex.A).Thing;

	private Building_Grave Grave => (Building_Grave)job.GetTarget(TargetIndex.B).Thing;

	private bool InGrave => Grave != null;

	private Thing Target => (Thing)(((object)Grave) ?? ((object)Corpse));

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Target, job, 1, -1, null, errorOnFailed);
	}

	public override string GetReport()
	{
		if (InGrave && Grave.def == ThingDefOf.Grave)
		{
			return "ReportDiggingUpCorpse".Translate();
		}
		return base.GetReport();
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil FindCellToDropCorpseToil()
	{
		Toil toil = ToilMaker.MakeToil("FindCellToDropCorpseToil");
		toil.initAction = delegate
		{
			IntVec3 result = IntVec3.Invalid;
			if (!Rand.Chance(0.8f) || !TryFindTableCell(out result))
			{
				bool flag = false;
				if (RCellFinder.TryFindRandomSpotJustOutsideColony(pawn, out var result2) && CellFinder.TryRandomClosewalkCellNear(result2, pawn.Map, 5, out result, (IntVec3 x) => pawn.CanReserve(x) && x.GetFirstItem(pawn.Map) == null))
				{
					flag = true;
				}
				if (!flag)
				{
					result = CellFinder.RandomClosewalkCellNear(pawn.Position, pawn.Map, 10, (IntVec3 x) => pawn.CanReserve(x) && x.GetFirstItem(pawn.Map) == null);
				}
			}
			job.SetTarget(TargetIndex.C, result);
		};
		toil.atomicWithPrevious = true;
		return toil;
	}

	private Toil ForbidAndNotifyMentalStateToil()
	{
		Toil toil = ToilMaker.MakeToil("ForbidAndNotifyMentalStateToil");
		toil.initAction = delegate
		{
			Corpse?.SetForbidden(value: true);
			if (pawn.MentalState is MentalState_CorpseObsession mentalState_CorpseObsession)
			{
				mentalState_CorpseObsession.Notify_CorpseHauled();
			}
		};
		toil.atomicWithPrevious = true;
		return toil;
	}

	private bool TryFindTableCell(out IntVec3 cell)
	{
		tmpCells.Clear();
		List<Building> allBuildingsColonist = pawn.Map.listerBuildings.allBuildingsColonist;
		for (int i = 0; i < allBuildingsColonist.Count; i++)
		{
			Building building = allBuildingsColonist[i];
			if (!building.def.IsTable)
			{
				continue;
			}
			foreach (IntVec3 item in building.OccupiedRect())
			{
				if (pawn.CanReserveAndReach(item, PathEndMode.OnCell, Danger.Deadly) && item.GetFirstItem(pawn.Map) == null)
				{
					tmpCells.Add(item);
				}
			}
		}
		if (!tmpCells.Any())
		{
			cell = IntVec3.Invalid;
			return false;
		}
		cell = tmpCells.RandomElement();
		return true;
	}
}
