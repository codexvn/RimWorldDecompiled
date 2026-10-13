using System;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000DA4 RID: 3492
	public class DistanceFromPlanetViewCenter : ModuleBase
	{
		// Token: 0x060058FC RID: 22780 RVA: 0x001CA600 File Offset: 0x001C8800
		public DistanceFromPlanetViewCenter() : base(0)
		{
		}

		// Token: 0x060058FD RID: 22781 RVA: 0x001CA7B9 File Offset: 0x001C89B9
		public DistanceFromPlanetViewCenter(Vector3 viewCenter, float viewAngle, bool invert = false) : base(0)
		{
			this.viewCenter = viewCenter;
			this.viewAngle = viewAngle;
			this.invert = invert;
		}

		// Token: 0x060058FE RID: 22782 RVA: 0x001CA7D8 File Offset: 0x001C89D8
		public override double GetValue(double x, double y, double z)
		{
			float valueInt = this.GetValueInt(x, y, z);
			if (this.invert)
			{
				return (double)(1f - valueInt);
			}
			return (double)valueInt;
		}

		// Token: 0x060058FF RID: 22783 RVA: 0x001CA802 File Offset: 0x001C8A02
		private float GetValueInt(double x, double y, double z)
		{
			if (this.viewAngle >= 180f)
			{
				return 0f;
			}
			return Mathf.Min(Vector3.Angle(this.viewCenter, new Vector3((float)x, (float)y, (float)z)) / this.viewAngle, 1f);
		}

		// Token: 0x0400407A RID: 16506
		public Vector3 viewCenter;

		// Token: 0x0400407B RID: 16507
		public float viewAngle;

		// Token: 0x0400407C RID: 16508
		public bool invert;
	}
}
