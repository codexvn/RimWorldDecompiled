using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI;

public abstract class JobDriver_BabyPlay : JobDriver
{
	public enum StartingConditions
	{
		Invalid,
		GotoBaby,
		PickupBaby
	}

	[CompilerGenerated]
	private sealed class _003CCreateStartingCondition_003Ed__15 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BabyPlay _003C_003E4__this;

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
		public _003CCreateStartingCondition_003Ed__15(int _003C_003E1__state)
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
				JobDriver_BabyPlay jobDriver_BabyPlay = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					if (jobDriver_BabyPlay.StartingCondition == StartingConditions.PickupBaby)
					{
						_003C_003E7__wrap1 = JobDriver_PickupToHold.Toils(jobDriver_BabyPlay, TargetIndex.A, subtractNumTakenFromJobCount: false).GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_007a;
					}
					if (jobDriver_BabyPlay.StartingCondition == StartingConditions.GotoBaby)
					{
						_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
						_003C_003E1__state = 2;
						return true;
					}
					throw new NotImplementedException(jobDriver_BabyPlay.StartingCondition.ToString());
				case 1:
					_003C_003E1__state = -3;
					goto IL_007a;
				case 2:
					{
						_003C_003E1__state = -1;
						break;
					}
					IL_007a:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					break;
				}
				return false;
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
			_003CCreateStartingCondition_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCreateStartingCondition_003Ed__15(0)
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
	private sealed class _003CMakeNewToils_003Ed__14 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_BabyPlay _003C_003E4__this;

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
			switch (_003C_003E1__state)
			{
			case -3:
			case 1:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
				break;
			case -4:
			case 4:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally2();
				}
				break;
			case -5:
			case 5:
				try
				{
				}
				finally
				{
					_003C_003Em__Finally3();
				}
				break;
			}
			_003C_003E7__wrap1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_BabyPlay CS_0024_003C_003E8__locals15 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
				{
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals15.FailOnDestroyedNullOrForbidden(TargetIndex.A);
					CS_0024_003C_003E8__locals15.FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					CS_0024_003C_003E8__locals15.AddFailCondition(() => !ChildcareUtility.CanSuckle(CS_0024_003C_003E8__locals15.Baby, out var _));
					CS_0024_003C_003E8__locals15.AddFinishAction(delegate
					{
						CS_0024_003C_003E8__locals15.AddPlayThoughtIfAppropriate();
					});
					CS_0024_003C_003E8__locals15.SetFinalizerJob((JobCondition condition) => (!CS_0024_003C_003E8__locals15.finishedSetup) ? null : ChildcareUtility.MakeBringBabyToSafetyJob(CS_0024_003C_003E8__locals15.pawn, CS_0024_003C_003E8__locals15.Baby));
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals15.CreateStartingCondition().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_00c4;
				}
				case 1:
					_003C_003E1__state = -3;
					goto IL_00c4;
				case 2:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_General.DoAtomic(delegate
					{
						CS_0024_003C_003E8__locals15.finishedSetup = true;
					});
					_003C_003E1__state = 3;
					return true;
				case 3:
					_003C_003E1__state = -1;
					_003C_003E7__wrap1 = CS_0024_003C_003E8__locals15.Play().GetEnumerator();
					_003C_003E1__state = -4;
					goto IL_0171;
				case 4:
					_003C_003E1__state = -4;
					goto IL_0171;
				case 5:
					{
						_003C_003E1__state = -5;
						break;
					}
					IL_0171:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 4;
						return true;
					}
					_003C_003Em__Finally2();
					_003C_003E7__wrap1 = null;
					_003C_003E7__wrap1 = JobDriver_PickupToHold.Toils(CS_0024_003C_003E8__locals15).GetEnumerator();
					_003C_003E1__state = -5;
					break;
					IL_00c4:
					if (_003C_003E7__wrap1.MoveNext())
					{
						Toil current2 = _003C_003E7__wrap1.Current;
						_003C_003E2__current = current2;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap1 = null;
					_003C_003E2__current = CS_0024_003C_003E8__locals15.SetPlayPercentage();
					_003C_003E1__state = 2;
					return true;
				}
				if (_003C_003E7__wrap1.MoveNext())
				{
					Toil current3 = _003C_003E7__wrap1.Current;
					_003C_003E2__current = current3;
					_003C_003E1__state = 5;
					return true;
				}
				_003C_003Em__Finally3();
				_003C_003E7__wrap1 = null;
				return false;
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

		private void _003C_003Em__Finally2()
		{
			_003C_003E1__state = -1;
			if (_003C_003E7__wrap1 != null)
			{
				_003C_003E7__wrap1.Dispose();
			}
		}

		private void _003C_003Em__Finally3()
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

	protected const float BabyPlayMemoryNeedIncrease = 0.6f;

	protected float roomPlayGainFactor = -1f;

	protected float initialPlayPercentage = -1f;

	protected bool finishedSetup;

	protected const TargetIndex BabyInd = TargetIndex.A;

	protected Pawn Baby => (Pawn)base.TargetThingA;

	protected abstract StartingConditions StartingCondition { get; }

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed);
	}

	public Toil SetPlayPercentage()
	{
		return Toils_General.Do(delegate
		{
			if (initialPlayPercentage == -1f)
			{
				initialPlayPercentage = Baby.needs.play?.CurLevelPercentage ?? 1f;
			}
		});
	}

	public void AddPlayThoughtIfAppropriate()
	{
		if (!Baby.DestroyedOrNull())
		{
			float num = Baby.needs.play?.CurLevelPercentage ?? (-1f);
			if (initialPlayPercentage >= 0f && num - initialPlayPercentage > 0.6f)
			{
				Baby.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.PlayedWithMe, pawn);
			}
		}
	}

	protected abstract IEnumerable<Toil> Play();

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__14))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__14(-2)
		{
			_003C_003E4__this = this
		};
	}

	[IteratorStateMachine(typeof(_003CCreateStartingCondition_003Ed__15))]
	private IEnumerable<Toil> CreateStartingCondition()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCreateStartingCondition_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref initialPlayPercentage, "initialPlayPercentage", 0f);
		Scribe_Values.Look(ref roomPlayGainFactor, "roomPlayGainFactor", 0f);
		Scribe_Values.Look(ref finishedSetup, "finishedSetup", defaultValue: false);
	}
}
