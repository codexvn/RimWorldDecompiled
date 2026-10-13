using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000461 RID: 1121
	public class PollutionInfo : MapComponent
	{
		// Token: 0x06001D67 RID: 7527 RVA: 0x00094327 File Offset: 0x00092527
		public PollutionInfo(Map map) : base(map)
		{
		}

		// Token: 0x06001D68 RID: 7528 RVA: 0x00094884 File Offset: 0x00092A84
		public override void MapGenerated()
		{
			if (ModsConfig.BiotechActive)
			{
				this.pollutionAtGen = (float)this.map.pollutionGrid.TotalPollution;
			}
		}

		// Token: 0x06001D69 RID: 7529 RVA: 0x000948A4 File Offset: 0x00092AA4
		public override void MapRemoved()
		{
			if (ModsConfig.BiotechActive)
			{
				float num = (float)this.map.pollutionGrid.TotalPollution - this.pollutionAtGen;
				CompProperties_DissolutionEffectPollution compProperties = ThingDefOf.Wastepack.GetCompProperties<CompProperties_DissolutionEffectPollution>();
				int amount = (int)(num / (float)compProperties.cellsToPollutePerDissolution);
				if (num > 0f)
				{
					CompDissolutionEffect_Goodwill.AddWorldDissolutionEvent(amount, this.map.Tile);
				}
			}
		}

		// Token: 0x06001D6A RID: 7530 RVA: 0x000948FE File Offset: 0x00092AFE
		public override void ExposeData()
		{
			if (ModsConfig.BiotechActive)
			{
				Scribe_Values.Look<float>(ref this.pollutionAtGen, "pollutionAtGen", 0f, false);
			}
		}

		// Token: 0x04001989 RID: 6537
		private float pollutionAtGen;
	}
}
