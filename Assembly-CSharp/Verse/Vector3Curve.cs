using System;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000203 RID: 515
	public class Vector3Curve
	{
		// Token: 0x06000DC2 RID: 3522 RVA: 0x0004198C File Offset: 0x0003FB8C
		public Vector3 Evaluate(float time, float nullCurveValue = 0f)
		{
			ComplexCurve complexCurve = this.x;
			float num = (complexCurve != null) ? complexCurve.Evaluate(time) : nullCurveValue;
			ComplexCurve complexCurve2 = this.y;
			float num2 = (complexCurve2 != null) ? complexCurve2.Evaluate(time) : nullCurveValue;
			ComplexCurve complexCurve3 = this.z;
			float num3 = (complexCurve3 != null) ? complexCurve3.Evaluate(time) : nullCurveValue;
			return new Vector3(num, num2, num3);
		}

		// Token: 0x04000814 RID: 2068
		public ComplexCurve x;

		// Token: 0x04000815 RID: 2069
		public ComplexCurve y;

		// Token: 0x04000816 RID: 2070
		public ComplexCurve z;
	}
}
