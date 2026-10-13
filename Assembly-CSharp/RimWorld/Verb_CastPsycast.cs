using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020022BF RID: 8895
	public class Verb_CastPsycast : Verb_CastAbility
	{
		// Token: 0x17001DA6 RID: 7590
		// (get) Token: 0x0600C185 RID: 49541 RVA: 0x00380765 File Offset: 0x0037E965
		public Psycast Psycast
		{
			get
			{
				return this.ability as Psycast;
			}
		}

		// Token: 0x0600C186 RID: 49542 RVA: 0x00380774 File Offset: 0x0037E974
		public override bool IsApplicableTo(LocalTargetInfo target, bool showMessages = false)
		{
			if (!ModLister.CheckRoyalty("Psycast"))
			{
				return false;
			}
			if (!base.IsApplicableTo(target, showMessages))
			{
				return false;
			}
			if (!this.Psycast.def.HasAreaOfEffect && !this.Psycast.CanApplyPsycastTo(target))
			{
				if (showMessages)
				{
					Messages.Message(this.ability.def.LabelCap + ": " + "AbilityTargetPsychicallyDeaf".Translate(), target.ToTargetInfo(this.ability.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600C187 RID: 49543 RVA: 0x00380816 File Offset: 0x0037EA16
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			if (!this.IsApplicableTo(target, false))
			{
				return;
			}
			base.OrderForceTarget(target);
		}

		// Token: 0x0600C188 RID: 49544 RVA: 0x0038082C File Offset: 0x0037EA2C
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.ValidateTarget(target, showMessages))
			{
				return false;
			}
			bool flag = this.ability.EffectComps.All((CompAbilityEffect e) => e.Props.canTargetBosses);
			Pawn pawn = target.Pawn;
			if (!flag && pawn != null && pawn.kindDef.isBoss)
			{
				Messages.Message("CommandPsycastInsanityImmune".Translate(), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.CasterPawn.psychicEntropy.PsychicSensitivity < 1E-45f)
			{
				Messages.Message("CommandPsycastZeroPsychicSensitivity".Translate(), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			if (this.Psycast.def.EntropyGain > 1E-45f && this.CasterPawn.psychicEntropy.WouldOverflowEntropy(this.Psycast.def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(this.CasterPawn)))
			{
				Messages.Message("CommandPsycastWouldExceedEntropy".Translate(), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			float num = this.Psycast.FinalPsyfocusCost(target);
			float num2 = PsycastUtility.TotalPsyfocusCostOfQueuedPsycasts(this.CasterPawn);
			float num3 = num + num2;
			if (num > 1E-45f && num3 > this.CasterPawn.psychicEntropy.CurrentPsyfocus + 0.0005f)
			{
				Messages.Message("CommandPsycastNotEnoughPsyfocus".Translate(num3.ToStringPercent("0.#"), (this.CasterPawn.psychicEntropy.CurrentPsyfocus - num2).ToStringPercent("0.#"), this.Psycast.def.label.Named("PSYCASTNAME"), this.caster.Named("CASTERNAME")), this.caster, MessageTypeDefOf.RejectInput, false);
				return false;
			}
			return true;
		}

		// Token: 0x0600C189 RID: 49545 RVA: 0x00380A24 File Offset: 0x0037EC24
		public override void OnGUI(LocalTargetInfo target)
		{
			bool flag = this.ability.EffectComps.Any((CompAbilityEffect e) => e.Props.psychic);
			bool flag2 = this.ability.EffectComps.All((CompAbilityEffect e) => e.Props.canTargetBosses);
			Texture2D texture2D = this.UIIcon;
			if (!this.Psycast.CanApplyPsycastTo(target))
			{
				texture2D = TexCommand.CannotShoot;
				this.DrawIneffectiveWarning(target, null);
			}
			if (target.IsValid && this.CanHitTarget(target) && this.IsApplicableTo(target, false))
			{
				foreach (LocalTargetInfo target2 in this.ability.GetAffectedTargets(target))
				{
					if (!flag2 && target2.Pawn.kindDef.isBoss)
					{
						this.DrawIneffectiveWarning(target2, "IneffectivePsychicImmune".Translate());
					}
					else if (flag)
					{
						if (this.Psycast.CanApplyPsycastTo(target2))
						{
							this.DrawSensitivityStat(target2);
						}
						else
						{
							this.DrawIneffectiveWarning(target2, null);
						}
					}
				}
				if (this.ability.EffectComps.Any((CompAbilityEffect e) => !e.Valid(target, false)))
				{
					texture2D = TexCommand.CannotShoot;
				}
			}
			else
			{
				texture2D = TexCommand.CannotShoot;
			}
			if (ThingRequiringRoyalPermissionUtility.IsViolatingRulesOfAnyFaction(HediffDefOf.PsychicAmplifier, this.CasterPawn, this.Psycast.def.level) && this.Psycast.def.DetectionChance > 0f)
			{
				TaggedString taggedString = "Illegal".Translate().ToUpper() + "\n" + this.Psycast.def.DetectionChance.ToStringPercent() + " " + "DetectionChance".Translate();
				Text.Font = GameFont.Small;
				Texture iconTex = texture2D;
				string text = taggedString;
				float angle = 0f;
				Color textBgColor = new Color(0.25f, 0f, 0f);
				GenUI.DrawMouseAttachment(iconTex, text, angle, default(Vector2), null, null, true, textBgColor, null, null);
			}
			else
			{
				GenUI.DrawMouseAttachment(texture2D);
			}
			base.DrawAttachmentExtraLabel(target);
		}

		// Token: 0x0600C18A RID: 49546 RVA: 0x00380CC0 File Offset: 0x0037EEC0
		private void DrawIneffectiveWarning(LocalTargetInfo target, string text = null)
		{
			Verb_CastPsycast.DrawIneffectiveWarningStatic(target, text);
		}

		// Token: 0x0600C18B RID: 49547 RVA: 0x00380CCC File Offset: 0x0037EECC
		public static void DrawIneffectiveWarningStatic(LocalTargetInfo target, string text = null)
		{
			if (target.Pawn != null)
			{
				Vector3 drawPos = target.Pawn.DrawPos;
				drawPos.z += 1f;
				if (text == null)
				{
					text = "Ineffective".Translate();
				}
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), text, Color.red);
			}
		}

		// Token: 0x0600C18C RID: 49548 RVA: 0x00380D30 File Offset: 0x0037EF30
		private void DrawSensitivityStat(LocalTargetInfo target)
		{
			if (target.Pawn != null && !target.Pawn.IsHiddenFromPlayer())
			{
				Pawn pawn = target.Pawn;
				float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
				Vector3 drawPos = pawn.DrawPos;
				drawPos.z += 1f;
				GenMapUI.DrawText(new Vector2(drawPos.x, drawPos.z), StatDefOf.PsychicSensitivity.LabelCap + ": " + statValue.ToString(), (statValue > float.Epsilon) ? Color.white : Color.red);
			}
		}

		// Token: 0x0400843E RID: 33854
		private const float StatLabelOffsetY = 1f;
	}
}
