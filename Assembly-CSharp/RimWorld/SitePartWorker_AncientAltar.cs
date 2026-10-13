using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200283E RID: 10302
	public class SitePartWorker_AncientAltar : SitePartWorker
	{
		// Token: 0x0600E16A RID: 57706 RVA: 0x004218CA File Offset: 0x0041FACA
		public override void Init(Site site, SitePart sitePart)
		{
			base.Init(site, sitePart);
			sitePart.relicThing = sitePart.parms.relicThing;
		}

		// Token: 0x0600E16B RID: 57707 RVA: 0x004218E5 File Offset: 0x0041FAE5
		public bool ShouldKeepMapForRelic(SitePart sitePart)
		{
			return sitePart.relicThing != null && !sitePart.relicThing.DestroyedOrNull() && sitePart.relicThing.MapHeld == sitePart.site.Map;
		}

		// Token: 0x0600E16C RID: 57708 RVA: 0x00421918 File Offset: 0x0041FB18
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (this.ShouldKeepMapForRelic(sitePart))
			{
				sitePart.relicThing.DeSpawnOrDeselect(DestroyMode.Vanish);
				if (sitePart.relicThing.holdingOwner != null)
				{
					sitePart.relicThing.holdingOwner.Remove(sitePart.relicThing);
				}
				sitePart.relicWasSpawned = false;
				return;
			}
			if (!sitePart.parms.relicLostSignal.NullOrEmpty())
			{
				Find.SignalManager.SendSignal(new Signal(sitePart.parms.relicLostSignal, false));
			}
		}

		// Token: 0x0600E16D RID: 57709 RVA: 0x0042199B File Offset: 0x0041FB9B
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}
	}
}
