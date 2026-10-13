using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200259A RID: 9626
	public abstract class GoodwillSituationWorker
	{
		// Token: 0x0600D301 RID: 54017 RVA: 0x003E28B7 File Offset: 0x003E0AB7
		public virtual string GetPostProcessedLabel(Faction other)
		{
			return this.def.label;
		}

		// Token: 0x0600D302 RID: 54018 RVA: 0x003E28C4 File Offset: 0x003E0AC4
		public string GetPostProcessedLabelCap(Faction other)
		{
			return this.GetPostProcessedLabel(other).CapitalizeFirst(this.def);
		}

		// Token: 0x0600D303 RID: 54019 RVA: 0x003E28D8 File Offset: 0x003E0AD8
		public virtual int GetMaxGoodwill(Faction other)
		{
			return 100;
		}

		// Token: 0x0600D304 RID: 54020 RVA: 0x00002501 File Offset: 0x00000701
		public virtual int GetNaturalGoodwillOffset(Faction other)
		{
			return 0;
		}

		// Token: 0x04008F3B RID: 36667
		public GoodwillSituationDef def;
	}
}
