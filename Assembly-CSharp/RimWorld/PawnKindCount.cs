using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200289A RID: 10394
	public class PawnKindCount : StartingPawnCount
	{
		// Token: 0x1700229D RID: 8861
		// (get) Token: 0x0600E38D RID: 58253 RVA: 0x00427F90 File Offset: 0x00426190
		public override string Summary
		{
			get
			{
				return "PawnCount".Translate(this.count.Named("COUNT"), ((this.count > 1) ? this.kindDef.GetLabelPlural(this.count) : this.kindDef.label).Named("KINDLABEL"));
			}
		}

		// Token: 0x0600E38E RID: 58254 RVA: 0x00427FF2 File Offset: 0x004261F2
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
		}

		// Token: 0x0600E38F RID: 58255 RVA: 0x0042800C File Offset: 0x0042620C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			num ^= this.count;
			num ^= (this.requiredAtStart ? 1 : 0);
			if (this.kindDef != null)
			{
				num ^= this.kindDef.GetHashCode();
			}
			return num;
		}

		// Token: 0x04009864 RID: 39012
		public PawnKindDef kindDef;
	}
}
