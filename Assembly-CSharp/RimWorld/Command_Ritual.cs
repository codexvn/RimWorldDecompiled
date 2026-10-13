using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02002685 RID: 9861
	[StaticConstructorOnStartup]
	public class Command_Ritual : Command
	{
		// Token: 0x17002142 RID: 8514
		// (get) Token: 0x0600D8A9 RID: 55465 RVA: 0x00403503 File Offset: 0x00401703
		private static Texture2D PenaltyArrowTex
		{
			get
			{
				if (Command_Ritual.penaltyArrowTex == null)
				{
					Command_Ritual.penaltyArrowTex = ContentFinder<Texture2D>.Get("UI/Icons/Rituals/QualityPenalty", true);
				}
				return Command_Ritual.penaltyArrowTex;
			}
		}

		// Token: 0x0600D8AA RID: 55466 RVA: 0x00403528 File Offset: 0x00401728
		public Command_Ritual(Precept_Ritual ritual, TargetInfo targetInfo, RitualObligation forObligation = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			this.ritual = ritual;
			this.targetInfo = targetInfo;
			this.forcedForRole = forcedForRole;
			this.obligation = forObligation;
			this.defaultLabel = ritual.GetBeginRitualText(this.obligation);
			this.defaultDesc = ritual.def.description;
			foreach (RitualObligationTrigger ritualObligationTrigger in ritual.obligationTriggers)
			{
				if (ritualObligationTrigger.TriggerExtraDesc != null)
				{
					this.defaultDesc = this.defaultDesc + "\n\n" + ritualObligationTrigger.TriggerExtraDesc;
				}
			}
			this.groupKey = (ritual.canMergeGizmosFromDifferentIdeos ? -1 : ritual.ideo.id);
			this.icon = ritual.Icon;
			if (!ritual.def.mergeRitualGizmosFromAllIdeos && !ritual.def.iconIgnoresIdeoColor)
			{
				this.defaultIconColor = ritual.ideo.Color;
			}
			if (!this.disabled)
			{
				this.ValidateDisabledState();
			}
		}

		// Token: 0x17002143 RID: 8515
		// (get) Token: 0x0600D8AB RID: 55467 RVA: 0x00403654 File Offset: 0x00401854
		public override string Desc
		{
			get
			{
				return this.ritual.TipLabel.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.ritual.TipMainPart();
			}
		}

		// Token: 0x17002144 RID: 8516
		// (get) Token: 0x0600D8AC RID: 55468 RVA: 0x00403680 File Offset: 0x00401880
		public override string DescPostfix
		{
			get
			{
				return this.ritual.TipExtraPart();
			}
		}

		// Token: 0x0600D8AD RID: 55469 RVA: 0x00403690 File Offset: 0x00401890
		public override void DrawIcon(Rect rect, Material buttonMat, GizmoRenderParms parms)
		{
			base.DrawIcon(rect, buttonMat, parms);
			if (this.ritual.RepeatPenaltyActive)
			{
				float value = Mathf.InverseLerp(1200000f, 0f, (float)this.ritual.TicksSinceLastPerformed);
				Widgets.FillableBar(rect.ContractedBy(1f), Mathf.Clamp01(value), Command_Ritual.CooldownBarTex, null, false);
				Text.Font = GameFont.Tiny;
				Text.Anchor = TextAnchor.UpperCenter;
				float num = (float)(1200000 - this.ritual.TicksSinceLastPerformed) / 60000f;
				if (num >= 1f)
				{
					num = (float)Mathf.RoundToInt(num);
				}
				else
				{
					num = (float)((int)(num * 10f)) / 10f;
				}
				Widgets.Label(rect, "PeriodDays".Translate(num));
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.DrawTexture(new Rect(rect.xMax - (float)this.PenaltyIconSize.x, rect.yMin + 4f, (float)this.PenaltyIconSize.x, (float)this.PenaltyIconSize.z), Command_Ritual.PenaltyArrowTex);
			}
		}

		// Token: 0x0600D8AE RID: 55470 RVA: 0x0040379B File Offset: 0x0040199B
		protected override GizmoResult GizmoOnGUIInt(Rect butRect, GizmoRenderParms parms)
		{
			if (!this.disabled)
			{
				this.ValidateDisabledState();
			}
			return base.GizmoOnGUIInt(butRect, parms);
		}

		// Token: 0x0600D8AF RID: 55471 RVA: 0x004037B4 File Offset: 0x004019B4
		private void ValidateDisabledState()
		{
			string text = this.ritual.behavior.CanStartRitualNow(this.targetInfo, this.ritual, null, null);
			if (!text.NullOrEmpty())
			{
				this.disabled = true;
				this.disabledReason = text;
				return;
			}
			if (this.ritual.abilityOnCooldownUntilTick > Find.TickManager.TicksGame)
			{
				this.disabled = true;
				this.disabledReason = "AbilityOnCooldown".Translate((this.ritual.abilityOnCooldownUntilTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Resolve();
				return;
			}
			if (this.ritual.def.sourcePawnRoleDef != null && this.ritual.def.sourceAbilityDef != null)
			{
				Precept_Role precept_Role = this.ritual.ideo.RolesListForReading.FirstOrDefault((Precept_Role r) => r.def == this.ritual.def.sourcePawnRoleDef);
				if (precept_Role != null)
				{
					Precept_RoleSingle precept_RoleSingle = precept_Role as Precept_RoleSingle;
					if (precept_RoleSingle != null && precept_RoleSingle.ChosenPawnSingle() != null)
					{
						Ability ability = precept_RoleSingle.AbilitiesForReading.FirstOrDefault((Ability a) => a.def == this.ritual.def.sourceAbilityDef);
						if (ability != null)
						{
							this.disabled = ability.GizmoDisabled(out this.disabledReason);
						}
					}
				}
			}
		}

		// Token: 0x0600D8B0 RID: 55472 RVA: 0x004038DE File Offset: 0x00401ADE
		public override void GizmoUpdateOnMouseover()
		{
			base.GizmoUpdateOnMouseover();
			RitualBehaviorWorker behavior = this.ritual.behavior;
			if (behavior == null)
			{
				return;
			}
			behavior.DrawPreviewOnTarget(this.targetInfo);
		}

		// Token: 0x0600D8B1 RID: 55473 RVA: 0x00403901 File Offset: 0x00401B01
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			this.ritual.ShowRitualBeginWindow(this.targetInfo, null, null, this.forcedForRole);
			SoundDefOf.Tick_Tiny.PlayOneShotOnCamera(null);
		}

		// Token: 0x040092E0 RID: 37600
		private Precept_Ritual ritual;

		// Token: 0x040092E1 RID: 37601
		private RitualObligation obligation;

		// Token: 0x040092E2 RID: 37602
		private TargetInfo targetInfo;

		// Token: 0x040092E3 RID: 37603
		private Dictionary<string, Pawn> forcedForRole;

		// Token: 0x040092E4 RID: 37604
		private readonly IntVec2 PenaltyIconSize = new IntVec2(16, 16);

		// Token: 0x040092E5 RID: 37605
		private static readonly Texture2D CooldownBarTex = SolidColorMaterials.NewSolidColorTexture(new Color32(170, 150, 0, 60));

		// Token: 0x040092E6 RID: 37606
		private static Texture2D penaltyArrowTex;
	}
}
