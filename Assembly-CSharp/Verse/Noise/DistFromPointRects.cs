using System;
using System.Collections.Generic;

namespace Verse.Noise
{
	// Token: 0x02000DAA RID: 3498
	public class DistFromPointRects : ModuleBase
	{
		// Token: 0x0600590F RID: 22799 RVA: 0x001CA600 File Offset: 0x001C8800
		public DistFromPointRects() : base(0)
		{
		}

		// Token: 0x06005910 RID: 22800 RVA: 0x001CAA04 File Offset: 0x001C8C04
		public DistFromPointRects(List<CellRect> rects) : base(0)
		{
			this.rects = rects;
		}

		// Token: 0x06005911 RID: 22801 RVA: 0x001CAA14 File Offset: 0x001C8C14
		public override double GetValue(double x, double y, double z)
		{
			double num = 0.0;
			double num2 = 0.0;
			for (int i = 0; i < this.rects.Count; i++)
			{
				double num3 = 1.0 - Math.Abs(x - (double)this.rects[i].CenterCell.x) / (double)((float)this.rects[i].Width / 2f);
				double num4 = 1.0 - Math.Abs(z - (double)this.rects[i].CenterCell.z) / (double)((float)this.rects[i].Height / 2f);
				if (num3 > num)
				{
					num = num3;
				}
				if (num4 > num2)
				{
					num2 = num4;
				}
			}
			return Math.Min(num, num2);
		}

		// Token: 0x04004084 RID: 16516
		public List<CellRect> rects;
	}
}
