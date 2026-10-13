using System;

namespace RimWorld.Planet
{
	// Token: 0x02003DEA RID: 15850
	public class SitePartWorker_Ambush : SitePartWorker
	{
		// Token: 0x06015A13 RID: 88595 RVA: 0x00679609 File Offset: 0x00677809
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints *= 0.8f;
			return sitePartParams;
		}

		// Token: 0x0400F05E RID: 61534
		private const float ThreatPointsFactor = 0.8f;
	}
}
