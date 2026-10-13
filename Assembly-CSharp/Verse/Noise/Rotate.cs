using System;

namespace Verse.Noise
{
	// Token: 0x02000DC7 RID: 3527
	public class Rotate : ModuleBase
	{
		// Token: 0x060059C0 RID: 22976 RVA: 0x001CCC19 File Offset: 0x001CAE19
		public Rotate() : base(1)
		{
			this.SetAngles(0.0, 0.0, 0.0);
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x001CC355 File Offset: 0x001CA555
		public Rotate(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x001CCC43 File Offset: 0x001CAE43
		public Rotate(double x, double y, double z, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.SetAngles(x, y, z);
		}

		// Token: 0x17000E42 RID: 3650
		// (get) Token: 0x060059C3 RID: 22979 RVA: 0x001CCC5F File Offset: 0x001CAE5F
		// (set) Token: 0x060059C4 RID: 22980 RVA: 0x001CCC67 File Offset: 0x001CAE67
		public double X
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(value, this.m_y, this.m_z);
			}
		}

		// Token: 0x17000E43 RID: 3651
		// (get) Token: 0x060059C5 RID: 22981 RVA: 0x001CCC7C File Offset: 0x001CAE7C
		// (set) Token: 0x060059C6 RID: 22982 RVA: 0x001CCC84 File Offset: 0x001CAE84
		public double Y
		{
			get
			{
				return this.m_y;
			}
			set
			{
				this.SetAngles(this.m_x, value, this.m_z);
			}
		}

		// Token: 0x17000E44 RID: 3652
		// (get) Token: 0x060059C7 RID: 22983 RVA: 0x001CCC5F File Offset: 0x001CAE5F
		// (set) Token: 0x060059C8 RID: 22984 RVA: 0x001CCC99 File Offset: 0x001CAE99
		public double Z
		{
			get
			{
				return this.m_x;
			}
			set
			{
				this.SetAngles(this.m_x, this.m_y, value);
			}
		}

		// Token: 0x060059C9 RID: 22985 RVA: 0x001CCCB0 File Offset: 0x001CAEB0
		private void SetAngles(double x, double y, double z)
		{
			double num = Math.Cos(x * 0.017453292519943295);
			double num2 = Math.Cos(y * 0.017453292519943295);
			double num3 = Math.Cos(z * 0.017453292519943295);
			double num4 = Math.Sin(x * 0.017453292519943295);
			double num5 = Math.Sin(y * 0.017453292519943295);
			double num6 = Math.Sin(z * 0.017453292519943295);
			this.m_x1Matrix = num5 * num4 * num6 + num2 * num3;
			this.m_y1Matrix = num * num6;
			this.m_z1Matrix = num5 * num3 - num2 * num4 * num6;
			this.m_x2Matrix = num5 * num4 * num3 - num2 * num6;
			this.m_y2Matrix = num * num3;
			this.m_z2Matrix = -num2 * num4 * num3 - num5 * num6;
			this.m_x3Matrix = -num5 * num;
			this.m_y3Matrix = num4;
			this.m_z3Matrix = num2 * num;
			this.m_x = x;
			this.m_y = y;
			this.m_z = z;
		}

		// Token: 0x060059CA RID: 22986 RVA: 0x001CCDB0 File Offset: 0x001CAFB0
		public override double GetValue(double x, double y, double z)
		{
			double x2 = this.m_x1Matrix * x + this.m_y1Matrix * y + this.m_z1Matrix * z;
			double y2 = this.m_x2Matrix * x + this.m_y2Matrix * y + this.m_z2Matrix * z;
			double z2 = this.m_x3Matrix * x + this.m_y3Matrix * y + this.m_z3Matrix * z;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x040040C9 RID: 16585
		private double m_x;

		// Token: 0x040040CA RID: 16586
		private double m_x1Matrix;

		// Token: 0x040040CB RID: 16587
		private double m_x2Matrix;

		// Token: 0x040040CC RID: 16588
		private double m_x3Matrix;

		// Token: 0x040040CD RID: 16589
		private double m_y;

		// Token: 0x040040CE RID: 16590
		private double m_y1Matrix;

		// Token: 0x040040CF RID: 16591
		private double m_y2Matrix;

		// Token: 0x040040D0 RID: 16592
		private double m_y3Matrix;

		// Token: 0x040040D1 RID: 16593
		private double m_z;

		// Token: 0x040040D2 RID: 16594
		private double m_z1Matrix;

		// Token: 0x040040D3 RID: 16595
		private double m_z2Matrix;

		// Token: 0x040040D4 RID: 16596
		private double m_z3Matrix;
	}
}
