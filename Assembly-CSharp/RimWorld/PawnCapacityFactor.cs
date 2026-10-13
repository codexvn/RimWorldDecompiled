using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEF RID: 6895
	public class PawnCapacityFactor
	{
		// Token: 0x06009A1A RID: 39450 RVA: 0x002CD748 File Offset: 0x002CB948
		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
			if (this.allowedDefect != 0f && num < 1f)
			{
				num = Mathf.InverseLerp(0f, 1f - this.allowedDefect, num);
			}
			if (num > this.max)
			{
				num = this.max;
			}
			if (this.useReciprocal)
			{
				if (Mathf.Abs(num) < 0.001f)
				{
					num = 5f;
				}
				else
				{
					num = Mathf.Min(1f / num, 5f);
				}
			}
			return num;
		}

		// Token: 0x04006A00 RID: 27136
		public PawnCapacityDef capacity;

		// Token: 0x04006A01 RID: 27137
		public float weight = 1f;

		// Token: 0x04006A02 RID: 27138
		public float max = 9999f;

		// Token: 0x04006A03 RID: 27139
		public bool useReciprocal;

		// Token: 0x04006A04 RID: 27140
		public float allowedDefect;

		// Token: 0x04006A05 RID: 27141
		private const float MaxReciprocalFactor = 5f;
	}
}
