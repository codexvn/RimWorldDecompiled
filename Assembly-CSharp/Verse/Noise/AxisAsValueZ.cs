using System;

namespace Verse.Noise
{
	// Token: 0x02000DA1 RID: 3489
	public class AxisAsValueZ : ModuleBase
	{
		// Token: 0x060058F4 RID: 22772 RVA: 0x001CA600 File Offset: 0x001C8800
		public AxisAsValueZ() : base(0)
		{
		}

		// Token: 0x060058F5 RID: 22773 RVA: 0x001CA655 File Offset: 0x001C8855
		public override double GetValue(double x, double y, double z)
		{
			return z;
		}
	}
}
