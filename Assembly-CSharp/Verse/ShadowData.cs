using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200048E RID: 1166
	public class ShadowData
	{
		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x0009FEEE File Offset: 0x0009E0EE
		public float BaseX
		{
			get
			{
				return this.volume.x;
			}
		}

		// Token: 0x17000588 RID: 1416
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x0009FEFB File Offset: 0x0009E0FB
		public float BaseY
		{
			get
			{
				return this.volume.y;
			}
		}

		// Token: 0x17000589 RID: 1417
		// (get) Token: 0x06001E82 RID: 7810 RVA: 0x0009FF08 File Offset: 0x0009E108
		public float BaseZ
		{
			get
			{
				return this.volume.z;
			}
		}

		// Token: 0x04001A47 RID: 6727
		public Vector3 volume = Vector3.one;

		// Token: 0x04001A48 RID: 6728
		public Vector3 offset = Vector3.zero;
	}
}
