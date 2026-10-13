using System;

namespace Verse.Noise
{
	// Token: 0x02000DA8 RID: 3496
	public class DistFromPoint : ModuleBase
	{
		// Token: 0x06005909 RID: 22793 RVA: 0x001CA952 File Offset: 0x001C8B52
		public DistFromPoint() : base(0)
		{
		}

		// Token: 0x0600590A RID: 22794 RVA: 0x001CA966 File Offset: 0x001C8B66
		public DistFromPoint(float span) : base(0)
		{
			this.span = span;
		}

		// Token: 0x0600590B RID: 22795 RVA: 0x001CA981 File Offset: 0x001C8B81
		public override double GetValue(double x, double y, double z)
		{
			if (this.span <= 0f)
			{
				Log.ErrorOnce("Span must be greater than zero", 8745456);
				return 0.0;
			}
			return Math.Sqrt(x * x + z * z) / (double)this.span;
		}

		// Token: 0x04004081 RID: 16513
		public float span = 1f;
	}
}
