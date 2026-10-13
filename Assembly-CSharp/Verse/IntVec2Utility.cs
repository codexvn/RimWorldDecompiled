using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000D1 RID: 209
	public static class IntVec2Utility
	{
		// Token: 0x0600056C RID: 1388 RVA: 0x0001C1D4 File Offset: 0x0001A3D4
		public static IntVec2 ToIntVec2(this Vector3 v)
		{
			return new IntVec2((int)v.x, (int)v.z);
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x0001C1E9 File Offset: 0x0001A3E9
		public static IntVec2 ToIntVec2(this Vector2 v)
		{
			return new IntVec2((int)v.x, (int)v.y);
		}
	}
}
