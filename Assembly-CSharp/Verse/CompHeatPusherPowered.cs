using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000901 RID: 2305
	public class CompHeatPusherPowered : CompHeatPusher
	{
		// Token: 0x17000B12 RID: 2834
		// (get) Token: 0x06003B85 RID: 15237 RVA: 0x0013A418 File Offset: 0x00138618
		public override bool ShouldPushHeatNow
		{
			get
			{
				return base.ShouldPushHeatNow && FlickUtility.WantsToBeOn(this.parent) && (this.powerComp == null || this.powerComp.PowerOn) && (this.refuelableComp == null || this.refuelableComp.HasFuel) && (this.breakdownableComp == null || !this.breakdownableComp.BrokenDown);
			}
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x0013A47C File Offset: 0x0013867C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
			this.breakdownableComp = this.parent.GetComp<CompBreakdownable>();
		}

		// Token: 0x04002AF5 RID: 10997
		protected CompPowerTrader powerComp;

		// Token: 0x04002AF6 RID: 10998
		protected CompRefuelable refuelableComp;

		// Token: 0x04002AF7 RID: 10999
		protected CompBreakdownable breakdownableComp;
	}
}
