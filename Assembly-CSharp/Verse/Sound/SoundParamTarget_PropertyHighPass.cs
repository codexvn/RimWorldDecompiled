using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E21 RID: 3617
	public class SoundParamTarget_PropertyHighPass : SoundParamTarget
	{
		// Token: 0x17000E98 RID: 3736
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x001D3DEE File Offset: 0x001D1FEE
		public override string Label
		{
			get
			{
				return "HighPassFilter-" + this.filterProperty.ToString();
			}
		}

		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x001D3E0B File Offset: 0x001D200B
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterHighPass);
			}
		}

		// Token: 0x06005B62 RID: 23394 RVA: 0x001D3E18 File Offset: 0x001D2018
		public override void SetOn(Sample sample, float value)
		{
			AudioHighPassFilter audioHighPassFilter = sample.source.GetComponent<AudioHighPassFilter>();
			if (audioHighPassFilter == null)
			{
				audioHighPassFilter = sample.source.gameObject.AddComponent<AudioHighPassFilter>();
			}
			if (this.filterProperty == HighPassFilterProperty.Cutoff)
			{
				audioHighPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == HighPassFilterProperty.Resonance)
			{
				audioHighPassFilter.highpassResonanceQ = value;
			}
		}

		// Token: 0x0400422F RID: 16943
		private HighPassFilterProperty filterProperty;
	}
}
