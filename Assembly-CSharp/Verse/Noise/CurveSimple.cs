using System;

namespace Verse.Noise
{
	// Token: 0x02000DAB RID: 3499
	public class CurveSimple : ModuleBase
	{
		// Token: 0x06005912 RID: 22802 RVA: 0x001CAAFD File Offset: 0x001C8CFD
		public CurveSimple(ModuleBase input, SimpleCurve curve) : base(1)
		{
			this.modules[0] = input;
			this.curve = curve;
		}

		// Token: 0x06005913 RID: 22803 RVA: 0x001CAB16 File Offset: 0x001C8D16
		public override double GetValue(double x, double y, double z)
		{
			return (double)this.curve.Evaluate((float)this.modules[0].GetValue(x, y, z));
		}

		// Token: 0x04004085 RID: 16517
		private SimpleCurve curve;
	}
}
