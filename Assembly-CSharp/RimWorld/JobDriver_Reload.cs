using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Utility;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Reload : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass5_0
	{
		public IReloadableComp reloadable;

		public JobDriver_Reload _003C_003E4__this;

		internal bool _003CMakeNewToils_003Eb__0()
		{
			return reloadable == null;
		}

		internal bool _003CMakeNewToils_003Eb__1()
		{
			return ReloadableUtility.OwnerOf(reloadable) != _003C_003E4__this.pawn;
		}

		internal bool _003CMakeNewToils_003Eb__2()
		{
			return !reloadable.NeedsReload(allowForceReload: true);
		}

		internal bool _003CMakeNewToils_003Eb__3()
		{
			return !_003C_003E4__this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty();
		}

		internal void _003CMakeNewToils_003Eb__4()
		{
			Thing carriedThing = _003C_003E4__this.pawn.carryTracker.CarriedThing;
			if (carriedThing != null && !carriedThing.Destroyed)
			{
				_003C_003E4__this.pawn.carryTracker.TryDropCarriedThing(_003C_003E4__this.pawn.Position, ThingPlaceMode.Near, out var _);
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass6_0
	{
		public JobDriver_Reload _003C_003E4__this;

		public IReloadableComp reloadable;

		internal bool _003CReloadAsMuchAsPossible_003Eb__0()
		{
			if (_003C_003E4__this.pawn.carryTracker.CarriedThing != null)
			{
				return _003C_003E4__this.pawn.carryTracker.CarriedThing.stackCount < reloadable.MinAmmoNeeded(allowForcedReload: true);
			}
			return true;
		}

		internal void _003CReloadAsMuchAsPossible_003Eb__1()
		{
			Thing carriedThing = _003C_003E4__this.pawn.carryTracker.CarriedThing;
			reloadable.ReloadFrom(carriedThing);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__5 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Reload _003C_003E4__this;

		private _003C_003Ec__DisplayClass5_0 _003C_003E8__1;

		private Toil _003CgetNextIngredient_003E5__2;

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
		public _003CMakeNewToils_003Ed__5(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			switch (_003C_003E1__state)
			{
			case -3:
			case 2:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 7:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			}
			_003C_003E8__1 = null;
			_003CgetNextIngredient_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_Reload jobDriver_Reload = _003C_003E4__this;
				Toil toil;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					_003C_003E8__1 = new _003C_003Ec__DisplayClass5_0();
					_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
					_003C_003Ec__DisplayClass5_0 _003C_003Ec__DisplayClass5_ = _003C_003E8__1;
					IReloadableComp reloadableComp = jobDriver_Reload.Gear?.TryGetComp<CompApparelReloadable>();
					_003C_003Ec__DisplayClass5_.reloadable = reloadableComp ?? jobDriver_Reload.Gear?.TryGetComp<CompEquippableAbilityReloadable>();
					jobDriver_Reload.FailOn(() => _003C_003E8__1.reloadable == null);
					jobDriver_Reload.FailOn(() => ReloadableUtility.OwnerOf(_003C_003E8__1.reloadable) != _003C_003E8__1._003C_003E4__this.pawn);
					jobDriver_Reload.FailOn(() => !_003C_003E8__1.reloadable.NeedsReload(allowForceReload: true));
					jobDriver_Reload.FailOnDestroyedOrNull(TargetIndex.A);
					jobDriver_Reload.FailOnIncapable(PawnCapacityDefOf.Manipulation);
					_003CgetNextIngredient_003E5__2 = Toils_General.Label();
					_003C_003E2__current = _003CgetNextIngredient_003E5__2;
					_003C_003E1__state = 1;
					return true;
				}
				case 1:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = jobDriver_Reload.ReloadAsMuchAsPossible(_003C_003E8__1.reloadable).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0172;
				case 2:
					_003C_003E1__state = -3;
					goto IL_0172;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B, putRemainderInQueue: false, subtractNumTakenFromJobCount: true).FailOnDestroyedNullOrForbidden(TargetIndex.B);
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Jump.JumpIf(_003CgetNextIngredient_003E5__2, () => !_003C_003E8__1._003C_003E4__this.job.GetTargetQueue(TargetIndex.B).NullOrEmpty());
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					_003C_003E7__wrap2 = jobDriver_Reload.ReloadAsMuchAsPossible(_003C_003E8__1.reloadable).GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_028e;
				case 7:
					_003C_003E1__state = -4;
					goto IL_028e;
				case 8:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_028e:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 7;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap2 = null;
					toil = ToilMaker.MakeToil("MakeNewToils");
					toil.initAction = delegate
					{
						Thing carriedThing = _003C_003E8__1._003C_003E4__this.pawn.carryTracker.CarriedThing;
						if (carriedThing != null && !carriedThing.Destroyed)
						{
							_003C_003E8__1._003C_003E4__this.pawn.carryTracker.TryDropCarriedThing(_003C_003E8__1._003C_003E4__this.pawn.Position, ThingPlaceMode.Near, out var _);
						}
					};
					toil.defaultCompleteMode = ToilCompleteMode.Instant;
					_003C_003E2__current = toil;
					_003C_003E1__state = 8;
					return true;
					IL_0172:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current2 = _003C_003E7__wrap2.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003C_003E2__current = Toils_JobTransforms.ExtractNextTargetFromQueue(TargetIndex.B);
					_003C_003E1__state = 3;
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
			if (_003C_003E7__wrap2 != null)
			{
				_003C_003E7__wrap2.Dispose();
			}
		}

		private void _003C_003Em__Finally2()
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
			_003CMakeNewToils_003Ed__5 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__5(0)
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
	private sealed class _003CReloadAsMuchAsPossible_003Ed__6 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Reload _003C_003E4__this;

		private IReloadableComp reloadable;

		public IReloadableComp _003C_003E3__reloadable;

		private _003C_003Ec__DisplayClass6_0 _003C_003E8__1;

		private Toil _003Cdone_003E5__2;

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
		public _003CReloadAsMuchAsPossible_003Ed__6(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003Cdone_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass6_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				_003C_003E8__1.reloadable = reloadable;
				_003Cdone_003E5__2 = Toils_General.Label();
				_003C_003E2__current = Toils_Jump.JumpIf(_003Cdone_003E5__2, () => _003C_003E8__1._003C_003E4__this.pawn.carryTracker.CarriedThing == null || _003C_003E8__1._003C_003E4__this.pawn.carryTracker.CarriedThing.stackCount < _003C_003E8__1.reloadable.MinAmmoNeeded(allowForcedReload: true));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(_003C_003E8__1.reloadable.BaseReloadTicks).WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E1__state = 2;
				return true;
			case 2:
			{
				_003C_003E1__state = -1;
				Toil toil = ToilMaker.MakeToil("ReloadAsMuchAsPossible");
				toil.initAction = delegate
				{
					Thing carriedThing = _003C_003E8__1._003C_003E4__this.pawn.carryTracker.CarriedThing;
					_003C_003E8__1.reloadable.ReloadFrom(carriedThing);
				};
				toil.defaultCompleteMode = ToilCompleteMode.Instant;
				_003C_003E2__current = toil;
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003Cdone_003E5__2;
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
			_003CReloadAsMuchAsPossible_003Ed__6 _003CReloadAsMuchAsPossible_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CReloadAsMuchAsPossible_003Ed__ = this;
			}
			else
			{
				_003CReloadAsMuchAsPossible_003Ed__ = new _003CReloadAsMuchAsPossible_003Ed__6(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CReloadAsMuchAsPossible_003Ed__.reloadable = _003C_003E3__reloadable;
			return _003CReloadAsMuchAsPossible_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private const TargetIndex GearInd = TargetIndex.A;

	private const TargetIndex AmmoInd = TargetIndex.B;

	private Thing Gear => job.GetTarget(TargetIndex.A).Thing;

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__5))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__5(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CReloadAsMuchAsPossible_003Ed__6))]
	private IEnumerable<Toil> ReloadAsMuchAsPossible(IReloadableComp reloadable)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CReloadAsMuchAsPossible_003Ed__6(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__reloadable = reloadable
		};
	}
}
