using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002449 RID: 9289
	public class ResistanceInteractionData : IExposable
	{
		// Token: 0x0600C90C RID: 51468 RVA: 0x003ABDD4 File Offset: 0x003A9FD4
		public void ExposeData()
		{
			Scribe_Values.Look<float>(ref this.resistanceReduction, "resistanceReduction", 0f, false);
			Scribe_Values.Look<float>(ref this.initiatorNegotiationAbilityFactor, "initiatorNegotiationAbilityFactor", 0f, false);
			Scribe_Values.Look<float>(ref this.recruiteeMoodFactor, "recruiteeMoodFactor", 0f, false);
			Scribe_Values.Look<string>(ref this.initiatorName, "initiatorName", null, false);
			Scribe_Values.Look<float>(ref this.recruiterOpinionFactor, "recruiterOpinionFactor", 0f, false);
		}

		// Token: 0x040088CC RID: 35020
		public float resistanceReduction;

		// Token: 0x040088CD RID: 35021
		public float initiatorNegotiationAbilityFactor;

		// Token: 0x040088CE RID: 35022
		public float recruiteeMoodFactor;

		// Token: 0x040088CF RID: 35023
		public string initiatorName;

		// Token: 0x040088D0 RID: 35024
		public float recruiterOpinionFactor;
	}
}
