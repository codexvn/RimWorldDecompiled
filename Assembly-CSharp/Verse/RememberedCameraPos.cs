using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020001EA RID: 490
	public class RememberedCameraPos : IExposable
	{
		// Token: 0x06000D2E RID: 3374 RVA: 0x0003E624 File Offset: 0x0003C824
		public RememberedCameraPos(Map map)
		{
			this.rootPos = map.Center.ToVector3Shifted();
			this.rootSize = 24f;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x0003E658 File Offset: 0x0003C858
		public void ExposeData()
		{
			Scribe_Values.Look<Vector3>(ref this.rootPos, "rootPos", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.rootSize, "rootSize", 0f, false);
		}

		// Token: 0x040007A7 RID: 1959
		public Vector3 rootPos;

		// Token: 0x040007A8 RID: 1960
		public float rootSize;
	}
}
