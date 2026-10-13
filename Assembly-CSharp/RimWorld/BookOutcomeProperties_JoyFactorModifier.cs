using System;

namespace RimWorld
{
	// Token: 0x0200193B RID: 6459
	public class BookOutcomeProperties_JoyFactorModifier : BookOutcomeProperties
	{
		// Token: 0x17001641 RID: 5697
		// (get) Token: 0x060092E8 RID: 37608 RVA: 0x002B3C5F File Offset: 0x002B1E5F
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerJoyFactorModifier);
			}
		}
	}
}
