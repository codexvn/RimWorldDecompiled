using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E23 RID: 3619
	public class SoundParamTarget_PropertyEcho : SoundParamTarget
	{
		// Token: 0x17000E9A RID: 3738
		// (get) Token: 0x06005B64 RID: 23396 RVA: 0x001D3E6A File Offset: 0x001D206A
		public override string Label
		{
			get
			{
				return "EchoFilter-" + this.filterProperty.ToString();
			}
		}

		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06005B65 RID: 23397 RVA: 0x001D3E87 File Offset: 0x001D2087
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterEcho);
			}
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x001D3E94 File Offset: 0x001D2094
		public override void SetOn(Sample sample, float value)
		{
			AudioEchoFilter audioEchoFilter = sample.source.GetComponent<AudioEchoFilter>();
			if (audioEchoFilter == null)
			{
				audioEchoFilter = sample.source.gameObject.AddComponent<AudioEchoFilter>();
			}
			if (this.filterProperty == EchoFilterProperty.Delay)
			{
				audioEchoFilter.delay = value;
			}
			if (this.filterProperty == EchoFilterProperty.DecayRatio)
			{
				audioEchoFilter.decayRatio = value;
			}
			if (this.filterProperty == EchoFilterProperty.WetMix)
			{
				audioEchoFilter.wetMix = value;
			}
			if (this.filterProperty == EchoFilterProperty.DryMix)
			{
				audioEchoFilter.dryMix = value;
			}
		}

		// Token: 0x04004235 RID: 16949
		private EchoFilterProperty filterProperty;
	}
}
