using System;

namespace Verse.Noise
{
	// Token: 0x02000DBF RID: 3519
	public class InverseLerp : ModuleBase
	{
		// Token: 0x060059A8 RID: 22952 RVA: 0x001CA658 File Offset: 0x001C8858
		public InverseLerp() : base(1)
		{
		}

		// Token: 0x060059A9 RID: 22953 RVA: 0x001CCA6D File Offset: 0x001CAC6D
		public InverseLerp(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x060059AA RID: 22954 RVA: 0x001CCA90 File Offset: 0x001CAC90
		public override double GetValue(double x, double y, double z)
		{
			double num = (this.modules[0].GetValue(x, y, z) - (double)this.from) / (double)(this.to - this.from);
			if (num < 0.0)
			{
				return 0.0;
			}
			if (num > 1.0)
			{
				return 1.0;
			}
			return num;
		}

		// Token: 0x040040C7 RID: 16583
		private float from;

		// Token: 0x040040C8 RID: 16584
		private float to;
	}
}
