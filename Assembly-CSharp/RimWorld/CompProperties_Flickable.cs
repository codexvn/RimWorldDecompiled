using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200198B RID: 6539
	public class CompProperties_Flickable : CompProperties
	{
		// Token: 0x0600944C RID: 37964 RVA: 0x002B9099 File Offset: 0x002B7299
		public CompProperties_Flickable()
		{
			this.compClass = typeof(CompFlickable);
		}

		// Token: 0x04005FE9 RID: 24553
		[NoTranslate]
		public string commandTexture = "UI/Commands/DesirePower";

		// Token: 0x04005FEA RID: 24554
		[NoTranslate]
		public string commandLabelKey = "CommandDesignateTogglePowerLabel";

		// Token: 0x04005FEB RID: 24555
		[NoTranslate]
		public string commandDescKey = "CommandDesignateTogglePowerDesc";
	}
}
