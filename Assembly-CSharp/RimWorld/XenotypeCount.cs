using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200289F RID: 10399
	public class XenotypeCount : StartingPawnCount
	{
		// Token: 0x170022A1 RID: 8865
		// (get) Token: 0x0600E3A6 RID: 58278 RVA: 0x0042873C File Offset: 0x0042693C
		public override string Summary
		{
			get
			{
				return "PawnCount".Translate(this.count.Named("COUNT"), (this.description.NullOrEmpty() ? this.xenotype.label : this.description).Named("KINDLABEL"));
			}
		}

		// Token: 0x0600E3A7 RID: 58279 RVA: 0x00428797 File Offset: 0x00426997
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<XenotypeDef>(ref this.xenotype, "xenotype");
			Scribe_Values.Look<DevelopmentalStage>(ref this.allowedDevelopmentalStages, "allowedDevelopmentalStages", DevelopmentalStage.None, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
		}

		// Token: 0x0600E3A8 RID: 58280 RVA: 0x004287D4 File Offset: 0x004269D4
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			num ^= this.allowedDevelopmentalStages.GetHashCode();
			num ^= this.description.GetHashCodeSafe();
			if (this.xenotype != null)
			{
				num ^= this.xenotype.GetHashCode();
			}
			return num;
		}

		// Token: 0x0400986F RID: 39023
		public XenotypeDef xenotype;

		// Token: 0x04009870 RID: 39024
		public DevelopmentalStage allowedDevelopmentalStages = DevelopmentalStage.Baby | DevelopmentalStage.Child | DevelopmentalStage.Adult;

		// Token: 0x04009871 RID: 39025
		[MustTranslate]
		public string description;
	}
}
