using System;

namespace Verse.Noise
{
	// Token: 0x02000DC2 RID: 3522
	public class Min : ModuleBase
	{
		// Token: 0x060059B1 RID: 22961 RVA: 0x001CC37E File Offset: 0x001CA57E
		public Min() : base(2)
		{
		}

		// Token: 0x060059B2 RID: 22962 RVA: 0x001CC387 File Offset: 0x001CA587
		public Min(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060059B3 RID: 22963 RVA: 0x001CCB3C File Offset: 0x001CAD3C
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Min(value, value2);
		}
	}
}
