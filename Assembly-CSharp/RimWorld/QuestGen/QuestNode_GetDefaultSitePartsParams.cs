using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A3E RID: 14910
	public class QuestNode_GetDefaultSitePartsParams : QuestNode
	{
		// Token: 0x06014673 RID: 83571 RVA: 0x0061855B File Offset: 0x0061675B
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x06014674 RID: 83572 RVA: 0x00618565 File Offset: 0x00616765
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x06014675 RID: 83573 RVA: 0x00618574 File Offset: 0x00616774
		private void SetVars(Slate slate)
		{
			List<SitePartDefWithParams> list;
			SiteMakerHelper.GenerateDefaultParams(slate.Get<float>("points", 0f, false), this.tile.GetValue(slate), this.faction.GetValue(slate), this.sitePartDefs.GetValue(slate), out list);
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].def == SitePartDefOf.PreciousLump)
					{
						list[i].parms.preciousLumpResources = slate.Get<ThingDef>("targetMineable", null, false);
					}
				}
			}
			slate.Set<List<SitePartDefWithParams>>(this.storeSitePartsParamsAs.GetValue(slate), list, false);
		}

		// Token: 0x0400E185 RID: 57733
		public SlateRef<PlanetTile> tile;

		// Token: 0x0400E186 RID: 57734
		public SlateRef<Faction> faction;

		// Token: 0x0400E187 RID: 57735
		public SlateRef<IEnumerable<SitePartDef>> sitePartDefs;

		// Token: 0x0400E188 RID: 57736
		[NoTranslate]
		public SlateRef<string> storeSitePartsParamsAs;
	}
}
