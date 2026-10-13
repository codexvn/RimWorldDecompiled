using System;

namespace Verse.Noise
{
	// Token: 0x02000DC4 RID: 3524
	public class OneMinus : ModuleBase
	{
		// Token: 0x060059B7 RID: 22967 RVA: 0x001CA658 File Offset: 0x001C8858
		public OneMinus() : base(1)
		{
		}

		// Token: 0x060059B8 RID: 22968 RVA: 0x001CC355 File Offset: 0x001CA555
		public OneMinus(ModuleBase module) : base(1)
		{
			this.modules[0] = module;
		}

		// Token: 0x060059B9 RID: 22969 RVA: 0x001CCB93 File Offset: 0x001CAD93
		public override double GetValue(double x, double y, double z)
		{
			return 1.0 - this.modules[0].GetValue(x, y, z);
		}
	}
}
