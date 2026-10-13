using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020033EB RID: 13291
	public class Targeter
	{
		// Token: 0x170030F5 RID: 12533
		// (get) Token: 0x06012B09 RID: 76553 RVA: 0x00588258 File Offset: 0x00586458
		public bool IsTargeting
		{
			get
			{
				return this.targetingSource != null || this.action != null;
			}
		}

		// Token: 0x06012B0A RID: 76554 RVA: 0x00588270 File Offset: 0x00586470
		public void BeginTargeting(ITargetingSource source, ITargetingSource parent = null, bool allowNonSelectedTargetingSource = false, Func<LocalTargetInfo, ITargetingSource> extraSourceGetter = null, Action actionWhenFinished = null, bool requiresAvailableVerb = true)
		{
			if (source.Targetable)
			{
				this.targetingSource = source;
				this.targetingSourceAdditionalPawns = new List<Pawn>();
			}
			else
			{
				Verb getVerb = source.GetVerb;
				if (getVerb.verbProps.nonInterruptingSelfCast)
				{
					getVerb.TryStartCastOn(getVerb.Caster, false, true, false, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing, getVerb.Caster);
				job.verbToUse = getVerb;
				source.CasterPawn.jobs.StartJob(job, JobCondition.InterruptForced, null, false, true, null, null, false, false, null, false, true, false);
			}
			this.action = null;
			this.caster = null;
			this.targetParams = null;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = null;
			this.targetingSourceParent = parent;
			this.targetingSourceGetter = extraSourceGetter;
			this.requiresAvailableVerb = requiresAvailableVerb;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
			this.allowNonSelectedTargetingSource = allowNonSelectedTargetingSource;
		}

		// Token: 0x06012B0B RID: 76555 RVA: 0x00588360 File Offset: 0x00586560
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null, bool requiresCastedSelected = true)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.highlightAction = null;
			this.targetValidator = null;
			this.onGuiAction = null;
			this.onUpdateAction = null;
			this.requiresCastedSelected = requiresCastedSelected;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x06012B0C RID: 76556 RVA: 0x005883DC File Offset: 0x005865DC
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Action<LocalTargetInfo> onGuiAction)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = null;
			this.actionWhenFinished = null;
			this.mouseAttachment = null;
			this.highlightAction = null;
			this.targetValidator = null;
			this.onGuiAction = onGuiAction;
			this.onUpdateAction = null;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x06012B0D RID: 76557 RVA: 0x00588454 File Offset: 0x00586654
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Action<LocalTargetInfo> highlightAction, Func<LocalTargetInfo, bool> targetValidator, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null, bool playSoundOnAction = true, Action<LocalTargetInfo> onGuiAction = null, Action<LocalTargetInfo> onUpdateAction = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = new List<Pawn>();
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.highlightAction = highlightAction;
			this.targetValidator = targetValidator;
			this.playSoundOnAction = playSoundOnAction;
			this.onGuiAction = onGuiAction;
			this.onUpdateAction = onUpdateAction;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x06012B0E RID: 76558 RVA: 0x005884D8 File Offset: 0x005866D8
		public void BeginTargeting(TargetingParameters targetParams, ITargetingSource ability, Action<LocalTargetInfo> action, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.actionWhenFinished = actionWhenFinished;
			this.caster = null;
			this.targetParams = targetParams;
			this.mouseAttachment = mouseAttachment;
			this.targetingSource = ability;
			this.highlightAction = null;
			this.targetValidator = null;
			this.onGuiAction = null;
			this.onUpdateAction = null;
			this.requiresCastedSelected = true;
			this.needsStopTargetingCall = false;
			this.playSoundOnAction = true;
		}

		// Token: 0x06012B0F RID: 76559 RVA: 0x00588558 File Offset: 0x00586758
		public void StopTargeting()
		{
			if (this.actionWhenFinished != null)
			{
				Action action = this.actionWhenFinished;
				this.actionWhenFinished = null;
				action();
			}
			this.targetingSource = null;
			this.action = null;
			this.targetParams = null;
			this.highlightAction = null;
			this.targetValidator = null;
			this.targetingSourceGetter = null;
			this.onUpdateAction = null;
			this.onGuiAction = null;
			this.requiresCastedSelected = true;
		}

		// Token: 0x06012B10 RID: 76560 RVA: 0x005885C0 File Offset: 0x005867C0
		public void ProcessInputEvents()
		{
			this.UpdateTargetingSource();
			this.ConfirmStillValid();
			if (this.IsTargeting)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
					this.needsStopTargetingCall = true;
					if (this.targetingSource != null)
					{
						if (!this.targetingSource.ValidateTarget(localTargetInfo, true))
						{
							Event.current.Use();
							return;
						}
						this.OrderVerbForceTarget();
					}
					else if (this.action != null)
					{
						if (this.targetValidator != null)
						{
							if (this.targetValidator(localTargetInfo))
							{
								this.action(localTargetInfo);
							}
							else
							{
								this.needsStopTargetingCall = false;
							}
						}
						else if (localTargetInfo.IsValid)
						{
							this.action(localTargetInfo);
						}
					}
					if (this.playSoundOnAction)
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					}
					if (this.targetingSource != null)
					{
						if (this.targetingSource.DestinationSelector != null)
						{
							this.BeginTargeting(this.targetingSource.DestinationSelector, this.targetingSource, false, this.targetingSourceGetter, null, true);
						}
						else if (this.targetingSource.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSource, null, false, this.targetingSourceGetter, null, true);
						}
						else if (this.targetingSourceParent != null && this.targetingSourceParent.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSourceParent, null, false, this.targetingSourceGetter, null, true);
						}
					}
					if (this.needsStopTargetingCall)
					{
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
		}

		// Token: 0x06012B11 RID: 76561 RVA: 0x00588794 File Offset: 0x00586994
		public void TargeterOnGUI()
		{
			if (this.targetingSource != null)
			{
				LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
				this.targetingSource.OnGUI(target);
			}
			if (this.action != null)
			{
				GenUI.DrawMouseAttachment(this.mouseAttachment ?? TexCommand.Attack);
			}
			if (this.onGuiAction != null)
			{
				LocalTargetInfo obj = this.CurrentTargetUnderMouse(true);
				this.onGuiAction(obj);
			}
			ITargetingSource targetingSource = this.targetingSource;
			bool flag;
			if (targetingSource == null)
			{
				flag = (null != null);
			}
			else
			{
				Verb getVerb = targetingSource.GetVerb;
				if (getVerb == null)
				{
					flag = (null != null);
				}
				else
				{
					VerbProperties verbProps = getVerb.verbProps;
					flag = (((verbProps != null) ? verbProps.mouseTargetingText : null) != null);
				}
			}
			if (flag)
			{
				Widgets.MouseAttachedLabel(this.targetingSource.GetVerb.verbProps.mouseTargetingText, 0f, 0f, null);
			}
		}

		// Token: 0x06012B12 RID: 76562 RVA: 0x00588850 File Offset: 0x00586A50
		public void TargeterUpdate()
		{
			this.UpdateTargetingSource();
			if (this.targetingSource != null)
			{
				this.targetingSource.DrawHighlight(this.CurrentTargetUnderMouse(true));
			}
			if (this.action != null)
			{
				LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
				if (this.highlightAction != null)
				{
					this.highlightAction(localTargetInfo);
				}
				else if (localTargetInfo.IsValid)
				{
					GenDraw.DrawTargetHighlight(localTargetInfo);
				}
			}
			if (this.onUpdateAction != null)
			{
				LocalTargetInfo obj = this.CurrentTargetUnderMouse(true);
				this.onUpdateAction(obj);
			}
		}

		// Token: 0x06012B13 RID: 76563 RVA: 0x005888D0 File Offset: 0x00586AD0
		public bool IsPawnTargeting(Pawn p)
		{
			if (this.caster == p)
			{
				return true;
			}
			if (this.targetingSource != null && this.targetingSource.CasterIsPawn)
			{
				if (this.targetingSource.CasterPawn == p)
				{
					return true;
				}
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					if (this.targetingSourceAdditionalPawns[i] == p)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06012B14 RID: 76564 RVA: 0x00588938 File Offset: 0x00586B38
		private void UpdateTargetingSource()
		{
			if (this.targetingSourceGetter != null)
			{
				ITargetingSource targetingSource = this.targetingSourceGetter(this.CurrentTargetUnderMouse(true));
				if (targetingSource != null)
				{
					this.targetingSource = targetingSource;
				}
			}
		}

		// Token: 0x06012B15 RID: 76565 RVA: 0x0058896C File Offset: 0x00586B6C
		private void ConfirmStillValid()
		{
			if (this.caster != null && (this.caster.Map != Find.CurrentMap || this.caster.Destroyed || (this.requiresCastedSelected && !Find.Selector.IsSelected(this.caster))))
			{
				this.StopTargeting();
			}
			if (this.targetingSource != null)
			{
				Selector selector = Find.Selector;
				if (this.targetingSource.Caster.MapHeld == Find.CurrentMap && !this.targetingSource.Caster.Destroyed && (this.allowNonSelectedTargetingSource || selector.IsSelected(this.targetingSource.Caster)) && (this.targetingSource.GetVerb == null || !this.requiresAvailableVerb || this.targetingSource.GetVerb.Available()))
				{
					CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.targetingSource as CompAbilityEffect_WithDest;
					if (compAbilityEffect_WithDest == null || !compAbilityEffect_WithDest.SelectedTargetInvalidated())
					{
						Verb_CastAbility verb_CastAbility = this.targetingSource as Verb_CastAbility;
						if (verb_CastAbility == null || verb_CastAbility.ability.CanQueueCast)
						{
							for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
							{
								if (this.targetingSourceAdditionalPawns[i].Destroyed || !selector.IsSelected(this.targetingSourceAdditionalPawns[i]))
								{
									this.StopTargeting();
									return;
								}
							}
							return;
						}
					}
				}
				this.StopTargeting();
				return;
			}
		}

		// Token: 0x06012B16 RID: 76566 RVA: 0x00588AC0 File Offset: 0x00586CC0
		private void OrderVerbForceTarget()
		{
			if (this.targetingSource.CasterIsPawn)
			{
				this.OrderPawnForceTarget(this.targetingSource);
				if (this.targetingSourceAdditionalPawns != null)
				{
					for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
					{
						Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
						if (targetingVerb != null)
						{
							this.OrderPawnForceTarget(targetingVerb);
						}
					}
					return;
				}
			}
			else
			{
				int numSelected = Find.Selector.NumSelected;
				List<object> selectedObjects = Find.Selector.SelectedObjects;
				for (int j = 0; j < numSelected; j++)
				{
					Building_Turret building_Turret = selectedObjects[j] as Building_Turret;
					if (building_Turret != null && building_Turret.Map == Find.CurrentMap)
					{
						LocalTargetInfo targ = this.CurrentTargetUnderMouse(true);
						building_Turret.OrderAttack(targ);
					}
				}
				if (this.targetingSource != null)
				{
					Building_Turret building_Turret2 = this.targetingSource.Caster as Building_Turret;
					if (building_Turret2 != null && building_Turret2 != null && building_Turret2.Map == Find.CurrentMap)
					{
						LocalTargetInfo targ2 = this.CurrentTargetUnderMouse(true);
						building_Turret2.OrderAttack(targ2);
					}
				}
			}
		}

		// Token: 0x06012B17 RID: 76567 RVA: 0x00588BC0 File Offset: 0x00586DC0
		public void OrderPawnForceTarget(ITargetingSource targetingSource)
		{
			LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
			if (!target.IsValid)
			{
				return;
			}
			targetingSource.OrderForceTarget(target);
		}

		// Token: 0x06012B18 RID: 76568 RVA: 0x00588BE8 File Offset: 0x00586DE8
		private LocalTargetInfo CurrentTargetUnderMouse(bool mustBeHittableNowIfNotMelee)
		{
			if (!this.IsTargeting)
			{
				return LocalTargetInfo.Invalid;
			}
			TargetingParameters targetingParameters = (this.targetingSource != null) ? this.targetingSource.targetParams : this.targetParams;
			ColonistBar.Entry entry;
			LocalTargetInfo localTargetInfo;
			if (Find.ColonistBar.TryGetEntryAt(UI.MousePositionOnUIInverted, out entry) && entry.pawn != null && targetingParameters.CanTarget(entry.pawn, null))
			{
				if (entry.pawn.Dead)
				{
					localTargetInfo = entry.pawn.Corpse;
				}
				else
				{
					localTargetInfo = entry.pawn;
				}
			}
			else
			{
				localTargetInfo = GenUI.TargetsAtMouse(targetingParameters, false, this.targetingSource).FirstOrFallback(LocalTargetInfo.Invalid);
			}
			if (localTargetInfo.IsValid && this.targetingSource != null)
			{
				if (mustBeHittableNowIfNotMelee && !(localTargetInfo.Thing is Pawn) && !this.targetingSource.IsMeleeAttack)
				{
					if (this.targetingSourceAdditionalPawns != null && this.targetingSourceAdditionalPawns.Any<Pawn>())
					{
						bool flag = false;
						for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
						{
							Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
							if (targetingVerb != null && targetingVerb.CanHitTarget(localTargetInfo))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							localTargetInfo = LocalTargetInfo.Invalid;
						}
					}
					else if (!this.targetingSource.CanHitTarget(localTargetInfo))
					{
						localTargetInfo = LocalTargetInfo.Invalid;
					}
				}
				if (localTargetInfo == this.targetingSource.Caster && !targetingParameters.canTargetSelf)
				{
					localTargetInfo = LocalTargetInfo.Invalid;
				}
			}
			return localTargetInfo;
		}

		// Token: 0x06012B19 RID: 76569 RVA: 0x00588D70 File Offset: 0x00586F70
		private Verb GetTargetingVerb(Pawn pawn)
		{
			Pawn_EquipmentTracker equipment = pawn.equipment;
			Verb verb = (equipment != null) ? equipment.AllEquipmentVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|35_0)) : null;
			if (verb != null)
			{
				return verb;
			}
			Pawn_ApparelTracker apparel = pawn.apparel;
			if (apparel == null)
			{
				return null;
			}
			return apparel.AllApparelVerbs.FirstOrDefault(new Func<Verb, bool>(this.<GetTargetingVerb>g__SameVerb|35_0));
		}

		// Token: 0x06012B1B RID: 76571 RVA: 0x00588DE5 File Offset: 0x00586FE5
		[CompilerGenerated]
		private bool <GetTargetingVerb>g__SameVerb|35_0(Verb x)
		{
			return x.verbProps == this.targetingSource.GetVerb.verbProps;
		}

		// Token: 0x0400C211 RID: 49681
		public ITargetingSource targetingSource;

		// Token: 0x0400C212 RID: 49682
		public ITargetingSource targetingSourceParent;

		// Token: 0x0400C213 RID: 49683
		public List<Pawn> targetingSourceAdditionalPawns;

		// Token: 0x0400C214 RID: 49684
		public Func<LocalTargetInfo, ITargetingSource> targetingSourceGetter;

		// Token: 0x0400C215 RID: 49685
		private Action<LocalTargetInfo> action;

		// Token: 0x0400C216 RID: 49686
		private Pawn caster;

		// Token: 0x0400C217 RID: 49687
		private TargetingParameters targetParams;

		// Token: 0x0400C218 RID: 49688
		private Action actionWhenFinished;

		// Token: 0x0400C219 RID: 49689
		private Texture2D mouseAttachment;

		// Token: 0x0400C21A RID: 49690
		private Action<LocalTargetInfo> highlightAction;

		// Token: 0x0400C21B RID: 49691
		private Func<LocalTargetInfo, bool> targetValidator;

		// Token: 0x0400C21C RID: 49692
		private bool playSoundOnAction = true;

		// Token: 0x0400C21D RID: 49693
		private bool requiresAvailableVerb = true;

		// Token: 0x0400C21E RID: 49694
		private bool requiresCastedSelected = true;

		// Token: 0x0400C21F RID: 49695
		private Action<LocalTargetInfo> onGuiAction;

		// Token: 0x0400C220 RID: 49696
		private Action<LocalTargetInfo> onUpdateAction;

		// Token: 0x0400C221 RID: 49697
		private bool needsStopTargetingCall;

		// Token: 0x0400C222 RID: 49698
		private bool allowNonSelectedTargetingSource;
	}
}
