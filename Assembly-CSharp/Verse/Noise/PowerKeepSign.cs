using System;

namespace Verse.Noise
{
	// Token: 0x02000DC6 RID: 3526
	public class PowerKeepSign : ModuleBase
	{
		// Token: 0x060059BD RID: 22973 RVA: 0x001CC37E File Offset: 0x001CA57E
		public PowerKeepSign() : base(2)
		{
		}

		// Token: 0x060059BE RID: 22974 RVA: 0x001CC387 File Offset: 0x001CA587
		public PowerKeepSign(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x001CCBD8 File Offset: 0x001CADD8
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			return (double)Math.Sign(value) * Math.Pow(Math.Abs(value), this.modules[1].GetValue(x, y, z));
		}
	}
}
