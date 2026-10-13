using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02003682 RID: 13954
	public class Reward_DefinedThingDef : Reward
	{
		// Token: 0x060135E5 RID: 79333 RVA: 0x005C2DAE File Offset: 0x005C0FAE
		public Reward_DefinedThingDef()
		{
		}

		// Token: 0x060135E6 RID: 79334 RVA: 0x005C3147 File Offset: 0x005C1347
		public Reward_DefinedThingDef(ThingDef thingDef)
		{
			this.thingDef = thingDef;
		}

		// Token: 0x170031E4 RID: 12772
		// (get) Token: 0x060135E7 RID: 79335 RVA: 0x005C3156 File Offset: 0x005C1356
		public override IEnumerable<GenUI.AnonymousStackElement> StackElements
		{
			get
			{
				yield return QuestPartUtility.GetStandardRewardStackElement(this.thingDef.LabelCap, Widgets.GetIconFor(this.thingDef, null, null, null), () => this.GetDescription(default(RewardsGeneratorParams)).CapitalizeFirst(), null);
				yield break;
			}
		}

		// Token: 0x060135E8 RID: 79336 RVA: 0x005C3166 File Offset: 0x005C1366
		public override void InitFromValue(float rewardValue, RewardsGeneratorParams parms, out float valueActuallyUsed)
		{
			valueActuallyUsed = rewardValue;
		}

		// Token: 0x060135E9 RID: 79337 RVA: 0x005C316B File Offset: 0x005C136B
		public override IEnumerable<QuestPart> GenerateQuestParts(int index, RewardsGeneratorParams parms, string customLetterLabel, string customLetterText, RulePack customLetterLabelRules, RulePack customLetterTextRules)
		{
			yield break;
		}

		// Token: 0x060135EA RID: 79338 RVA: 0x005C3174 File Offset: 0x005C1374
		public override string GetDescription(RewardsGeneratorParams parms)
		{
			return this.thingDef.description;
		}

		// Token: 0x060135EB RID: 79339 RVA: 0x005C3181 File Offset: 0x005C1381
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
		}

		// Token: 0x0400D6D5 RID: 54997
		public ThingDef thingDef;
	}
}
