using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_CarryToEntityHolder : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public Thing takee;

		public CompEntityHolder platform;

		public Pawn taker;

		internal void _003CChainTakeeToPlatformToils_003Eb__0()
		{
			Thing thing = takee;
			platform.Container.TryAddOrTransfer(thing, 1);
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			if (compHoldingPlatformTarget != null)
			{
				compHoldingPlatformTarget.Notify_HeldOnPlatform(platform.Container);
				if (compHoldingPlatformTarget.Props.capturedLetterLabel != null)
				{
					Find.LetterStack.ReceiveLetter(compHoldingPlatformTarget.Props.capturedLetterLabel, compHoldingPlatformTarget.Props.capturedLetterText.Formatted(taker.Named("PAWN")), LetterDefOf.NeutralEvent, platform.parent);
				}
			}
		}
	}

	[CompilerGenerated]
	private sealed class _003CChainTakeeToPlatformToils_003Ed__9 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private Thing takee;

		public Thing _003C_003E3__takee;

		private CompEntityHolder platform;

		public CompEntityHolder _003C_003E3__platform;

		private Pawn taker;

		public Pawn _003C_003E3__taker;

		private TargetIndex platformIndex;

		public TargetIndex _003C_003E3__platformIndex;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

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
		public _003CChainTakeeToPlatformToils_003Ed__9(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
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
				_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__1.takee = takee;
				_003C_003E8__1.platform = platform;
				_003C_003E8__1.taker = taker;
				_003C_003E2__current = Toils_Goto.GotoThing(platformIndex, PathEndMode.ClosestTouch);
				_003C_003E1__state = 1;
				return true;
			case 1:
			{
				_003C_003E1__state = -1;
				Toil toil = Toils_General.WaitWith(platformIndex, 300, useProgressBar: true);
				toil.PlaySustainerOrSound(SoundDefOf.ChainToPlatform);
				_003C_003E2__current = toil;
				_003C_003E1__state = 2;
				return true;
			}
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Do(delegate
				{
					Thing thing = _003C_003E8__1.takee;
					_003C_003E8__1.platform.Container.TryAddOrTransfer(thing, 1);
					CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget != null)
					{
						compHoldingPlatformTarget.Notify_HeldOnPlatform(_003C_003E8__1.platform.Container);
						if (compHoldingPlatformTarget.Props.capturedLetterLabel != null)
						{
							Find.LetterStack.ReceiveLetter(compHoldingPlatformTarget.Props.capturedLetterLabel, compHoldingPlatformTarget.Props.capturedLetterText.Formatted(_003C_003E8__1.taker.Named("PAWN")), LetterDefOf.NeutralEvent, _003C_003E8__1.platform.parent);
						}
					}
				});
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
			_003CChainTakeeToPlatformToils_003Ed__9 _003CChainTakeeToPlatformToils_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CChainTakeeToPlatformToils_003Ed__ = this;
			}
			else
			{
				_003CChainTakeeToPlatformToils_003Ed__ = new _003CChainTakeeToPlatformToils_003Ed__9(0);
			}
			_003CChainTakeeToPlatformToils_003Ed__.taker = _003C_003E3__taker;
			_003CChainTakeeToPlatformToils_003Ed__.takee = _003C_003E3__takee;
			_003CChainTakeeToPlatformToils_003Ed__.platform = _003C_003E3__platform;
			_003CChainTakeeToPlatformToils_003Ed__.platformIndex = _003C_003E3__platformIndex;
			return _003CChainTakeeToPlatformToils_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__8 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_CarryToEntityHolder _003C_003E4__this;

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
		public _003CMakeNewToils_003Ed__8(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 4)
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
				JobDriver_CarryToEntityHolder CS_0024_003C_003E8__locals16 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals16.FailOnDestroyedOrNull(TargetIndex.B);
					CS_0024_003C_003E8__locals16.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					CS_0024_003C_003E8__locals16.FailOn(() => !CS_0024_003C_003E8__locals16.DestHolder.Available);
					CS_0024_003C_003E8__locals16.FailOn(() => CS_0024_003C_003E8__locals16.Takee is Pawn pawn2 && !(pawn2.GetComp<CompActivity>()?.IsDormant ?? true));
					CS_0024_003C_003E8__locals16.FailOn(() => CS_0024_003C_003E8__locals16.Takee.TryGetComp<CompHoldingPlatformTarget>().EntityHolder != CS_0024_003C_003E8__locals16.DestHolder);
					if (CS_0024_003C_003E8__locals16.pawn.carryTracker.CarriedThing != CS_0024_003C_003E8__locals16.Takee)
					{
						_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00be;
				case 1:
					_003C_003E1__state = -1;
					goto IL_00be;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = ChainTakeeToPlatformToils(CS_0024_003C_003E8__locals16.pawn, CS_0024_003C_003E8__locals16.Takee, CS_0024_003C_003E8__locals16.DestHolder, TargetIndex.A).GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_015a;
				case 4:
					_003C_003E1__state = -3;
					goto IL_015a;
				case 5:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_015a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = Toils_General.Do(delegate
					{
						if (CS_0024_003C_003E8__locals16.Takee is Pawn pawn && (!pawn.RaceProps.Humanlike || pawn.IsMutant))
						{
							TaleRecorder.RecordTale(TaleDefOf.Captured, CS_0024_003C_003E8__locals16.pawn, pawn);
						}
					});
					_003C_003E1__state = 5;
					return true;
					IL_00be:
					_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
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
			_003CMakeNewToils_003Ed__8 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__8(0)
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

	private const TargetIndex DestHolderIndex = TargetIndex.A;

	private const TargetIndex TakeeIndex = TargetIndex.B;

	private const int EnterDelayTicks = 300;

	private Thing Takee => job.GetTarget(TargetIndex.B).Thing;

	private CompEntityHolder DestHolder => job.GetTarget(TargetIndex.A).Thing.TryGetComp<CompEntityHolder>();

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (pawn.Reserve(Takee, job, 1, -1, null, errorOnFailed))
		{
			return pawn.Reserve(DestHolder.parent, job, 1, -1, null, errorOnFailed);
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__8))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__8(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CChainTakeeToPlatformToils_003Ed__9))]
	public static IEnumerable<Toil> ChainTakeeToPlatformToils(Pawn taker, Thing takee, CompEntityHolder platform, TargetIndex platformIndex)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CChainTakeeToPlatformToils_003Ed__9(-2)
		{
			_003C_003E3__taker = taker,
			_003C_003E3__takee = takee,
			_003C_003E3__platform = platform,
			_003C_003E3__platformIndex = platformIndex
		};
	}
}
