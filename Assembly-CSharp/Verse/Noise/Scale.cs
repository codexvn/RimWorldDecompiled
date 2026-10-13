using System;

namespace Verse.Noise
{
	// Token: 0x02000DC8 RID: 3528
	public class Scale : ModuleBase
	{
		// Token: 0x060059CB RID: 22987 RVA: 0x001CCE1E File Offset: 0x001CB01E
		public Scale() : base(1)
		{
		}

		// Token: 0x060059CC RID: 22988 RVA: 0x001CCE54 File Offset: 0x001CB054
		public Scale(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060059CD RID: 22989 RVA: 0x001CCE94 File Offset: 0x001CB094
		public Scale(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x060059CE RID: 22990 RVA: 0x001CCEF4 File Offset: 0x001CB0F4
		// (set) Token: 0x060059CF RID: 22991 RVA: 0x001CCEFC File Offset: 0x001CB0FC
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.m_x = value;
			}
		}

		// Token: 0x17000E46 RID: 3654
		// (get) Token: 0x060059D0 RID: 22992 RVA: 0x001CCF05 File Offset: 0x001CB105
		// (set) Token: 0x060059D1 RID: 22993 RVA: 0x001CCF0D File Offset: 0x001CB10D
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.m_y = value;
			}
		}

		// Token: 0x17000E47 RID: 3655
		// (get) Token: 0x060059D2 RID: 22994 RVA: 0x001CCF16 File Offset: 0x001CB116
		// (set) Token: 0x060059D3 RID: 22995 RVA: 0x001CCF1E File Offset: 0x001CB11E
		public double Z
		{
			get
			{
				return this.m_z;
			}
			set
			{
				this.m_z = value;
			}
		}

		// Token: 0x060059D4 RID: 22996 RVA: 0x001CCF27 File Offset: 0x001CB127
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x * this.m_x, y * this.m_y, z * this.m_z);
		}

		// Token: 0x040040D5 RID: 16597
		private double m_x = 1.0;

		// Token: 0x040040D6 RID: 16598
		private double m_y = 1.0;

		// Token: 0x040040D7 RID: 16599
		private double m_z = 1.0;
	}
}
