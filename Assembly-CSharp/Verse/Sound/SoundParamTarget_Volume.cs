using System;

namespace Verse.Sound
{
	// Token: 0x02000E1B RID: 3611
	public class SoundParamTarget_Volume : SoundParamTarget
	{
		// Token: 0x17000E94 RID: 3732
		// (get) Token: 0x06005B56 RID: 23382 RVA: 0x001D3D06 File Offset: 0x001D1F06
		public override string Label
		{
			get
			{
				return "Volume";
			}
		}

		// Token: 0x06005B57 RID: 23383 RVA: 0x001D3D0D File Offset: 0x001D1F0D
		public override void SetOn(Sample sample, float value)
		{
			sample.SignalMappedVolume(value, this);
		}
	}
}
