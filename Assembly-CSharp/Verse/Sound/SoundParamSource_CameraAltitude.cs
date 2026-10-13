using System;

namespace Verse.Sound
{
	// Token: 0x02000E11 RID: 3601
	public class SoundParamSource_CameraAltitude : SoundParamSource
	{
		// Token: 0x17000E8B RID: 3723
		// (get) Token: 0x06005B3C RID: 23356 RVA: 0x001D3AFD File Offset: 0x001D1CFD
		public override string Label
		{
			get
			{
				return "Camera altitude";
			}
		}

		// Token: 0x06005B3D RID: 23357 RVA: 0x001D3B04 File Offset: 0x001D1D04
		public override float ValueFor(Sample samp)
		{
			return Find.Camera.transform.position.y;
		}
	}
}
