using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020036AB RID: 13995
	public struct RewardsGeneratorParams
	{
		// Token: 0x060136F7 RID: 79607 RVA: 0x005C6012 File Offset: 0x005C4212
		public string ConfigError()
		{
			if (this.rewardValue <= 0f)
			{
				return "rewardValue is " + this.rewardValue.ToString();
			}
			if (this.thingRewardDisallowed && this.thingRewardRequired)
			{
				return "thing reward is both disallowed and required";
			}
			return null;
		}

		// Token: 0x060136F8 RID: 79608 RVA: 0x005C604E File Offset: 0x005C424E
		public override string ToString()
		{
			return GenText.FieldsToString<RewardsGeneratorParams>(this);
		}

		// Token: 0x0400D76D RID: 55149
		public float rewardValue;

		// Token: 0x0400D76E RID: 55150
		public Faction giverFaction;

		// Token: 0x0400D76F RID: 55151
		public string chosenPawnSignal;

		// Token: 0x0400D770 RID: 55152
		public bool giveToCaravan;

		// Token: 0x0400D771 RID: 55153
		public float minGeneratedRewardValue;

		// Token: 0x0400D772 RID: 55154
		public bool thingRewardDisallowed;

		// Token: 0x0400D773 RID: 55155
		public bool thingRewardRequired;

		// Token: 0x0400D774 RID: 55156
		public bool thingRewardItemsOnly;

		// Token: 0x0400D775 RID: 55157
		public List<ThingDef> disallowedThingDefs;

		// Token: 0x0400D776 RID: 55158
		public bool allowRoyalFavor;

		// Token: 0x0400D777 RID: 55159
		public bool allowGoodwill;

		// Token: 0x0400D778 RID: 55160
		public bool allowDevelopmentPoints;

		// Token: 0x0400D779 RID: 55161
		public bool allowXenogermReimplantation;

		// Token: 0x0400D77A RID: 55162
		public float populationIntent;
	}
}
