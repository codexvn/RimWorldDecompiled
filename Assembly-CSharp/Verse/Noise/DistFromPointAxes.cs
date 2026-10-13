using System;

namespace Verse.Noise
{
	// Token: 0x02000DA9 RID: 3497
	public class DistFromPointAxes : ModuleBase
	{
		// Token: 0x0600590C RID: 22796 RVA: 0x001CA600 File Offset: 0x001C8800
		public DistFromPointAxes() : base(0)
		{
		}

		// Token: 0x0600590D RID: 22797 RVA: 0x001CA9BD File Offset: 0x001C8BBD
		public DistFromPointAxes(float spanX, float spanZ) : base(0)
		{
			this.spanX = spanX;
			this.spanZ = spanZ;
		}

		// Token: 0x0600590E RID: 22798 RVA: 0x001CA9D4 File Offset: 0x001C8BD4
		public override double GetValue(double x, double y, double z)
		{
			double val = Math.Abs(x) / (double)this.spanX;
			double val2 = Math.Abs(z) / (double)this.spanZ;
			return Math.Max(val, val2);
		}

		// Token: 0x04004082 RID: 16514
		public float spanX;

		// Token: 0x04004083 RID: 16515
		public float spanZ;
	}
}
