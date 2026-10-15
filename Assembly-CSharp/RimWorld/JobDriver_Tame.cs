using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_Tame : JobDriver_InteractAnimal
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__3 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_Tame _003C_003E4__this;

		private Func<bool> _003CnoLongerDesignated_003E5__2;

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
		public _003CMakeNewToils_003Ed__3(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			int num = _003C_003E1__state;
			if (num == -3 || num == 3)
			{
				try
				{
				}
				finally
				{
					_003C_003Em__Finally1();
				}
			}
			_003CnoLongerDesignated_003E5__2 = null;
			_003C_003E7__wrap2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_Tame CS_0024_003C_003E8__locals9 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					_003CnoLongerDesignated_003E5__2 = () => CS_0024_003C_003E8__locals9.Map.designationManager.DesignationOn(CS_0024_003C_003E8__locals9.Animal, DesignationDefOf.Tame) == null;
					if (CS_0024_003C_003E8__locals9.job.GetTarget(TargetIndex.C).HasThing)
					{
						_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.C).FailOn(_003CnoLongerDesignated_003E5__2);
						_003C_003E1__state = 1;
						return true;
					}
					goto IL_00ce;
				case 1:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Haul.TakeToInventory(TargetIndex.C, CS_0024_003C_003E8__locals9.job.count).FailOn(_003CnoLongerDesignated_003E5__2);
					_003C_003E1__state = 2;
					return true;
				case 2:
					_003C_003E1__state = -1;
					goto IL_00ce;
				case 3:
					_003C_003E1__state = -3;
					goto IL_0122;
				case 4:
				{
					_003C_003E1__state = -1;
					Toil toil = ToilMaker.MakeToil("MakeNewToils");
					toil.initAction = delegate
					{
						if (CS_0024_003C_003E8__locals9.job.GetTarget(TargetIndex.A).Thing is Pawn pawn && AnimalPenUtility.NeedsToBeManagedByRope(pawn) && pawn.Faction == Faction.OfPlayer && AnimalPenUtility.GetCurrentPenOf(pawn, allowUnenclosedPens: false) == null)
						{
							RopingPriority ropingPriority = RopingPriority.Closest;
							string jobFailReason;
							CompAnimalPenMarker penAnimalShouldBeTakenTo = AnimalPenUtility.GetPenAnimalShouldBeTakenTo(CS_0024_003C_003E8__locals9.pawn, pawn, out jobFailReason, forced: false, canInteractWhileSleeping: true, allowUnenclosedPens: true, ignoreSkillRequirements: true, ropingPriority);
							Job job = null;
							if (penAnimalShouldBeTakenTo != null)
							{
								job = WorkGiver_TakeToPen.MakeJob(CS_0024_003C_003E8__locals9.pawn, pawn, penAnimalShouldBeTakenTo, allowUnenclosedPens: true, ropingPriority, out jobFailReason);
							}
							if (job != null)
							{
								CS_0024_003C_003E8__locals9.pawn.jobs.StartJob(job, JobCondition.Succeeded, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, null);
							}
							else
							{
								Messages.Message("MessageTameNoSuitablePens".Translate(pawn.Named("ANIMAL")), pawn, MessageTypeDefOf.NeutralEvent);
							}
						}
					};
					toil.defaultCompleteMode = ToilCompleteMode.Instant;
					_003C_003E2__current = toil;
					_003C_003E1__state = 5;
					return true;
				}
				case 5:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_00ce:
					_003C_003E7__wrap2 = ((JobDriver_InteractAnimal)CS_0024_003C_003E8__locals9).MakeNewToils().GetEnumerator();
					_003C_003E1__state = -3;
					goto IL_0122;
					IL_0122:
					if (_003C_003E7__wrap2.MoveNext())
					{
						Toil current = _003C_003E7__wrap2.Current;
						current.FailOn(_003CnoLongerDesignated_003E5__2);
						_003C_003E2__current = current;
						_003C_003E1__state = 3;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap2 = null;
					_003C_003E2__current = Toils_Interpersonal.TryRecruit(TargetIndex.A);
					_003C_003E1__state = 4;
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

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__3 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__3(0)
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

	private const TargetIndex FoodIndex = TargetIndex.C;

	protected override bool CanInteractNow => !TameUtility.TriedToTameTooRecently(base.Animal);

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__3))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__3(-2)
		{
			_003C_003E4__this = this
		};
	}
}
