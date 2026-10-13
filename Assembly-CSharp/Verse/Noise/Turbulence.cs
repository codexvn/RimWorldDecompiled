using System;

namespace Verse.Noise
{
	// Token: 0x02000DD0 RID: 3536
	public class Turbulence : ModuleBase
	{
		// Token: 0x06005A08 RID: 23048 RVA: 0x001CD6AE File Offset: 0x001CB8AE
		public Turbulence() : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
		}

		// Token: 0x06005A09 RID: 23049 RVA: 0x001CD6E8 File Offset: 0x001CB8E8
		public Turbulence(ModuleBase input) : base(1)
		{
			this.m_xDistort = new Perlin();
			this.m_yDistort = new Perlin();
			this.m_zDistort = new Perlin();
			this.modules[0] = input;
		}

		// Token: 0x06005A0A RID: 23050 RVA: 0x001CD735 File Offset: 0x001CB935
		public Turbulence(double power, ModuleBase input) : this(new Perlin(), new Perlin(), new Perlin(), power, input)
		{
		}

		// Token: 0x06005A0B RID: 23051 RVA: 0x001CD74E File Offset: 0x001CB94E
		public Turbulence(Perlin x, Perlin y, Perlin z, double power, ModuleBase input) : base(1)
		{
			this.m_xDistort = x;
			this.m_yDistort = y;
			this.m_zDistort = z;
			this.modules[0] = input;
			this.Power = power;
		}

		// Token: 0x17000E54 RID: 3668
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x001CD78D File Offset: 0x001CB98D
		// (set) Token: 0x06005A0D RID: 23053 RVA: 0x001CD79A File Offset: 0x001CB99A
		public double Frequency
		{
			get
			{
				return this.m_xDistort.Frequency;
			}
			set
			{
				this.m_xDistort.Frequency = value;
				this.m_yDistort.Frequency = value;
				this.m_zDistort.Frequency = value;
			}
		}

		// Token: 0x17000E55 RID: 3669
		// (get) Token: 0x06005A0E RID: 23054 RVA: 0x001CD7C0 File Offset: 0x001CB9C0
		// (set) Token: 0x06005A0F RID: 23055 RVA: 0x001CD7C8 File Offset: 0x001CB9C8
		public double Power
		{
			get
			{
				return this.m_power;
			}
			set
			{
				this.m_power = value;
			}
		}

		// Token: 0x17000E56 RID: 3670
		// (get) Token: 0x06005A10 RID: 23056 RVA: 0x001CD7D1 File Offset: 0x001CB9D1
		// (set) Token: 0x06005A11 RID: 23057 RVA: 0x001CD7DE File Offset: 0x001CB9DE
		public int Roughness
		{
			get
			{
				return this.m_xDistort.OctaveCount;
			}
			set
			{
				this.m_xDistort.OctaveCount = value;
				this.m_yDistort.OctaveCount = value;
				this.m_zDistort.OctaveCount = value;
			}
		}

		// Token: 0x17000E57 RID: 3671
		// (get) Token: 0x06005A12 RID: 23058 RVA: 0x001CD804 File Offset: 0x001CBA04
		// (set) Token: 0x06005A13 RID: 23059 RVA: 0x001CD811 File Offset: 0x001CBA11
		public int Seed
		{
			get
			{
				return this.m_xDistort.Seed;
			}
			set
			{
				this.m_xDistort.Seed = value;
				this.m_yDistort.Seed = value + 1;
				this.m_zDistort.Seed = value + 2;
			}
		}

		// Token: 0x06005A14 RID: 23060 RVA: 0x001CD83C File Offset: 0x001CBA3C
		public override double GetValue(double x, double y, double z)
		{
			double x2 = x + this.m_xDistort.GetValue(x + 0.189422607421875, y + 0.99371337890625, z + 0.4781646728515625) * this.m_power;
			double y2 = y + this.m_yDistort.GetValue(x + 0.4046478271484375, y + 0.276611328125, z + 0.9230499267578125) * this.m_power;
			double z2 = z + this.m_zDistort.GetValue(x + 0.82122802734375, y + 0.1710968017578125, z + 0.6842803955078125) * this.m_power;
			return this.modules[0].GetValue(x2, y2, z2);
		}

		// Token: 0x040040E6 RID: 16614
		private const double X0 = 0.189422607421875;

		// Token: 0x040040E7 RID: 16615
		private const double Y0 = 0.99371337890625;

		// Token: 0x040040E8 RID: 16616
		private const double Z0 = 0.4781646728515625;

		// Token: 0x040040E9 RID: 16617
		private const double X1 = 0.4046478271484375;

		// Token: 0x040040EA RID: 16618
		private const double Y1 = 0.276611328125;

		// Token: 0x040040EB RID: 16619
		private const double Z1 = 0.9230499267578125;

		// Token: 0x040040EC RID: 16620
		private const double X2 = 0.82122802734375;

		// Token: 0x040040ED RID: 16621
		private const double Y2 = 0.1710968017578125;

		// Token: 0x040040EE RID: 16622
		private const double Z2 = 0.6842803955078125;

		// Token: 0x040040EF RID: 16623
		private double m_power = 1.0;

		// Token: 0x040040F0 RID: 16624
		private Perlin m_xDistort;

		// Token: 0x040040F1 RID: 16625
		private Perlin m_yDistort;

		// Token: 0x040040F2 RID: 16626
		private Perlin m_zDistort;
	}
}
