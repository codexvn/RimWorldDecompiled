using System;

namespace Verse
{
	// Token: 0x02000727 RID: 1831
	public class HediffCompProperties_SurgeryInspectable : HediffCompProperties
	{
		// Token: 0x06002C93 RID: 11411 RVA: 0x000E6E2F File Offset: 0x000E502F
		public HediffCompProperties_SurgeryInspectable()
		{
			this.compClass = typeof(HediffComp_SurgeryInspectable);
		}

		// Token: 0x040023AF RID: 9135
		[MustTranslate]
		public string surgicalDetectionDesc;

		// Token: 0x040023B0 RID: 9136
		public bool preventLetterIfPreviouslyDetected;
	}
}
