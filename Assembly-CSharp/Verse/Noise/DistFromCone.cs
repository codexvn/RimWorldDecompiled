using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DA7 RID: 3495
	public class DistFromCone : ModuleBase
	{
		// Token: 0x06005906 RID: 22790 RVA: 0x001CA600 File Offset: 0x001C8800
		public DistFromCone() : base(0)
		{
		}

		// Token: 0x06005907 RID: 22791 RVA: 0x001CA87C File Offset: 0x001C8A7C
		public DistFromCone(float gradient, float span) : base(0)
		{
			ModuleBase moduleBase = new AxisAsValueX();
			moduleBase = new Rotate(0.0, (double)(Mathf.Atan2(1f, gradient) * 57.29578f), 0.0, moduleBase);
			ModuleBase moduleBase2 = new AxisAsValueX();
			moduleBase2 = new Rotate(0.0, (double)(Mathf.Atan2(1f, -gradient) * 57.29578f), 0.0, moduleBase2);
			this.shape = new Min(moduleBase, moduleBase2);
			this.shape = new ScaleBias(-1.0, 1.0, this.shape);
			this.span = span;
		}

		// Token: 0x06005908 RID: 22792 RVA: 0x001CA92A File Offset: 0x001C8B2A
		public override double GetValue(double x, double y, double z)
		{
			return this.shape.GetValue(x / (double)this.span, y / (double)this.span, z / (double)this.span);
		}

		// Token: 0x0400407F RID: 16511
		private ModuleBase shape;

		// Token: 0x04004080 RID: 16512
		private float span;
	}
}
