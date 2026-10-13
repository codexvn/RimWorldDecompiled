using System;

namespace Verse.Noise
{
	// Token: 0x02000DCF RID: 3535
	public class Translate : ModuleBase
	{
		// Token: 0x060059FE RID: 23038 RVA: 0x001CD57E File Offset: 0x001CB77E
		public Translate() : base(1)
		{
		}

		// Token: 0x060059FF RID: 23039 RVA: 0x001CD5B4 File Offset: 0x001CB7B4
		public Translate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005A00 RID: 23040 RVA: 0x001CD5F4 File Offset: 0x001CB7F4
		public Translate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.X = x;
			this.Y = y;
			this.Z = z;
		}

		// Token: 0x17000E51 RID: 3665
		// (get) Token: 0x06005A01 RID: 23041 RVA: 0x001CD654 File Offset: 0x001CB854
		// (set) Token: 0x06005A02 RID: 23042 RVA: 0x001CD65C File Offset: 0x001CB85C
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

		// Token: 0x17000E52 RID: 3666
		// (get) Token: 0x06005A03 RID: 23043 RVA: 0x001CD665 File Offset: 0x001CB865
		// (set) Token: 0x06005A04 RID: 23044 RVA: 0x001CD66D File Offset: 0x001CB86D
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

		// Token: 0x17000E53 RID: 3667
		// (get) Token: 0x06005A05 RID: 23045 RVA: 0x001CD676 File Offset: 0x001CB876
		// (set) Token: 0x06005A06 RID: 23046 RVA: 0x001CD67E File Offset: 0x001CB87E
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

		// Token: 0x06005A07 RID: 23047 RVA: 0x001CD687 File Offset: 0x001CB887
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x + this.m_x, y + this.m_y, z + this.m_z);
		}

		// Token: 0x040040E3 RID: 16611
		private double m_x = 1.0;

		// Token: 0x040040E4 RID: 16612
		private double m_y = 1.0;

		// Token: 0x040040E5 RID: 16613
		private double m_z = 1.0;
	}
}
