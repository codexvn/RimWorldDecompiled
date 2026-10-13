using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002CFA RID: 11514
	public abstract class CompInteractable : ThingComp, ITargetingSource
	{
		// Token: 0x170028F5 RID: 10485
		// (get) Token: 0x060102B9 RID: 66233 RVA: 0x004AD86B File Offset: 0x004ABA6B
		public CompProperties_Interactable Props
		{
			get
			{
				return (CompProperties_Interactable)this.props;
			}
		}

		// Token: 0x170028F6 RID: 10486
		// (get) Token: 0x060102BA RID: 66234 RVA: 0x004AD878 File Offset: 0x004ABA78
		public bool OnCooldown
		{
			get
			{
				return this.cooldownTicks > 0;
			}
		}

		// Token: 0x170028F7 RID: 10487
		// (get) Token: 0x060102BB RID: 66235 RVA: 0x004AD883 File Offset: 0x004ABA83
		public virtual bool Active
		{
			get
			{
				return this.activeTicks > 0;
			}
		}

		// Token: 0x170028F8 RID: 10488
		// (get) Token: 0x060102BC RID: 66236 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanCooldown
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170028F9 RID: 10489
		// (get) Token: 0x060102BD RID: 66237 RVA: 0x004AD88E File Offset: 0x004ABA8E
		public virtual int TicksToActivate
		{
			get
			{
				return this.Props.ticksToActivate;
			}
		}

		// Token: 0x170028FA RID: 10490
		// (get) Token: 0x060102BE RID: 66238 RVA: 0x004AD89C File Offset: 0x004ABA9C
		public virtual NamedArgument? ExtraNamedArg
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170028FB RID: 10491
		// (get) Token: 0x060102BF RID: 66239 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExposeKey
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170028FC RID: 10492
		// (get) Token: 0x060102C0 RID: 66240 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool HideInteraction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170028FD RID: 10493
		// (get) Token: 0x060102C1 RID: 66241 RVA: 0x004AD8B2 File Offset: 0x004ABAB2
		protected virtual string ActivateOptionLabel
		{
			get
			{
				return this.Props.jobString.CapitalizeFirst();
			}
		}

		// Token: 0x170028FE RID: 10494
		// (get) Token: 0x060102C2 RID: 66242 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool CasterIsPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170028FF RID: 10495
		// (get) Token: 0x060102C3 RID: 66243 RVA: 0x00002501 File Offset: 0x00000701
		public bool IsMeleeAttack
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002900 RID: 10496
		// (get) Token: 0x060102C4 RID: 66244 RVA: 0x000028E7 File Offset: 0x00000AE7
		public bool Targetable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002901 RID: 10497
		// (get) Token: 0x060102C5 RID: 66245 RVA: 0x00002501 File Offset: 0x00000701
		public bool MultiSelect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002902 RID: 10498
		// (get) Token: 0x060102C6 RID: 66246 RVA: 0x00002501 File Offset: 0x00000701
		public bool HidePawnTooltips
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002903 RID: 10499
		// (get) Token: 0x060102C7 RID: 66247 RVA: 0x0048FDB8 File Offset: 0x0048DFB8
		public Thing Caster
		{
			get
			{
				return this.parent;
			}
		}

		// Token: 0x17002904 RID: 10500
		// (get) Token: 0x060102C8 RID: 66248 RVA: 0x00002C42 File Offset: 0x00000E42
		public Pawn CasterPawn
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002905 RID: 10501
		// (get) Token: 0x060102C9 RID: 66249 RVA: 0x00002C42 File Offset: 0x00000E42
		public Verb GetVerb
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002906 RID: 10502
		// (get) Token: 0x060102CA RID: 66250 RVA: 0x004AD8C4 File Offset: 0x004ABAC4
		public TargetingParameters targetParams
		{
			get
			{
				return this.Props.targetingParameters;
			}
		}

		// Token: 0x17002907 RID: 10503
		// (get) Token: 0x060102CB RID: 66251 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual ITargetingSource DestinationSelector
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17002908 RID: 10504
		// (get) Token: 0x060102CC RID: 66252 RVA: 0x004AD8D4 File Offset: 0x004ABAD4
		public Texture2D UIIcon
		{
			get
			{
				if (!(this.activateTex != null))
				{
					return this.activateTex = ContentFinder<Texture2D>.Get(this.Props.activateTexPath, true);
				}
				return this.activateTex;
			}
		}

		// Token: 0x060102CD RID: 66253 RVA: 0x004AD910 File Offset: 0x004ABB10
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.refuelable = this.parent.TryGetComp<CompRefuelable>();
			this.power = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x060102CE RID: 66254 RVA: 0x004AD934 File Offset: 0x004ABB34
		public override void CompTick()
		{
			if (this.Active)
			{
				if (this.TryInteractTick())
				{
					this.Deactivate();
				}
				else
				{
					if (this.ShouldDeactivate())
					{
						this.SendDeactivateMessage();
						this.Deactivate();
						return;
					}
					this.activeTicks--;
				}
				if (this.activeTicks <= 0)
				{
					this.StartCooldown();
				}
			}
			else if (this.OnCooldown)
			{
				if (this.CanCooldown)
				{
					this.cooldownTicks--;
				}
				if (this.parent.Spawned)
				{
					if (this.progressBarEffecter == null)
					{
						this.progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
					}
					this.progressBarEffecter.EffectTick(this.parent, TargetInfo.Invalid);
					MoteProgressBar mote = ((SubEffecter_ProgressBar)this.progressBarEffecter.children[0]).mote;
					mote.progress = 1f - (float)this.cooldownTicks / (float)this.Props.cooldownTicks;
					mote.offsetZ = -0.8f;
				}
				else
				{
					Effecter effecter = this.progressBarEffecter;
					if (effecter != null)
					{
						effecter.Cleanup();
					}
					this.progressBarEffecter = null;
				}
				if (this.Props.cooldownFleck != null && this.parent.IsHashIntervalTick(this.Props.cooldownFleckSpawnIntervalTicks))
				{
					FleckCreationData dataStatic = FleckMaker.GetDataStatic(this.parent.DrawPos, this.parent.Map, this.Props.cooldownFleck, this.Props.cooldownFleckScale);
					this.parent.Map.flecks.CreateFleck(dataStatic);
				}
				if (this.cooldownTicks <= 0)
				{
					this.CooldownEnded(true);
				}
			}
			Effecter effecter2 = this.interactionEffecter;
			if (effecter2 == null)
			{
				return;
			}
			effecter2.EffectTick(this.parent, this.parent);
		}

		// Token: 0x060102CF RID: 66255 RVA: 0x004ADAF6 File Offset: 0x004ABCF6
		public void ResetCooldown(bool sendMessage = true)
		{
			if (this.Active)
			{
				this.Deactivate();
			}
			if (this.OnCooldown)
			{
				this.cooldownTicks = 0;
				this.CooldownEnded(sendMessage);
			}
		}

		// Token: 0x060102D0 RID: 66256 RVA: 0x004ADB1C File Offset: 0x004ABD1C
		protected virtual void SendDeactivateMessage()
		{
			Messages.Message("MessageActivationCanceled".Translate(this.parent), this.parent, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x060102D1 RID: 66257 RVA: 0x004ADB50 File Offset: 0x004ABD50
		public virtual AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
		{
			if (this.Active)
			{
				return "AlreadyActive".Translate();
			}
			if (this.OnCooldown)
			{
				return this.Props.onCooldownString + " (" + "DurationLeft".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, true, true, false)) + ")";
			}
			if (this.refuelable != null && !this.refuelable.HasFuel)
			{
				return this.refuelable.Props.outOfFuelMessage ?? "NoFuel".Translate();
			}
			if (this.Props.requiresPower && this.power != null && !this.power.PowerOn)
			{
				return "NoPower".Translate().CapitalizeFirst();
			}
			if (activateBy != null)
			{
				if (activateBy.Dead)
				{
					return "PawnIsDead".Translate(activateBy);
				}
				if (this.Props.activateStat != null && (this.Props.activateStat.Worker.IsDisabledFor(activateBy) || this.Props.activateStat.statFactors.Any((StatDef s) => s.Worker.IsDisabledFor(activateBy))))
				{
					return "Incapable".Translate().CapitalizeFirst();
				}
				if (this.parent.PositionHeld.IsForbidden(activateBy) && !activateBy.Drafted && !this.Props.ignoreForbidden)
				{
					return "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + activateBy.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label;
				}
				if (this.parent.IsForbidden(activateBy) && !activateBy.Drafted && !this.Props.ignoreForbidden)
				{
					return "CannotPrioritizeCellForbidden".Translate();
				}
				if (activateBy.Downed)
				{
					return "MessageRitualPawnDowned".Translate(activateBy);
				}
				if (activateBy.Deathresting)
				{
					return "IsDeathresting".Translate(activateBy.Named("PAWN"));
				}
				if (!activateBy.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
				{
					return "MessageIncapableOfManipulation".Translate(activateBy);
				}
				if (!activateBy.CanReach(this.parent.SpawnedParentOrMe, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					return "CannotReach".Translate();
				}
			}
			return true;
		}

		// Token: 0x060102D2 RID: 66258 RVA: 0x004ADE4C File Offset: 0x004AC04C
		public void Interact(Pawn caster, bool force = false)
		{
			if (this.CanInteract(caster, false).Accepted || force)
			{
				this.activeTicks = this.Props.activeTicks;
				if (!this.Props.soundActivate.NullOrUndefined())
				{
					this.Props.soundActivate.PlayOneShot(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				if (this.Props.fleckOnUsed != null)
				{
					FleckMaker.AttachedOverlay(caster, this.Props.fleckOnUsed, Vector3.zero, this.Props.fleckOnUsedScale, -1f);
				}
				this.OnInteracted(caster);
				this.parent.BroadcastCompSignal("Interacted");
				QuestUtility.SendQuestTargetSignals(this.parent.questTags, "Activated", this.parent.Named("SUBJECT"), caster.Named("ACTIVATOR"));
				if (!string.IsNullOrEmpty(this.Props.messageCompletedString))
				{
					Messages.Message(this.Props.messageCompletedString.Formatted(caster.Named("PAWN")), caster, MessageTypeDefOf.NeutralEvent, false);
				}
			}
		}

		// Token: 0x060102D3 RID: 66259 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void OnInteracted(Pawn caster)
		{
		}

		// Token: 0x060102D4 RID: 66260 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual bool TryInteractTick()
		{
			return true;
		}

		// Token: 0x060102D5 RID: 66261 RVA: 0x00002501 File Offset: 0x00000701
		protected virtual bool ShouldDeactivate()
		{
			return false;
		}

		// Token: 0x060102D6 RID: 66262 RVA: 0x004ADF71 File Offset: 0x004AC171
		protected virtual void Deactivate()
		{
			this.activeTicks = 0;
		}

		// Token: 0x060102D7 RID: 66263 RVA: 0x004ADF7A File Offset: 0x004AC17A
		protected virtual void StartCooldown()
		{
			this.cooldownTicks = this.Props.cooldownTicks;
		}

		// Token: 0x060102D8 RID: 66264 RVA: 0x004ADF90 File Offset: 0x004AC190
		protected virtual void CooldownEnded(bool sendMessage = true)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			if (sendMessage && !string.IsNullOrEmpty(this.Props.messageCooldownEnded))
			{
				Messages.Message(this.Props.messageCooldownEnded, this.parent, MessageTypeDefOf.NeutralEvent, false);
			}
		}

		// Token: 0x060102D9 RID: 66265 RVA: 0x004ADFEB File Offset: 0x004AC1EB
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Effecter effecter = this.progressBarEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.progressBarEffecter = null;
			base.PostDestroy(mode, previousMap);
		}

		// Token: 0x060102DA RID: 66266 RVA: 0x004AE00D File Offset: 0x004AC20D
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.HideInteraction)
			{
				yield break;
			}
			if (this.parent.SpawnedOrAnyParentSpawned)
			{
				string defaultLabel;
				if (string.IsNullOrEmpty(this.Props.activateLabelString))
				{
					defaultLabel = "OrderActivation".Translate() + "...";
				}
				else
				{
					defaultLabel = this.Props.activateLabelString;
				}
				string defaultDesc;
				if (this.ExtraNamedArg != null)
				{
					if (string.IsNullOrEmpty(this.Props.activateDescString))
					{
						defaultDesc = "OrderActivationDesc".Translate(this.parent.Named("THING"), this.ExtraNamedArg.Value);
					}
					else
					{
						defaultDesc = this.Props.activateDescString.Formatted(this.parent.Named("THING"), this.ExtraNamedArg.Value);
					}
				}
				else if (string.IsNullOrEmpty(this.Props.activateDescString))
				{
					defaultDesc = "OrderActivationDesc".Translate(this.parent.Named("THING"));
				}
				else
				{
					defaultDesc = this.Props.activateDescString.Formatted(this.parent.Named("THING"));
				}
				Command_Action command_Action = new Command_Action
				{
					defaultLabel = defaultLabel,
					defaultDesc = defaultDesc,
					icon = this.UIIcon,
					groupable = false,
					action = delegate()
					{
						SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
						Find.Targeter.BeginTargeting(this, null, false, null, null, true);
					}
				};
				AcceptanceReport acceptanceReport = this.CanInteract(null, true);
				if (!acceptanceReport.Accepted)
				{
					command_Action.Disable(acceptanceReport.Reason.CapitalizeFirst());
				}
				yield return command_Action;
			}
			if (DebugSettings.ShowDevGizmos && this.OnCooldown)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEV: Reset cooldown",
					action = delegate()
					{
						this.cooldownTicks = 0;
						this.CooldownEnded(true);
					}
				};
			}
			yield break;
		}

		// Token: 0x060102DB RID: 66267 RVA: 0x004AE020 File Offset: 0x004AC220
		public override string CompInspectStringExtra()
		{
			TaggedString t = base.CompInspectStringExtra();
			if (!t.NullOrEmpty())
			{
				t += "\n";
			}
			if (this.OnCooldown)
			{
				t += this.Props.onCooldownString.CapitalizeFirst() + ": " + "DurationLeft".Translate(this.cooldownTicks.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst() + ".";
				if (!this.CanCooldown)
				{
					t += string.Format(" ({0})", "Paused".Translate());
				}
			}
			if (!this.Props.inspectString.NullOrEmpty())
			{
				if (!t.NullOrEmpty())
				{
					t += "\n";
				}
				t += this.Props.inspectString;
			}
			else if (this.Props.showMustBeActivatedByColonist)
			{
				if (!t.NullOrEmpty())
				{
					t += "\n";
				}
				t += "MustBeActivatedByColonist".Translate();
			}
			if (!this.Active && this.Props.maintainProgress && this.progress > 0f)
			{
				if (this.Props.remainingSecondsInInspectString)
				{
					t += "\n" + "Activation".Translate() + ": " + Mathf.FloorToInt((float)this.TicksToActivate * (1f - this.progress)).ToStringSecondsFromTicks("F0");
				}
				else
				{
					t += "\n" + "Activation".Translate() + ": " + this.progress.ToStringPercent();
				}
			}
			return t.Resolve();
		}

		// Token: 0x060102DC RID: 66268 RVA: 0x004AE206 File Offset: 0x004AC406
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			if (this.HideInteraction)
			{
				yield break;
			}
			AcceptanceReport acceptanceReport = this.CanInteract(selPawn, true);
			FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(this.ActivateOptionLabel, delegate()
			{
				this.OrderForceTarget(selPawn);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), selPawn, this.parent, "ReservedBy", null);
			if (!acceptanceReport.Accepted)
			{
				floatMenuOption.Disabled = true;
				FloatMenuOption floatMenuOption2 = floatMenuOption;
				floatMenuOption2.Label = floatMenuOption2.Label + " (" + acceptanceReport.Reason.UncapitalizeFirst() + ")";
			}
			yield return floatMenuOption;
			yield break;
		}

		// Token: 0x060102DD RID: 66269 RVA: 0x004AE220 File Offset: 0x004AC420
		public override void PostExposeData()
		{
			if (string.IsNullOrEmpty(this.ExposeKey))
			{
				Scribe_Values.Look<int>(ref this.cooldownTicks, "cooldownTicks", 0, false);
				Scribe_Values.Look<int>(ref this.activeTicks, "activeTicks", 0, false);
				Scribe_Values.Look<float>(ref this.progress, "progress", 0f, false);
				return;
			}
			Scribe_Values.Look<int>(ref this.cooldownTicks, this.ExposeKey + "_cooldownTicks", 0, false);
			Scribe_Values.Look<int>(ref this.activeTicks, this.ExposeKey + "_activeTicks", 0, false);
			Scribe_Values.Look<float>(ref this.progress, this.ExposeKey + "_progress", 0f, false);
		}

		// Token: 0x060102DE RID: 66270 RVA: 0x004AE2D0 File Offset: 0x004AC4D0
		public bool CanHitTarget(LocalTargetInfo target)
		{
			return this.ValidateTarget(target, false);
		}

		// Token: 0x060102DF RID: 66271 RVA: 0x004AE2DC File Offset: 0x004AC4DC
		public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!target.IsValid || target.Pawn == null)
			{
				return false;
			}
			Pawn pawn = target.Pawn;
			AcceptanceReport acceptanceReport = this.CanInteract(pawn, true);
			if (!acceptanceReport.Accepted)
			{
				if (showMessages && !acceptanceReport.Reason.NullOrEmpty())
				{
					Messages.Message("CannotGenericWorkCustom".Translate(this.Props.jobString) + ": " + acceptanceReport.Reason.CapitalizeFirst(), pawn, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x060102E0 RID: 66272 RVA: 0x0045EA21 File Offset: 0x0045CC21
		public void DrawHighlight(LocalTargetInfo target)
		{
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x060102E1 RID: 66273 RVA: 0x004AE378 File Offset: 0x004AC578
		public virtual void OrderForceTarget(LocalTargetInfo target)
		{
			if (this.ValidateTarget(target, false))
			{
				Job job = JobMaker.MakeJob(JobDefOf.InteractThing, this.parent);
				target.Pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}
		}

		// Token: 0x060102E2 RID: 66274 RVA: 0x004AE3C0 File Offset: 0x004AC5C0
		public void OnGUI(LocalTargetInfo target)
		{
			string label;
			if (string.IsNullOrEmpty(this.Props.guiLabelString))
			{
				label = "ChooseWhoShouldActivate".Translate();
			}
			else
			{
				label = this.Props.guiLabelString;
			}
			Widgets.MouseAttachedLabel(label, 0f, 0f, null);
			if (this.ValidateTarget(target, false) && this.Props.targetingParameters.CanTarget(target.Pawn, this))
			{
				GenUI.DrawMouseAttachment(this.UIIcon);
				return;
			}
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}

		// Token: 0x060102E3 RID: 66275 RVA: 0x004AE455 File Offset: 0x004AC655
		public void Notify_InteractionStarted()
		{
			if (this.Props.interactionEffecter != null)
			{
				this.interactionEffecter = this.Props.interactionEffecter.Spawn(this.parent, this.parent.Map, 1f);
			}
		}

		// Token: 0x060102E4 RID: 66276 RVA: 0x004AE490 File Offset: 0x004AC690
		public void Notify_InteractionEnded()
		{
			if (this.Props.interactionEffecter != null)
			{
				Effecter effecter = this.interactionEffecter;
				if (effecter != null)
				{
					effecter.Cleanup();
				}
				this.interactionEffecter = null;
			}
		}

		// Token: 0x0400A8FC RID: 43260
		protected int cooldownTicks;

		// Token: 0x0400A8FD RID: 43261
		private int activeTicks;

		// Token: 0x0400A8FE RID: 43262
		public float progress;

		// Token: 0x0400A8FF RID: 43263
		[Unsaved(false)]
		public Effecter interactionEffecter;

		// Token: 0x0400A900 RID: 43264
		[Unsaved(false)]
		private Effecter progressBarEffecter;

		// Token: 0x0400A901 RID: 43265
		[Unsaved(false)]
		private Texture2D activateTex;

		// Token: 0x0400A902 RID: 43266
		[Unsaved(false)]
		private CompRefuelable refuelable;

		// Token: 0x0400A903 RID: 43267
		[Unsaved(false)]
		protected CompPowerTrader power;

		// Token: 0x0400A904 RID: 43268
		public const string InteractedSignal = "Interacted";
	}
}
