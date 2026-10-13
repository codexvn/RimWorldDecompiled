using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002848 RID: 10312
	public class SitePartWorker_MechCluster : SitePartWorker
	{
		// Token: 0x0600E188 RID: 57736 RVA: 0x00421E6F File Offset: 0x0042006F
		public override bool IsAvailable()
		{
			return base.IsAvailable() && Faction.OfMechanoids != null;
		}

		// Token: 0x0600E189 RID: 57737 RVA: 0x004218AA File Offset: 0x0041FAAA
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600E18A RID: 57738 RVA: 0x00421E83 File Offset: 0x00420083
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, 750f);
			return sitePartParams;
		}

		// Token: 0x04009788 RID: 38792
		public const float MinPoints = 750f;
	}
}
