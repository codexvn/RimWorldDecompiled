using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using Verse;
using Verse.Grammar;

namespace RimWorld.Planet
{
	// Token: 0x02003DF5 RID: 15861
	public class SitePartWorker_PreciousLump : SitePartWorker
	{
		// Token: 0x06015A31 RID: 88625 RVA: 0x0042199B File Offset: 0x0041FB9B
		public override string GetPostProcessedThreatLabel(Site site, SitePart sitePart)
		{
			if (site.MainSitePartDef == this.def)
			{
				return null;
			}
			return base.GetPostProcessedThreatLabel(site, sitePart);
		}

		// Token: 0x06015A32 RID: 88626 RVA: 0x0067A204 File Offset: 0x00678404
		public override void Notify_GeneratedByQuestGen(SitePart part, Slate slate, List<Rule> outExtraDescriptionRules, Dictionary<string, string> outExtraDescriptionConstants)
		{
			base.Notify_GeneratedByQuestGen(part, slate, outExtraDescriptionRules, outExtraDescriptionConstants);
			if (part.site.ActualThreatPoints > 0f)
			{
				outExtraDescriptionRules.Add(new Rule_String("lumpThreatDescription", "\n\n" + "PreciousLumpHostileThreat".Translate()));
				return;
			}
			outExtraDescriptionRules.Add(new Rule_String("lumpThreatDescription", ""));
		}
	}
}
