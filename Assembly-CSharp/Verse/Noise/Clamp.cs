using System;

namespace Verse.Noise
{
	// Token: 0x02000DB9 RID: 3513
	public class Clamp : ModuleBase
	{
		// Token: 0x06005983 RID: 22915 RVA: 0x001CC51C File Offset: 0x001CA71C
		public Clamp() : base(1)
		{
		}

		// Token: 0x06005984 RID: 22916 RVA: 0x001CC543 File Offset: 0x001CA743
		public Clamp(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x06005985 RID: 22917 RVA: 0x001CC573 File Offset: 0x001CA773
		public Clamp(double min, double max, ModuleBase input) : base(1)
		{
			this.Minimum = min;
			this.Maximum = max;
			this.modules[0] = input;
		}

		// Token: 0x17000E3A RID: 3642
		// (get) Token: 0x06005986 RID: 22918 RVA: 0x001CC5B1 File Offset: 0x001CA7B1
		// (set) Token: 0x06005987 RID: 22919 RVA: 0x001CC5B9 File Offset: 0x001CA7B9
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
			}
		}

		// Token: 0x17000E3B RID: 3643
		// (get) Token: 0x06005988 RID: 22920 RVA: 0x001CC5C2 File Offset: 0x001CA7C2
		// (set) Token: 0x06005989 RID: 22921 RVA: 0x001CC5CA File Offset: 0x001CA7CA
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
			}
		}

		// Token: 0x0600598A RID: 22922 RVA: 0x001CC5D3 File Offset: 0x001CA7D3
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x001CC5E4 File Offset: 0x001CA7E4
		public override double GetValue(double x, double y, double z)
		{
			if (this.m_min > this.m_max)
			{
				double min = this.m_min;
				this.m_min = this.m_max;
				this.m_max = min;
			}
			double value = this.modules[0].GetValue(x, y, z);
			if (value < this.m_min)
			{
				return this.m_min;
			}
			if (value > this.m_max)
			{
				return this.m_max;
			}
			return value;
		}

		// Token: 0x040040BF RID: 16575
		private double m_min = -1.0;

		// Token: 0x040040C0 RID: 16576
		private double m_max = 1.0;
	}
}
