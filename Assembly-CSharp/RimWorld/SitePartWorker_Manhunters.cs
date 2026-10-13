using System;
using System.Collections.Generic;
using RimWorld.Planet;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002847 RID: 10311
	public class SitePartWorker_Manhunters : SitePartWorker
	{
		// Token: 0x0600E182 RID: 57730 RVA: 0x004218AA File Offset: 0x0041FAAA
		public override string GetArrivedLetterPart(Map map, out LetterDef preferredLetterDef, out LookTargets lookTargets)
		{
			string arrivedLetterPart = base.GetArrivedLetterPart(map, out preferredLetterDef, out lookTargets);
			lookTargets = new LookTargets(map.Parent);
			return arrivedLetterPart;
		}

		// Token: 0x0600E183 RID: 57731 RVA: 0x00421D3C File Offset: 0x0041FF3C
		public override SitePartParams GenerateDefaultParams(float myThreatPoints, PlanetTile tile, Faction faction)
		{
			SitePartParams sitePartParams = base.GenerateDefaultParams(myThreatPoints, tile, faction);
			if (ManhunterPackGenStepUtility.TryGetAnimalsKind(sitePartParams.threatPoints, tile, out sitePartParams.animalKind))
			{
				sitePartParams.threatPoints = Mathf.Max(sitePartParams.threatPoints, sitePartParams.animalKind.combatPower);
			}
			return sitePartParams;
		}

		// Token: 0x0600E184 RID: 57732 RVA: 0x00421D84 File Offset: 0x0041FF84
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			int animalsCount = this.GetAnimalsCount(part.parms);
			string output = GenLabel.BestKindLabel(part.parms.animalKind, Gender.None, true, animalsCount);
			outExtraDescriptionRules.Add(new Rule_String("count", animalsCount.ToString()));
			outExtraDescriptionRules.Add(new Rule_String("kindLabel", output));
			outExtraDescriptionConstants.Add("count", animalsCount.ToString());
		}

		// Token: 0x0600E185 RID: 57733 RVA: 0x00421DF8 File Offset: 0x0041FFF8
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			int animalsCount = this.GetAnimalsCount(sitePart.parms);
			return base.GetPostProcessedThreatLabel(site, sitePart) + ": " + "KnownSiteThreatEnemyCountAppend".Translate(animalsCount.ToString(), GenLabel.BestKindLabel(sitePart.parms.animalKind, Gender.None, true, animalsCount));
		}

		// Token: 0x0600E186 RID: 57734 RVA: 0x00421E5C File Offset: 0x0042005C
		private int GetAnimalsCount(SitePartParams parms)
		{
			return AggressiveAnimalIncidentUtility.GetAnimalsCount(parms.animalKind, parms.threatPoints);
		}
	}
}
