using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02002443 RID: 9283
	public class Pawn_DraftController : IExposable
	{
		// Token: 0x17001E53 RID: 7763
		// (get) Token: 0x0600C8E6 RID: 51430 RVA: 0x003AAFE8 File Offset: 0x003A91E8
		// (set) Token: 0x0600C8E7 RID: 51431 RVA: 0x003AAFF0 File Offset: 0x003A91F0
		public bool Drafted
		{
			get
			{
				return this.draftedInt;
			}
			set
			{
				if (value == this.draftedInt)
				{
					return;
				}
				this.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
				this.fireAtWillInt = true;
				this.draftedInt = value;
				if (!value && this.pawn.Spawned)
				{
					this.pawn.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this.pawn);
				}
				this.pawn.jobs.ClearQueuedJobs(true);
				if (this.pawn.jobs.curJob != null && this.pawn.jobs.IsCurrentJobPlayerInterruptible())
				{
					JobDriver_Ingest jobDriver_Ingest = this.pawn.jobs.curDriver as JobDriver_Ingest;
					if (jobDriver_Ingest != null && jobDriver_Ingest.EatingFromInventory)
					{
						this.pawn.inventory.innerContainer.TryAddRangeOrTransfer(this.pawn.carryTracker.innerContainer, true, false);
					}
					if (!value)
					{
						Pawn pawn = this.pawn.carryTracker.CarriedThing as Pawn;
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (pawn != null && ChildcareUtility.CanSuckle(pawn, out breastfeedFailReason))
						{
							Job job = ChildcareUtility.MakeBringBabyToSafetyJob(this.pawn, pawn);
							Pawn_JobTracker jobs = this.pawn.jobs;
							Job newJob = job;
							JobCondition lastJobEndCondition = JobCondition.InterruptForced;
							ThinkNode jobGiver = null;
							bool resumeCurJobAfterwards = false;
							bool cancelBusyStances = true;
							ThinkTreeDef thinkTree = null;
							bool? keepCarryingThingOverride = new bool?(true);
							jobs.StartJob(newJob, lastJobEndCondition, jobGiver, resumeCurJobAfterwards, cancelBusyStances, thinkTree, null, false, false, keepCarryingThingOverride, false, true, false);
						}
						else if (this.pawn.IsCarrying())
						{
							Thing thing;
							this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
						else
						{
							this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
						}
					}
					else
					{
						Pawn pawn2 = this.pawn.carryTracker.CarriedThing as Pawn;
						ChildcareUtility.BreastfeedFailReason? breastfeedFailReason;
						if (pawn2 != null && ChildcareUtility.CanSuckle(pawn2, out breastfeedFailReason))
						{
							Messages.Message("MessageDraftedPawnCarryingBaby".Translate(this.pawn), this.pawn, MessageTypeDefOf.NeutralEvent, true);
						}
						this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				if (this.draftedInt)
				{
					Lord lord = this.pawn.GetLord();
					if (lord != null && lord.LordJob is LordJob_VoluntarilyJoinable)
					{
						lord.Notify_PawnLost(this.pawn, PawnLostCondition.Drafted, null);
					}
					this.autoUndrafter.Notify_Drafted();
					Pawn_JobTracker jobs2 = this.pawn.jobs;
					if (jobs2 != null)
					{
						jobs2.SetFormingCaravanTick(true);
					}
					this.pawn.inventory.DropAllPackingCaravanThings();
					CompCanBeDormant compCanBeDormant = this.pawn.TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
				}
				else
				{
					if (this.pawn.playerSettings != null)
					{
						this.pawn.playerSettings.animalsReleased = false;
					}
					if (this.pawn.IsFormingCaravan())
					{
						Pawn_JobTracker jobs3 = this.pawn.jobs;
						if (jobs3 != null)
						{
							jobs3.SetFormingCaravanTick(false);
						}
					}
				}
				foreach (Pawn pawn3 in PawnUtility.SpawnedMasteredPawns(this.pawn))
				{
					pawn3.jobs.Notify_MasterDraftedOrUndrafted();
				}
			}
		}

		// Token: 0x17001E54 RID: 7764
		// (get) Token: 0x0600C8E8 RID: 51432 RVA: 0x003AB324 File Offset: 0x003A9524
		// (set) Token: 0x0600C8E9 RID: 51433 RVA: 0x003AB32C File Offset: 0x003A952C
		public bool FireAtWill
		{
			get
			{
				return this.fireAtWillInt;
			}
			set
			{
				this.fireAtWillInt = value;
				if (this.fireAtWillInt)
				{
					return;
				}
				if (this.pawn.stances.curStance is Stance_Warmup)
				{
					this.pawn.stances.CancelBusyStanceSoft();
				}
			}
		}

		// Token: 0x17001E55 RID: 7765
		// (get) Token: 0x0600C8EA RID: 51434 RVA: 0x003AB368 File Offset: 0x003A9568
		public bool ShowDraftGizmo
		{
			get
			{
				return (!ModsConfig.BiotechActive || !this.pawn.IsColonyMech || this.pawn.GetMechControlGroup() != null) && (!this.pawn.IsSubhuman || this.pawn.IsColonySubhumanPlayerControlled);
			}
		}

		// Token: 0x0600C8EB RID: 51435 RVA: 0x003AB3B5 File Offset: 0x003A95B5
		public Pawn_DraftController(Pawn pawn)
		{
			this.pawn = pawn;
			this.autoUndrafter = new AutoUndrafter(pawn);
		}

		// Token: 0x0600C8EC RID: 51436 RVA: 0x003AB3D8 File Offset: 0x003A95D8
		public void ExposeData()
		{
			Scribe_Values.Look<bool>(ref this.draftedInt, "drafted", false, false);
			Scribe_Values.Look<bool>(ref this.fireAtWillInt, "fireAtWill", true, false);
			Scribe_Deep.Look<AutoUndrafter>(ref this.autoUndrafter, "autoUndrafter", new object[]
			{
				this.pawn
			});
		}

		// Token: 0x0600C8ED RID: 51437 RVA: 0x003AB428 File Offset: 0x003A9628
		public void DraftControllerTickInterval(int delta)
		{
			this.autoUndrafter.AutoUndraftTickInterval(delta);
		}

		// Token: 0x0600C8EE RID: 51438 RVA: 0x003AB436 File Offset: 0x003A9636
		internal IEnumerable<Gizmo> GetGizmos()
		{
			if (this.ShowDraftGizmo)
			{
				Command_Toggle command_Toggle = new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Command_ColonistDraft,
					isActive = (() => this.Drafted),
					toggleAction = delegate()
					{
						this.Drafted = !this.Drafted;
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Drafting, KnowledgeAmount.SpecificInteraction);
						if (this.Drafted)
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.QueueOrders, OpportunityType.GoodToKnow);
						}
					},
					defaultDesc = "CommandToggleDraftDesc".Translate(),
					icon = TexCommand.Draft,
					turnOnSound = SoundDefOf.DraftOn,
					turnOffSound = SoundDefOf.DraftOff,
					groupKeyIgnoreContent = 81729172,
					defaultLabel = (this.Drafted ? "CommandUndraftLabel" : "CommandDraftLabel").Translate()
				};
				if (this.pawn.Downed)
				{
					command_Toggle.Disable("IsIncapped".Translate(this.pawn.LabelShort, this.pawn));
				}
				if (this.pawn.Deathresting)
				{
					command_Toggle.Disable("IsDeathresting".Translate(this.pawn.Named("PAWN")));
				}
				if (ModsConfig.BiotechActive && this.pawn.IsColonyMech)
				{
					AcceptanceReport report = MechanitorUtility.CanDraftMech(this.pawn);
					if (!report)
					{
						command_Toggle.Disable(report.Reason);
					}
				}
				command_Toggle.tutorTag = ((!this.Drafted) ? "Draft" : "Undraft");
				yield return command_Toggle;
			}
			if (this.Drafted && this.pawn.equipment.Primary != null && this.pawn.equipment.Primary.def.IsRangedWeapon)
			{
				yield return new Command_Toggle
				{
					hotKey = KeyBindingDefOf.Misc6,
					isActive = (() => this.FireAtWill),
					toggleAction = delegate()
					{
						this.FireAtWill = !this.FireAtWill;
					},
					icon = TexCommand.FireAtWill,
					defaultLabel = "CommandFireAtWillLabel".Translate(),
					defaultDesc = "CommandFireAtWillDesc".Translate(),
					tutorTag = "FireAtWillToggle"
				};
			}
			yield break;
		}

		// Token: 0x0600C8EF RID: 51439 RVA: 0x003AB446 File Offset: 0x003A9646
		internal void Notify_PrimaryWeaponChanged()
		{
			this.fireAtWillInt = true;
		}

		// Token: 0x040088B8 RID: 35000
		public Pawn pawn;

		// Token: 0x040088B9 RID: 35001
		private bool draftedInt;

		// Token: 0x040088BA RID: 35002
		private bool fireAtWillInt = true;

		// Token: 0x040088BB RID: 35003
		private AutoUndrafter autoUndrafter;
	}
}
