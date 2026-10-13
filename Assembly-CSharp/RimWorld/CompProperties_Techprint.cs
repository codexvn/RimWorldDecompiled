using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002E75 RID: 11893
	public class CompProperties_Techprint : CompProperties
	{
		// Token: 0x06010BF6 RID: 68598 RVA: 0x004D38FB File Offset: 0x004D1AFB
		public CompProperties_Techprint()
		{
			this.compClass = typeof(CompTechprint);
		}

		// Token: 0x06010BF7 RID: 68599 RVA: 0x004D3914 File Offset: 0x004D1B14
		public override void ResolveReferences(ThingDef parentDef)
		{
			if (parentDef.descriptionHyperlinks == null)
			{
				parentDef.descriptionHyperlinks = new List<DefHyperlink>();
			}
			List<Def> unlockedDefs = this.project.UnlockedDefs;
			for (int i = 0; i < unlockedDefs.Count; i++)
			{
				ThingDef thingDef = unlockedDefs[i] as ThingDef;
				if (thingDef != null)
				{
					parentDef.descriptionHyperlinks.Add(thingDef);
				}
				else
				{
					RecipeDef recipeDef = unlockedDefs[i] as RecipeDef;
					if (recipeDef != null && !recipeDef.products.NullOrEmpty<ThingDefCountClass>())
					{
						for (int j = 0; j < recipeDef.products.Count; j++)
						{
							parentDef.descriptionHyperlinks.Add(recipeDef.products[j].thingDef);
						}
					}
				}
			}
			parentDef.description += "\n\n" + "Unlocks".Translate() + ": " + (from x in this.project.UnlockedDefs
			select x.label).ToCommaList(false, false).CapitalizeFirst();
		}

		// Token: 0x0400AE81 RID: 44673
		public ResearchProjectDef project;
	}
}
