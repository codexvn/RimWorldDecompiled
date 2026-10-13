using System;

namespace Verse.Noise
{
	// Token: 0x02000DCB RID: 3531
	public class SmoothMin : ModuleBase
	{
		// Token: 0x060059EA RID: 23018 RVA: 0x001CD2B1 File Offset: 0x001CB4B1
		public SmoothMin() : base(2)
		{
		}

		// Token: 0x060059EB RID: 23019 RVA: 0x001CD2C9 File Offset: 0x001CB4C9
		public SmoothMin(ModuleBase lhs, ModuleBase rhs, double smoothness) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.m_smoothness = smoothness;
		}

		// Token: 0x060059EC RID: 23020 RVA: 0x001CD2FC File Offset: 0x001CB4FC
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return GenMath.SmoothMin(value, value2, this.m_smoothness);
		}

		// Token: 0x040040DE RID: 16606
		private double m_smoothness = 1.0;
	}
}
