using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF0 RID: 6896
	public class PawnCapacityOffset
	{
		// Token: 0x06009A1C RID: 39452 RVA: 0x002CD7E2 File Offset: 0x002CB9E2
		public float GetOffset(float capacityEfficiency)
		{
			return (Mathf.Min(capacityEfficiency, this.max) - 1f) * this.scale;
		}

		// Token: 0x04006A06 RID: 27142
		public PawnCapacityDef capacity;

		// Token: 0x04006A07 RID: 27143
		public float scale = 1f;

		// Token: 0x04006A08 RID: 27144
		public float max = 9999f;
	}
}
