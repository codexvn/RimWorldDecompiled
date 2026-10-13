using System;

namespace Verse.Noise
{
	// Token: 0x02000DB6 RID: 3510
	public class Arbitrary : ModuleBase
	{
		// Token: 0x06005976 RID: 22902 RVA: 0x001CA658 File Offset: 0x001C8858
		public Arbitrary() : base(1)
		{
		}

		// Token: 0x06005977 RID: 22903 RVA: 0x001CC3C5 File Offset: 0x001CA5C5
		public Arbitrary(ModuleBase source, Func<double, double> processor) : base(1)
		{
			this.modules[0] = source;
			this.processor = processor;
		}

		// Token: 0x06005978 RID: 22904 RVA: 0x001CC3DE File Offset: 0x001CA5DE
		public override double GetValue(double x, double y, double z)
		{
			return this.processor(this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x040040B9 RID: 16569
		private Func<double, double> processor;
	}
}
