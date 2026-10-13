using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F92 RID: 8082
	public class GenStep_AnimaTrees : GenStep_SpecialTrees
	{
		// Token: 0x17001B29 RID: 6953
		// (get) Token: 0x0600B28C RID: 45708 RVA: 0x00332090 File Offset: 0x00330290
		public override int SeedPart
		{
			get
			{
				return 647816171;
			}
		}

		// Token: 0x0600B28D RID: 45709 RVA: 0x00332097 File Offset: 0x00330297
		public override int DesiredTreeCountForMap(Map map)
		{
			return Mathf.Max(Mathf.RoundToInt(GenStep_AnimaTrees.Density * (float)map.Area), 1);
		}

		// Token: 0x0600B28E RID: 45710 RVA: 0x003320B4 File Offset: 0x003302B4
		protected override float GetGrowth()
		{
			return GenStep_AnimaTrees.GrowthRange.RandomInRange;
		}

		// Token: 0x04007A4B RID: 31307
		public static readonly float Density = 1.25E-05f;

		// Token: 0x04007A4C RID: 31308
		private static readonly FloatRange GrowthRange = new FloatRange(0.5f, 0.75f);
	}
}
