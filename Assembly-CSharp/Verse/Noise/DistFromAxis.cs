using System;

namespace Verse.Noise
{
	// Token: 0x02000DA5 RID: 3493
	public class DistFromAxis : ModuleBase
	{
		// Token: 0x06005900 RID: 22784 RVA: 0x001CA600 File Offset: 0x001C8800
		public DistFromAxis() : base(0)
		{
		}

		// Token: 0x06005901 RID: 22785 RVA: 0x001CA83E File Offset: 0x001C8A3E
		public DistFromAxis(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005902 RID: 22786 RVA: 0x001CA84E File Offset: 0x001C8A4E
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(x) / (double)this.span;
		}

		// Token: 0x0400407D RID: 16509
		public float span;
	}
}
