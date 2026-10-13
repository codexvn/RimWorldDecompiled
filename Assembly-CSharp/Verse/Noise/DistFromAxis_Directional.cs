using System;

namespace Verse.Noise
{
	// Token: 0x02000DA6 RID: 3494
	public class DistFromAxis_Directional : ModuleBase
	{
		// Token: 0x06005903 RID: 22787 RVA: 0x001CA600 File Offset: 0x001C8800
		public DistFromAxis_Directional() : base(0)
		{
		}

		// Token: 0x06005904 RID: 22788 RVA: 0x001CA85E File Offset: 0x001C8A5E
		public DistFromAxis_Directional(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x06005905 RID: 22789 RVA: 0x001CA86E File Offset: 0x001C8A6E
		public override double GetValue(double x, double y, double z)
		{
			return x / (double)this.span;
		}

		// Token: 0x0400407E RID: 16510
		public float span;
	}
}
