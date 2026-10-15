using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_TendEntity : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__15 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_TendEntity _003C_003E4__this;

		private Toil _003CreserveMedicine_003E5__2;

		private Toil _003CgotoToil_003E5__3;

		private Toil _003CwaitToil_003E5__4;

		private List<Toil>.Enumerator _003C_003E7__wrap4;

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
		public _003CMakeNewToils_003Ed__15(int _003C_003E1__state)
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
			_003CreserveMedicine_003E5__2 = null;
			_003CgotoToil_003E5__3 = null;
			_003CwaitToil_003E5__4 = null;
			_003C_003E7__wrap4 = default(List<Toil>.Enumerator);
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			try
			{
				int num = _003C_003E1__state;
				JobDriver_TendEntity CS_0024_003C_003E8__locals22 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals22.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					CS_0024_003C_003E8__locals22.FailOn(delegate
					{
						Pawn innerPawn = CS_0024_003C_003E8__locals22.InnerPawn;
						if (innerPawn == null || innerPawn.Destroyed)
						{
							return true;
						}
						if (CS_0024_003C_003E8__locals22.MedicineUsed != null && innerPawn.playerSettings != null && !innerPawn.playerSettings.medCare.AllowsMedicine(CS_0024_003C_003E8__locals22.MedicineUsed.def))
						{
							return true;
						}
						CompHoldingPlatformTarget compHoldingPlatformTarget = innerPawn.TryGetComp<CompHoldingPlatformTarget>();
						return (compHoldingPlatformTarget != null && compHoldingPlatformTarget.containmentMode == EntityContainmentMode.Release) ? true : false;
					});
					CS_0024_003C_003E8__locals22.AddEndCondition(delegate
					{
						if (HealthAIUtility.ShouldBeTendedNowByPlayer(CS_0024_003C_003E8__locals22.InnerPawn))
						{
							return JobCondition.Ongoing;
						}
						return (CS_0024_003C_003E8__locals22.job.playerForced && CS_0024_003C_003E8__locals22.InnerPawn.health.HasHediffsNeedingTend()) ? JobCondition.Ongoing : JobCondition.Succeeded;
					});
					_003CreserveMedicine_003E5__2 = null;
					_003CgotoToil_003E5__3 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
					if (CS_0024_003C_003E8__locals22.usesMedicine)
					{
						List<Toil> list = JobDriver_TendPatient.CollectMedicineToils(CS_0024_003C_003E8__locals22.pawn, CS_0024_003C_003E8__locals22.InnerPawn, CS_0024_003C_003E8__locals22.job, _003CgotoToil_003E5__3, out _003CreserveMedicine_003E5__2);
						_003C_003E7__wrap4 = list.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0100;
					}
					goto IL_011f;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0100;
				case 2:
				{
					_003C_003E1__state = -1;
					int ticks = (int)(1f / CS_0024_003C_003E8__locals22.pawn.GetStatValue(StatDefOf.MedicalTendSpeed) * 600f);
					_003CwaitToil_003E5__4 = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: true, maintainPosture: false, maintainSleep: false, TargetIndex.A);
					_003CwaitToil_003E5__4.activeSkill = () => SkillDefOf.Medicine;
					_003CwaitToil_003E5__4.FailOnCannotTouch(TargetIndex.A, PathEndMode.ClosestTouch).WithProgressBarToilDelay(TargetIndex.A).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
					_003C_003E2__current = Toils_Jump.JumpIf(_003CwaitToil_003E5__4, () => !CS_0024_003C_003E8__locals22.usesMedicine || !CS_0024_003C_003E8__locals22.IsMedicineInDoctorInventory);
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Tend.PickupMedicine(TargetIndex.B, CS_0024_003C_003E8__locals22.InnerPawn).FailOnDestroyedOrNull(TargetIndex.B);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CwaitToil_003E5__4;
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Tend.FinalizeTend(CS_0024_003C_003E8__locals22.InnerPawn);
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals22.usesMedicine)
					{
						_003C_003E2__current = JobDriver_TendPatient.FindMoreMedicineToil(CS_0024_003C_003E8__locals22.pawn, CS_0024_003C_003E8__locals22.InnerPawn, TargetIndex.B, CS_0024_003C_003E8__locals22.job, _003CreserveMedicine_003E5__2);
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_02a6;
				case 7:
					_003C_003E1__state = -1;
					goto IL_02a6;
				case 8:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_02a6:
					_003C_003E2__current = Toils_Jump.Jump(_003CgotoToil_003E5__3);
					_003C_003E1__state = 8;
					return true;
					IL_011f:
					_003C_003E2__current = _003CgotoToil_003E5__3;
					_003C_003E1__state = 2;
					return true;
					IL_0100:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Toil current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = default(List<Toil>.Enumerator);
					goto IL_011f;
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
			((IDisposable)_003C_003E7__wrap4/*cast due to .constrained prefix*/).Dispose();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__15(0)
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

	private bool usesMedicine;

	private const TargetIndex PlatformIndex = TargetIndex.A;

	private const TargetIndex MedicineIndex = TargetIndex.B;

	private const int MaxMedicineReservations = 10;

	private Thing Platform => base.TargetThingA;

	private Pawn InnerPawn => (Platform as Building_HoldingPlatform)?.HeldPawn;

	private Thing MedicineUsed => job.targetB.Thing;

	protected bool IsMedicineInDoctorInventory
	{
		get
		{
			if (MedicineUsed != null)
			{
				return pawn.inventory.Contains(MedicineUsed);
			}
			return false;
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref usesMedicine, "usesMedicine", defaultValue: false);
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		usesMedicine = MedicineUsed != null;
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Platform, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (usesMedicine)
		{
			int num = pawn.Map.reservationManager.CanReserveStack(pawn, MedicineUsed, 10);
			if (num <= 0)
			{
				return false;
			}
			int stackCount = Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(InnerPawn));
			if (!pawn.Reserve(MedicineUsed, job, 10, stackCount, null, errorOnFailed))
			{
				return false;
			}
		}
		return true;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__15))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string GetReport()
	{
		return JobUtility.GetResolvedJobReport(job.def.reportString, InnerPawn, job.targetB, job.targetC);
	}
}
