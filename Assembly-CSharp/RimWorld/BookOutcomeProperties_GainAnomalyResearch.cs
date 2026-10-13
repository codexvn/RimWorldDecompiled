using System;

namespace RimWorld
{
	// Token: 0x02001929 RID: 6441
	public class BookOutcomeProperties_GainAnomalyResearch : BookOutcomeProperties_GainResearch
	{
		// Token: 0x1700162F RID: 5679
		// (get) Token: 0x0600927F RID: 37503 RVA: 0x002B2777 File Offset: 0x002B0977
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerGainAnomalyResearch);
			}
		}
	}
}
