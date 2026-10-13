using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020021F0 RID: 8688
	public abstract class CompAbilityEffect : AbilityComp
	{
		// Token: 0x17001D06 RID: 7430
		// (get) Token: 0x0600BE46 RID: 48710 RVA: 0x0037310A File Offset: 0x0037130A
		public CompProperties_AbilityEffect Props
		{
			get
			{
				return (CompProperties_AbilityEffect)this.props;
			}
		}

		// Token: 0x17001D07 RID: 7431
		// (get) Token: 0x0600BE47 RID: 48711 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool HideTargetPawnTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D08 RID: 7432
		// (get) Token: 0x0600BE48 RID: 48712 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldHideGizmo
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001D09 RID: 7433
		// (get) Token: 0x0600BE49 RID: 48713 RVA: 0x00373117 File Offset: 0x00371317
		protected bool SendLetter
		{
			get
			{
				return this.Props.sendLetter && !this.Props.customLetterText.NullOrEmpty() && !this.Props.customLetterLabel.NullOrEmpty();
			}
		}

		// Token: 0x0600BE4A RID: 48714 RVA: 0x00373150 File Offset: 0x00371350
		public virtual void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.Props.screenShakeIntensity > 1E-45f)
			{
				Find.CameraDriver.shaker.DoShake(this.Props.screenShakeIntensity);
			}
			Pawn pawn = this.parent.pawn;
			Pawn pawn2 = target.Pawn;
			if (pawn2 != null && !pawn2.IsSlaveOfColony)
			{
				Faction homeFaction = pawn2.HomeFaction;
				if (this.Props.goodwillImpact != 0 && pawn.Faction == Faction.OfPlayer && homeFaction != null && !homeFaction.HostileTo(pawn.Faction) && (this.Props.applyGoodwillImpactToLodgers || !pawn2.IsQuestLodger()) && !pawn2.IsQuestHelper())
				{
					Faction.OfPlayer.TryAffectGoodwillWith(homeFaction, this.Props.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			if (this.Props.clamorType != null)
			{
				GenClamor.DoClamor(this.parent.pawn, target.Cell, (float)this.Props.clamorRadius, this.Props.clamorType);
			}
			Gender gender = pawn.gender;
			SoundDef soundDef;
			if (gender != Gender.Male)
			{
				if (gender != Gender.Female)
				{
					soundDef = this.Props.sound;
				}
				else
				{
					soundDef = (this.Props.soundFemale ?? this.Props.sound);
				}
			}
			else
			{
				soundDef = (this.Props.soundMale ?? this.Props.sound);
			}
			if (soundDef != null)
			{
				soundDef.PlayOneShot(new TargetInfo(target.Cell, this.parent.pawn.Map, false));
			}
			if (!this.Props.message.NullOrEmpty())
			{
				Messages.Message(this.Props.message, this.parent.pawn, this.Props.messageType ?? MessageTypeDefOf.SilentInput, true);
			}
		}

		// Token: 0x0600BE4B RID: 48715 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Apply(GlobalTargetInfo target)
		{
		}

		// Token: 0x0600BE4C RID: 48716 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool AICanTargetNow(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x0600BE4D RID: 48717 RVA: 0x00373328 File Offset: 0x00371528
		public virtual bool CanApplyOn(LocalTargetInfo target, LocalTargetInfo dest)
		{
			return this.Props.availableWhenTargetIsWounded || (target.Pawn.health.hediffSet.BleedRateTotal <= 0f && !target.Pawn.health.HasHediffsNeedingTend(false));
		}

		// Token: 0x0600BE4E RID: 48718 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool CanApplyOn(GlobalTargetInfo target)
		{
			return true;
		}

		// Token: 0x0600BE4F RID: 48719 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostApplied(List<LocalTargetInfo> targets, Map map)
		{
		}

		// Token: 0x0600BE50 RID: 48720 RVA: 0x00373376 File Offset: 0x00371576
		public virtual IEnumerable<PreCastAction> GetPreCastActions()
		{
			return Enumerable.Empty<PreCastAction>();
		}

		// Token: 0x0600BE51 RID: 48721 RVA: 0x0037337D File Offset: 0x0037157D
		public virtual IEnumerable<Mote> CustomWarmupMotes(LocalTargetInfo target)
		{
			return Enumerable.Empty<Mote>();
		}

		// Token: 0x0600BE52 RID: 48722 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void DrawEffectPreview(LocalTargetInfo target)
		{
		}

		// Token: 0x0600BE53 RID: 48723 RVA: 0x00373384 File Offset: 0x00371584
		public virtual bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			if (pawn != null)
			{
				if (!this.Props.canTargetBaby && !AbilityUtility.ValidateMustNotBeBaby(pawn, throwMessages, this.parent))
				{
					return false;
				}
				if (!this.Props.canTargetBosses && pawn.kindDef.isBoss)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600BE54 RID: 48724 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool Valid(GlobalTargetInfo target, bool throwMessages = false)
		{
			return true;
		}

		// Token: 0x0600BE55 RID: 48725 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExtraLabelMouseAttachment(LocalTargetInfo target)
		{
			return null;
		}

		// Token: 0x0600BE56 RID: 48726 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string WorldMapExtraLabel(GlobalTargetInfo target)
		{
			return null;
		}

		// Token: 0x0600BE57 RID: 48727 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Window ConfirmationDialog(LocalTargetInfo target, Action confirmAction)
		{
			return null;
		}

		// Token: 0x0600BE58 RID: 48728 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual Window ConfirmationDialog(GlobalTargetInfo target, Action confirmAction)
		{
			return null;
		}

		// Token: 0x0600BE59 RID: 48729 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual string ExtraTooltipPart()
		{
			return null;
		}

		// Token: 0x0600BE5A RID: 48730 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void OnGizmoUpdate()
		{
		}
	}
}
