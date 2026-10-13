using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02003509 RID: 13577
	public static class Bezier
	{
		// Token: 0x06013143 RID: 78147 RVA: 0x005A9068 File Offset: 0x005A7268
		public static Vector2 EvaluateQuadratic(Vector2 a, Vector2 b, Vector2 c, float t)
		{
			Vector2 a2 = Vector2.Lerp(a, b, t);
			Vector2 b2 = Vector2.Lerp(b, c, t);
			return Vector2.Lerp(a2, b2, t);
		}

		// Token: 0x06013144 RID: 78148 RVA: 0x005A9090 File Offset: 0x005A7290
		public static Vector2 EvaluateCubic(Vector2 a, Vector2 b, Vector2 c, Vector2 d, float t)
		{
			Vector2 a2 = Bezier.EvaluateQuadratic(a, b, c, t);
			Vector2 b2 = Bezier.EvaluateQuadratic(b, c, d, t);
			return Vector2.Lerp(a2, b2, t);
		}
	}
}
