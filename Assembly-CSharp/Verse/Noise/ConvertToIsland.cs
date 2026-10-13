using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DA2 RID: 3490
	public class ConvertToIsland : ModuleBase
	{
		// Token: 0x060058F6 RID: 22774 RVA: 0x001CA658 File Offset: 0x001C8858
		public ConvertToIsland() : base(1)
		{
		}

		// Token: 0x060058F7 RID: 22775 RVA: 0x001CA661 File Offset: 0x001C8861
		public ConvertToIsland(Vector3 viewCenter, float viewAngle, ModuleBase input) : base(1)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.modules[0] = input;
		}

		// Token: 0x060058F8 RID: 22776 RVA: 0x001CA684 File Offset: 0x001C8884
		public override double GetValue(double x, double y, double z)
		{
			float num = Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z));
			double value = this.modules[0].GetValue(x, y, z);
			float num2 = Mathf.Max(2.5f, this.viewAngle * 0.25f);
			float num3 = Mathf.Max(0.8f, this.viewAngle * 0.1f);
			if (num < this.viewAngle - num2)
			{
				return value;
			}
			float num4 = GenMath.LerpDouble(this.viewAngle - num2, this.viewAngle - num3, 0f, 0.62f, num);
			if (value > -0.11999999731779099)
			{
				return (value - -0.11999999731779099) * (double)(1f - num4 * 0.7f) - (double)(num4 * 0.3f) + -0.11999999731779099;
			}
			return value - (double)(num4 * 0.3f);
		}

		// Token: 0x04004075 RID: 16501
		public Vector3 viewCenter;

		// Token: 0x04004076 RID: 16502
		public float viewAngle;

		// Token: 0x04004077 RID: 16503
		private const float WaterLevel = -0.12f;
	}
}
