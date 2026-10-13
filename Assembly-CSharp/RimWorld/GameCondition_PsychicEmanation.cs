using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02001D4C RID: 7500
	public class GameCondition_PsychicEmanation : GameCondition
	{
		// Token: 0x170019F6 RID: 6646
		// (get) Token: 0x0600A80A RID: 43018 RVA: 0x00301C48 File Offset: 0x002FFE48
		public override string Label
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.label + ": " + this.gender.GetLabel(false).CapitalizeFirst();
				}
				if (this.gender != Gender.None)
				{
					return string.Concat(new string[]
					{
						this.def.label,
						": ",
						this.level.GetLabel().CapitalizeFirst(),
						" (",
						this.gender.GetLabel(false).ToLower(),
						")"
					});
				}
				return this.def.label + ": " + this.level.GetLabel().CapitalizeFirst();
			}
		}

		// Token: 0x170019F7 RID: 6647
		// (get) Token: 0x0600A80B RID: 43019 RVA: 0x00301D0C File Offset: 0x002FFF0C
		public override string LetterText
		{
			get
			{
				if (this.level == PsychicDroneLevel.GoodMedium)
				{
					return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower());
				}
				return this.def.letterText.Formatted(this.gender.GetLabel(false).ToLower(), this.level.GetLabel());
			}
		}

		// Token: 0x170019F8 RID: 6648
		// (get) Token: 0x0600A80C RID: 43020 RVA: 0x00301D89 File Offset: 0x002FFF89
		public override string Description
		{
			get
			{
				return base.Description.Formatted(this.gender.GetLabel(false).ToLower());
			}
		}

		// Token: 0x0600A80D RID: 43021 RVA: 0x00301DB1 File Offset: 0x002FFFB1
		public override void PostMake()
		{
			base.PostMake();
			this.level = this.def.defaultDroneLevel;
		}

		// Token: 0x0600A80E RID: 43022 RVA: 0x00301DCC File Offset: 0x002FFFCC
		public override void RandomizeSettings(float points, Map map, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			if (this.def.defaultDroneLevel == PsychicDroneLevel.GoodMedium)
			{
				this.level = PsychicDroneLevel.GoodMedium;
			}
			else if (points < 800f)
			{
				this.level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				this.level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				this.level = PsychicDroneLevel.BadHigh;
			}
			if (map.mapPawns.FreeColonistsCount > 0)
			{
				this.gender = map.mapPawns.FreeColonists.RandomElement<Pawn>().gender;
			}
			else
			{
				this.gender = Rand.Element<Gender>(Gender.Male, Gender.Female);
			}
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneLevel", this.level.GetLabel()));
			outExtraDescriptionRules.Add(new Rule_String("psychicDroneGender", this.gender.GetLabel(false)));
		}

		// Token: 0x0600A80F RID: 43023 RVA: 0x00301E88 File Offset: 0x00300088
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
			Scribe_Values.Look<PsychicDroneLevel>(ref this.level, "level", PsychicDroneLevel.None, false);
		}

		// Token: 0x0400744B RID: 29771
		public Gender gender;

		// Token: 0x0400744C RID: 29772
		public PsychicDroneLevel level = PsychicDroneLevel.BadMedium;

		// Token: 0x0400744D RID: 29773
		public const float MaxPointsDroneLow = 800f;

		// Token: 0x0400744E RID: 29774
		public const float MaxPointsDroneMedium = 2000f;
	}
}
