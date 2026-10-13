using System;

namespace Verse.Sound
{
	// Token: 0x02000E13 RID: 3603
	public class SoundParamSource_MusicPlayingFadeOut : SoundParamSource
	{
		// Token: 0x17000E8D RID: 3725
		// (get) Token: 0x06005B42 RID: 23362 RVA: 0x001D3B4D File Offset: 0x001D1D4D
		public override string Label
		{
			get
			{
				return "Music playing";
			}
		}

		// Token: 0x06005B43 RID: 23363 RVA: 0x001D3B54 File Offset: 0x001D1D54
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing || Find.MusicManagerPlay == null)
			{
				return 1f;
			}
			return Find.MusicManagerPlay.subtleAmbienceSoundVolumeMultiplier;
		}
	}
}
