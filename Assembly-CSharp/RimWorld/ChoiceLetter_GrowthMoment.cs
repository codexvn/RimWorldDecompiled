using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02003373 RID: 13171
	public class ChoiceLetter_GrowthMoment : LetterWithTimeout
	{
		// Token: 0x17003052 RID: 12370
		// (get) Token: 0x06012837 RID: 75831 RVA: 0x00002501 File Offset: 0x00000701
		public override bool CanDismissWithRightClick
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17003053 RID: 12371
		// (get) Token: 0x06012838 RID: 75832 RVA: 0x00575E6F File Offset: 0x0057406F
		public override bool CanShowInLetterStack
		{
			get
			{
				return !this.ArchiveView && !base.TimeoutPassed;
			}
		}

		// Token: 0x17003054 RID: 12372
		// (get) Token: 0x06012839 RID: 75833 RVA: 0x00575E84 File Offset: 0x00574084
		public bool ArchiveView
		{
			get
			{
				return this.choiceMade || this.pawn.DestroyedOrNull();
			}
		}

		// Token: 0x17003055 RID: 12373
		// (get) Token: 0x0601283A RID: 75834 RVA: 0x00575E9B File Offset: 0x0057409B
		public bool ShowInfoTabs
		{
			get
			{
				return !this.ArchiveView && (!this.passionChoices.NullOrEmpty<SkillDef>() || !this.traitChoices.NullOrEmpty<Trait>());
			}
		}

		// Token: 0x0601283B RID: 75835 RVA: 0x00575EC4 File Offset: 0x005740C4
		public void ConfigureGrowthLetter(Pawn pawn, int passionChoiceCount, int traitChoiceCount, int passionGainsCount, List<string> enabledWorkTypes, Name oldName)
		{
			this.pawn = pawn;
			this.passionChoiceCount = passionChoiceCount;
			this.passionGainsCount = passionGainsCount;
			this.traitChoiceCount = traitChoiceCount;
			this.enabledWorkTypes = enabledWorkTypes;
			this.oldName = oldName;
			this.growthTier = pawn.ageTracker.GrowthTier;
			if (passionGainsCount > passionChoiceCount)
			{
				Log.Error("ConfigureGrowthLetter: passionGainsCount > passionChoiceCount.");
				passionGainsCount = passionChoiceCount;
			}
			this.CacheLetterText();
		}

		// Token: 0x0601283C RID: 75836 RVA: 0x00575F28 File Offset: 0x00574128
		private void CacheLetterText()
		{
			this.text = "BirthdayBiologicalGrowthMoment".Translate(this.pawn, this.pawn.ageTracker.AgeBiologicalYears);
			if (this.pawn.ageTracker.AgeBiologicalYears >= GrowthUtility.GrowthMomentAges[0])
			{
				this.text += "\n\n" + GrowthUtility.GrowthFlavorForTier(this.pawn, this.growthTier);
			}
			if (!this.enabledWorkTypes.NullOrEmpty<string>())
			{
				this.text += "\n\n" + "BirthdayBiologicalAgeWorkTypes".Translate(this.pawn, this.pawn.ageTracker.AgeBiologicalYears) + ":\n" + this.enabledWorkTypes.ToLineList("  - ");
			}
			this.mouseoverText = this.text;
			if (this.pawn.Name != this.oldName)
			{
				TaggedString t = "BirthdayNickname".Translate(this.oldName.ToStringFull.Colorize(ColoredText.NameColor), this.pawn.LabelShort.Colorize(ColoredText.NameColor));
				this.mouseoverText += "\n\n" + t;
			}
			if (this.passionChoiceCount > 0 || this.traitChoiceCount > 0)
			{
				this.mouseoverText += "\n\n" + "BirthdayChooseHowPawnWillGrow".Translate(this.pawn);
			}
		}

		// Token: 0x0601283D RID: 75837 RVA: 0x005760D8 File Offset: 0x005742D8
		public override void OpenLetter()
		{
			this.TrySetChoices();
			Find.MainTabsRoot.EscapeCurrentTab(false);
			Dialog_GrowthMomentChoices window = new Dialog_GrowthMomentChoices(this.text, this);
			Find.WindowStack.Add(window);
		}

		// Token: 0x0601283E RID: 75838 RVA: 0x0057610E File Offset: 0x0057430E
		protected override string GetMouseoverText()
		{
			return this.mouseoverText.Resolve();
		}

		// Token: 0x0601283F RID: 75839 RVA: 0x0057611C File Offset: 0x0057431C
		public void MakeChoices(List<SkillDef> skills, Trait trait)
		{
			if (this.ArchiveView)
			{
				return;
			}
			this.choiceMade = true;
			this.chosenPassions = skills;
			this.chosenTrait = trait;
			if (!skills.NullOrEmpty<SkillDef>())
			{
				foreach (SkillDef skillDef in skills)
				{
					SkillRecord skill = this.pawn.skills.GetSkill(skillDef);
					if (skill.passion == Passion.Major)
					{
						string format = "{0} Tried to upgrade a passion for {1} but it's already major";
						Pawn pawn = this.pawn;
						Log.Warning(string.Format(format, (pawn != null) ? pawn.LabelShort : null, skillDef));
						return;
					}
					skill.passion = skill.passion.IncrementPassion();
					if (ModsConfig.BiotechActive && this.pawn.genes != null)
					{
						foreach (Gene gene in this.pawn.genes.GenesListForReading)
						{
							if (gene.Active && gene.def.passionMod != null && gene.def.passionMod.skill == skill.def && gene.def.passionMod.modType == PassionMod.PassionModType.DropAll)
							{
								Passion? passionPreAdd = gene.passionPreAdd;
								if (passionPreAdd != null)
								{
									if (passionPreAdd.Value == Passion.None)
									{
										gene.passionPreAdd = new Passion?(Passion.Minor);
									}
									else if (passionPreAdd.Value == Passion.Minor)
									{
										gene.passionPreAdd = new Passion?(Passion.Major);
									}
								}
								else
								{
									gene.passionPreAdd = new Passion?(skill.passion);
								}
							}
						}
					}
				}
			}
			if (trait != ChoiceLetter_GrowthMoment.NoTrait && trait != null)
			{
				this.pawn.story.traits.GainTrait(trait, false);
				TraitUtility.ApplySkillGainFromTrait(this.pawn, trait);
			}
			if (this.pawn.ageTracker.AgeBiologicalYears == 13)
			{
				PawnGenerator.TryGenerateSexualityTraitFor(this.pawn, true);
			}
			this.pawn.ageTracker.growthPoints = 0f;
			this.pawn.ageTracker.canGainGrowthPoints = true;
		}

		// Token: 0x06012840 RID: 75840 RVA: 0x00576378 File Offset: 0x00574578
		public static List<SkillDef> PassionOptions(Pawn pawn, int count, bool checkGenes)
		{
			return (from s in DefDatabase<SkillDef>.AllDefsListForReading
			where ChoiceLetter_GrowthMoment.IsValidGrowthPassionOption(pawn, s, checkGenes)
			select s).InRandomOrder(null).Take(count).ToList<SkillDef>();
		}

		// Token: 0x06012841 RID: 75841 RVA: 0x005763C0 File Offset: 0x005745C0
		private void TrySetChoices()
		{
			if (this.choiceMade || this.pawn.DestroyedOrNull())
			{
				return;
			}
			if (this.passionChoiceCount > 0 && this.passionChoices == null)
			{
				this.passionChoices = ChoiceLetter_GrowthMoment.PassionOptions(this.pawn, this.passionChoiceCount, false);
			}
			if (this.traitChoiceCount > 0 && this.traitChoices == null)
			{
				if (this.traitChoiceCount > 2 && Rand.Value < 0.35f)
				{
					this.traitChoiceCount--;
					this.noTraitOptionShown = true;
				}
				this.traitChoices = PawnGenerator.GenerateTraitsFor(this.pawn, this.traitChoiceCount, null, true);
			}
		}

		// Token: 0x06012842 RID: 75842 RVA: 0x00576468 File Offset: 0x00574668
		private static bool IsValidGrowthPassionOption(Pawn pawn, SkillDef skill, bool checkGenes)
		{
			SkillRecord skill2 = pawn.skills.GetSkill(skill);
			if (checkGenes && pawn.genes != null)
			{
				foreach (Gene gene in pawn.genes.GenesListForReading)
				{
					if (gene.Active && gene.def.passionMod != null && gene.def.passionMod.modType == PassionMod.PassionModType.DropAll && gene.def.passionMod.skill == skill)
					{
						return false;
					}
				}
			}
			return !skill2.PermanentlyDisabled && skill2.passion != Passion.Major;
		}

		// Token: 0x06012843 RID: 75843 RVA: 0x00576528 File Offset: 0x00574728
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.pawn, "pawn", false);
			Scribe_Collections.Look<Trait>(ref this.traitChoices, "traitChoices", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.growthTier, "growthTier", 0, false);
			Scribe_Collections.Look<SkillDef>(ref this.passionChoices, "passionChoices", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.enabledWorkTypes, "enabledWorkTypes", LookMode.Undefined, Array.Empty<object>());
			Scribe_Collections.Look<SkillDef>(ref this.chosenPassions, "chosenPassions", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<Trait>(ref this.chosenTrait, "chosenTrait", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.passionChoiceCount, "passionChoiceCount", 0, false);
			Scribe_Values.Look<int>(ref this.passionGainsCount, "passionGainsCount", 0, false);
			Scribe_Values.Look<int>(ref this.traitChoiceCount, "traitChoiceCount", 0, false);
			Scribe_Values.Look<TaggedString>(ref this.text, "text", default(TaggedString), false);
			Scribe_Values.Look<TaggedString>(ref this.mouseoverText, "mouseoverText", default(TaggedString), false);
			Scribe_Values.Look<bool>(ref this.choiceMade, "choiceMade", false, false);
			Scribe_Deep.Look<Name>(ref this.oldName, "oldName", Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.noTraitOptionShown, "noTraitOptionShown", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				List<Trait> list = this.traitChoices;
				if (list != null)
				{
					list.RemoveAll((Trait x) => x == null);
				}
				List<Trait> list2 = this.traitChoices;
				if (list2 != null)
				{
					list2.RemoveAll((Trait x) => x.def == null);
				}
				List<SkillDef> list3 = this.passionChoices;
				if (list3 == null)
				{
					return;
				}
				list3.RemoveAll((SkillDef x) => x == null);
			}
		}

		// Token: 0x0400C04B RID: 49227
		public const float ChanceForNoTraitOption = 0.35f;

		// Token: 0x0400C04C RID: 49228
		public static Trait NoTrait = new Trait();

		// Token: 0x0400C04D RID: 49229
		public Pawn pawn;

		// Token: 0x0400C04E RID: 49230
		public TaggedString text;

		// Token: 0x0400C04F RID: 49231
		public TaggedString mouseoverText;

		// Token: 0x0400C050 RID: 49232
		private int passionChoiceCount;

		// Token: 0x0400C051 RID: 49233
		public int traitChoiceCount;

		// Token: 0x0400C052 RID: 49234
		public int passionGainsCount;

		// Token: 0x0400C053 RID: 49235
		public List<SkillDef> passionChoices;

		// Token: 0x0400C054 RID: 49236
		public List<Trait> traitChoices;

		// Token: 0x0400C055 RID: 49237
		public bool noTraitOptionShown;

		// Token: 0x0400C056 RID: 49238
		public Trait chosenTrait;

		// Token: 0x0400C057 RID: 49239
		public List<SkillDef> chosenPassions;

		// Token: 0x0400C058 RID: 49240
		public List<string> enabledWorkTypes;

		// Token: 0x0400C059 RID: 49241
		public bool choiceMade;

		// Token: 0x0400C05A RID: 49242
		public Name oldName;

		// Token: 0x0400C05B RID: 49243
		public int growthTier;
	}
}
