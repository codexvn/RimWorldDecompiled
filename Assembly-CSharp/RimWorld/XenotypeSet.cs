using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B36 RID: 6966
	public class XenotypeSet
	{
		// Token: 0x170017F3 RID: 6131
		public XenotypeChance this[int index]
		{
			get
			{
				return this.xenotypeChances[index];
			}
		}

		// Token: 0x170017F4 RID: 6132
		// (get) Token: 0x06009B9F RID: 39839 RVA: 0x002D2457 File Offset: 0x002D0657
		public int Count
		{
			get
			{
				return this.xenotypeChances.Count;
			}
		}

		// Token: 0x170017F5 RID: 6133
		// (get) Token: 0x06009BA0 RID: 39840 RVA: 0x002D2464 File Offset: 0x002D0664
		public float BaselinerChance
		{
			get
			{
				if (!ModsConfig.BiotechActive || this.xenotypeChances.NullOrEmpty<XenotypeChance>())
				{
					return 1f;
				}
				float num = 0f;
				for (int i = 0; i < this.xenotypeChances.Count; i++)
				{
					if (this.xenotypeChances[i].xenotype != XenotypeDefOf.Baseliner)
					{
						num += this.xenotypeChances[i].chance;
					}
				}
				return 1f - num;
			}
		}

		// Token: 0x06009BA1 RID: 39841 RVA: 0x002D24DC File Offset: 0x002D06DC
		public bool Contains(XenotypeDef xenotype)
		{
			if (xenotype == XenotypeDefOf.Baseliner && this.BaselinerChance > 0f)
			{
				return true;
			}
			for (int i = 0; i < this.xenotypeChances.Count; i++)
			{
				if (this.xenotypeChances[i].xenotype == xenotype)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04006C12 RID: 27666
		private List<XenotypeChance> xenotypeChances = new List<XenotypeChance>();
	}
}
