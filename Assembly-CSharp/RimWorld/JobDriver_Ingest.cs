using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Ingest : JobDriver, IEatingDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass24_0
	{
		public JobDriver_Ingest _003C_003E4__this;

		public Toil reserveExtraFoodToCollect;

		public Predicate<Thing> _003C_003E9__2;

		internal void _003CTakeExtraIngestibles_003Eb__0()
		{
			if (_003C_003E4__this.pawn.inventory.innerContainer.TotalStackCountOfDef(_003C_003E4__this.IngestibleSource.def) < _003C_003E4__this.job.takeExtraIngestibles)
			{
				Thing thing = GenClosest.ClosestThingReachable(_003C_003E4__this.pawn.Position, _003C_003E4__this.pawn.Map, ThingRequest.ForDef(_003C_003E4__this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(_003C_003E4__this.pawn), 30f, (Thing x) => _003C_003E4__this.pawn.CanReserve(x, 10, 1) && !x.IsForbidden(_003C_003E4__this.pawn) && x.IsSociallyProper(_003C_003E4__this.pawn));
				if (thing != null)
				{
					_003C_003E4__this.job.SetTarget(TargetIndex.C, thing);
					_003C_003E4__this.JumpToToil(reserveExtraFoodToCollect);
				}
			}
		}

		internal bool _003CTakeExtraIngestibles_003Eb__2(Thing x)
		{
			if (_003C_003E4__this.pawn.CanReserve(x, 10, 1) && !x.IsForbidden(_003C_003E4__this.pawn))
			{
				return x.IsSociallyProper(_003C_003E4__this.pawn);
			}
			return false;
		}

		internal int _003CTakeExtraIngestibles_003Eb__1()
		{
			return _003C_003E4__this.job.takeExtraIngestibles - _003C_003E4__this.pawn.inventory.innerContainer.TotalStackCountOfDef(_003C_003E4__this.IngestibleSource.def);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__19 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Ingest _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__19(int _003C_003E1__state)
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
				JobDriver_Ingest CS_0024_003C_003E8__locals18 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (!CS_0024_003C_003E8__locals18.usingNutrientPasteDispenser)
					{
						CS_0024_003C_003E8__locals18.FailOn(() => !CS_0024_003C_003E8__locals18.IngestibleSource.Destroyed && !CS_0024_003C_003E8__locals18.IngestibleSource.IngestibleNow);
					}
					CS_0024_003C_003E8__locals18.chewing = Toils_Ingest.ChewIngestible(CS_0024_003C_003E8__locals18.pawn, CS_0024_003C_003E8__locals18.ChewDurationMultiplier, TargetIndex.A, TargetIndex.B).FailOn((Toil x) => !CS_0024_003C_003E8__locals18.IngestibleSource.Spawned && (CS_0024_003C_003E8__locals18.pawn.carryTracker == null || CS_0024_003C_003E8__locals18.pawn.carryTracker.CarriedThing != CS_0024_003C_003E8__locals18.IngestibleSource)).FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals18.PrepareToIngestToils(CS_0024_003C_003E8__locals18.chewing).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00cc;
				case 1:
					_003C_003E1__state = -3;
					goto IL_00cc;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Ingest.FinalizeIngest(CS_0024_003C_003E8__locals18.pawn, TargetIndex.A);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Jump.JumpIf(CS_0024_003C_003E8__locals18.chewing, () => CS_0024_003C_003E8__locals18.job.GetTarget(TargetIndex.A).Thing is Corpse && CS_0024_003C_003E8__locals18.pawn.needs.food.CurLevelPercentage < 0.9f);
					_003C_003E1__state = 4;
					return true;
				case 4:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00cc:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = CS_0024_003C_003E8__locals18.chewing;
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
			_003CMakeNewToils_003Ed__19 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__19(0)
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
	private sealed class _003CPrepareToIngestToils_Dispenser_003Ed__21 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Ingest _003C_003E4__this;

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
		public _003CPrepareToIngestToils_Dispenser_003Ed__21(int _003C_003E1__state)
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
			JobDriver_Ingest jobDriver_Ingest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.TakeMealFromDispenser(TargetIndex.A, jobDriver_Ingest.pawn);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.CarryIngestibleToChewSpot(jobDriver_Ingest.pawn, TargetIndex.A).FailOnDestroyedNullOrForbidden(TargetIndex.A);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
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
			_003CPrepareToIngestToils_Dispenser_003Ed__21 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPrepareToIngestToils_Dispenser_003Ed__21(0)
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
	private sealed class _003CPrepareToIngestToils_NonToolUser_003Ed__23 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Ingest _003C_003E4__this;

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
		public _003CPrepareToIngestToils_NonToolUser_003Ed__23(int _003C_003E1__state)
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
			JobDriver_Ingest jobDriver_Ingest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E2__current = jobDriver_Ingest.ReserveFood();
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 2;
				return true;
			case 2:
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
			_003CPrepareToIngestToils_NonToolUser_003Ed__23 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CPrepareToIngestToils_NonToolUser_003Ed__23(0)
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
	private sealed class _003CPrepareToIngestToils_ToolUser_003Ed__22 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Ingest _003C_003E4__this;

		private Toil chewToil;

		public Toil _003C_003E3__chewToil;

		private Toil _003CgotoToPickup_003E5__2;

		private IEnumerator<Toil> _003C_003E7__wrap2;

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
		public _003CPrepareToIngestToils_ToolUser_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 8)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CgotoToPickup_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_Ingest CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals9.eatingFromInventory)
					{
						_003C_003E2__current = Toils_Misc.TakeItemFromInventoryToCarrier(CS_0024_003C_003E8__locals9.pawn, TargetIndex.A);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003E2__current = CS_0024_003C_003E8__locals9.ReserveFood();
					_003C_003E1__state = 2;
					return true;
				case 1:
					_003C_003E1__state = -1;
					goto IL_0187;
				case 2:
					_003C_003E1__state = -1;
					_003CgotoToPickup_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					_003C_003E2__current = Toils_Jump.JumpIf(_003CgotoToPickup_003E5__2, () => CS_0024_003C_003E8__locals9.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedNullOrForbidden(TargetIndex.A);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Jump.Jump(chewToil);
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CgotoToPickup_003E5__2;
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Ingest.PickupIngestible(TargetIndex.A, CS_0024_003C_003E8__locals9.pawn);
					_003C_003E1__state = 7;
					return true;
				case 7:
					_003C_003E1__state = -1;
					_003CgotoToPickup_003E5__2 = null;
					goto IL_0187;
				case 8:
					_003C_003E1__state = -3;
					goto IL_01d9;
				case 9:
					_003C_003E1__state = -1;
					goto IL_022b;
				case 10:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0187:
					if (CS_0024_003C_003E8__locals9.job.takeExtraIngestibles > 0)
					{
						_003C_003E7__wrap2 = CS_0024_003C_003E8__locals9.TakeExtraIngestibles().GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_01d9;
					}
					goto IL_01f3;
					IL_01d9:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 8;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					goto IL_01f3;
					IL_022b:
					_003C_003E2__current = Toils_Ingest.FindAdjacentEatSurface(TargetIndex.B, TargetIndex.A);
					_003C_003E1__state = 10;
					return true;
					IL_01f3:
					if (!CS_0024_003C_003E8__locals9.pawn.Drafted)
					{
						_003C_003E2__current = Toils_Ingest.CarryIngestibleToChewSpot(CS_0024_003C_003E8__locals9.pawn, TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
						_003C_003E1__state = 9;
						return true;
					}
					goto IL_022b;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
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
			_003CPrepareToIngestToils_ToolUser_003Ed__22 _003CPrepareToIngestToils_ToolUser_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CPrepareToIngestToils_ToolUser_003Ed__ = this;
			}
			else
			{
				_003CPrepareToIngestToils_ToolUser_003Ed__ = new _003CPrepareToIngestToils_ToolUser_003Ed__22(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CPrepareToIngestToils_ToolUser_003Ed__.chewToil = _003C_003E3__chewToil;
			return _003CPrepareToIngestToils_ToolUser_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CTakeExtraIngestibles_003Ed__24 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Ingest _003C_003E4__this;

		private _003C_003Ec__DisplayClass24_0 _003C_003E8__1;

		private Toil _003CfindExtraFoodToCollect_003E5__2;

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
		public _003CTakeExtraIngestibles_003Ed__24(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003CfindExtraFoodToCollect_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_Ingest jobDriver_Ingest = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass24_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				if (!jobDriver_Ingest.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return false;
				}
				_003C_003E8__1.reserveExtraFoodToCollect = Toils_Ingest.ReserveFoodFromStackForIngesting(TargetIndex.C);
				_003CfindExtraFoodToCollect_003E5__2 = ToilMaker.MakeToil("TakeExtraIngestibles");
				_003CfindExtraFoodToCollect_003E5__2.initAction = delegate
				{
					if (_003C_003E8__1._003C_003E4__this.pawn.inventory.innerContainer.TotalStackCountOfDef(_003C_003E8__1._003C_003E4__this.IngestibleSource.def) < _003C_003E8__1._003C_003E4__this.job.takeExtraIngestibles)
					{
						Thing thing = GenClosest.ClosestThingReachable(_003C_003E8__1._003C_003E4__this.pawn.Position, _003C_003E8__1._003C_003E4__this.pawn.Map, ThingRequest.ForDef(_003C_003E8__1._003C_003E4__this.IngestibleSource.def), PathEndMode.Touch, TraverseParms.For(_003C_003E8__1._003C_003E4__this.pawn), 30f, (Thing x) => _003C_003E8__1._003C_003E4__this.pawn.CanReserve(x, 10, 1) && !x.IsForbidden(_003C_003E8__1._003C_003E4__this.pawn) && x.IsSociallyProper(_003C_003E8__1._003C_003E4__this.pawn));
						if (thing != null)
						{
							_003C_003E8__1._003C_003E4__this.job.SetTarget(TargetIndex.C, thing);
							_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.reserveExtraFoodToCollect);
						}
					}
				};
				_003CfindExtraFoodToCollect_003E5__2.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = Toils_Jump.Jump(_003CfindExtraFoodToCollect_003E5__2);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003C_003E8__1.reserveExtraFoodToCollect;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.TakeToInventory(TargetIndex.C, () => _003C_003E8__1._003C_003E4__this.job.takeExtraIngestibles - _003C_003E8__1._003C_003E4__this.pawn.inventory.innerContainer.TotalStackCountOfDef(_003C_003E8__1._003C_003E4__this.IngestibleSource.def));
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CfindExtraFoodToCollect_003E5__2;
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
			_003CTakeExtraIngestibles_003Ed__24 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CTakeExtraIngestibles_003Ed__24(0)
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

	private bool usingNutrientPasteDispenser;

	private bool eatingFromInventory;

	private Toil chewing;

	public const float EatCorpseBodyPartsUntilFoodLevelPct = 0.9f;

	public const TargetIndex IngestibleSourceInd = TargetIndex.A;

	private const TargetIndex TableCellInd = TargetIndex.B;

	private const TargetIndex ExtraIngestiblesToCollectInd = TargetIndex.C;

	private Thing IngestibleSource => job.GetTarget(TargetIndex.A).Thing;

	public bool EatingFromInventory => eatingFromInventory;

	public bool GainingNutritionNow
	{
		get
		{
			Thing ingestibleSource = IngestibleSource;
			if (ingestibleSource.DestroyedOrNull() || !ingestibleSource.def.IsNutritionGivingIngestible)
			{
				return false;
			}
			return base.CurToil == chewing;
		}
	}

	private float ChewDurationMultiplier
	{
		get
		{
			Thing ingestibleSource = IngestibleSource;
			if (ingestibleSource.def.ingestible != null && !ingestibleSource.def.ingestible.useEatingSpeedStat)
			{
				return 1f;
			}
			return 1f / pawn.GetStatValue(StatDefOf.EatingSpeed);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref usingNutrientPasteDispenser, "usingNutrientPasteDispenser", defaultValue: false);
		Scribe_Values.Look(ref eatingFromInventory, "eatingFromInventory", defaultValue: false);
	}

	public override string GetReport()
	{
		if (usingNutrientPasteDispenser)
		{
			return JobUtility.GetResolvedJobReportRaw(job.def.reportString, ThingDefOf.MealNutrientPaste.label, ThingDefOf.MealNutrientPaste, "", "", "", "");
		}
		Thing thing = job.targetA.Thing;
		if (thing?.def.ingestible != null)
		{
			if (!thing.def.ingestible.ingestReportStringEat.NullOrEmpty() && (thing.def.ingestible.ingestReportString.NullOrEmpty() || (int)pawn.RaceProps.intelligence < 1))
			{
				return thing.def.ingestible.ingestReportStringEat.Formatted(job.targetA.Thing.LabelShort, job.targetA.Thing);
			}
			if (!thing.def.ingestible.ingestReportString.NullOrEmpty())
			{
				return thing.def.ingestible.ingestReportString.Formatted(job.targetA.Thing.LabelShort, job.targetA.Thing);
			}
		}
		return base.GetReport();
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		usingNutrientPasteDispenser = IngestibleSource is Building_NutrientPasteDispenser;
		eatingFromInventory = pawn.inventory != null && pawn.inventory.Contains(IngestibleSource);
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Faction != null && !(IngestibleSource is Building_NutrientPasteDispenser))
		{
			Thing ingestibleSource = IngestibleSource;
			int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(ingestibleSource, pawn, job.count);
			if (!pawn.Reserve(ingestibleSource, job, 10, maxAmountToPickup, null, errorOnFailed))
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__19))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__19(-2)
		{
			_003C_003E4__this = this
		};
	}

	private IEnumerable<Toil> PrepareToIngestToils(Toil chewToil)
	{
		if (usingNutrientPasteDispenser)
		{
			return PrepareToIngestToils_Dispenser();
		}
		if (pawn.RaceProps.ToolUser && (!pawn.IsMutant || !pawn.mutant.Def.disableEatingAtTable))
		{
			return PrepareToIngestToils_ToolUser(chewToil);
		}
		return PrepareToIngestToils_NonToolUser();
	}

	[IteratorStateMachine(typeof(_003CPrepareToIngestToils_Dispenser_003Ed__21))]
	private IEnumerable<Toil> PrepareToIngestToils_Dispenser()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPrepareToIngestToils_Dispenser_003Ed__21(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CPrepareToIngestToils_ToolUser_003Ed__22))]
	private IEnumerable<Toil> PrepareToIngestToils_ToolUser(Toil chewToil)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPrepareToIngestToils_ToolUser_003Ed__22(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__chewToil = chewToil
		};
	}

	[IteratorStateMachine(typeof(_003CPrepareToIngestToils_NonToolUser_003Ed__23))]
	private IEnumerable<Toil> PrepareToIngestToils_NonToolUser()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CPrepareToIngestToils_NonToolUser_003Ed__23(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CTakeExtraIngestibles_003Ed__24))]
	private IEnumerable<Toil> TakeExtraIngestibles()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CTakeExtraIngestibles_003Ed__24(-2)
		{
			_003C_003E4__this = this
		};
	}

	private Toil ReserveFood()
	{
		Toil toil = ToilMaker.MakeToil("ReserveFood");
		toil.initAction = delegate
		{
			if (pawn.Faction != null)
			{
				Thing thing = job.GetTarget(TargetIndex.A).Thing;
				if (pawn.carryTracker.CarriedThing != thing)
				{
					int maxAmountToPickup = FoodUtility.GetMaxAmountToPickup(thing, pawn, job.count);
					if (maxAmountToPickup != 0)
					{
						if (!pawn.Reserve(thing, job, 10, maxAmountToPickup))
						{
							Log.Error("Pawn food reservation for " + pawn?.ToString() + " on job " + this?.ToString() + " failed, because it could not register food from " + thing?.ToString() + " - amount: " + maxAmountToPickup);
							pawn.jobs.EndCurrentJob(JobCondition.Errored);
						}
						job.count = maxAmountToPickup;
					}
				}
			}
		};
		toil.defaultCompleteMode = ToilCompleteMode.Instant;
		toil.atomicWithPrevious = true;
		return toil;
	}

	public override bool ModifyCarriedThingDrawPos(ref Vector3 drawPos, ref bool flip)
	{
		IntVec3 cell = job.GetTarget(TargetIndex.B).Cell;
		return ModifyCarriedThingDrawPosWorker(ref drawPos, ref flip, cell, pawn);
	}

	public static bool ModifyCarriedThingDrawPosWorker(ref Vector3 drawPos, ref bool flip, IntVec3 placeCell, Pawn pawn)
	{
		if (pawn.pather.Moving)
		{
			return false;
		}
		Thing carriedThing = pawn.carryTracker.CarriedThing;
		if (carriedThing == null || !carriedThing.IngestibleNow)
		{
			return false;
		}
		if (placeCell.IsValid && placeCell.AdjacentToCardinal(pawn.Position) && placeCell.HasEatSurface(pawn.Map) && carriedThing.def.ingestible.ingestHoldUsesTable)
		{
			drawPos = new Vector3((float)placeCell.x + 0.5f, drawPos.y, (float)placeCell.z + 0.5f);
			return true;
		}
		HoldOffset holdOffset = carriedThing.def.ingestible.ingestHoldOffsetStanding?.Pick(pawn.Rotation);
		if (holdOffset != null)
		{
			drawPos += holdOffset.offset;
			flip = holdOffset.flip;
			return true;
		}
		return false;
	}
}
