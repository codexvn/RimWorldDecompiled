using System;

namespace Verse.Sound
{
	// Token: 0x02000E0F RID: 3599
	public class SoundParamSource_AggregateSize : SoundParamSource
	{
		// Token: 0x17000E89 RID: 3721
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x001D3A68 File Offset: 0x001D1C68
		public override string Label
		{
			get
			{
				return "Aggregate size";
			}
		}

		// Token: 0x06005B37 RID: 23351 RVA: 0x001D3A6F File Offset: 0x001D1C6F
		public override float ValueFor(Sample samp)
		{
			if (samp.ExternalParams.sizeAggregator == null)
			{
				return 0f;
			}
			return samp.ExternalParams.sizeAggregator.AggregateSize;
		}
	}
}
