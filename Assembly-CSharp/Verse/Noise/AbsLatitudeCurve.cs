using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000D9F RID: 3487
	public class AbsLatitudeCurve : ModuleBase
	{
		// Token: 0x060058EF RID: 22767 RVA: 0x001CA600 File Offset: 0x001C8800
		public AbsLatitudeCurve() : base(0)
		{
		}

		// Token: 0x060058F0 RID: 22768 RVA: 0x001CA609 File Offset: 0x001C8809
		public AbsLatitudeCurve(SimpleCurve curve, float planetRadius) : base(0)
		{
			this.curve = curve;
			this.planetRadius = planetRadius;
		}

		// Token: 0x060058F1 RID: 22769 RVA: 0x001CA620 File Offset: 0x001C8820
		public override double GetValue(double x, double y, double z)
		{
			float f = Mathf.Asin((float)(y / (double)this.planetRadius)) * 57.29578f;
			return (double)this.curve.Evaluate(Mathf.Abs(f));
		}

		// Token: 0x04004073 RID: 16499
		public SimpleCurve curve;

		// Token: 0x04004074 RID: 16500
		public float planetRadius;
	}
}
