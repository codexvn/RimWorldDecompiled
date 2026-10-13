using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003FC RID: 1020
	public struct FleckCreationData
	{
		// Token: 0x040016E4 RID: 5860
		public FleckDef def;

		// Token: 0x040016E5 RID: 5861
		public Vector3 spawnPosition;

		// Token: 0x040016E6 RID: 5862
		public float rotation;

		// Token: 0x040016E7 RID: 5863
		public float scale;

		// Token: 0x040016E8 RID: 5864
		public Vector3? exactScale;

		// Token: 0x040016E9 RID: 5865
		public Color? instanceColor;

		// Token: 0x040016EA RID: 5866
		public float velocityAngle;

		// Token: 0x040016EB RID: 5867
		public float velocitySpeed;

		// Token: 0x040016EC RID: 5868
		public Vector3? velocity;

		// Token: 0x040016ED RID: 5869
		public float rotationRate;

		// Token: 0x040016EE RID: 5870
		public float? solidTimeOverride;

		// Token: 0x040016EF RID: 5871
		public float? airTimeLeft;

		// Token: 0x040016F0 RID: 5872
		public int ageTicksOverride;

		// Token: 0x040016F1 RID: 5873
		public FleckAttachLink link;

		// Token: 0x040016F2 RID: 5874
		public float targetSize;

		// Token: 0x040016F3 RID: 5875
		public float orbitSpeed;

		// Token: 0x040016F4 RID: 5876
		public float orbitSnapStrength;
	}
}
