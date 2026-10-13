using System;

namespace Verse.Noise
{
	// Token: 0x02000DB4 RID: 3508
	public class Abs : ModuleBase
	{
		// Token: 0x06005970 RID: 22896 RVA: 0x001CA658 File Offset: 0x001C8858
		public Abs() : base(1)
		{
		}

		// Token: 0x06005971 RID: 22897 RVA: 0x001CC355 File Offset: 0x001CA555
		public Abs(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005972 RID: 22898 RVA: 0x001CC367 File Offset: 0x001CA567
		public override double GetValue(double x, double y, double z)
		{
			return Math.Abs(this.modules[0].GetValue(x, y, z));
		}
	}
}
