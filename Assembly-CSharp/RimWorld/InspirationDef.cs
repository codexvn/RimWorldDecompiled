using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A57 RID: 6743
	public class InspirationDef : Def
	{
		// Token: 0x1700172C RID: 5932
		// (get) Token: 0x06009797 RID: 38807 RVA: 0x002C44F2 File Offset: 0x002C26F2
		public InspirationWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (InspirationWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x040064CA RID: 25802
		public Type inspirationClass = typeof(Inspiration);

		// Token: 0x040064CB RID: 25803
		public Type workerClass = typeof(InspirationWorker);

		// Token: 0x040064CC RID: 25804
		public float baseCommonality = 1f;

		// Token: 0x040064CD RID: 25805
		public float baseDurationDays = 1f;

		// Token: 0x040064CE RID: 25806
		public bool allowedOnAnimals;

		// Token: 0x040064CF RID: 25807
		public bool allowedOnNonColonists;

		// Token: 0x040064D0 RID: 25808
		public bool allowedOnDownedPawns = true;

		// Token: 0x040064D1 RID: 25809
		public List<StatDef> requiredNonDisabledStats;

		// Token: 0x040064D2 RID: 25810
		public List<SkillRequirement> requiredSkills;

		// Token: 0x040064D3 RID: 25811
		public List<SkillRequirement> requiredAnySkill;

		// Token: 0x040064D4 RID: 25812
		public List<WorkTypeDef> requiredNonDisabledWorkTypes;

		// Token: 0x040064D5 RID: 25813
		public List<WorkTypeDef> requiredAnyNonDisabledWorkType;

		// Token: 0x040064D6 RID: 25814
		public WorkTags requiredNonDisabledWorkTags;

		// Token: 0x040064D7 RID: 25815
		public List<PawnCapacityDef> requiredCapacities;

		// Token: 0x040064D8 RID: 25816
		public List<SkillDef> associatedSkills;

		// Token: 0x040064D9 RID: 25817
		public float minAge;

		// Token: 0x040064DA RID: 25818
		public List<StatModifier> statOffsets;

		// Token: 0x040064DB RID: 25819
		public List<StatModifier> statFactors;

		// Token: 0x040064DC RID: 25820
		[MustTranslate]
		public string beginLetter;

		// Token: 0x040064DD RID: 25821
		[MustTranslate]
		public string beginLetterLabel;

		// Token: 0x040064DE RID: 25822
		public LetterDef beginLetterDef;

		// Token: 0x040064DF RID: 25823
		[MustTranslate]
		public string endMessage;

		// Token: 0x040064E0 RID: 25824
		[MustTranslate]
		public string baseInspectLine;

		// Token: 0x040064E1 RID: 25825
		[Unsaved(false)]
		private InspirationWorker workerInt;
	}
}
