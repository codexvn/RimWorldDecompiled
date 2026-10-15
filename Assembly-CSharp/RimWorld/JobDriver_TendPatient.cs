using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld;

public class JobDriver_TendPatient : JobDriver
{
	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__19 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_TendPatient _003C_003E4__this;

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
				JobDriver_TendPatient CS_0024_003C_003E8__locals61 = _003C_003E4__this;
				switch (num)
				{
				default:
					return false;
				case 0:
					_003C_003E1__state = -1;
					CS_0024_003C_003E8__locals61.FailOnDespawnedNullOrForbidden(TargetIndex.A);
					CS_0024_003C_003E8__locals61.FailOn(delegate
					{
						if (CS_0024_003C_003E8__locals61.MedicineUsed != null && CS_0024_003C_003E8__locals61.pawn.Faction == Faction.OfPlayer && CS_0024_003C_003E8__locals61.Deliveree.playerSettings != null && !CS_0024_003C_003E8__locals61.Deliveree.playerSettings.medCare.AllowsMedicine(CS_0024_003C_003E8__locals61.MedicineUsed.def))
						{
							return true;
						}
						return (CS_0024_003C_003E8__locals61.pawn == CS_0024_003C_003E8__locals61.Deliveree && CS_0024_003C_003E8__locals61.pawn.Faction == Faction.OfPlayer && CS_0024_003C_003E8__locals61.pawn.playerSettings != null && !CS_0024_003C_003E8__locals61.pawn.playerSettings.selfTend) ? true : false;
					});
					CS_0024_003C_003E8__locals61.AddEndCondition(delegate
					{
						if (CS_0024_003C_003E8__locals61.pawn.Faction == Faction.OfPlayer && HealthAIUtility.ShouldBeTendedNowByPlayer(CS_0024_003C_003E8__locals61.Deliveree))
						{
							return JobCondition.Ongoing;
						}
						return ((CS_0024_003C_003E8__locals61.job.playerForced || CS_0024_003C_003E8__locals61.pawn.Faction != Faction.OfPlayer) && CS_0024_003C_003E8__locals61.Deliveree.health.HasHediffsNeedingTend()) ? JobCondition.Ongoing : JobCondition.Succeeded;
					});
					CS_0024_003C_003E8__locals61.FailOnAggroMentalState(TargetIndex.A);
					_003CreserveMedicine_003E5__2 = null;
					_003CgotoToil_003E5__3 = Toils_Goto.GotoThing(TargetIndex.A, CS_0024_003C_003E8__locals61.pathEndMode);
					if (CS_0024_003C_003E8__locals61.usesMedicine)
					{
						List<Toil> list = CollectMedicineToils(CS_0024_003C_003E8__locals61.pawn, CS_0024_003C_003E8__locals61.Deliveree, CS_0024_003C_003E8__locals61.job, _003CgotoToil_003E5__3, out _003CreserveMedicine_003E5__2);
						_003C_003E7__wrap4 = list.GetEnumerator();
						_003C_003E1__state = -3;
						goto IL_0107;
					}
					goto IL_0126;
				case 1:
					_003C_003E1__state = -3;
					goto IL_0107;
				case 2:
				{
					_003C_003E1__state = -1;
					int ticks = (int)(1f / CS_0024_003C_003E8__locals61.pawn.GetStatValue(StatDefOf.MedicalTendSpeed) * 600f);
					if (!CS_0024_003C_003E8__locals61.job.draftedTend || CS_0024_003C_003E8__locals61.pawn == CS_0024_003C_003E8__locals61.TargetPawnA)
					{
						_003CwaitToil_003E5__4 = Toils_General.Wait(ticks);
					}
					else
					{
						_003CwaitToil_003E5__4 = Toils_General.WaitWith(TargetIndex.A, ticks, useProgressBar: false, maintainPosture: true, maintainSleep: false, TargetIndex.A, CS_0024_003C_003E8__locals61.pathEndMode);
						_003CwaitToil_003E5__4.AddFinishAction(delegate
						{
							if (CS_0024_003C_003E8__locals61.Deliveree != null && CS_0024_003C_003E8__locals61.Deliveree != CS_0024_003C_003E8__locals61.pawn && CS_0024_003C_003E8__locals61.Deliveree.CurJob != null && (CS_0024_003C_003E8__locals61.Deliveree.CurJob.def == JobDefOf.Wait || CS_0024_003C_003E8__locals61.Deliveree.CurJob.def == JobDefOf.Wait_MaintainPosture))
							{
								CS_0024_003C_003E8__locals61.Deliveree.jobs.EndCurrentJob(JobCondition.InterruptForced);
							}
						});
					}
					_003CwaitToil_003E5__4.WithProgressBarToilDelay(TargetIndex.A).PlaySustainerOrSound(SoundDefOf.Interact_Tend);
					_003CwaitToil_003E5__4.activeSkill = () => SkillDefOf.Medicine;
					_003CwaitToil_003E5__4.handlingFacing = true;
					_003CwaitToil_003E5__4.tickIntervalAction = delegate(int delta)
					{
						if (CS_0024_003C_003E8__locals61.pawn == CS_0024_003C_003E8__locals61.Deliveree && CS_0024_003C_003E8__locals61.pawn.Faction != Faction.OfPlayer && CS_0024_003C_003E8__locals61.pawn.IsHashIntervalTick(100, delta) && !CS_0024_003C_003E8__locals61.pawn.Position.Fogged(CS_0024_003C_003E8__locals61.pawn.Map))
						{
							FleckMaker.ThrowMetaIcon(CS_0024_003C_003E8__locals61.pawn.Position, CS_0024_003C_003E8__locals61.pawn.Map, FleckDefOf.HealingCross);
						}
						if (CS_0024_003C_003E8__locals61.pawn != CS_0024_003C_003E8__locals61.Deliveree)
						{
							CS_0024_003C_003E8__locals61.pawn.rotationTracker.FaceTarget(CS_0024_003C_003E8__locals61.Deliveree);
						}
					};
					_003CwaitToil_003E5__4.FailOn(() => CS_0024_003C_003E8__locals61.pawn != CS_0024_003C_003E8__locals61.Deliveree && !CS_0024_003C_003E8__locals61.pawn.CanReachImmediate(CS_0024_003C_003E8__locals61.Deliveree.SpawnedParentOrMe, CS_0024_003C_003E8__locals61.pathEndMode));
					_003C_003E2__current = Toils_Jump.JumpIf(_003CwaitToil_003E5__4, () => !CS_0024_003C_003E8__locals61.usesMedicine || !CS_0024_003C_003E8__locals61.IsMedicineInDoctorInventory);
					_003C_003E1__state = 3;
					return true;
				}
				case 3:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Tend.PickupMedicine(TargetIndex.B, CS_0024_003C_003E8__locals61.Deliveree).FailOnDestroyedOrNull(TargetIndex.B);
					_003C_003E1__state = 4;
					return true;
				case 4:
					_003C_003E1__state = -1;
					_003C_003E2__current = _003CwaitToil_003E5__4;
					_003C_003E1__state = 5;
					return true;
				case 5:
					_003C_003E1__state = -1;
					_003C_003E2__current = Toils_Tend.FinalizeTend(CS_0024_003C_003E8__locals61.Deliveree);
					_003C_003E1__state = 6;
					return true;
				case 6:
					_003C_003E1__state = -1;
					if (CS_0024_003C_003E8__locals61.usesMedicine)
					{
						_003C_003E2__current = FindMoreMedicineToil(CS_0024_003C_003E8__locals61.pawn, CS_0024_003C_003E8__locals61.Deliveree, TargetIndex.B, CS_0024_003C_003E8__locals61.job, _003CreserveMedicine_003E5__2);
						_003C_003E1__state = 7;
						return true;
					}
					goto IL_0327;
				case 7:
					_003C_003E1__state = -1;
					goto IL_0327;
				case 8:
					{
						_003C_003E1__state = -1;
						return false;
					}
					IL_0126:
					_003C_003E2__current = _003CgotoToil_003E5__3;
					_003C_003E1__state = 2;
					return true;
					IL_0327:
					_003C_003E2__current = Toils_Jump.Jump(_003CgotoToil_003E5__3);
					_003C_003E1__state = 8;
					return true;
					IL_0107:
					if (_003C_003E7__wrap4.MoveNext())
					{
						Toil current = _003C_003E7__wrap4.Current;
						_003C_003E2__current = current;
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Em__Finally1();
					_003C_003E7__wrap4 = default(List<Toil>.Enumerator);
					goto IL_0126;
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

	private bool usesMedicine;

	private PathEndMode pathEndMode;

	public const int BaseTendDuration = 600;

	private const int TicksBetweenSelfTendMotes = 100;

	private const TargetIndex MedicineIndex = TargetIndex.B;

	private const TargetIndex MedicineHolderIndex = TargetIndex.C;

	private static List<Toil> tmpCollectToils = new List<Toil>();

	protected Thing MedicineUsed => job.targetB.Thing;

	protected Pawn Deliveree => job.targetA.Pawn;

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

	protected Pawn_InventoryTracker MedicineHolderInventory => MedicineUsed?.ParentHolder as Pawn_InventoryTracker;

	protected Pawn OtherPawnMedicineHolder => job.targetC.Pawn;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe_Values.Look(ref usesMedicine, "usesMedicine", defaultValue: false);
		Scribe_Values.Look(ref pathEndMode, "pathEndMode", PathEndMode.None);
	}

	public override void Notify_Starting()
	{
		base.Notify_Starting();
		usesMedicine = MedicineUsed != null;
		if (Deliveree == pawn)
		{
			pathEndMode = PathEndMode.OnCell;
		}
		else if (Deliveree.InBed())
		{
			pathEndMode = PathEndMode.InteractionCell;
		}
		else if (Deliveree != pawn)
		{
			pathEndMode = PathEndMode.ClosestTouch;
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(Deliveree, job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (MedicineUsed != null)
		{
			int num = pawn.Map.reservationManager.CanReserveStack(pawn, MedicineUsed, 10);
			if (num <= 0 || !pawn.Reserve(MedicineUsed, job, 10, Mathf.Min(num, Medicine.GetMedicineCountToFullyHeal(Deliveree)), null, errorOnFailed))
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

	public override void Notify_DamageTaken(DamageInfo dinfo)
	{
		base.Notify_DamageTaken(dinfo);
		if (dinfo.Def.ExternalViolenceFor(pawn) && pawn.Faction != Faction.OfPlayer && pawn == Deliveree)
		{
			pawn.jobs.CheckForJobOverride();
		}
	}

	public static List<Toil> CollectMedicineToils(Pawn doctor, Pawn patient, Job job, Toil gotoToil, out Toil reserveMedicine)
	{
		tmpCollectToils.Clear();
		Thing medicineUsed = job.targetB.Thing;
		Pawn_InventoryTracker medicineHolderInventory = medicineUsed?.ParentHolder as Pawn_InventoryTracker;
		Pawn otherPawnMedicineHolder = job.targetC.Pawn;
		reserveMedicine = Toils_Tend.ReserveMedicine(TargetIndex.B, patient).FailOnDespawnedNullOrForbidden(TargetIndex.B);
		tmpCollectToils.Add(Toils_Jump.JumpIf(gotoToil, () => medicineUsed != null && doctor.inventory.Contains(medicineUsed)));
		Toil toil = Toils_Goto.GotoThing(TargetIndex.C, PathEndMode.Touch).FailOn(() => otherPawnMedicineHolder != medicineHolderInventory?.pawn || otherPawnMedicineHolder.IsForbidden(doctor));
		tmpCollectToils.Add(Toils_Haul.CheckItemCarriedByOtherPawn(medicineUsed, TargetIndex.C, toil));
		tmpCollectToils.Add(reserveMedicine);
		tmpCollectToils.Add(Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.ClosestTouch).FailOnDespawnedNullOrForbidden(TargetIndex.B));
		tmpCollectToils.Add(Toils_Tend.PickupMedicine(TargetIndex.B, patient).FailOnDestroyedOrNull(TargetIndex.B));
		tmpCollectToils.Add(Toils_Haul.CheckForGetOpportunityDuplicate(reserveMedicine, TargetIndex.B, TargetIndex.None, takeFromValidStorage: true));
		tmpCollectToils.Add(Toils_Jump.Jump(gotoToil));
		tmpCollectToils.Add(toil);
		tmpCollectToils.Add(Toils_General.Wait(25).WithProgressBarToilDelay(TargetIndex.C));
		tmpCollectToils.Add(Toils_Haul.TakeFromOtherInventory(medicineUsed, doctor.inventory.innerContainer, medicineHolderInventory?.innerContainer, Medicine.GetMedicineCountToFullyHeal(patient), TargetIndex.B));
		return tmpCollectToils;
	}

	public static Toil FindMoreMedicineToil(Pawn doctor, Pawn patient, TargetIndex medicineIndex, Job job, Toil reserveMedicine)
	{
		Toil toil = ToilMaker.MakeToil("FindMoreMedicineToil");
		toil.initAction = delegate
		{
			if (job.GetTarget(medicineIndex).Thing.DestroyedOrNull())
			{
				Thing thing = HealthAIUtility.FindBestMedicine(doctor, patient);
				if (thing != null)
				{
					job.SetTarget(medicineIndex, thing);
					doctor.jobs.curDriver.JumpToToil(reserveMedicine);
				}
			}
		};
		return toil;
	}
}
