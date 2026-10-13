using System;

namespace Verse.Sound
{
	// Token: 0x02000E14 RID: 3604
	public class SoundParamSource_AmbientVolume : SoundParamSource
	{
		// Token: 0x17000E8E RID: 3726
		// (get) Token: 0x06005B45 RID: 23365 RVA: 0x001D3B75 File Offset: 0x001D1D75
		public override string Label
		{
			get
			{
				return "Ambient volume";
			}
		}

		// Token: 0x06005B46 RID: 23366 RVA: 0x001D3B7C File Offset: 0x001D1D7C
		public override float ValueFor(Sample samp)
		{
			return Prefs.VolumeAmbient;
		}
	}
}
