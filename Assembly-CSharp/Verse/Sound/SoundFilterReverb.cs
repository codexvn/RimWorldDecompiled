using System;
using UnityEngine;

namespace Verse.Sound
{
	// Token: 0x02000E0A RID: 3594
	public class SoundFilterReverb : SoundFilter
	{
		// Token: 0x06005B2C RID: 23340 RVA: 0x001D3930 File Offset: 0x001D1B30
		public override void SetupOn(AudioSource source)
		{
			AudioReverbFilter orMakeFilterOn = SoundFilter.GetOrMakeFilterOn<AudioReverbFilter>(source);
			this.baseSetup.ApplyTo(orMakeFilterOn);
		}

		// Token: 0x0400420F RID: 16911
		[Description("The base setup for this filter.\n\nOnly used if no parameters ever affect this filter.")]
		private ReverbSetup baseSetup = new ReverbSetup();
	}
}
