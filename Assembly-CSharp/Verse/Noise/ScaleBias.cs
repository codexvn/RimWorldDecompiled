using System;

namespace Verse.Noise
{
	// Token: 0x02000DC9 RID: 3529
	public class ScaleBias : ModuleBase
	{
		// Token: 0x060059D5 RID: 22997 RVA: 0x001CCF4E File Offset: 0x001CB14E
		public ScaleBias() : base(1)
		{
		}

		// Token: 0x060059D6 RID: 22998 RVA: 0x001CCF66 File Offset: 0x001CB166
		public ScaleBias(ModuleBase input) : base(1)
		{
			this.modules[0] = input;
		}

		// Token: 0x060059D7 RID: 22999 RVA: 0x001CCF87 File Offset: 0x001CB187
		public ScaleBias(double scale, double bias, ModuleBase input) : base(1)
		{
			this.modules[0] = input;
			this.Bias = bias;
			this.Scale = scale;
		}

		// Token: 0x17000E48 RID: 3656
		// (get) Token: 0x060059D8 RID: 23000 RVA: 0x001CCFB6 File Offset: 0x001CB1B6
		// (set) Token: 0x060059D9 RID: 23001 RVA: 0x001CCFBE File Offset: 0x001CB1BE
		public double Bias
		{
			get
			{
				return this.bias;
			}
			set
			{
				this.bias = value;
			}
		}

		// Token: 0x17000E49 RID: 3657
		// (get) Token: 0x060059DA RID: 23002 RVA: 0x001CCFC7 File Offset: 0x001CB1C7
		// (set) Token: 0x060059DB RID: 23003 RVA: 0x001CCFCF File Offset: 0x001CB1CF
		public double Scale
		{
			get
			{
				return this.scale;
			}
			set
			{
				this.scale = value;
			}
		}

		// Token: 0x060059DC RID: 23004 RVA: 0x001CCFD8 File Offset: 0x001CB1D8
		public override double GetValue(double x, double y, double z)
		{
			return this.modules[0].GetValue(x, y, z) * this.scale + this.bias;
		}

		// Token: 0x040040D8 RID: 16600
		private double scale = 1.0;

		// Token: 0x040040D9 RID: 16601
		private double bias;
	}
}
