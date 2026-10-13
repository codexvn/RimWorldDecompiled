using System;

namespace Verse.Noise
{
	// Token: 0x02000DB5 RID: 3509
	public class Add : ModuleBase
	{
		// Token: 0x06005973 RID: 22899 RVA: 0x001CC37E File Offset: 0x001CA57E
		public Add() : base(2)
		{
		}

		// Token: 0x06005974 RID: 22900 RVA: 0x001CC387 File Offset: 0x001CA587
		public Add(ModuleBase lhs, ModuleBase rhs) : base(2)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
		}

		// Token: 0x06005975 RID: 22901 RVA: 0x001CC3A2 File Offset: 0x001CA5A2
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) + this.modules[1].GetValue(x, y, z);
		}
	}
}
