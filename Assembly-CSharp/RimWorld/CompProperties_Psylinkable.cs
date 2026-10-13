using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DC9 RID: 11721
	public class CompProperties_Psylinkable : CompProperties
	{
		// Token: 0x060107C0 RID: 67520 RVA: 0x004C1567 File Offset: 0x004BF767
		public CompProperties_Psylinkable()
		{
			this.compClass = typeof(CompPsylinkable);
		}

		// Token: 0x0400ABEA RID: 44010
		public List<int> requiredSubplantCountPerPsylinkLevel;

		// Token: 0x0400ABEB RID: 44011
		public MeditationFocusDef requiredFocus;

		// Token: 0x0400ABEC RID: 44012
		public SoundDef linkSound;

		// Token: 0x0400ABED RID: 44013
		[MustTranslate]
		public string enoughPlantsLetterLabel;

		// Token: 0x0400ABEE RID: 44014
		[MustTranslate]
		public string enoughPlantsLetterText;
	}
}
