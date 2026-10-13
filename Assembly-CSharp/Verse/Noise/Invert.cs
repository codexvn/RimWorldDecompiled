using System;

namespace Verse.Noise
{
	// Token: 0x02000DC0 RID: 3520
	public class Invert : ModuleBase
	{
		// Token: 0x060059AB RID: 22955 RVA: 0x001CA658 File Offset: 0x001C8858
		public Invert() : base(1)
		{
		}

		// Token: 0x060059AC RID: 22956 RVA: 0x001CC355 File Offset: 0x001CA555
		public Invert(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060059AD RID: 22957 RVA: 0x001CCAF2 File Offset: 0x001CACF2
		public override double GetValue(double x, double y, double z)
		{
			return -this.modules[0].GetValue(x, y, z);
		}
	}
}
