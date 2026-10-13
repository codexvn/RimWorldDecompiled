using System;

namespace Verse.Sound
{
	// Token: 0x02000E12 RID: 3602
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x17000E8C RID: 3724
		// (get) Token: 0x06005B3F RID: 23359 RVA: 0x001D3B1A File Offset: 0x001D1D1A
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06005B40 RID: 23360 RVA: 0x001D3B21 File Offset: 0x001D1D21
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 0f;
			}
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return Find.CurrentMap.mapTemperature.OutdoorTemp;
		}
	}
}
