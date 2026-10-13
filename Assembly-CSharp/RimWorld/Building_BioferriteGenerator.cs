using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029DF RID: 10719
	public class Building_BioferriteGenerator : Building
	{
		// Token: 0x17002478 RID: 9336
		// (get) Token: 0x0600EC6C RID: 60524 RVA: 0x0044A9E4 File Offset: 0x00448BE4
		private CompHeatPusherPowered HeatPusher
		{
			get
			{
				CompHeatPusherPowered result;
				if ((result = this.heatPusher) == null)
				{
					result = (this.heatPusher = base.GetComp<CompHeatPusherPowered>());
				}
				return result;
			}
		}

		// Token: 0x0600EC6D RID: 60525 RVA: 0x0044AA0A File Offset: 0x00448C0A
		public override bool IsWorking()
		{
			return this.HeatPusher.ShouldPushHeatNow;
		}

		// Token: 0x04009CC3 RID: 40131
		[Unsaved(false)]
		private CompHeatPusherPowered heatPusher;
	}
}
