using System;
using UnityEngine;

namespace LudeonTK
{
	// Token: 0x02000068 RID: 104
	public static class KeyFrameExtensions
	{
		// Token: 0x06000243 RID: 579 RVA: 0x0000C928 File Offset: 0x0000AB28
		public static Vector2 ToV2(this Keyframe kf)
		{
			return new Vector2(kf.time, kf.value);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000C93D File Offset: 0x0000AB3D
		public static Vector4 ToVector4(this Keyframe kf)
		{
			return new Vector4(kf.time, kf.value, kf.inTangent, kf.outTangent);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000C960 File Offset: 0x0000AB60
		public static Keyframe ToKeyframe(this Vector4 kf)
		{
			return new Keyframe(kf.x, kf.y, kf.z, kf.w);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000C97F File Offset: 0x0000AB7F
		public static Keyframe ToKeyframe(this Vector2 v2)
		{
			return new Keyframe(v2.x, v2.y);
		}
	}
}
