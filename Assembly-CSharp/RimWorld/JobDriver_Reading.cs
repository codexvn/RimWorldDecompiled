using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Reading : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__14 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Reading _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__14(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 1)
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
				JobDriver_Reading CS_0024_003C_003E8__locals11 = _003C_003E4__this;
				int duration;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals11.SetFinalizerJob(delegate(JobCondition condition)
					{
						if (!CS_0024_003C_003E8__locals11.pawn.IsCarryingThing(CS_0024_003C_003E8__locals11.Book))
						{
							return (Job)null;
						}
						if (condition != JobCondition.Succeeded)
						{
							CS_0024_003C_003E8__locals11.pawn.carryTracker.TryDropCarriedThing(CS_0024_003C_003E8__locals11.pawn.Position, ThingPlaceMode.Direct, out var _);
							return (Job)null;
						}
						return HaulAIUtility.HaulToStorageJob(CS_0024_003C_003E8__locals11.pawn, CS_0024_003C_003E8__locals11.Book, forced: false);
					});
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals11.PrepareToReadBook().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0083;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0083;
				case 2:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0083:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					duration = (CS_0024_003C_003E8__locals11.job.playerForced ? 5000 : CS_0024_003C_003E8__locals11.job.def.joyDuration);
					_003C_003E2__current = CS_0024_003C_003E8__locals11.ReadBook(duration);
					_003C_003E1__state = 2;
					return true;
				}
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
			_003CMakeNewToils_003Ed__14 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__14(0)
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

	[CompilerGenerated]
	private sealed class _003CPrepareToReadBook_003Ed__16 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Reading _003C_003E4__this;

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
		public _003CPrepareToReadBook_003Ed__16(int _003C_003E1__state)
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
			JobDriver_Reading jobDriver_Reading = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (jobDriver_Reading.carrying)
				{
					return false;
				}
				if (jobDriver_Reading.hasInInventory)
				{
					_003C_003E2__current = Toils_Misc.TakeItemFromInventoryToCarrier(jobDriver_Reading.pawn, TargetIndex.A);
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = Toils_Goto.GotoCell(jobDriver_Reading.Book.PositionHeld, PathEndMode.ClosestTouch).FailOnDestroyedOrNull(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00bf;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.A, putRemainderInQueue: false, subtractNumTakenFromJobCount: false, failIfStackCountLessThanJobCount: false, reserve: true, canTakeFromInventory: true);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00bf;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_Reading.FindAdjacentReadingSurface();
				_003C_003E1__state = 5;
				return true;
			case 5:
				{
					_003C_003E1__state = -1;
					return false;
				}
				IL_00bf:
				_003C_003E2__current = jobDriver_Reading.CarryToReadingSpot().FailOnDestroyedOrNull(TargetIndex.A);
				_003C_003E1__state = 4;
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
			_003CPrepareToReadBook_003Ed__16 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPrepareToReadBook_003Ed__16(0)
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

	private bool hasInInventory;

	private bool carrying;

	private bool isLearningDesire;

	private bool isReading;

	public const TargetIndex BookIndex = TargetIndex.A;

	public const TargetIndex SurfaceIndex = TargetIndex.B;

	private const int ManualReadTicks = 5000;

	private const int ChairSearchRadius = 32;

	private const int UrgentJobCheckIntervalTicks = 600;

	public Book Book => job.GetTarget(TargetIndex.A).Thing as Book;

	public bool IsReading => isReading;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(Book, job, 1, 1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__14))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		job.count = 1;
		hasInInventory = pawn.inventory != null && pawn.inventory.Contains(Book);
		carrying = pawn?.carryTracker.CarriedThing == Book;
		isLearningDesire = pawn?.learning != null && pawn.learning.ActiveLearningDesires.Contains(LearningDesireDefOf.Reading);
	}

	[IteratorStateMachine(typeof(_003CPrepareToReadBook_003Ed__16))]
	private IEnumerable<Toil> PrepareToReadBook()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPrepareToReadBook_003Ed__16(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil ReadBook(int duration)
	{
		Toil toil = Toils_General.Wait(duration);
		toil.debugName = "Reading";
		toil.FailOnDestroyedNullOrForbidden(TargetIndex.A);
		toil.handlingFacing = true;
		toil.initAction = delegate
		{
			Book.IsOpen = true;
			pawn.pather.StopDead();
			job.showCarryingInspectLine = false;
		};
		toil.tickIntervalAction = delegate(int delta)
		{
			if (job.GetTarget(TargetIndex.B).IsValid)
			{
				pawn.rotationTracker.FaceCell(job.GetTarget(TargetIndex.B).Cell);
			}
			else if (Book.Spawned)
			{
				pawn.rotationTracker.FaceCell(Book.Position);
			}
			else if (pawn.Rotation == Rot4.North)
			{
				pawn.Rotation = new Rot4(Rand.Range(1, 4));
			}
			float readingBonus = BookUtility.GetReadingBonus(pawn);
			isReading = true;
			Book.OnBookReadTick(pawn, delta, readingBonus);
			pawn.skills?.Learn(SkillDefOf.Intellectual, 0.1f * (float)delta);
			pawn.GainComfortFromCellIfPossible(delta);
			if (pawn.CurJob != null && pawn.needs?.joy != null)
			{
				JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.GoToNextToil;
				if (pawn.CurJob.playerForced || pawn.learning != null)
				{
					fullJoyAction = JoyTickFullJoyAction.None;
				}
				JoyUtility.JoyTickCheckEnd(pawn, delta, fullJoyAction, Book.JoyFactor * readingBonus);
			}
			if (isLearningDesire && job != null)
			{
				if (pawn.needs?.learning != null)
				{
					LearningUtility.LearningTickCheckEnd(pawn, delta, job.playerForced);
				}
				else
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
				}
			}
			if (pawn.IsHashIntervalTick(600, delta))
			{
				pawn.jobs.CheckForJobOverride(9.1f);
			}
		};
		toil.AddEndCondition(() => BookUtility.CanReadBook(Book, pawn, out var _) ? JobCondition.Ongoing : JobCondition.InterruptForced);
		toil.AddFinishAction(delegate
		{
			Book.IsOpen = false;
			TaleRecorder.RecordTale(TaleDefOf.ReadBook, pawn, Book);
			JoyUtility.TryGainRecRoomThought(pawn);
		});
		if (isLearningDesire && !job.playerForced)
		{
			toil.defaultCompleteMode = ToilCompleteMode.Never;
		}
		return toil;
	}

	private Toil CarryToReadingSpot()
	{
		Toil toil = ToilMaker.MakeToil("CarryToReadingSpot");
		toil.initAction = delegate
		{
			if (!TryGetClosestChairFreeSittingSpot(skipInteractionCells: true, out var cell) && !TryGetClosestChairFreeSittingSpot(skipInteractionCells: false, out cell))
			{
				cell = RCellFinder.SpotToChewStandingNear(pawn, Book, (IntVec3 c) => !c.Fogged(pawn.Map) && pawn.CanReserveSittableOrSpot(c));
			}
			if (!cell.IsValid)
			{
				pawn.pather.StartPath(pawn.Position, PathEndMode.OnCell);
			}
			else
			{
				pawn.ReserveSittableOrSpot(cell, pawn.CurJob);
				pawn.Map.pawnDestinationReservationManager.Reserve(pawn, pawn.CurJob, cell);
				pawn.pather.StartPath(cell, PathEndMode.OnCell);
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
		return toil;
	}

	private bool TryGetClosestChairFreeSittingSpot(bool skipInteractionCells, out IntVec3 cell)
	{
		Thing thing = GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.BuildingArtificial), PathEndMode.OnCell, TraverseParms.For(pawn), 32f, (Thing t) => ValidateChair(t, pawn, skipInteractionCells) && t.Position.GetDangerFor(pawn, t.Map) == Danger.None);
		if (thing != null)
		{
			return TryFindFreeSittingSpotOnThing(thing, pawn, skipInteractionCells, out cell);
		}
		cell = IntVec3.Invalid;
		return false;
	}

	private Toil FindAdjacentReadingSurface()
	{
		Toil toil = ToilMaker.MakeToil("FindAdjacentReadingSurface");
		toil.initAction = delegate
		{
			Map map = pawn.Map;
			IntVec3 position = pawn.Position;
			Building firstThing = pawn.Position.GetFirstThing<Building>(pawn.Map);
			if (firstThing != null && firstThing.def.building != null && firstThing.def.building.isSittable)
			{
				if (!TryFaceClosestSurface(position, map))
				{
					job.SetTarget(TargetIndex.B, position + firstThing.Rotation.FacingCell);
					pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
				}
			}
			else
			{
				TryFaceClosestSurface(position, map);
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		return toil;
	}

	private bool TryFaceClosestSurface(IntVec3 pos, Map map)
	{
		for (int i = 0; i < 4; i++)
		{
			IntVec3 intVec = pos + new Rot4(i).FacingCell;
			if (intVec.GetSurfaceType(map) == SurfaceType.Eat)
			{
				job.SetTarget(TargetIndex.B, intVec);
				pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
				return true;
			}
		}
		for (int j = 0; j < 4; j++)
		{
			IntVec3 intVec2 = pos + new Rot4(j).FacingCell;
			if (intVec2.GetSurfaceType(map) == SurfaceType.Item)
			{
				job.SetTarget(TargetIndex.B, intVec2);
				pawn.jobs.curDriver.rotateToFace = TargetIndex.B;
				return true;
			}
		}
		return false;
	}

	private static bool ValidateChair(Thing t, Pawn pawn, bool skipInteractionCells)
	{
		if (t.def.building == null || !t.def.building.isSittable)
		{
			return false;
		}
		if (!TryFindFreeSittingSpotOnThing(t, pawn, skipInteractionCells, out var _))
		{
			return false;
		}
		if (t.Fogged())
		{
			return false;
		}
		if (t.IsForbidden(pawn))
		{
			return false;
		}
		if (!pawn.CanReserve(t))
		{
			return false;
		}
		if (!t.IsSociallyProper(pawn))
		{
			return false;
		}
		if (t.IsBurning())
		{
			return false;
		}
		if (t.HostileTo(pawn))
		{
			return false;
		}
		return true;
	}

	private static bool TryFindFreeSittingSpotOnThing(Thing t, Pawn pawn, bool skipInteractionCells, out IntVec3 cell)
	{
		foreach (IntVec3 item in t.OccupiedRect())
		{
			if ((!skipInteractionCells || !item.IsBuildingInteractionCell(pawn.Map)) && !item.Fogged(pawn.Map) && pawn.CanReserveSittableOrSpot(item))
			{
				cell = item;
				return true;
			}
		}
		cell = default(IntVec3);
		return false;
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref carrying, "carrying", defaultValue: false);
		Scribe_Values.Look(ref hasInInventory, "hasInInventory", defaultValue: false);
		Scribe_Values.Look(ref isLearningDesire, "wasLearningDesire", defaultValue: false);
	}
}
