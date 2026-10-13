using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020019F1 RID: 6641
	public class BackstoryDef : Def
	{
		// Token: 0x170016D8 RID: 5848
		// (get) Token: 0x060095E8 RID: 38376 RVA: 0x002BE2F5 File Offset: 0x002BC4F5
		public bool IsPlayerColonyChildBackstory
		{
			get
			{
				List<string> list = this.spawnCategories;
				return list != null && list.Contains("Child");
			}
		}

		// Token: 0x170016D9 RID: 5849
		// (get) Token: 0x060095E9 RID: 38377 RVA: 0x002BE310 File Offset: 0x002BC510
		public List<WorkTypeDef> DisabledWorkTypes
		{
			get
			{
				if (this.cachedDisabledWorkTypes == null)
				{
					this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
					List<WorkTypeDef> allDefsListForReading = DefDatabase<WorkTypeDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						if (!this.AllowsWorkType(allDefsListForReading[i]))
						{
							this.cachedDisabledWorkTypes.Add(allDefsListForReading[i]);
						}
					}
				}
				return this.cachedDisabledWorkTypes;
			}
		}

		// Token: 0x170016DA RID: 5850
		// (get) Token: 0x060095EA RID: 38378 RVA: 0x002BE36E File Offset: 0x002BC56E
		public IEnumerable<WorkGiverDef> DisabledWorkGivers
		{
			get
			{
				List<WorkGiverDef> list = DefDatabase<WorkGiverDef>.AllDefsListForReading;
				int num;
				for (int i = 0; i < list.Count; i = num + 1)
				{
					if (!this.AllowsWorkGiver(list[i]))
					{
						yield return list[i];
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x060095EB RID: 38379 RVA: 0x002BE380 File Offset: 0x002BC580
		public override void PostLoad()
		{
			base.PostLoad();
			this.untranslatedTitle = this.title;
			this.untranslatedTitleFemale = this.titleFemale;
			this.untranslatedTitleShort = this.titleShort;
			this.untranslatedTitleShortFemale = this.titleShortFemale;
			this.untranslatedDesc = this.description;
			if (this.description.NullOrEmpty() && !this.baseDesc.NullOrEmpty())
			{
				this.description = this.baseDesc;
			}
			this.description = this.description.TrimEnd();
			this.description = this.description.Replace("\r", "");
		}

		// Token: 0x060095EC RID: 38380 RVA: 0x002BE421 File Offset: 0x002BC621
		public string TitleFor(Gender g)
		{
			if (g != Gender.Female || this.titleFemale.NullOrEmpty())
			{
				return this.title;
			}
			return this.titleFemale;
		}

		// Token: 0x060095ED RID: 38381 RVA: 0x002BE441 File Offset: 0x002BC641
		public string TitleCapFor(Gender g)
		{
			return this.TitleFor(g).CapitalizeFirst();
		}

		// Token: 0x060095EE RID: 38382 RVA: 0x002BE44F File Offset: 0x002BC64F
		public string TitleShortFor(Gender g)
		{
			if (g == Gender.Female && !this.titleShortFemale.NullOrEmpty())
			{
				return this.titleShortFemale;
			}
			if (!this.titleShort.NullOrEmpty())
			{
				return this.titleShort;
			}
			return this.TitleFor(g);
		}

		// Token: 0x060095EF RID: 38383 RVA: 0x002BE484 File Offset: 0x002BC684
		public BodyTypeDef BodyTypeFor(Gender g)
		{
			if (this.bodyTypeGlobal != null || g == Gender.None)
			{
				return this.bodyTypeGlobal;
			}
			if (g == Gender.Female)
			{
				return this.bodyTypeFemale;
			}
			return this.bodyTypeMale;
		}

		// Token: 0x060095F0 RID: 38384 RVA: 0x002BE4AC File Offset: 0x002BC6AC
		public TaggedString FullDescriptionFor(Pawn p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.description.Formatted(p.Named("PAWN")).AdjustedFor(p, "PAWN", true).Resolve());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			List<SkillDef> allDefsListForReading = DefDatabase<SkillDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				SkillDef skillDef = allDefsListForReading[i];
				foreach (SkillGain skillGain in this.skillGains)
				{
					if (skillGain.skill == skillDef)
					{
						stringBuilder.AppendLine(skillDef.skillLabel.CapitalizeFirst() + ":   " + skillGain.amount.ToString("+##;-##"));
						break;
					}
				}
			}
			if (this.DisabledWorkTypes.Any<WorkTypeDef>() || this.DisabledWorkGivers.Any<WorkGiverDef>())
			{
				stringBuilder.AppendLine();
			}
			foreach (WorkTypeDef workTypeDef in this.DisabledWorkTypes)
			{
				stringBuilder.AppendLine(workTypeDef.gerundLabel.CapitalizeFirst() + " " + "DisabledLower".Translate());
			}
			foreach (WorkGiverDef workGiverDef in this.DisabledWorkGivers)
			{
				stringBuilder.AppendLine(workGiverDef.workType.gerundLabel.CapitalizeFirst() + ": " + workGiverDef.LabelCap + " " + "DisabledLower".Translate());
			}
			if (ModsConfig.RoyaltyActive)
			{
				this.unlockedMeditationTypesTemp.Clear();
				foreach (MeditationFocusDef meditationFocusDef in DefDatabase<MeditationFocusDef>.AllDefs)
				{
					for (int j = 0; j < meditationFocusDef.requiredBackstoriesAny.Count; j++)
					{
						BackstoryCategoryAndSlot backstoryCategoryAndSlot = meditationFocusDef.requiredBackstoriesAny[j];
						if (this.spawnCategories.Contains(backstoryCategoryAndSlot.categoryName) && backstoryCategoryAndSlot.slot == this.slot)
						{
							this.unlockedMeditationTypesTemp.Add(meditationFocusDef.LabelCap);
							break;
						}
					}
				}
				if (this.unlockedMeditationTypesTemp.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("MeditationFocusesUnlocked".Translate() + ": ");
					stringBuilder.AppendLine(this.unlockedMeditationTypesTemp.ToLineList("  - "));
				}
			}
			if (!this.modContentPack.IsOfficialMod)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Source".Translate() + ": " + this.modContentPack.Name);
			}
			string str = stringBuilder.ToString().TrimEndNewlines();
			return Find.ActiveLanguageWorker.PostProcessed(str);
		}

		// Token: 0x060095F1 RID: 38385 RVA: 0x002BE820 File Offset: 0x002BCA20
		public void SetTitle(string newTitle, string newTitleFemale)
		{
			this.title = newTitle;
			this.titleFemale = newTitleFemale;
		}

		// Token: 0x060095F2 RID: 38386 RVA: 0x002BE830 File Offset: 0x002BCA30
		public void SetTitleShort(string newTitleShort, string newTitleShortFemale)
		{
			this.titleShort = newTitleShort;
			this.titleShortFemale = newTitleShortFemale;
		}

		// Token: 0x060095F3 RID: 38387 RVA: 0x002BE840 File Offset: 0x002BCA40
		public bool DisallowsTrait(TraitDef def, int degree)
		{
			if (this.disallowedTraits == null)
			{
				return false;
			}
			for (int i = 0; i < this.disallowedTraits.Count; i++)
			{
				if (this.disallowedTraits[i].def == def && this.disallowedTraits[i].degree == degree)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060095F4 RID: 38388 RVA: 0x002BE898 File Offset: 0x002BCA98
		private bool AllowsWorkType(WorkTypeDef workType)
		{
			return (this.workDisables & workType.workTags) == WorkTags.None;
		}

		// Token: 0x060095F5 RID: 38389 RVA: 0x002BE8AA File Offset: 0x002BCAAA
		private bool AllowsWorkGiver(WorkGiverDef workGiver)
		{
			return (this.workDisables & workGiver.workTags) == WorkTags.None;
		}

		// Token: 0x060095F6 RID: 38390 RVA: 0x002BE8BC File Offset: 0x002BCABC
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (this.title.NullOrEmpty())
			{
				yield return "null title, description is " + this.description;
			}
			if (this.titleShort.NullOrEmpty())
			{
				yield return "null titleShort, description is " + this.description;
			}
			if ((this.workDisables & WorkTags.Violent) != WorkTags.None && this.spawnCategories.Contains("Pirate"))
			{
				yield return "cannot do Violent work but can spawn as a pirate";
			}
			if (this.shuffleable && this.spawnCategories.Count == 0)
			{
				yield return "no spawn categories";
			}
			if (this.forcedTraits != null)
			{
				using (List<BackstoryTrait>.Enumerator enumerator2 = this.forcedTraits.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BackstoryTrait forcedTrait = enumerator2.Current;
						if (!forcedTrait.def.degreeDatas.Any((TraitDegreeData d) => d.degree == forcedTrait.degree))
						{
							yield return string.Concat(new string[]
							{
								"Backstory ",
								this.title,
								" has invalid trait ",
								forcedTrait.def.defName,
								" degree=",
								forcedTrait.degree.ToString()
							});
						}
					}
				}
				List<BackstoryTrait>.Enumerator enumerator2 = default(List<BackstoryTrait>.Enumerator);
			}
			if (Prefs.DevMode)
			{
				foreach (SkillGain skillGain in this.skillGains)
				{
					if (skillGain.skill.IsDisabled(this.workDisables, this.DisabledWorkTypes))
					{
						string str = "modifies skill ";
						SkillDef skill = skillGain.skill;
						yield return str + ((skill != null) ? skill.ToString() : null) + " but also disables this skill";
					}
				}
				List<SkillGain>.Enumerator enumerator3 = default(List<SkillGain>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x040061ED RID: 25069
		public const string PlayerColonistCategoryChild = "Child";

		// Token: 0x040061EE RID: 25070
		[MayTranslate]
		public string baseDesc;

		// Token: 0x040061EF RID: 25071
		[NoTranslate]
		public string identifier;

		// Token: 0x040061F0 RID: 25072
		public BackstorySlot slot;

		// Token: 0x040061F1 RID: 25073
		[MustTranslate]
		public string title;

		// Token: 0x040061F2 RID: 25074
		[MustTranslate]
		public string titleFemale;

		// Token: 0x040061F3 RID: 25075
		[MustTranslate]
		public string titleShort;

		// Token: 0x040061F4 RID: 25076
		[MustTranslate]
		public string titleShortFemale;

		// Token: 0x040061F5 RID: 25077
		public List<SkillGain> skillGains = new List<SkillGain>();

		// Token: 0x040061F6 RID: 25078
		public WorkTags workDisables;

		// Token: 0x040061F7 RID: 25079
		public WorkTags requiredWorkTags;

		// Token: 0x040061F8 RID: 25080
		[NoTranslate]
		public List<string> spawnCategories = new List<string>();

		// Token: 0x040061F9 RID: 25081
		public BodyTypeDef bodyTypeGlobal;

		// Token: 0x040061FA RID: 25082
		public BodyTypeDef bodyTypeFemale;

		// Token: 0x040061FB RID: 25083
		public BodyTypeDef bodyTypeMale;

		// Token: 0x040061FC RID: 25084
		public List<BackstoryTrait> forcedTraits;

		// Token: 0x040061FD RID: 25085
		public List<BackstoryTrait> disallowedTraits;

		// Token: 0x040061FE RID: 25086
		public RulePackDef nameMaker;

		// Token: 0x040061FF RID: 25087
		public List<PossessionThingDefCountClass> possessions = new List<PossessionThingDefCountClass>();

		// Token: 0x04006200 RID: 25088
		public bool shuffleable = true;

		// Token: 0x04006201 RID: 25089
		public bool requiresSpawnCategory;

		// Token: 0x04006202 RID: 25090
		[Unsaved(false)]
		public string untranslatedTitle;

		// Token: 0x04006203 RID: 25091
		[Unsaved(false)]
		public string untranslatedTitleFemale;

		// Token: 0x04006204 RID: 25092
		[Unsaved(false)]
		public string untranslatedTitleShort;

		// Token: 0x04006205 RID: 25093
		[Unsaved(false)]
		public string untranslatedTitleShortFemale;

		// Token: 0x04006206 RID: 25094
		[Unsaved(false)]
		public string untranslatedDesc;

		// Token: 0x04006207 RID: 25095
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		// Token: 0x04006208 RID: 25096
		private List<string> unlockedMeditationTypesTemp = new List<string>();
	}
}
