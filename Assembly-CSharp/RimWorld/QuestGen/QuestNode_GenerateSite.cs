using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.Grammar;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A2F RID: 14895
	public class QuestNode_GenerateSite : QuestNode
	{
		// Token: 0x06014642 RID: 83522 RVA: 0x00617CB4 File Offset: 0x00615EB4
		protected override bool TestRunInt(Slate slate)
		{
			if (!Find.Storyteller.difficulty.allowViolentQuests && this.sitePartsParams.GetValue(slate) != null)
			{
				using (IEnumerator<SitePartDefWithParams> enumerator = this.sitePartsParams.GetValue(slate).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def.wantsThreatPoints)
						{
							return false;
						}
					}
				}
				return true;
			}
			return true;
		}

		// Token: 0x06014643 RID: 83523 RVA: 0x00617D34 File Offset: 0x00615F34
		protected override void RunInt()
		{
			Slate slate = QuestGen.slate;
			Site var = QuestGen_Sites.GenerateSite(this.sitePartsParams.GetValue(slate), this.tile.GetValue(slate), this.faction.GetValue(slate), this.hiddenSitePartsPossible.GetValue(slate), this.singleSitePartRules.GetValue(slate), this.worldObjectDef.GetValue(slate));
			if (this.storeAs.GetValue(slate) != null)
			{
				QuestGen.slate.Set<Site>(this.storeAs.GetValue(slate), var, false);
			}
		}

		// Token: 0x0400E154 RID: 57684
		public SlateRef<IEnumerable<SitePartDefWithParams>> sitePartsParams;

		// Token: 0x0400E155 RID: 57685
		public SlateRef<Faction> faction;

		// Token: 0x0400E156 RID: 57686
		public SlateRef<PlanetTile> tile;

		// Token: 0x0400E157 RID: 57687
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400E158 RID: 57688
		public SlateRef<RulePack> singleSitePartRules;

		// Token: 0x0400E159 RID: 57689
		public SlateRef<bool> hiddenSitePartsPossible;

		// Token: 0x0400E15A RID: 57690
		public SlateRef<WorldObjectDef> worldObjectDef;

		// Token: 0x0400E15B RID: 57691
		private const string RootSymbol = "root";
	}
}
