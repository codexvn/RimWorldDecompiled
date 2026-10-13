using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200193D RID: 6461
	public class BookOutcomeProperties_MentalBreak : BookOutcomeProperties
	{
		// Token: 0x17001643 RID: 5699
		// (get) Token: 0x060092EF RID: 37615 RVA: 0x002B3CBB File Offset: 0x002B1EBB
		public override Type DoerClass
		{
			get
			{
				return typeof(ReadingOutcomeDoerMentalBreak);
			}
		}

		// Token: 0x04005DB0 RID: 23984
		public FloatRange chancePerHourRange = new FloatRange(0f, 0.2f);

		// Token: 0x04005DB1 RID: 23985
		public FloatRange chanceMinMaxRange = new FloatRange(0.01f, 0.9f);
	}
}
