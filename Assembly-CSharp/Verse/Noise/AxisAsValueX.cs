using System;

namespace Verse.Noise
{
	// Token: 0x02000DA0 RID: 3488
	public class AxisAsValueX : ModuleBase
	{
		// Token: 0x060058F2 RID: 22770 RVA: 0x001CA600 File Offset: 0x001C8800
		public AxisAsValueX() : base(0)
		{
		}

		// Token: 0x060058F3 RID: 22771 RVA: 0x00009C2B File Offset: 0x00007E2B
		public override double GetValue(double x, double y, double z)
		{
			return x;
		}
	}
}
