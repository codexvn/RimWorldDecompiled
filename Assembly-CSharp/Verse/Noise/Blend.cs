using System;

namespace Verse.Noise
{
	// Token: 0x02000DB7 RID: 3511
	public class Blend : ModuleBase
	{
		// Token: 0x06005979 RID: 22905 RVA: 0x001CC3FB File Offset: 0x001CA5FB
		public Blend() : base(3)
		{
		}

		// Token: 0x0600597A RID: 22906 RVA: 0x001CC404 File Offset: 0x001CA604
		public Blend(ModuleBase lhs, ModuleBase rhs, ModuleBase controller) : base(3)
		{
			this.modules[0] = lhs;
			this.modules[1] = rhs;
			this.modules[2] = controller;
		}

		// Token: 0x17000E38 RID: 3640
		// (get) Token: 0x0600597B RID: 22907 RVA: 0x001CC428 File Offset: 0x001CA628
		// (set) Token: 0x0600597C RID: 22908 RVA: 0x001CC432 File Offset: 0x001CA632
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

		// Token: 0x0600597D RID: 22909 RVA: 0x001CC440 File Offset: 0x001CA640
		public override double GetValue(double x, double y, double z)
		{
			double value = this.modules[0].GetValue(x, y, z);
			double value2 = this.modules[1].GetValue(x, y, z);
			double position = (this.modules[2].GetValue(x, y, z) + 1.0) / 2.0;
			return Utils.InterpolateLinear(value, value2, position);
		}
	}
}
