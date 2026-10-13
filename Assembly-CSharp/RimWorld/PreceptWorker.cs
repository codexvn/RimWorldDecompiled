using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC2 RID: 6850
	public class PreceptWorker
	{
		// Token: 0x0600995E RID: 39262 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool ShouldSkipThing(Ideo ideo, ThingDef thingDef)
		{
			return false;
		}

		// Token: 0x0600995F RID: 39263 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float GetThingOrder(PreceptThingChance thingChance)
		{
			return 0f;
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x06009960 RID: 39264 RVA: 0x002CAFCF File Offset: 0x002C91CF
		public virtual IEnumerable<PreceptThingChance> ThingDefs
		{
			get
			{
				return from dc in this.def.buildingDefChances
				select dc;
			}
		}

		// Token: 0x06009961 RID: 39265 RVA: 0x0020FC7C File Offset: 0x0020DE7C
		public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo, FactionDef generatingFor)
		{
			return true;
		}

		// Token: 0x06009962 RID: 39266 RVA: 0x002CB000 File Offset: 0x002C9200
		[Obsolete("Will be removed in a future update")]
		public virtual AcceptanceReport CanUse(ThingDef def, Ideo ideo)
		{
			return this.CanUse(def, ideo, null);
		}

		// Token: 0x06009963 RID: 39267 RVA: 0x002CB00B File Offset: 0x002C920B
		public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo, FactionDef generatingFor = null)
		{
			foreach (PreceptThingChance preceptThingChance in this.ThingDefs)
			{
				if (this.CanUse(preceptThingChance.def, ideo, generatingFor))
				{
					yield return preceptThingChance;
				}
			}
			IEnumerator<PreceptThingChance> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06009964 RID: 39268 RVA: 0x002CB029 File Offset: 0x002C9229
		[Obsolete("Will be removed in a future update")]
		public virtual IEnumerable<PreceptThingChance> ThingDefsForIdeo(Ideo ideo)
		{
			return this.ThingDefsForIdeo(ideo, null);
		}

		// Token: 0x040068B4 RID: 26804
		public PreceptDef def;
	}
}
