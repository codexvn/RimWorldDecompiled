using System;

namespace Verse.Noise
{
	// Token: 0x02000DBD RID: 3517
	public class Exponent : ModuleBase
	{
		// Token: 0x0600599F RID: 22943 RVA: 0x001CC93E File Offset: 0x001CAB3E
		public Exponent() : base(1)
		{
		}

		// Token: 0x060059A0 RID: 22944 RVA: 0x001CC956 File Offset: 0x001CAB56
		public Exponent(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060059A1 RID: 22945 RVA: 0x001CC977 File Offset: 0x001CAB77
		public Exponent(double exponent, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Value = exponent;
		}

		// Token: 0x17000E41 RID: 3649
		// (get) Token: 0x060059A2 RID: 22946 RVA: 0x001CC99F File Offset: 0x001CAB9F
		// (set) Token: 0x060059A3 RID: 22947 RVA: 0x001CC9A7 File Offset: 0x001CABA7
		public double Value
		{
			get
			{
				return this.m_exponent;
			}
			set
			{
				this.m_exponent = value;
			}
		}

		// Token: 0x060059A4 RID: 22948 RVA: 0x001CC9B0 File Offset: 0x001CABB0
		public override double GetValue(double x, double y, double z)
		{
			return Math.Pow(Math.Abs((this.modules[0].GetValue(x, y, z) + 1.0) / 2.0), this.m_exponent) * 2.0 - 1.0;
		}

		// Token: 0x040040C4 RID: 16580
		private double m_exponent = 1.0;
	}
}
