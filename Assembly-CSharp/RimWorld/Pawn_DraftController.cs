using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld;

public class Pawn_DraftController : IExposable
{
	[CompilerGenerated]
	private sealed class _003CGetGizmos_003Ed__15 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public Pawn_DraftController _003C_003E4__this;

		Gizmo IEnumerator<Gizmo>.Current
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
		public _003CGetGizmos_003Ed__15(int _003C_003E1__state)
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
			Pawn_DraftController CS_0024_003C_003E8__locals20 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals20.ShowDraftGizmo)
				{
					Command_Toggle command_Toggle = new Command_Toggle
					{
						hotKey = KeyBindingDefOf.Command_ColonistDraft,
						isActive = () => CS_0024_003C_003E8__locals20.Drafted,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals20.Drafted = !CS_0024_003C_003E8__locals20.Drafted;
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
							if (CS_0024_003C_003E8__locals20.Drafted)
							{
								LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
							}
						},
						defaultDesc = "CommandToggleDraftDesc".Translate(),
						icon = TexCommand.Draft,
						turnOnSound = SoundDefOf.DraftOn,
						turnOffSound = SoundDefOf.DraftOff,
						groupKeyIgnoreContent = 81729172,
						defaultLabel = (CS_0024_003C_003E8__locals20.Drafted ? "CommandUndraftLabel" : "CommandDraftLabel").Translate()
					};
					if (CS_0024_003C_003E8__locals20.pawn.Downed)
					{
						command_Toggle.Disable("IsIncapped".Translate(CS_0024_003C_003E8__locals20.pawn.LabelShort, CS_0024_003C_003E8__locals20.pawn));
					}
					if (CS_0024_003C_003E8__locals20.pawn.Deathresting)
					{
						command_Toggle.Disable("IsDeathresting".Translate(CS_0024_003C_003E8__locals20.pawn.Named("PAWN")));
					}
					if (ModsConfig.BiotechActive && CS_0024_003C_003E8__locals20.pawn.IsColonyMech)
					{
						AcceptanceReport acceptanceReport = MechanitorUtility.CanDraftMech(CS_0024_003C_003E8__locals20.pawn);
						if (!acceptanceReport)
						{
							command_Toggle.Disable(acceptanceReport.Reason);
						}
					}
					command_Toggle.tutorTag = ((!CS_0024_003C_003E8__locals20.Drafted) ? "Draft" : "Undraft");
					_003C_003E2__current = command_Toggle;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01a3;
			case 1:
				_003C_003E1__state = -1;
				goto IL_01a3;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01a3:
				if (CS_0024_003C_003E8__locals20.Drafted && CS_0024_003C_003E8__locals20.pawn.equipment.Primary != null && CS_0024_003C_003E8__locals20.pawn.equipment.Primary.def.IsRangedWeapon)
				{
					_003C_003E2__current = new Command_Toggle
					{
						hotKey = KeyBindingDefOf.Misc6,
						isActive = () => CS_0024_003C_003E8__locals20.FireAtWill,
						toggleAction = delegate
						{
							CS_0024_003C_003E8__locals20.FireAtWill = !CS_0024_003C_003E8__locals20.FireAtWill;
						},
						icon = TexCommand.FireAtWill,
						defaultLabel = "CommandFireAtWillLabel".Translate(),
						defaultDesc = "CommandFireAtWillDesc".Translate(),
						tutorTag = "FireAtWillToggle"
					};
					_003C_003E1__state = 2;
					return true;
				}
				break;
			}
			return false;
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
		IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
		{
			_003CGetGizmos_003Ed__15 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CGetGizmos_003Ed__15(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Gizmo>)this).GetEnumerator();
		}
	}

	public Pawn pawn;

	private bool draftedInt;

	private bool fireAtWillInt = true;

	private AutoUndrafter autoUndrafter;

	public bool Drafted
	{
		get
		{
			return draftedInt;
		}
		set
		{
			if (value == draftedInt)
			{
				return;
			}
			pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
			fireAtWillInt = true;
			draftedInt = value;
			if (!value && pawn.Spawned)
			{
				pawn.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(pawn);
			}
			pawn.jobs.ClearQueuedJobs();
			if (pawn.jobs.curJob != null && pawn.jobs.IsCurrentJobPlayerInterruptible())
			{
				if (pawn.jobs.curDriver is JobDriver_Ingest { EatingFromInventory: not false })
				{
					pawn.inventory.innerContainer.TryAddRangeOrTransfer(pawn.carryTracker.innerContainer);
				}
				ChildcareUtility.BreastfeedFailReason? reason;
				if (!value)
				{
					if (pawn.carryTracker.CarriedThing is Pawn baby && ChildcareUtility.CanSuckle(baby, out reason))
					{
						Job newJob = ChildcareUtility.MakeBringBabyToSafetyJob(pawn, baby);
						Pawn_JobTracker jobs = pawn.jobs;
						bool? keepCarryingThingOverride = true;
						jobs.StartJob(newJob, JobCondition.InterruptForced, null, resumeCurJobAfterwards: false, cancelBusyStances: true, null, null, fromQueue: false, canReturnCurJobToPool: false, keepCarryingThingOverride);
					}
					else if (pawn.IsCarrying())
					{
						pawn.carryTracker.TryDropCarriedThing(pawn.Position, ThingPlaceMode.Near, out var _);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
					}
					else
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
					}
				}
				else
				{
					if (pawn.carryTracker.CarriedThing is Pawn baby2 && ChildcareUtility.CanSuckle(baby2, out reason))
					{
						Messages.Message("MessageDraftedPawnCarryingBaby".Translate(pawn), pawn, MessageTypeDefOf.NeutralEvent);
					}
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
				}
			}
			if (draftedInt)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.Drafted, null);
				}
				autoUndrafter.Notify_Drafted();
				pawn.jobs?.SetFormingCaravanTick(clear: true);
				pawn.inventory.DropAllPackingCaravanThings();
				pawn.TryGetComp<CompCanBeDormant>()?.WakeUp();
			}
			else
			{
				if (pawn.playerSettings != null)
				{
					pawn.playerSettings.animalsReleased = false;
				}
				if (pawn.IsFormingCaravan())
				{
					pawn.jobs?.SetFormingCaravanTick();
				}
			}
			foreach (Pawn item in PawnUtility.SpawnedMasteredPawns(pawn))
			{
				item.jobs.Notify_MasterDraftedOrUndrafted();
			}
		}
	}

	public bool FireAtWill
	{
		get
		{
			return fireAtWillInt;
		}
		set
		{
			fireAtWillInt = value;
			if (!fireAtWillInt && pawn.stances.curStance is Stance_Warmup)
			{
				pawn.stances.CancelBusyStanceSoft();
			}
		}
	}

	public bool ShowDraftGizmo
	{
		get
		{
			if (ModsConfig.BiotechActive && pawn.IsColonyMech && pawn.GetMechControlGroup() == null)
			{
				return false;
			}
			if (pawn.IsSubhuman && !pawn.IsColonySubhumanPlayerControlled)
			{
				return false;
			}
			return true;
		}
	}

	public Pawn_DraftController(Pawn pawn)
	{
		this.pawn = pawn;
		autoUndrafter = new AutoUndrafter(pawn);
	}

	public void ExposeData()
	{
		Scribe_Values.Look(ref draftedInt, "drafted", defaultValue: false);
		Scribe_Values.Look(ref fireAtWillInt, "fireAtWill", defaultValue: true);
		Scribe_Deep.Look(ref autoUndrafter, "autoUndrafter", pawn);
	}

	public void DraftControllerTickInterval(int delta)
	{
		autoUndrafter.AutoUndraftTickInterval(delta);
	}

	[IteratorStateMachine(typeof(_003CGetGizmos_003Ed__15))]
	internal IEnumerable<Gizmo> GetGizmos()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetGizmos_003Ed__15(-2)
		{
			_003C_003E4__this = this
		};
	}

	internal void Notify_PrimaryWeaponChanged()
	{
		fireAtWillInt = true;
	}
}
