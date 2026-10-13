using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002503 RID: 9475
	public class FoodPolicy : Policy
	{
		// Token: 0x0600CEFE RID: 52990 RVA: 0x003CC5D7 File Offset: 0x003CA7D7
		public FoodPolicy()
		{
		}

		// Token: 0x0600CEFF RID: 52991 RVA: 0x003CC5EA File Offset: 0x003CA7EA
		public FoodPolicy(int id, string label) : base(id, label)
		{
		}

		// Token: 0x17001FC1 RID: 8129
		// (get) Token: 0x0600CF00 RID: 52992 RVA: 0x003CC5FF File Offset: 0x003CA7FF
		protected override string LoadKey
		{
			get
			{
				return "FoodPolicy";
			}
		}

		// Token: 0x0600CF01 RID: 52993 RVA: 0x003CC606 File Offset: 0x003CA806
		public bool Allows(ThingDef def)
		{
			return this.filter.Allows(def);
		}

		// Token: 0x0600CF02 RID: 52994 RVA: 0x003CC614 File Offset: 0x003CA814
		public bool Allows(Thing thing)
		{
			return this.filter.Allows(thing);
		}

		// Token: 0x0600CF03 RID: 52995 RVA: 0x003CC624 File Offset: 0x003CA824
		public override void CopyFrom(Policy other)
		{
			FoodPolicy foodPolicy = other as FoodPolicy;
			if (foodPolicy == null)
			{
				return;
			}
			this.filter.CopyAllowancesFrom(foodPolicy.filter);
		}

		// Token: 0x0600CF04 RID: 52996 RVA: 0x003CC64D File Offset: 0x003CA84D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x04008CE2 RID: 36066
		public ThingFilter filter = new ThingFilter();
	}
}
