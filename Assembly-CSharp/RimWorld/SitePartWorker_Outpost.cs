using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002849 RID: 10313
	public class SitePartWorker_Outpost : SitePartWorker
	{
		// Token: 0x0600E18C RID: 57740 RVA: 0x004218AA File Offset: 0x0041FAAA
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600E18D RID: 57741 RVA: 0x00421EA4 File Offset: 0x004200A4
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int enemiesCount = this.GetEnemiesCount(part.site, part.parms);
			outExtraDescriptionRules.Add(new Rule_String("enemiesCount", enemiesCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("enemiesLabel", this.GetEnemiesLabel(part.site, enemiesCount)));
		}

		// Token: 0x0600E18E RID: 57742 RVA: 0x00421F04 File Offset: 0x00420104
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.Faction.IsPlayer)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(this.GetEnemiesCount(site, sitePart.parms), "Enemies".Translate());
		}

		// Token: 0x0600E18F RID: 57743 RVA: 0x00421F68 File Offset: 0x00420168
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, faction.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Settlement, null));
			sitePartParams.lootMarketValue = SitePartWorker_Outpost.ThreatPointsLootMarketValue.Evaluate(sitePartParams.threatPoints);
			return sitePartParams;
		}

		// Token: 0x0600E190 RID: 57744 RVA: 0x00421FB8 File Offset: 0x004201B8
		protected int GetEnemiesCount(Site site, SitePartParams parms)
		{
			return PawnGroupMakerUtility.GeneratePawnKindsExample(new PawnGroupMakerParms
			{
				tile = site.Tile,
				faction = site.Faction,
				groupKind = PawnGroupKindDefOf.Settlement,
				points = parms.threatPoints,
				inhabitants = true,
				seed = new int?(OutpostSitePartUtility.GetPawnGroupMakerSeed(parms))
			}).Count<PawnKindDef>();
		}

		// Token: 0x0600E191 RID: 57745 RVA: 0x0042201C File Offset: 0x0042021C
		protected string GetEnemiesLabel(Site site, int enemiesCount)
		{
			if (site.Faction == null)
			{
				return (enemiesCount == 1) ? "Enemy".Translate() : "Enemies".Translate();
			}
			if (enemiesCount != 1)
			{
				return site.Faction.def.pawnsPlural;
			}
			return site.Faction.def.pawnSingular;
		}

		// Token: 0x04009789 RID: 38793
		public static readonly SimpleCurve ThreatPointsLootMarketValue = new SimpleCurve
		{
			{
				new CurvePoint(100f, 200f),
				true
			},
			{
				new CurvePoint(250f, 450f),
				true
			},
			{
				new CurvePoint(800f, 1000f),
				true
			},
			{
				new CurvePoint(10000f, 2000f),
				true
			}
		};
	}
}
