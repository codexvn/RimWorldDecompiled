using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001954 RID: 6484
	public class CompProperties_CameraShaker : CompProperties
	{
		// Token: 0x0600936C RID: 37740 RVA: 0x002B5B36 File Offset: 0x002B3D36
		public CompProperties_CameraShaker()
		{
			this.compClass = typeof(CompCameraShaker);
		}

		// Token: 0x04005ED9 RID: 24281
		public float mag = 0.05f;
	}
}
