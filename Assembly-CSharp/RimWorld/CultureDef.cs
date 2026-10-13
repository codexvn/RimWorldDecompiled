using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A16 RID: 6678
	public class CultureDef : Def
	{
		// Token: 0x170016F4 RID: 5876
		// (get) Token: 0x0600968D RID: 38541 RVA: 0x002C0BC8 File Offset: 0x002BEDC8
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x0600968E RID: 38542 RVA: 0x002C0BF0 File Offset: 0x002BEDF0
		public RulePackDef GetPawnNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.pawnNameMakerFemale != null)
			{
				return this.pawnNameMakerFemale;
			}
			return this.pawnNameMaker;
		}

		// Token: 0x0600968F RID: 38543 RVA: 0x002C0C0B File Offset: 0x002BEE0B
		public override IEnumerable<string> ConfigErrors()
		{
			if (!this.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.priority <= 0f)
					{
						yield return "style category " + thingStyleCategoryWithPriority.category.LabelCap + " has <= 0 priority. It must be positive.";
					}
				}
				List<ThingStyleCategoryWithPriority>.Enumerator enumerator = default(List<ThingStyleCategoryWithPriority>.Enumerator);
			}
			if (this.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
			{
				yield return "no style item tags defined.";
			}
			if (ModsConfig.IdeologyActive && !this.allowedPlaceTags.Any<string>())
			{
				yield return "at least one allowedPlaceTags must be defined in ideology.";
			}
			yield break;
			yield break;
		}

		// Token: 0x040062DF RID: 25311
		public RulePackDef pawnNameMaker;

		// Token: 0x040062E0 RID: 25312
		public RulePackDef pawnNameMakerFemale;

		// Token: 0x040062E1 RID: 25313
		public RulePackDef ideoNameMaker;

		// Token: 0x040062E2 RID: 25314
		public RulePackDef deityNameMaker;

		// Token: 0x040062E3 RID: 25315
		public RulePackDef deityTypeMaker;

		// Token: 0x040062E4 RID: 25316
		public RulePackDef leaderTitleMaker;

		// Token: 0x040062E5 RID: 25317
		public RulePackDef festivalNameMaker;

		// Token: 0x040062E6 RID: 25318
		public List<ThingStyleCategoryWithPriority> thingStyleCategories;

		// Token: 0x040062E7 RID: 25319
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x040062E8 RID: 25320
		public IdeoWeaponClassPair preferredWeaponClasses;

		// Token: 0x040062E9 RID: 25321
		[NoTranslate]
		public List<string> allowedPlaceTags = new List<string>();

		// Token: 0x040062EA RID: 25322
		public string iconPath;

		// Token: 0x040062EB RID: 25323
		public Color iconColor = Color.white;

		// Token: 0x040062EC RID: 25324
		private Texture2D icon;
	}
}
