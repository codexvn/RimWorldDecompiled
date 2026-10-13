using System;

namespace Verse.Noise
{
	// Token: 0x02000DCC RID: 3532
	public class Subtract : ModuleBase
	{
		// Token: 0x060059ED RID: 23021 RVA: 0x001CC37E File Offset: 0x001CA57E
		public Subtract() : base(2)
		{
		}

		// Token: 0x060059EE RID: 23022 RVA: 0x001CC387 File Offset: 0x001CA587
		public Subtract(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060059EF RID: 23023 RVA: 0x001CD336 File Offset: 0x001CB536
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) - this.modules[1].GetValue(x, y, z);
		}
	}
}
