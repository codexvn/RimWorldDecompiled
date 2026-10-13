using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002841 RID: 10305
	public class SitePartWorker_ConditionCauser : SitePartWorker
	{
		// Token: 0x0600E171 RID: 57713 RVA: 0x004219B8 File Offset: 0x0041FBB8
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int worldRange = sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange;
			return base.GetPostProcessedThreatLabel(site, sitePart) + " (" + "ConditionCauserRadius".Translate(worldRange) + ")";
		}

		// Token: 0x0600E172 RID: 57714 RVA: 0x00421A14 File Offset: 0x0041FC14
		public override void Init(Site site, SitePart sitePart)
		{
			sitePart.conditionCauser = ThingMaker.MakeThing(sitePart.def.conditionCauserDef, null);
			sitePart.conditionCauser.SetFaction(site.Faction, null);
			CompCauseGameCondition compCauseGameCondition = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition>();
			compCauseGameCondition.RandomizeSettings(site);
			compCauseGameCondition.LinkWithSite(sitePart.site);
		}

		// Token: 0x0600E173 RID: 57715 RVA: 0x00421A67 File Offset: 0x0041FC67
		public override void SitePartWorkerTick(SitePart sitePart)
		{
			if (!sitePart.conditionCauser.DestroyedOrNull() && !sitePart.conditionCauser.Spawned)
			{
				sitePart.conditionCauser.DoTick();
			}
		}

		// Token: 0x0600E174 RID: 57716 RVA: 0x00421A8E File Offset: 0x0041FC8E
		public override void PostDrawExtraSelectionOverlays(SitePart sitePart)
		{
			base.PostDrawExtraSelectionOverlays(sitePart);
			GenDraw.DrawWorldRadiusRing(sitePart.site.Tile, sitePart.def.conditionCauserDef.GetCompProperties<CompProperties_CausesGameCondition>().worldRange, null);
		}

		// Token: 0x0600E175 RID: 57717 RVA: 0x00421AC0 File Offset: 0x0041FCC0
		public override void Notify_SiteMapAboutToBeRemoved(SitePart sitePart)
		{
			base.Notify_SiteMapAboutToBeRemoved(sitePart);
			if (!sitePart.conditionCauser.DestroyedOrNull() && sitePart.conditionCauser.Spawned && sitePart.conditionCauser.Map == sitePart.site.Map)
			{
				sitePart.conditionCauser.DeSpawn(DestroyMode.Vanish);
				sitePart.conditionCauserWasSpawned = false;
			}
		}

		// Token: 0x0600E176 RID: 57718 RVA: 0x00421B19 File Offset: 0x0041FD19
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			slate.Set<Thing>("conditionCauser", part.conditionCauser, false);
			outExtraDescriptionRules.Add(new Rule_String("problemCauserLabel", part.conditionCauser.Label));
		}
	}
}
