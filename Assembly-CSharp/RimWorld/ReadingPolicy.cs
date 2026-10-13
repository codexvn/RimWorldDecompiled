using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002510 RID: 9488
	public class ReadingPolicy : Policy
	{
		// Token: 0x17001FD1 RID: 8145
		// (get) Token: 0x0600CF4C RID: 53068 RVA: 0x003CD50A File Offset: 0x003CB70A
		protected override string LoadKey
		{
			get
			{
				return "ReadingPolicy";
			}
		}

		// Token: 0x0600CF4D RID: 53069 RVA: 0x003CD511 File Offset: 0x003CB711
		public ReadingPolicy()
		{
		}

		// Token: 0x0600CF4E RID: 53070 RVA: 0x003CD524 File Offset: 0x003CB724
		public ReadingPolicy(int id, string label) : base(id, label)
		{
			this.defFilter.SetAllowAll(null, true);
			this.effectFilter = new ThingFilter(ThingCategoryDefOf.BookEffects, true);
		}

		// Token: 0x0600CF4F RID: 53071 RVA: 0x003CD558 File Offset: 0x003CB758
		public override void CopyFrom(Policy other)
		{
			ReadingPolicy readingPolicy = other as ReadingPolicy;
			if (readingPolicy == null)
			{
				return;
			}
			this.defFilter.CopyAllowancesFrom(readingPolicy.defFilter);
			this.effectFilter.CopyAllowancesFrom(readingPolicy.effectFilter);
		}

		// Token: 0x0600CF50 RID: 53072 RVA: 0x003CD592 File Offset: 0x003CB792
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingFilter>(ref this.defFilter, "defFilter", Array.Empty<object>());
			Scribe_Deep.Look<ThingFilter>(ref this.effectFilter, "effectFilter", Array.Empty<object>());
		}

		// Token: 0x04008CFC RID: 36092
		public ThingFilter defFilter = new ThingFilter();

		// Token: 0x04008CFD RID: 36093
		public ThingFilter effectFilter;
	}
}
