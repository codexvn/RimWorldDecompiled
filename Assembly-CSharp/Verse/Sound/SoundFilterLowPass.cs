using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E07 RID: 3591
	public class SoundFilterLowPass : SoundFilter
	{
		// Token: 0x06005B26 RID: 23334 RVA: 0x001D384A File Offset: 0x001D1A4A
		public override void SetupOn(AudioSource source)
		{
			AudioLowPassFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioLowPassFilter>(source);
			orMakeFilterOn.cutoffFrequency = this.cutoffFrequency;
			orMakeFilterOn.lowpassResonanceQ = this.lowpassResonaceQ;
		}

		// Token: 0x04004207 RID: 16903
		[EditSliderRange(50f, 20000f)]
		[Description("This filter will attenuate frequencies above this cutoff frequency.")]
		public float cutoffFrequency = 10000f;

		// Token: 0x04004208 RID: 16904
		[EditSliderRange(1f, 10f)]
		[Description("The resonance Q value.")]
		public float lowpassResonaceQ = 1f;
	}
}
