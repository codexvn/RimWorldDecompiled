using System;

namespace Verse.Sound
{
	// Token: 0x02000E18 RID: 3608
	public class SoundParamSource_Random : SoundParamSource
	{
		// Token: 0x17000E90 RID: 3728
		// (get) Token: 0x06005B4C RID: 23372 RVA: 0x001D3CB2 File Offset: 0x001D1EB2
		public override string Label
		{
			get
			{
				return "Random";
			}
		}

		// Token: 0x06005B4D RID: 23373 RVA: 0x001D3CB9 File Offset: 0x001D1EB9
		public override float ValueFor(Sample samp)
		{
			return Rand.Value;
		}
	}
}
