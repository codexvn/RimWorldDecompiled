using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF8 RID: 6904
	public class StyleItemTagWeighted
	{
		// Token: 0x170017AA RID: 6058
		// (get) Token: 0x06009A3B RID: 39483 RVA: 0x002CDDCB File Offset: 0x002CBFCB
		public string Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x170017AB RID: 6059
		// (get) Token: 0x06009A3C RID: 39484 RVA: 0x002CDDD3 File Offset: 0x002CBFD3
		public float TotalWeight
		{
			get
			{
				return this.baseWeight * this.weightFactor;
			}
		}

		// Token: 0x06009A3D RID: 39485 RVA: 0x000024DF File Offset: 0x000006DF
		public StyleItemTagWeighted()
		{
		}

		// Token: 0x06009A3E RID: 39486 RVA: 0x002CDDE2 File Offset: 0x002CBFE2
		public StyleItemTagWeighted(string tag, float baseWeight, float weightFactor = 1f)
		{
			this.tag = tag;
			this.baseWeight = baseWeight;
			this.weightFactor = weightFactor;
		}

		// Token: 0x06009A3F RID: 39487 RVA: 0x002CDDFF File Offset: 0x002CBFFF
		public void Add(StyleItemTagWeighted other)
		{
			this.baseWeight += other.baseWeight;
			this.weightFactor *= other.weightFactor;
		}

		// Token: 0x04006A41 RID: 27201
		[NoTranslate]
		private string tag;

		// Token: 0x04006A42 RID: 27202
		private float baseWeight;

		// Token: 0x04006A43 RID: 27203
		private float weightFactor;
	}
}
