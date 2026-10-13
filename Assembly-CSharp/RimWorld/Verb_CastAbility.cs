using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020022BB RID: 8891
	public class Verb_CastAbility : Verb, IAbilityVerb
	{
		// Token: 0x17001D9B RID: 7579
		// (get) Token: 0x0600C161 RID: 49505 RVA: 0x0037FE91 File Offset: 0x0037E091
		// (set) Token: 0x0600C162 RID: 49506 RVA: 0x0037FE99 File Offset: 0x0037E099
		public Ability Ability
		{
			get
			{
				return this.ability;
			}
			set
			{
				this.ability = value;
			}
		}

		// Token: 0x17001D9C RID: 7580
		// (get) Token: 0x0600C163 RID: 49507 RVA: 0x0037FEA2 File Offset: 0x0037E0A2
		public static Color RadiusHighlightColor
		{
			get
			{
				return new Color(0.3f, 0.8f, 1f);
			}
		}

		// Token: 0x17001D9D RID: 7581
		// (get) Token: 0x0600C164 RID: 49508 RVA: 0x0037FEB8 File Offset: 0x0037E0B8
		public override string ReportLabel
		{
			get
			{
				return this.ability.def.label;
			}
		}

		// Token: 0x17001D9E RID: 7582
		// (get) Token: 0x0600C165 RID: 49509 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool MultiSelect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001D9F RID: 7583
		// (get) Token: 0x0600C166 RID: 49510 RVA: 0x0037FECC File Offset: 0x0037E0CC
		public override bool HidePawnTooltips
		{
			get
			{
				using (List<CompAbilityEffect>.Enumerator enumerator = this.ability.EffectComps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.HideTargetPawnTooltip)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17001DA0 RID: 7584
		// (get) Token: 0x0600C167 RID: 49511 RVA: 0x0037FF2C File Offset: 0x0037E12C
		public override ITargetingSource DestinationSelector
		{
			get
			{
				CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
				if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
				{
					return compAbilityEffect_WithDest;
				}
				return null;
			}
		}

		// Token: 0x0600C168 RID: 49512 RVA: 0x0037FF59 File Offset: 0x0037E159
		protected override bool TryCastShot()
		{
			return this.ability.Activate(this.currentTarget, this.currentDestination);
		}

		// Token: 0x0600C169 RID: 49513 RVA: 0x0037FF74 File Offset: 0x0037E174
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			CompAbilityEffect_WithDest compAbilityEffect_WithDest = this.ability.CompOfType<CompAbilityEffect_WithDest>();
			if (compAbilityEffect_WithDest != null && compAbilityEffect_WithDest.Props.destination == AbilityEffectDestination.Selected)
			{
				compAbilityEffect_WithDest.SetTarget(target);
				return;
			}
			this.ability.QueueCastingJob(target, null);
		}

		// Token: 0x0600C16A RID: 49514 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			return true;
		}

		// Token: 0x0600C16B RID: 49515 RVA: 0x0037FFB8 File Offset: 0x0037E1B8
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (this.EffectiveRange > 0f)
			{
				if (!this.CanHitTarget(target))
				{
					if (target.IsValid && showMessages)
					{
						string t = "CannotUseAbility".Translate(this.ability.def.label) + ": ";
						if (this.ability.verb.OutOfRange(this.ability.pawn.Position, target, target.HasThing ? target.Thing.OccupiedRect() : CellRect.SingleCell(target.Cell)))
						{
							Messages.Message(t + "AbilityOutOfRange".Translate(), new LookTargets(new TargetInfo[]
							{
								this.ability.pawn,
								target.ToTargetInfo(this.ability.pawn.Map)
							}), MessageTypeDefOf.RejectInput, false);
						}
						else if (this.ability.pawn.Spawned)
						{
							Messages.Message(t + "AbilityCannotHitTarget".Translate(), new LookTargets(new TargetInfo[]
							{
								this.ability.pawn,
								target.ToTargetInfo(this.ability.pawn.Map)
							}), MessageTypeDefOf.RejectInput, false);
						}
					}
					return false;
				}
			}
			else if (!this.ability.pawn.CanReach(target, PathEndMode.Touch, this.ability.pawn.NormalMaxDanger(), false, false, TraverseMode.ByPawn))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message("CannotUseAbility".Translate(this.ability.def.label) + ": " + "AbilityCannotReachTarget".Translate(), new LookTargets(new TargetInfo[]
					{
						this.ability.pawn,
						target.ToTargetInfo(this.ability.pawn.Map)
					}), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			if (!this.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			for (int i = 0; i < this.ability.EffectComps.Count; i++)
			{
				if (!this.ability.EffectComps[i].Valid(target, showMessages))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600C16C RID: 49516 RVA: 0x00380240 File Offset: 0x0037E440
		public override bool CanHitTarget(LocalTargetInfo targ)
		{
			return this.EffectiveRange <= 0f || base.CanHitTarget(targ);
		}

		// Token: 0x0600C16D RID: 49517 RVA: 0x00380258 File Offset: 0x0037E458
		public override void OnGUI(LocalTargetInfo target)
		{
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false) && this.ValidateTarget(target, false))
			{
				base.OnGUI(target);
			}
			else
			{
				GenUI.DrawMouseAttachment(TexCommand.CannotShoot);
			}
			this.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x0600C16E RID: 49518 RVA: 0x00380294 File Offset: 0x0037E494
		protected void DrawAttachmentExtraLabel(LocalTargetInfo target)
		{
			foreach (CompAbilityEffect compAbilityEffect in this.ability.EffectComps)
			{
				string text = compAbilityEffect.ExtraLabelMouseAttachment(target);
				if (!text.NullOrEmpty())
				{
					Widgets.MouseAttachedLabel(text, 0f, 0f, null);
					break;
				}
			}
		}

		// Token: 0x0600C16F RID: 49519 RVA: 0x00380310 File Offset: 0x0037E510
		public override bool TryStartCastOn(LocalTargetInfo castTarg, LocalTargetInfo destTarg, bool surpriseAttack = false, bool canHitNonTargetPawns = true, bool preventFriendlyFire = false, bool nonInterruptingSelfCast = false)
		{
			bool flag = base.TryStartCastOn(castTarg, destTarg, surpriseAttack, canHitNonTargetPawns, preventFriendlyFire, nonInterruptingSelfCast);
			if (flag && this.ability.def.stunTargetWhileCasting && this.ability.def.verbProperties.warmupTime > 0f)
			{
				Pawn pawn = castTarg.Thing as Pawn;
				if (pawn != null && pawn != this.ability.pawn)
				{
					pawn.stances.stunner.StunFor(this.ability.def.verbProperties.warmupTime.SecondsToTicks(), this.ability.pawn, false, false, false);
					if (!pawn.Awake())
					{
						RestUtility.WakeUp(pawn, true);
					}
				}
			}
			return flag;
		}

		// Token: 0x0600C170 RID: 49520 RVA: 0x003803C8 File Offset: 0x0037E5C8
		public override void DrawHighlight(LocalTargetInfo target)
		{
			AbilityDef def = this.ability.def;
			if (this.EffectiveRange > 0f)
			{
				this.verbProps.DrawRadiusRing(this.caster.Position, this);
			}
			if (this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				if (def.HasAreaOfEffect)
				{
					if (target.IsValid)
					{
						GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
						GenDraw.DrawRadiusRing(target.Cell, def.EffectRadius, Verb_CastAbility.RadiusHighlightColor, null);
					}
				}
				else
				{
					GenDraw.DrawTargetHighlightWithLayer(target.CenterVector3, AltitudeLayer.MetaOverlays);
				}
			}
			if (target.IsValid)
			{
				this.ability.DrawEffectPreviews(target);
			}
		}

		// Token: 0x17001DA1 RID: 7585
		// (get) Token: 0x0600C171 RID: 49521 RVA: 0x00380474 File Offset: 0x0037E674
		public override Texture2D UIIcon
		{
			get
			{
				return this.ability.def.uiIcon;
			}
		}

		// Token: 0x0600C172 RID: 49522 RVA: 0x00380486 File Offset: 0x0037E686
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Ability>(ref this.ability, "ability", false);
		}

		// Token: 0x0400843C RID: 33852
		public Ability ability;
	}
}
