using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020024FF RID: 9471
	public class DrugPolicyEntry : IExposable
	{
		// Token: 0x0600CEEB RID: 52971 RVA: 0x003CC188 File Offset: 0x003CA388
		public void CopyFrom(DrugPolicyEntry other)
		{
			this.drug = other.drug;
			this.allowedForAddiction = other.allowedForAddiction;
			this.allowedForJoy = other.allowedForJoy;
			this.allowScheduled = other.allowScheduled;
			this.daysFrequency = other.daysFrequency;
			this.onlyIfMoodBelow = other.onlyIfMoodBelow;
			this.onlyIfJoyBelow = other.onlyIfJoyBelow;
			this.takeToInventory = other.takeToInventory;
		}

		// Token: 0x0600CEEC RID: 52972 RVA: 0x003CC1F8 File Offset: 0x003CA3F8
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.drug, "drug");
			Scribe_Values.Look<bool>(ref this.allowedForAddiction, "allowedForAddiction", false, false);
			Scribe_Values.Look<bool>(ref this.allowedForJoy, "allowedForJoy", false, false);
			Scribe_Values.Look<bool>(ref this.allowScheduled, "allowScheduled", false, false);
			Scribe_Values.Look<float>(ref this.daysFrequency, "daysFrequency", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfMoodBelow, "onlyIfMoodBelow", 1f, false);
			Scribe_Values.Look<float>(ref this.onlyIfJoyBelow, "onlyIfJoyBelow", 1f, false);
			Scribe_Values.Look<int>(ref this.takeToInventory, "takeToInventory", 0, false);
		}

		// Token: 0x04008CD1 RID: 36049
		public ThingDef drug;

		// Token: 0x04008CD2 RID: 36050
		public bool allowedForAddiction;

		// Token: 0x04008CD3 RID: 36051
		public bool allowedForJoy;

		// Token: 0x04008CD4 RID: 36052
		public bool allowScheduled;

		// Token: 0x04008CD5 RID: 36053
		public float daysFrequency = 1f;

		// Token: 0x04008CD6 RID: 36054
		public float onlyIfMoodBelow = 1f;

		// Token: 0x04008CD7 RID: 36055
		public float onlyIfJoyBelow = 1f;

		// Token: 0x04008CD8 RID: 36056
		public int takeToInventory;

		// Token: 0x04008CD9 RID: 36057
		public string takeToInventoryTempBuffer;
	}
}
