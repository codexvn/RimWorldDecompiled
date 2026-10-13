using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020024FE RID: 9470
	public class ApparelPolicy : Policy
	{
		// Token: 0x0600CEE6 RID: 52966 RVA: 0x003CC10F File Offset: 0x003CA30F
		public ApparelPolicy()
		{
		}

		// Token: 0x0600CEE7 RID: 52967 RVA: 0x003CC122 File Offset: 0x003CA322
		public ApparelPolicy(int id, string label) : base(id, label)
		{
		}

		// Token: 0x17001FBC RID: 8124
		// (get) Token: 0x0600CEE8 RID: 52968 RVA: 0x003CC137 File Offset: 0x003CA337
		protected override string LoadKey
		{
			get
			{
				return "ApparelPolicy";
			}
		}

		// Token: 0x0600CEE9 RID: 52969 RVA: 0x003CC140 File Offset: 0x003CA340
		public override void CopyFrom(Policy other)
		{
			ApparelPolicy apparelPolicy = other as ApparelPolicy;
			if (apparelPolicy == null)
			{
				return;
			}
			this.filter.CopyAllowancesFrom(apparelPolicy.filter);
		}

		// Token: 0x0600CEEA RID: 52970 RVA: 0x003CC169 File Offset: 0x003CA369
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingFilter>(ref this.filter, "filter", Array.Empty<object>());
		}

		// Token: 0x04008CD0 RID: 36048
		public ThingFilter filter = new ThingFilter();
	}
}
