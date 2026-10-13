using System;

namespace Verse.Noise
{
	// Token: 0x02000DBE RID: 3518
	public class Filter : ModuleBase
	{
		// Token: 0x060059A5 RID: 22949 RVA: 0x001CA658 File Offset: 0x001C8858
		public Filter() : base(1)
		{
		}

		// Token: 0x060059A6 RID: 22950 RVA: 0x001CCA05 File Offset: 0x001CAC05
		public Filter(ModuleBase module, float from, float to) : base(1)
		{
			this.modules[0] = module;
			this.from = from;
			this.to = to;
		}

		// Token: 0x060059A7 RID: 22951 RVA: 0x001CCA28 File Offset: 0x001CAC28
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			if (value >= (double)this.from && value <= (double)this.to)
			{
				return 1.0;
			}
			return 0.0;
		}

		// Token: 0x040040C5 RID: 16581
		private float from;

		// Token: 0x040040C6 RID: 16582
		private float to;
	}
}
