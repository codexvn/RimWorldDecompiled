using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020021EB RID: 8683
	public class Command_AbilitySpeech : Command_Ability
	{
		// Token: 0x17001D03 RID: 7427
		// (get) Token: 0x0600BE3E RID: 48702 RVA: 0x00372CC4 File Offset: 0x00370EC4
		protected Precept_Ritual Ritual
		{
			get
			{
				if (this.ritualCached == null)
				{
					CompAbilityEffect_StartRitual compAbilityEffect_StartRitual = this.ability.CompOfType<CompAbilityEffect_StartRitual>();
					if (compAbilityEffect_StartRitual != null)
					{
						this.ritualCached = compAbilityEffect_StartRitual.Ritual;
					}
				}
				return this.ritualCached;
			}
		}

		// Token: 0x17001D04 RID: 7428
		// (get) Token: 0x0600BE3F RID: 48703 RVA: 0x00372CFC File Offset: 0x00370EFC
		public override string Tooltip
		{
			get
			{
				if (this.Ritual == null)
				{
					return "";
				}
				string text = this.Ritual.Label.Colorize(ColoredText.TipSectionTitleColor) + "\n\n" + this.Ritual.def.description.Formatted(this.ability.pawn.Named("ORGANIZER")).Resolve() + "\n";
				if (this.ability.CooldownTicksRemaining > 0)
				{
					text = string.Concat(new string[]
					{
						text,
						"\n",
						"AbilitySpeechCooldown".Translate().Resolve(),
						": ",
						this.ability.CooldownTicksRemaining.ToStringTicksToPeriod(true, false, true, true, false)
					});
				}
				if (this.Ritual.outcomeEffect != null)
				{
					text = text + "\n" + this.Ritual.outcomeEffect.ExtraAlertParagraph(this.Ritual);
				}
				text = string.Concat(new string[]
				{
					text,
					"\n\n",
					("AbilitySpeechTargetsLabel".Translate().Resolve() + ":").Colorize(ColoredText.TipSectionTitleColor),
					"\n",
					this.Ritual.targetFilter.GetTargetInfos(this.ability.pawn).ToLineList(" -  ", true)
				});
				return text.CapitalizeFirst();
			}
		}

		// Token: 0x0600BE40 RID: 48704 RVA: 0x00372E78 File Offset: 0x00371078
		public Command_AbilitySpeech(Ability ability, Pawn pawn) : base(ability, pawn)
		{
			Precept_Ritual ritual = this.Ritual;
			this.defaultLabel = ((ritual != null) ? ritual.GetBeginRitualText(null) : "");
			this.icon = ((ability.def.iconPath == null) ? this.Ritual.Icon : ContentFinder<Texture2D>.Get(ability.def.iconPath, true));
		}

		// Token: 0x040082B6 RID: 33462
		private Precept_Ritual ritualCached;
	}
}
