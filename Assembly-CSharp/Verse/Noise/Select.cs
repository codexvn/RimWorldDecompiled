using System;

namespace Verse.Noise
{
	// Token: 0x02000DCA RID: 3530
	public class Select : ModuleBase
	{
		// Token: 0x060059DD RID: 23005 RVA: 0x001CCFF8 File Offset: 0x001CB1F8
		public Select() : base(3)
		{
		}

		// Token: 0x060059DE RID: 23006 RVA: 0x001CD020 File Offset: 0x001CB220
		public Select(ModuleBase inputA, ModuleBase inputB, ModuleBase controller) : base(3)
		{
			this.modules[0] = inputA;
			this.modules[1] = inputB;
			this.modules[2] = controller;
		}

		// Token: 0x060059DF RID: 23007 RVA: 0x001CD06D File Offset: 0x001CB26D
		public Select(double min, double max, double fallOff, ModuleBase inputA, ModuleBase inputB) : this(inputA, inputB, null)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = fallOff;
		}

		// Token: 0x17000E4A RID: 3658
		// (get) Token: 0x060059E0 RID: 23008 RVA: 0x001CC428 File Offset: 0x001CA628
		// (set) Token: 0x060059E1 RID: 23009 RVA: 0x001CC432 File Offset: 0x001CA632
		public ModuleBase Controller
		{
			get
			{
				return this.modules[2];
			}
			set
			{
				this.modules[2] = value;
			}
		}

		// Token: 0x17000E4B RID: 3659
		// (get) Token: 0x060059E2 RID: 23010 RVA: 0x001CD08F File Offset: 0x001CB28F
		// (set) Token: 0x060059E3 RID: 23011 RVA: 0x001CD098 File Offset: 0x001CB298
		public double FallOff
		{
			get
			{
				return this.m_fallOff;
			}
			set
			{
				double num = this.m_max - this.m_min;
				this.m_raw = value;
				this.m_fallOff = ((value > num / 2.0) ? (num / 2.0) : value);
			}
		}

		// Token: 0x17000E4C RID: 3660
		// (get) Token: 0x060059E4 RID: 23012 RVA: 0x001CD0DC File Offset: 0x001CB2DC
		// (set) Token: 0x060059E5 RID: 23013 RVA: 0x001CD0E4 File Offset: 0x001CB2E4
		public double Maximum
		{
			get
			{
				return this.m_max;
			}
			set
			{
				this.m_max = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x17000E4D RID: 3661
		// (get) Token: 0x060059E6 RID: 23014 RVA: 0x001CD0F9 File Offset: 0x001CB2F9
		// (set) Token: 0x060059E7 RID: 23015 RVA: 0x001CD101 File Offset: 0x001CB301
		public double Minimum
		{
			get
			{
				return this.m_min;
			}
			set
			{
				this.m_min = value;
				this.FallOff = this.m_raw;
			}
		}

		// Token: 0x060059E8 RID: 23016 RVA: 0x001CD116 File Offset: 0x001CB316
		public void SetBounds(double min, double max)
		{
			this.m_min = min;
			this.m_max = max;
			this.FallOff = this.m_fallOff;
		}

		// Token: 0x060059E9 RID: 23017 RVA: 0x001CD134 File Offset: 0x001CB334
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[2].GetValue(x, y, z);
			if (this.m_fallOff > 0.0)
			{
				if (value < this.m_min - this.m_fallOff)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				if (value < this.m_min + this.m_fallOff)
				{
					double num = this.m_min - this.m_fallOff;
					double num2 = this.m_min + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num) / (num2 - num));
					return Utils.InterpolateLinear(this.modules[0].GetValue(x, y, z), this.modules[1].GetValue(x, y, z), position);
				}
				if (value < this.m_max - this.m_fallOff)
				{
					return this.modules[1].GetValue(x, y, z);
				}
				if (value < this.m_max + this.m_fallOff)
				{
					double num3 = this.m_max - this.m_fallOff;
					double num4 = this.m_max + this.m_fallOff;
					double position = Utils.MapCubicSCurve((value - num3) / (num4 - num3));
					return Utils.InterpolateLinear(this.modules[1].GetValue(x, y, z), this.modules[0].GetValue(x, y, z), position);
				}
				return this.modules[0].GetValue(x, y, z);
			}
			else
			{
				if (value < this.m_min || value > this.m_max)
				{
					return this.modules[0].GetValue(x, y, z);
				}
				return this.modules[1].GetValue(x, y, z);
			}
		}

		// Token: 0x040040DA RID: 16602
		private double m_fallOff;

		// Token: 0x040040DB RID: 16603
		private double m_raw;

		// Token: 0x040040DC RID: 16604
		private double m_min = -1.0;

		// Token: 0x040040DD RID: 16605
		private double m_max = 1.0;
	}
}
