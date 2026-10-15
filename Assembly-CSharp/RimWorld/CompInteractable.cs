using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public abstract class CompInteractable : ThingComp, ITargetingSource
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass64_0
	{
		public CompInteractable _003C_003E4__this;

		public Pawn selPawn;

		internal void _003CCompFloatMenuOptions_003Eb__0()
		{
			_003C_003E4__this.OrderForceTarget(selPawn);
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompFloatMenuOptions_003Ed__64 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompInteractable _003C_003E4__this;

		private Pawn selPawn;

		public Pawn _003C_003E3__selPawn;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CCompFloatMenuOptions_003Ed__64(int _003C_003E1__state)
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
			CompInteractable compInteractable = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass64_0 CS_0024_003C_003E8__locals4 = new _003C_003Ec__DisplayClass64_0
				{
					_003C_003E4__this = _003C_003E4__this,
					selPawn = selPawn
				};
				if (compInteractable.HideInteraction)
				{
					return false;
				}
				AcceptanceReport acceptanceReport = compInteractable.CanInteract(CS_0024_003C_003E8__locals4.selPawn);
				FloatMenuOption floatMenuOption = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(compInteractable.ActivateOptionLabel, delegate
				{
					CS_0024_003C_003E8__locals4._003C_003E4__this.OrderForceTarget(CS_0024_003C_003E8__locals4.selPawn);
				}), CS_0024_003C_003E8__locals4.selPawn, compInteractable.parent);
				if (!acceptanceReport.Accepted)
				{
					floatMenuOption.Disabled = true;
					floatMenuOption.Label = floatMenuOption.Label + " (" + acceptanceReport.Reason.UncapitalizeFirst() + ")";
				}
				_003C_003E2__current = floatMenuOption;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CCompFloatMenuOptions_003Ed__64 _003CCompFloatMenuOptions_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CCompFloatMenuOptions_003Ed__ = this;
			}
			else
			{
				_003CCompFloatMenuOptions_003Ed__ = new _003CCompFloatMenuOptions_003Ed__64(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CCompFloatMenuOptions_003Ed__.selPawn = _003C_003E3__selPawn;
			return _003CCompFloatMenuOptions_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	[CompilerGenerated]
	private sealed class _003CCompGetGizmosExtra_003Ed__62 : IEnumerable<Gizmo>, IEnumerable, IEnumerator<Gizmo>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Gizmo _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public CompInteractable _003C_003E4__this;

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
		public _003CCompGetGizmosExtra_003Ed__62(int _003C_003E1__state)
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
			CompInteractable CS_0024_003C_003E8__locals21 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (CS_0024_003C_003E8__locals21.HideInteraction)
				{
					return false;
				}
				if (CS_0024_003C_003E8__locals21.parent.SpawnedOrAnyParentSpawned)
				{
					string defaultLabel = ((!string.IsNullOrEmpty(CS_0024_003C_003E8__locals21.Props.activateLabelString)) ? CS_0024_003C_003E8__locals21.Props.activateLabelString : ((string)("OrderActivation".Translate() + "...")));
					string defaultDesc = (CS_0024_003C_003E8__locals21.ExtraNamedArg.HasValue ? ((!string.IsNullOrEmpty(CS_0024_003C_003E8__locals21.Props.activateDescString)) ? ((string)CS_0024_003C_003E8__locals21.Props.activateDescString.Formatted(CS_0024_003C_003E8__locals21.parent.Named("THING"), CS_0024_003C_003E8__locals21.ExtraNamedArg.Value)) : ((string)"OrderActivationDesc".Translate(CS_0024_003C_003E8__locals21.parent.Named("THING"), CS_0024_003C_003E8__locals21.ExtraNamedArg.Value))) : ((!string.IsNullOrEmpty(CS_0024_003C_003E8__locals21.Props.activateDescString)) ? ((string)CS_0024_003C_003E8__locals21.Props.activateDescString.Formatted(CS_0024_003C_003E8__locals21.parent.Named("THING"))) : ((string)"OrderActivationDesc".Translate(CS_0024_003C_003E8__locals21.parent.Named("THING")))));
					Command_Action command_Action = new Command_Action
					{
						defaultLabel = defaultLabel,
						defaultDesc = defaultDesc,
						icon = CS_0024_003C_003E8__locals21.UIIcon,
						groupable = false,
						action = delegate
						{
							SoundDefOf.Tick_Tiny.PlayOneShotOnCamera();
							Find.Targeter.BeginTargeting(CS_0024_003C_003E8__locals21);
						}
					};
					AcceptanceReport acceptanceReport = CS_0024_003C_003E8__locals21.CanInteract();
					if (!acceptanceReport.Accepted)
					{
						command_Action.Disable(acceptanceReport.Reason.CapitalizeFirst());
					}
					_003C_003E2__current = command_Action;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_01dd;
			case 1:
				_003C_003E1__state = -1;
				goto IL_01dd;
			case 2:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_01dd:
				if (DebugSettings.ShowDevGizmos && CS_0024_003C_003E8__locals21.OnCooldown)
				{
					_003C_003E2__current = new Command_Action
					{
						defaultLabel = "DEV: Reset cooldown",
						action = delegate
						{
							CS_0024_003C_003E8__locals21.cooldownTicks = 0;
							CS_0024_003C_003E8__locals21.CooldownEnded();
						}
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
			_003CCompGetGizmosExtra_003Ed__62 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CCompGetGizmosExtra_003Ed__62(0)
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

	protected int cooldownTicks;

	private int activeTicks;

	public float progress;

	[Unsaved(false)]
	public Effecter interactionEffecter;

	[Unsaved(false)]
	private Effecter progressBarEffecter;

	[Unsaved(false)]
	private Texture2D activateTex;

	[Unsaved(false)]
	private CompRefuelable refuelable;

	[Unsaved(false)]
	protected CompPowerTrader power;

	public const string InteractedSignal = "Interacted";

	public CompProperties_Interactable Props => (CompProperties_Interactable)props;

	public bool OnCooldown => cooldownTicks > 0;

	public virtual bool Active => activeTicks > 0;

	public virtual bool CanCooldown => true;

	public virtual int TicksToActivate => Props.ticksToActivate;

	public virtual NamedArgument? ExtraNamedArg => null;

	public virtual string ExposeKey => null;

	public virtual bool HideInteraction => false;

	protected virtual string ActivateOptionLabel => Props.jobString.CapitalizeFirst();

	public bool CasterIsPawn => true;

	public bool IsMeleeAttack => false;

	public bool Targetable => true;

	public bool MultiSelect => false;

	public bool HidePawnTooltips => false;

	public Thing Caster => parent;

	public Pawn CasterPawn => null;

	public Verb GetVerb => null;

	public TargetingParameters targetParams => Props.targetingParameters;

	public virtual ITargetingSource DestinationSelector => null;

	public Texture2D UIIcon
	{
		get
		{
			if (!(activateTex != null))
			{
				return activateTex = ContentFinder<Texture2D>.Get(Props.activateTexPath);
			}
			return activateTex;
		}
	}

	public override void PostSpawnSetup(bool respawningAfterLoad)
	{
		refuelable = parent.TryGetComp<CompRefuelable>();
		power = parent.TryGetComp<CompPowerTrader>();
	}

	public override void CompTick()
	{
		if (Active)
		{
			if (TryInteractTick())
			{
				Deactivate();
			}
			else
			{
				if (ShouldDeactivate())
				{
					SendDeactivateMessage();
					Deactivate();
					return;
				}
				activeTicks--;
			}
			if (activeTicks <= 0)
			{
				StartCooldown();
			}
		}
		else if (OnCooldown)
		{
			if (CanCooldown)
			{
				cooldownTicks--;
			}
			if (parent.Spawned)
			{
				if (progressBarEffecter == null)
				{
					progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
				}
				progressBarEffecter.EffectTick(parent, TargetInfo.Invalid);
				MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
				mote.progress = 1f - (float)cooldownTicks / (float)Props.cooldownTicks;
				mote.offsetZ = -0.8f;
			}
			else
			{
				progressBarEffecter?.Cleanup();
				progressBarEffecter = null;
			}
			if (Props.cooldownFleck != null && parent.IsHashIntervalTick(Props.cooldownFleckSpawnIntervalTicks))
			{
				FleckCreationData dataStatic = FleckMaker.GetDataStatic(parent.DrawPos, parent.Map, Props.cooldownFleck, Props.cooldownFleckScale);
				parent.Map.flecks.CreateFleck(dataStatic);
			}
			if (cooldownTicks <= 0)
			{
				CooldownEnded();
			}
		}
		interactionEffecter?.EffectTick(parent, parent);
	}

	public void ResetCooldown(bool sendMessage = true)
	{
		if (Active)
		{
			Deactivate();
		}
		if (OnCooldown)
		{
			cooldownTicks = 0;
			CooldownEnded(sendMessage);
		}
	}

	protected virtual void SendDeactivateMessage()
	{
		Messages.Message("MessageActivationCanceled".Translate(parent), parent, MessageTypeDefOf.NeutralEvent);
	}

	public virtual AcceptanceReport CanInteract(Pawn activateBy = null, bool checkOptionalItems = true)
	{
		if (Active)
		{
			return "AlreadyActive".Translate();
		}
		if (OnCooldown)
		{
			return Props.onCooldownString + " (" + "DurationLeft".Translate(cooldownTicks.ToStringTicksToPeriod()) + ")";
		}
		if (refuelable != null && !refuelable.HasFuel)
		{
			return refuelable.Props.outOfFuelMessage ?? ((string)"NoFuel".Translate());
		}
		if (Props.requiresPower && power != null && !power.PowerOn)
		{
			return "NoPower".Translate().CapitalizeFirst();
		}
		if (activateBy != null)
		{
			if (activateBy.Dead)
			{
				return "PawnIsDead".Translate(activateBy);
			}
			if (Props.activateStat != null && (Props.activateStat.Worker.IsDisabledFor(activateBy) || Props.activateStat.statFactors.Any((StatDef s) => s.Worker.IsDisabledFor(activateBy))))
			{
				return "Incapable".Translate().CapitalizeFirst();
			}
			if (parent.PositionHeld.IsForbidden(activateBy) && !activateBy.Drafted && !Props.ignoreForbidden)
			{
				return "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + ": " + activateBy.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label;
			}
			if (parent.IsForbidden(activateBy) && !activateBy.Drafted && !Props.ignoreForbidden)
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
			if (!activateBy.CanReach(parent.SpawnedParentOrMe, PathEndMode.ClosestTouch, Danger.Deadly))
			{
				return "CannotReach".Translate();
			}
		}
		return true;
	}

	public void Interact(Pawn caster, bool force = false)
	{
		if (CanInteract(caster, checkOptionalItems: false).Accepted || force)
		{
			activeTicks = Props.activeTicks;
			if (!Props.soundActivate.NullOrUndefined())
			{
				Props.soundActivate.PlayOneShot(SoundInfo.InMap(parent));
			}
			if (Props.fleckOnUsed != null)
			{
				FleckMaker.AttachedOverlay(caster, Props.fleckOnUsed, Vector3.zero, Props.fleckOnUsedScale);
			}
			OnInteracted(caster);
			parent.BroadcastCompSignal("Interacted");
			QuestUtility.SendQuestTargetSignals(parent.questTags, "Activated", parent.Named("SUBJECT"), caster.Named("ACTIVATOR"));
			if (!string.IsNullOrEmpty(Props.messageCompletedString))
			{
				Messages.Message(Props.messageCompletedString.Formatted(caster.Named("PAWN")), caster, MessageTypeDefOf.NeutralEvent, historical: false);
			}
		}
	}

	protected virtual void OnInteracted(Pawn caster)
	{
	}

	protected virtual bool TryInteractTick()
	{
		return true;
	}

	protected virtual bool ShouldDeactivate()
	{
		return false;
	}

	protected virtual void Deactivate()
	{
		activeTicks = 0;
	}

	protected virtual void StartCooldown()
	{
		cooldownTicks = Props.cooldownTicks;
	}

	protected virtual void CooldownEnded(bool sendMessage = true)
	{
		progressBarEffecter?.Cleanup();
		progressBarEffecter = null;
		if (sendMessage && !string.IsNullOrEmpty(Props.messageCooldownEnded))
		{
			Messages.Message(Props.messageCooldownEnded, parent, MessageTypeDefOf.NeutralEvent, historical: false);
		}
	}

	public override void PostDestroy(DestroyMode mode, Map previousMap)
	{
		progressBarEffecter?.Cleanup();
		progressBarEffecter = null;
		base.PostDestroy(mode, previousMap);
	}

	[IteratorStateMachine(typeof(_003CCompGetGizmosExtra_003Ed__62))]
	public override IEnumerable<Gizmo> CompGetGizmosExtra()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompGetGizmosExtra_003Ed__62(-2)
		{
			_003C_003E4__this = this
		};
	}

	public override string CompInspectStringExtra()
	{
		TaggedString taggedString = base.CompInspectStringExtra();
		if (!taggedString.NullOrEmpty())
		{
			taggedString += "\n";
		}
		if (OnCooldown)
		{
			taggedString += Props.onCooldownString.CapitalizeFirst() + ": " + "DurationLeft".Translate(cooldownTicks.ToStringTicksToPeriod()).CapitalizeFirst() + ".";
			if (!CanCooldown)
			{
				taggedString += $" ({"Paused".Translate()})";
			}
		}
		if (!Props.inspectString.NullOrEmpty())
		{
			if (!taggedString.NullOrEmpty())
			{
				taggedString += "\n";
			}
			taggedString += Props.inspectString;
		}
		else if (Props.showMustBeActivatedByColonist)
		{
			if (!taggedString.NullOrEmpty())
			{
				taggedString += "\n";
			}
			taggedString += "MustBeActivatedByColonist".Translate();
		}
		if (!Active && Props.maintainProgress && progress > 0f)
		{
			if (Props.remainingSecondsInInspectString)
			{
				taggedString += "\n" + "Activation".Translate() + ": " + Mathf.FloorToInt((float)TicksToActivate * (1f - progress)).ToStringSecondsFromTicks("F0");
			}
			else
			{
				taggedString += "\n" + "Activation".Translate() + ": " + progress.ToStringPercent();
			}
		}
		return taggedString.Resolve();
	}

	[IteratorStateMachine(typeof(_003CCompFloatMenuOptions_003Ed__64))]
	public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CCompFloatMenuOptions_003Ed__64(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__selPawn = selPawn
		};
	}

	public override void PostExposeData()
	{
		if (string.IsNullOrEmpty(ExposeKey))
		{
			Scribe_Values.Look(ref cooldownTicks, "cooldownTicks", 0);
			Scribe_Values.Look(ref activeTicks, "activeTicks", 0);
			Scribe_Values.Look(ref progress, "progress", 0f);
		}
		else
		{
			Scribe_Values.Look(ref cooldownTicks, ExposeKey + "_cooldownTicks", 0);
			Scribe_Values.Look(ref activeTicks, ExposeKey + "_activeTicks", 0);
			Scribe_Values.Look(ref progress, ExposeKey + "_progress", 0f);
		}
	}

	public bool CanHitTarget(LocalTargetInfo target)
	{
		return ValidateTarget(target, showMessages: false);
	}

	public bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
	{
		if (!target.IsValid || target.Pawn == null)
		{
			return false;
		}
		Pawn pawn = target.Pawn;
		AcceptanceReport acceptanceReport = CanInteract(pawn);
		if (!acceptanceReport.Accepted)
		{
			if (showMessages && !acceptanceReport.Reason.NullOrEmpty())
			{
				Messages.Message("CannotGenericWorkCustom".Translate(Props.jobString) + ": " + acceptanceReport.Reason.CapitalizeFirst(), pawn, MessageTypeDefOf.RejectInput, historical: false);
			}
			return false;
		}
		return true;
	}

	public void DrawHighlight(LocalTargetInfo target)
	{
		if (target.IsValid)
		{
			GenDraw.DrawTargetHighlight(target);
		}
	}

	public virtual void OrderForceTarget(LocalTargetInfo target)
	{
		if (ValidateTarget(target, showMessages: false))
		{
			Job job = JobMaker.MakeJob(JobDefOf.InteractThing, parent);
			target.Pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	public void OnGUI(LocalTargetInfo target)
	{
		string label = ((!string.IsNullOrEmpty(Props.guiLabelString)) ? Props.guiLabelString : ((string)"ChooseWhoShouldActivate".Translate()));
		Widgets.MouseAttachedLabel(label, 0f, 0f, null);
		if (ValidateTarget(target, showMessages: false) && Props.targetingParameters.CanTarget(target.Pawn, this))
		{
			GenUI.DrawMouseAttachment(UIIcon);
		}
		else
		{
			GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
		}
	}

	public void Notify_InteractionStarted()
	{
		if (Props.interactionEffecter != null)
		{
			interactionEffecter = Props.interactionEffecter.Spawn(parent, parent.Map);
		}
	}

	public void Notify_InteractionEnded()
	{
		if (Props.interactionEffecter != null)
		{
			interactionEffecter?.Cleanup();
			interactionEffecter = null;
		}
	}
}
