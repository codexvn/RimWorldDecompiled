using System;

namespace Verse.Noise
{
	// Token: 0x02000DC1 RID: 3521
	public class Max : ModuleBase
	{
		// Token: 0x060059AE RID: 22958 RVA: 0x001CC37E File Offset: 0x001CA57E
		public Max() : base(2)
		{
		}

		// Token: 0x060059AF RID: 22959 RVA: 0x001CC387 File Offset: 0x001CA587
		public Max(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060059B0 RID: 22960 RVA: 0x001CCB08 File Offset: 0x001CAD08
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			return Math.Max(value, value2);
		}
	}
}
