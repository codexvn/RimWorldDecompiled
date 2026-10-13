using System;

namespace Verse.Noise
{
	// Token: 0x02000DC3 RID: 3523
	public class Multiply : ModuleBase
	{
		// Token: 0x060059B4 RID: 22964 RVA: 0x001CC37E File Offset: 0x001CA57E
		public Multiply() : base(2)
		{
		}

		// Token: 0x060059B5 RID: 22965 RVA: 0x001CC387 File Offset: 0x001CA587
		public Multiply(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060059B6 RID: 22966 RVA: 0x001CCB70 File Offset: 0x001CAD70
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.modules[1].GetValue(x, y, z);
		}
	}
}
