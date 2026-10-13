using System;

namespace Verse.Noise
{
	// Token: 0x02000DA3 RID: 3491
	public class CutOff : ModuleBase
	{
		// Token: 0x060058F9 RID: 22777 RVA: 0x001CA600 File Offset: 0x001C8800
		public CutOff() : base(0)
		{
		}

		// Token: 0x060058FA RID: 22778 RVA: 0x001CA75D File Offset: 0x001C895D
		public CutOff(bool invert, bool zAxis = false) : base(0)
		{
			this.inverted = invert;
			this.zAxis = zAxis;
		}

		// Token: 0x060058FB RID: 22779 RVA: 0x001CA774 File Offset: 0x001C8974
		public override double GetValue(double x, double y, double z)
		{
			double num = this.zAxis ? z : x;
			if (this.inverted)
			{
				return (double)((num < 0.0) ? 1 : 0);
			}
			return (double)((num >= 0.0) ? 1 : 0);
		}

		// Token: 0x04004078 RID: 16504
		private bool inverted;

		// Token: 0x04004079 RID: 16505
		private bool zAxis;
	}
}
