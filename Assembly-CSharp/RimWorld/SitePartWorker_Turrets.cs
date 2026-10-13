using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200284E RID: 10318
	public class SitePartWorker_Turrets : SitePartWorker
	{
		// Token: 0x0600E1A1 RID: 57761 RVA: 0x004218AA File Offset: 0x0041FAAA
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600E1A2 RID: 57762 RVA: 0x004223D0 File Offset: 0x004205D0
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			sitePartParams.mortarsCount = Rand.RangeInclusive(0, 1);
			sitePartParams.turretsCount = Mathf.Clamp(Mathf.RoundToInt(sitePartParams.threatPoints / ThingDefOf.Turret_MiniTurret.building.combatPower), 2, 11);
			return sitePartParams;
		}

		// Token: 0x0600E1A3 RID: 57763 RVA: 0x0042241C File Offset: 0x0042061C
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			string threatsInfo = this.GetThreatsInfo(part.parms, part.site.Faction);
			outExtraDescriptionRules.Add(new Rule_String("threatsInfo", threatsInfo));
		}

		// Token: 0x0600E1A4 RID: 57764 RVA: 0x0042245D File Offset: 0x0042065D
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + this.GetThreatsInfo(sitePart.parms, site.Faction);
		}

		// Token: 0x0600E1A5 RID: 57765 RVA: 0x00422484 File Offset: 0x00420684
		private string GetThreatsInfo(SitePartParams parms, Faction faction)
		{
			this.threatsTmp.Clear();
			int num = parms.mortarsCount + 1;
			if (parms.turretsCount != 0)
			{
				string value;
				if (parms.turretsCount == 1)
				{
					value = "Turret".Translate();
				}
				else
				{
					value = "Turrets".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.turretsCount.ToString(), value));
			}
			if (parms.mortarsCount != 0)
			{
				string value;
				if (parms.mortarsCount == 1)
				{
					value = "Mortar".Translate();
				}
				else
				{
					value = "Mortars".Translate();
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(parms.mortarsCount.ToString(), value));
			}
			if (num != 0)
			{
				string value;
				if (faction == null)
				{
					value = ((num == 1) ? "Enemy".Translate() : "Enemies".Translate());
				}
				else
				{
					value = ((num == 1) ? faction.def.pawnSingular : faction.def.pawnsPlural);
				}
				this.threatsTmp.Add("KnownSiteThreatEnemyCountAppend".Translate(num.ToString(), value));
			}
			return this.threatsTmp.ToCommaList(true, false);
		}

		// Token: 0x0400978A RID: 38794
		private const int MinTurrets = 2;

		// Token: 0x0400978B RID: 38795
		private const int MaxTurrets = 11;

		// Token: 0x0400978C RID: 38796
		private List<string> threatsTmp = new List<string>();
	}
}
