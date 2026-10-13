using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019C5 RID: 6597
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x170016AE RID: 5806
		// (get) Token: 0x06009520 RID: 38176 RVA: 0x002BB8E4 File Offset: 0x002B9AE4
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x06009521 RID: 38177 RVA: 0x002BB8EB File Offset: 0x002B9AEB
		public override float ValueFor(Sample samp)
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return GenLocalDate.HourFloat(Find.CurrentMap);
		}
	}
}
