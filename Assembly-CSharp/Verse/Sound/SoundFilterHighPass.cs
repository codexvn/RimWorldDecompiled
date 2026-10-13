using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E08 RID: 3592
	public class SoundFilterHighPass : SoundFilter
	{
		// Token: 0x06005B28 RID: 23336 RVA: 0x001D3887 File Offset: 0x001D1A87
		public override void SetupOn(AudioSource source)
		{
			AudioHighPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioHighPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.highpassResonanceQ = this.highpassResonanceQ;
		}

		// Token: 0x04004209 RID: 16905
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies below this cutoff frequency.")]
		public float cutoffFrequency = 10000f;

		// Token: 0x0400420A RID: 16906
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		public float highpassResonanceQ = 1f;
	}
}
