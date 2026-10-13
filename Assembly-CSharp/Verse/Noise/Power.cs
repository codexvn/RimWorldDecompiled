using System;

namespace Verse.Noise
{
	// Token: 0x02000DC5 RID: 3525
	public class Power : ModuleBase
	{
		// Token: 0x060059BA RID: 22970 RVA: 0x001CC37E File Offset: 0x001CA57E
		public Power() : base(2)
		{
		}

		// Token: 0x060059BB RID: 22971 RVA: 0x001CC387 File Offset: 0x001CA587
		public Power(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x060059BC RID: 22972 RVA: 0x001CCBAF File Offset: 0x001CADAF
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z));
		}
	}
}
