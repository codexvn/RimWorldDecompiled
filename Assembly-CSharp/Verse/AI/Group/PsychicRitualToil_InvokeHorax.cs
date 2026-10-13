using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse.AI.Group
{
	// Token: 0x02001073 RID: 4211
	public class PsychicRitualToil_InvokeHorax : PsychicRitualToil
	{
		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x060066FC RID: 26364 RVA: 0x00209329 File Offset: 0x00207529
		public int TicksSinceStarted
		{
			get
			{
				return Find.TickManager.TicksGame - this.startingTick;
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x060066FD RID: 26365 RVA: 0x0020933C File Offset: 0x0020753C
		public int TicksLeft
		{
			get
			{
				return Mathf.RoundToInt(this.hoursUntilOutcome * 2500f) - this.TicksSinceStarted;
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x060066FE RID: 26366 RVA: 0x00209356 File Offset: 0x00207556
		public bool ShouldDoCompleteSoonEffect
		{
			get
			{
				return this.TicksLeft <= EffecterDefOf.PsychicRitual_CompleteSoon.maintainTicks;
			}
		}

		// Token: 0x060066FF RID: 26367 RVA: 0x00209370 File Offset: 0x00207570
		public PsychicRitualToil_InvokeHorax(PsychicRitualRoleDef invokerRole, IEnumerable<IntVec3> invokerPositions, PsychicRitualRoleDef targetRole, IEnumerable<IntVec3> targetPositions, PsychicRitualRoleDef chanterRole, IEnumerable<IntVec3> chanterPositions, PsychicRitualRoleDef defenderRole, IEnumerable<IntVec3> defenderPositions, IngredientCount requiredOffering)
		{
			this.invokerRole = invokerRole;
			this.targetRole = targetRole;
			this.chanterRole = chanterRole;
			this.defenderRole = defenderRole;
			this.requiredOffering = requiredOffering;
			this.invokerPositions = new List<IntVec3>(invokerPositions ?? Enumerable.Empty<IntVec3>());
			this.targetPositions = new List<IntVec3>(targetPositions ?? Enumerable.Empty<IntVec3>());
			this.chanterPositions = new List<IntVec3>(chanterPositions ?? Enumerable.Empty<IntVec3>());
			this.defenderPositions = new List<IntVec3>(defenderPositions ?? Enumerable.Empty<IntVec3>());
		}

		// Token: 0x06006700 RID: 26368 RVA: 0x0020940B File Offset: 0x0020760B
		protected PsychicRitualToil_InvokeHorax()
		{
		}

		// Token: 0x06006701 RID: 26369 RVA: 0x00209420 File Offset: 0x00207620
		public virtual void HoldRequiredOfferings(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			if (this.requiredOffering == null)
			{
				return;
			}
			foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
			{
				foreach (Thing thing in ((IEnumerable<Thing>)pawn.inventory.GetDirectlyHeldThings()))
				{
					if (this.requiredOffering.filter.Allows(thing))
					{
						pawn.inventory.innerContainer.TryTransferToContainer(thing, pawn.carryTracker.innerContainer, Mathf.CeilToInt(this.requiredOffering.GetBaseCount()), true);
					}
				}
			}
		}

		// Token: 0x06006702 RID: 26370 RVA: 0x002094F8 File Offset: 0x002076F8
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			this.startingTick = Find.TickManager.TicksGame;
			this.HoldRequiredOfferings(psychicRitual, parent);
			Lord lord = psychicRitual.lord;
			TaggedString taggedString;
			MessageTypeDef def;
			if (((lord != null) ? lord.faction : null) == Faction.OfPlayer)
			{
				taggedString = "PsychicRitualDef_InvocationCircle_PsychicRitualBegunMessage".Translate(psychicRitual.assignments.FirstAssignedPawn(this.invokerRole), psychicRitual.def.label);
				def = MessageTypeDefOf.NeutralEvent;
			}
			else
			{
				taggedString = "MessageAIPsychicRitualBegan".Translate(psychicRitual.lord.faction.Named("FACTION")).CapitalizeFirst() + ": " + psychicRitual.def.LabelCap;
				def = MessageTypeDefOf.ThreatSmall;
			}
			Messages.Message(taggedString.CapitalizeFirst(), psychicRitual.assignments.Target, def, true);
		}

		// Token: 0x06006703 RID: 26371 RVA: 0x002095DC File Offset: 0x002077DC
		public override void UpdateAllDuties(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			LocalTargetInfo value = (LocalTargetInfo)psychicRitual.assignments.Target;
			int num = 0;
			bool playerRitual = psychicRitual.lord.faction == Faction.OfPlayer;
			if (this.invokerRole != null)
			{
				foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
				{
					IntVec3 bestStandableRolePosition = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.invokerPositions[num++], value.Cell, psychicRitual.Map, 8f);
					this.SetPawnDuty(pawn, psychicRitual, parent, DutyDefOf.Invoke, new LocalTargetInfo?(value), new LocalTargetInfo?(bestStandableRolePosition), null, null, 0f, null);
				}
			}
			if (this.targetRole != null)
			{
				num = 0;
				foreach (Pawn pawn2 in psychicRitual.assignments.AssignedPawns(this.targetRole))
				{
					IntVec3 bestStandableRolePosition2 = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.targetPositions[num++], value.Cell, psychicRitual.Map, 8f);
					Pawn pawn3 = pawn2;
					DutyDef idle = DutyDefOf.Idle;
					LocalTargetInfo? focus = new LocalTargetInfo?(bestStandableRolePosition2);
					Rot4? overrideFacing = new Rot4?(Rot4.South);
					this.SetPawnDuty(pawn3, psychicRitual, parent, idle, focus, null, null, null, 0f, overrideFacing);
				}
			}
			if (this.chanterRole != null)
			{
				num = 0;
				foreach (Pawn pawn4 in psychicRitual.assignments.AssignedPawns(this.chanterRole))
				{
					IntVec3 bestStandableRolePosition3 = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.chanterPositions[num++], value.Cell, psychicRitual.Map, 8f);
					this.SetPawnDuty(pawn4, psychicRitual, parent, DutyDefOf.PsychicRitualDance, new LocalTargetInfo?(value), new LocalTargetInfo?(bestStandableRolePosition3), null, null, 0f, null);
				}
			}
			if (this.defenderRole != null)
			{
				num = 0;
				foreach (Pawn pawn5 in psychicRitual.assignments.AssignedPawns(this.defenderRole))
				{
					IntVec3 bestStandableRolePosition4 = psychicRitual.def.GetBestStandableRolePosition(playerRitual, this.defenderPositions[num++], value.Cell, psychicRitual.Map, 8f);
					this.SetPawnDuty(pawn5, psychicRitual, parent, DutyDefOf.DefendInvoker, new LocalTargetInfo?(bestStandableRolePosition4), null, null, null, 3f, null);
				}
			}
		}

		// Token: 0x06006704 RID: 26372 RVA: 0x00209904 File Offset: 0x00207B04
		protected virtual void UpdateProgressBar(PsychicRitual psychicRitual)
		{
			TargetInfo target = psychicRitual.assignments.Target;
			if (!target.IsValid)
			{
				return;
			}
			if (this.progressBar == null)
			{
				Thing thing = psychicRitual.assignments.Target.Thing;
				if (thing != null && !thing.Destroyed)
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.SpawnAttached(thing, thing.MapHeld, 1f);
				}
				else
				{
					this.progressBar = EffecterDefOf.ProgressBarAlwaysVisible.Spawn(target.Cell, target.Map, 1f);
				}
			}
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.EffectTick(target, TargetInfo.Invalid);
			}
			Effecter effecter2 = this.progressBar;
			SubEffecter_ProgressBar subEffecter_ProgressBar = ((effecter2 != null) ? effecter2.children[0] : null) as SubEffecter_ProgressBar;
			MoteProgressBar moteProgressBar = (subEffecter_ProgressBar != null) ? subEffecter_ProgressBar.mote : null;
			if (moteProgressBar != null)
			{
				moteProgressBar.progress = Mathf.Clamp01((float)this.TicksSinceStarted / (this.hoursUntilOutcome * 2500f));
				moteProgressBar.alwaysShow = true;
			}
		}

		// Token: 0x06006705 RID: 26373 RVA: 0x002099FC File Offset: 0x00207BFC
		public override bool Tick(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			if (this.requiredOffering != null)
			{
				float num = this.requiredOffering.GetBaseCount();
				foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
				{
					Pawn_CarryTracker carryTracker = pawn.carryTracker;
					Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
					if (thing != null && this.requiredOffering.filter.Allows(thing))
					{
						num -= (float)thing.stackCount;
					}
				}
				if (num > 0f)
				{
					psychicRitual.CancelPsychicRitual("PsychicRitualToil_InvokeHorax_MissingRequiredOffering".Translate(psychicRitual.assignments.FirstAssignedPawn(this.invokerRole).Named("PAWN"), this.requiredOffering.filter.Summary));
					return true;
				}
			}
			if (this.targetRole != null)
			{
				Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.targetRole);
				if (pawn2.mindState.duty != null && pawn2.PositionHeld != pawn2.mindState.duty.focus.Cell)
				{
					if (psychicRitual.lord.ownedPawns.Contains(pawn2))
					{
						psychicRitual.lord.Notify_PawnLost(pawn2, PawnLostCondition.LordRejected, null);
					}
					psychicRitual.CancelPsychicRitual("PsychicRitualToil_InvokeHorax_PawnIsOutOfPosition".Translate(pawn2.Named("PAWN")));
					return true;
				}
			}
			this.UpdateProgressBar(psychicRitual);
			this.TickSound(psychicRitual);
			this.TickVfx(psychicRitual);
			return (float)this.TicksSinceStarted > this.hoursUntilOutcome * 2500f;
		}

		// Token: 0x06006706 RID: 26374 RVA: 0x00209BA0 File Offset: 0x00207DA0
		private void TickVfx(PsychicRitual psychicRitual)
		{
			if (this.effecterOngoing == null)
			{
				this.effecterOngoing = EffecterDefOf.PsychicRitual_Sustained.Spawn(psychicRitual.assignments.Target.Cell, psychicRitual.assignments.Target.Cell, psychicRitual.Map, 1f);
				foreach (Pawn pawn in psychicRitual.assignments.AllAssignedPawns)
				{
					if (psychicRitual.assignments.RoleForPawn(pawn, true).showInvocationVfx)
					{
						this.motes.Add(MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_HateChantShadow, new Vector3(0f, 0f, 0f), 1f, -1f));
						this.motes.Add(MoteMaker.MakeAttachedOverlay(pawn, ThingDefOf.Mote_PsychicRitualInvocation, new Vector3(0f, 0f, 0f), 1f, -1f));
					}
				}
				float num = 4f;
				int num2 = 6;
				ThingDef[] list = new ThingDef[]
				{
					ThingDefOf.Mote_RitualCandleA,
					ThingDefOf.Mote_RitualCandleB,
					ThingDefOf.Mote_RitualCandleC
				};
				for (int i = 0; i < num2; i++)
				{
					float f = (float)i / (float)num2 * 3.1415927f * 2f;
					IntVec3 cell = psychicRitual.assignments.Target.Cell;
					cell.x += (int)(num * Mathf.Cos(f));
					cell.z += (int)(num * Mathf.Sin(f));
					this.motes.Add(MoteMaker.MakeStaticMote(cell, psychicRitual.Map, Rand.Element<ThingDef>(list), 1f));
				}
			}
			this.effecterOngoing.EffectTick(psychicRitual.assignments.Target, psychicRitual.assignments.Target);
			foreach (Mote mote in this.motes)
			{
				if (mote != null)
				{
					mote.Maintain();
				}
			}
			TargetInfo targetInfo = new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false);
			if (this.completeSoonEffecter == null && this.ShouldDoCompleteSoonEffect)
			{
				this.completeSoonEffecter = EffecterDefOf.PsychicRitual_CompleteSoon.Spawn(targetInfo, targetInfo, 1f);
			}
			Effecter effecter = this.completeSoonEffecter;
			if (effecter != null)
			{
				effecter.EffectTick(targetInfo, targetInfo);
			}
			if (Rand.MTBEventOccurs(8f, 60f, 1f))
			{
				Pawn pawn2 = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
				if (pawn2 != null && pawn2.Spawned)
				{
					SocialInteractionUtility.ImitateInteractionWithNoPawn(pawn2, InteractionDefOf.CreepyWords);
				}
			}
		}

		// Token: 0x06006707 RID: 26375 RVA: 0x00209E7C File Offset: 0x0020807C
		private void TickSound(PsychicRitual psychicRitual)
		{
			if (this.TicksLeft <= 300)
			{
				this.TickCompleteSfx(psychicRitual);
				return;
			}
			if (this.sustainerOngoingHuman == null)
			{
				this.sustainerOngoingHuman = SoundDefOf.PsychicRitual_Ongoing_Human.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
			}
			if (Mathf.Clamp01((float)this.TicksSinceStarted / (this.hoursUntilOutcome * 2500f)) >= 0.6f && this.sustainerOngoing == null)
			{
				this.sustainerOngoing = SoundDefOf.PsychicRitual_Ongoing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
			}
			Sustainer sustainer = this.sustainerOngoingHuman;
			if (sustainer != null)
			{
				sustainer.Maintain();
			}
			Sustainer sustainer2 = this.sustainerOngoing;
			if (sustainer2 == null)
			{
				return;
			}
			sustainer2.Maintain();
		}

		// Token: 0x06006708 RID: 26376 RVA: 0x00209F5C File Offset: 0x0020815C
		private void TickCompleteSfx(PsychicRitual psychicRitual)
		{
			if (this.completeFadeoutTriggered)
			{
				if (this.sustainerOngoingHuman != null)
				{
					this.sustainerOngoingHuman.End();
					this.sustainerOngoingHuman = null;
				}
				if (this.sustainerOngoing != null)
				{
					this.sustainerOngoing.End();
					this.sustainerOngoing = null;
					return;
				}
			}
			else
			{
				if (this.sustainerOngoingHuman == null)
				{
					this.sustainerOngoingHuman = SoundDefOf.PsychicRitual_Ongoing_Human.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
				}
				if (this.sustainerOngoing == null)
				{
					this.sustainerOngoing = SoundDefOf.PsychicRitual_Ongoing.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.PerTick));
				}
				SoundDefOf.PsychicRitual_Complete.PlayOneShot(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.None));
				this.completeFadeoutTriggered = true;
			}
		}

		// Token: 0x06006709 RID: 26377 RVA: 0x0020A05C File Offset: 0x0020825C
		public virtual void ConsumeRequiredOffering(PsychicRitual psychicRitual)
		{
			if (this.requiredOffering == null)
			{
				return;
			}
			float num = this.requiredOffering.GetBaseCount();
			foreach (Pawn pawn in psychicRitual.assignments.AssignedPawns(this.invokerRole))
			{
				Pawn_CarryTracker carryTracker = pawn.carryTracker;
				Thing thing = (carryTracker != null) ? carryTracker.CarriedThing : null;
				if (thing != null && this.requiredOffering.filter.Allows(thing))
				{
					int num2 = Mathf.Min(Mathf.CeilToInt(num), thing.stackCount);
					if (num2 < thing.stackCount)
					{
						thing.stackCount -= num2;
					}
					else
					{
						thing.Destroy(DestroyMode.Vanish);
					}
					num -= (float)num2;
					if (num <= 0f)
					{
						return;
					}
				}
			}
			if (num > 0f)
			{
				throw new InvalidOperationException(string.Format("Invocation should consume {0}, but {1} is unaccounted for.", this.requiredOffering.Summary, num));
			}
		}

		// Token: 0x0600670A RID: 26378 RVA: 0x0020A154 File Offset: 0x00208354
		public override void End(PsychicRitual psychicRitual, PsychicRitualGraph parent, bool success)
		{
			base.End(psychicRitual, parent, success);
			Effecter effecter = this.progressBar;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBar = null;
			PsychicRitualDef_InvocationCircle psychicRitualDef_InvocationCircle = psychicRitual.def as PsychicRitualDef_InvocationCircle;
			if (psychicRitualDef_InvocationCircle != null && psychicRitualDef_InvocationCircle.TargetRole != null)
			{
				Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(psychicRitualDef_InvocationCircle.TargetRole);
				if (pawn != null && pawn.IsPrisonerOfColony)
				{
					pawn.guest.WaitInsteadOfEscapingFor(1250);
				}
			}
			if (!success)
			{
				this.PlayInterruptedSfx(psychicRitual);
				return;
			}
			psychicRitual.succeeded = true;
			TargetInfo targetInfo = new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false);
			EffecterDefOf.PsychicRitual_Complete.SpawnMaintained(targetInfo, targetInfo, 1f);
			float power;
			psychicRitual.def.CalculateMaxPower(psychicRitual.assignments, null, out power);
			psychicRitual.maxPower = new float?(1f);
			psychicRitual.power = power;
			this.ConsumeRequiredOffering(psychicRitual);
			PsychicRitualRoleAssignments assignments = psychicRitual.assignments;
			Pawn pawn2 = (assignments != null) ? assignments.FirstAssignedPawn(this.invokerRole) : null;
			if (pawn2 != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.PerformedPsychicRitual, new object[]
				{
					pawn2
				});
			}
		}

		// Token: 0x0600670B RID: 26379 RVA: 0x0020A274 File Offset: 0x00208474
		public override string GetJobReport(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
			if (psychicRitualRoleDef == null)
			{
				return base.GetJobReport(psychicRitual, parent, pawn);
			}
			string str = " (" + "DurationLeft".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() + ")";
			if (pawn.Downed)
			{
				return null;
			}
			if (psychicRitualRoleDef == this.invokerRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Invoker".Translate().Resolve() + str;
			}
			if (psychicRitualRoleDef == this.targetRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Target".Translate().Resolve() + str;
			}
			if (psychicRitualRoleDef == this.chanterRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Chanter".Translate().Resolve() + str;
			}
			if (psychicRitualRoleDef == this.defenderRole)
			{
				return "PsychicRitualToil_InvokeHorax_JobReport_Defender".Translate().Resolve() + str;
			}
			return base.GetJobReport(psychicRitual, parent, pawn);
		}

		// Token: 0x0600670C RID: 26380 RVA: 0x0020A370 File Offset: 0x00208570
		public override string GetReport(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			return "DurationLeft".Translate(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false)).Resolve() ?? "";
		}

		// Token: 0x0600670D RID: 26381 RVA: 0x0020A3B0 File Offset: 0x002085B0
		public override ThinkResult Notify_DutyResult(PsychicRitual psychicRitual, PsychicRitualGraph parent, ThinkResult result, Pawn pawn, JobIssueParams issueParams)
		{
			if (result.Job == null)
			{
				PsychicRitualRoleDef psychicRitualRoleDef = psychicRitual.assignments.RoleForPawn(pawn, true);
				if (psychicRitualRoleDef != null && (psychicRitualRoleDef == this.invokerRole || psychicRitualRoleDef == this.chanterRole) && pawn.Position != pawn.mindState.duty.focusSecond.Cell)
				{
					psychicRitual.LeaveOrCancelPsychicRitual(psychicRitualRoleDef, pawn, "PsychicRitualToil_InvokeHorax_PawnIsOutOfPosition".Translate(pawn.Named("PAWN")));
				}
			}
			return base.Notify_DutyResult(psychicRitual, parent, result, pawn, issueParams);
		}

		// Token: 0x0600670E RID: 26382 RVA: 0x0020A43B File Offset: 0x0020863B
		private Gizmo DebugFinishGizmo()
		{
			return new Command_Action
			{
				defaultLabel = "Debug: Finish psychic ritual",
				defaultDesc = "Finishes the current ritual and causes its outcome to occur immediately.",
				action = delegate()
				{
					this.hoursUntilOutcome = 0f;
				}
			};
		}

		// Token: 0x0600670F RID: 26383 RVA: 0x0020A46A File Offset: 0x0020866A
		public override IEnumerable<Gizmo> GetBuildingGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Building building)
		{
			foreach (Gizmo gizmo in base.GetBuildingGizmos(psychicRitual, parent, building))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.godMode)
			{
				yield return this.DebugFinishGizmo();
			}
			yield break;
			yield break;
		}

		// Token: 0x06006710 RID: 26384 RVA: 0x0020A48F File Offset: 0x0020868F
		public override IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, PsychicRitualGraph parent, Pawn pawn)
		{
			foreach (Gizmo gizmo in base.GetPawnGizmos(psychicRitual, parent, pawn))
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (DebugSettings.godMode)
			{
				yield return this.DebugFinishGizmo();
			}
			yield break;
			yield break;
		}

		// Token: 0x06006711 RID: 26385 RVA: 0x0020A4B4 File Offset: 0x002086B4
		private void PlayInterruptedSfx(PsychicRitual psychicRitual)
		{
			SoundDefOf.PsychicRitual_Interrupted.PlayOneShot(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.None));
			SoundDefOf.PsychicRitual_Interrupted_Human.PlayOneShot(SoundInfo.InMap(new TargetInfo(psychicRitual.assignments.Target.Cell, psychicRitual.Map, false), MaintenanceType.None));
			if (this.sustainerOngoingHuman != null)
			{
				this.sustainerOngoingHuman.info.volumeFactor = 0f;
				this.sustainerOngoingHuman.End();
				this.sustainerOngoingHuman = null;
			}
			if (this.sustainerOngoing != null)
			{
				this.sustainerOngoing.info.volumeFactor = 0f;
				this.sustainerOngoing.End();
				this.sustainerOngoing = null;
			}
		}

		// Token: 0x06006712 RID: 26386 RVA: 0x0020A580 File Offset: 0x00208780
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<float>(ref this.hoursUntilHoraxEffect, "hoursUntilHoraxEffect", 0f, false);
			Scribe_Values.Look<float>(ref this.hoursUntilOutcome, "hoursUntilOutcome", 0f, false);
			Scribe_Collections.Look<IntVec3>(ref this.invokerPositions, "invokerPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.targetPositions, "targetPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.chanterPositions, "chanterPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<IntVec3>(ref this.defenderPositions, "defenderPositions", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.targetRole, "targetRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.chanterRole, "chanterRole");
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.defenderRole, "defenderRole");
			Scribe_Values.Look<int>(ref this.startingTick, "startingTick", 0, false);
			Scribe_Deep.Look<IngredientCount>(ref this.requiredOffering, "requiredOffering", Array.Empty<object>());
		}

		// Token: 0x04004972 RID: 18802
		public float hoursUntilHoraxEffect;

		// Token: 0x04004973 RID: 18803
		public float hoursUntilOutcome;

		// Token: 0x04004974 RID: 18804
		protected List<IntVec3> invokerPositions;

		// Token: 0x04004975 RID: 18805
		protected List<IntVec3> targetPositions;

		// Token: 0x04004976 RID: 18806
		protected List<IntVec3> chanterPositions;

		// Token: 0x04004977 RID: 18807
		protected List<IntVec3> defenderPositions;

		// Token: 0x04004978 RID: 18808
		public PsychicRitualRoleDef invokerRole;

		// Token: 0x04004979 RID: 18809
		public PsychicRitualRoleDef targetRole;

		// Token: 0x0400497A RID: 18810
		public PsychicRitualRoleDef chanterRole;

		// Token: 0x0400497B RID: 18811
		public PsychicRitualRoleDef defenderRole;

		// Token: 0x0400497C RID: 18812
		public IngredientCount requiredOffering;

		// Token: 0x0400497D RID: 18813
		private const float CreepyWordsMTBSeconds = 8f;

		// Token: 0x0400497E RID: 18814
		private int startingTick;

		// Token: 0x0400497F RID: 18815
		private Effecter progressBar;

		// Token: 0x04004980 RID: 18816
		private Sustainer sustainerOngoingHuman;

		// Token: 0x04004981 RID: 18817
		private Sustainer sustainerOngoing;

		// Token: 0x04004982 RID: 18818
		private Effecter effecterOngoing;

		// Token: 0x04004983 RID: 18819
		private Effecter completeSoonEffecter;

		// Token: 0x04004984 RID: 18820
		private bool completeFadeoutTriggered;

		// Token: 0x04004985 RID: 18821
		private readonly List<Mote> motes = new List<Mote>();
	}
}
