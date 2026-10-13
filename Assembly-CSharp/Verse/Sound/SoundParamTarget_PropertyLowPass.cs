using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E1F RID: 3615
	public class SoundParamTarget_PropertyLowPass : SoundParamTarget
	{
		// Token: 0x17000E96 RID: 3734
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x001D3D70 File Offset: 0x001D1F70
		public override string Label
		{
			get
			{
				return "LowPassFilter-" + this.filterProperty.ToString();
			}
		}

		// Token: 0x17000E97 RID: 3735
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x001D3D8D File Offset: 0x001D1F8D
		public override Type NeededFilterType
		{
			get
			{
				return typeof(SoundFilterLowPass);
			}
		}

		// Token: 0x06005B5E RID: 23390 RVA: 0x001D3D9C File Offset: 0x001D1F9C
		public override void SetOn(Sample sample, float value)
		{
			AudioLowPassFilter audioLowPassFilter = sample.source.GetComponent<AudioLowPassFilter>();
			if (audioLowPassFilter == null)
			{
				audioLowPassFilter = sample.source.gameObject.AddComponent<AudioLowPassFilter>();
			}
			if (this.filterProperty == LowPassFilterProperty.Cutoff)
			{
				audioLowPassFilter.cutoffFrequency = value;
			}
			if (this.filterProperty == LowPassFilterProperty.Resonance)
			{
				audioLowPassFilter.lowpassResonanceQ = value;
			}
		}

		// Token: 0x0400422B RID: 16939
		private LowPassFilterProperty filterProperty;
	}
}
