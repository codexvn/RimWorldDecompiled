using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002691 RID: 9873
	public class RitualAttachableOutcomeEffectWorker_DiscoverAncientComplex : RitualAttachableOutcomeEffectWorker
	{
		// Token: 0x0600D8E5 RID: 55525 RVA: 0x004045FC File Offset: 0x004027FC
		public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual, RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
		{
			if (!this.CanApplyNow(jobRitual.Ritual, jobRitual.Map))
			{
				extraOutcomeDesc = null;
				return;
			}
			extraOutcomeDesc = this.def.letterInfoText;
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.OpportunitySite_AncientComplex, StorytellerUtility.DefaultThreatPointsNow(jobRitual.Map));
			letterLookTargets = new LookTargets((letterLookTargets.targets ?? new List<GlobalTargetInfo>()).Concat(quest.QuestLookTargets));
		}

		// Token: 0x0600D8E6 RID: 55526 RVA: 0x00404670 File Offset: 0x00402870
		public override AcceptanceReport CanApplyNow(Precept_Ritual ritual, Map map)
		{
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				Site site = worldObject as Site;
				if (site != null)
				{
					if (site.parts.Any((SitePart p) => p.def == SitePartDefOf.AncientComplex))
					{
						return "RitualAttachedOutcomeCantApply_AncientComplexIsPresent".Translate();
					}
				}
			}
			foreach (Quest quest in Find.QuestManager.QuestsListForReading)
			{
				if (quest.root == QuestScriptDefOf.AncientComplex_Mission && !quest.Historical)
				{
					return "RitualAttachedOutcomeCantApply_AncientComplexIsPresent".Translate();
				}
			}
			float points = StorytellerUtility.DefaultThreatPointsNow(map);
			if (!QuestScriptDefOf.OpportunitySite_AncientComplex.CanRun(points, map))
			{
				return "RitualAttachedOutcomeCantApply_NotAvailableRightNow".Translate();
			}
			return base.CanApplyNow(ritual, map);
		}
	}
}
