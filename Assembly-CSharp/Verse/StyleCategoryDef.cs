using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002E9 RID: 745
	public class StyleCategoryDef : Def
	{
		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06001247 RID: 4679 RVA: 0x00055214 File Offset: 0x00053414
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					if (this.iconPath.NullOrEmpty())
					{
						this.cachedIcon = BaseContent.BadTex;
					}
					else
					{
						this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
					}
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06001248 RID: 4680 RVA: 0x00055264 File Offset: 0x00053464
		public List<BuildableDef> AllDesignatorBuildables
		{
			get
			{
				if (this.cachedAllDesignatorBuildables == null)
				{
					this.cachedAllDesignatorBuildables = new List<BuildableDef>();
					if (this.addDesignators != null)
					{
						foreach (BuildableDef item in this.addDesignators)
						{
							this.cachedAllDesignatorBuildables.Add(item);
						}
					}
					if (this.addDesignatorGroups != null)
					{
						foreach (DesignatorDropdownGroupDef designatorDropdownGroupDef in this.addDesignatorGroups)
						{
							this.cachedAllDesignatorBuildables.AddRange(designatorDropdownGroupDef.BuildablesWithoutDefaultDesignators());
						}
					}
				}
				return this.cachedAllDesignatorBuildables;
			}
		}

		// Token: 0x06001249 RID: 4681 RVA: 0x00055334 File Offset: 0x00053534
		public ThingStyleDef GetStyleForThingDef(BuildableDef thingDef, Precept precept = null)
		{
			ThingStyleDef result;
			try
			{
				for (int i = 0; i < this.thingDefStyles.Count; i++)
				{
					if (this.thingDefStyles[i].ThingDef == thingDef)
					{
						StyleCategoryDef.tmpAvailableStyles.Add(this.thingDefStyles[i].StyleDef);
					}
				}
				if (StyleCategoryDef.tmpAvailableStyles.Count == 0)
				{
					result = null;
				}
				else if (StyleCategoryDef.tmpAvailableStyles.Count == 1 || precept == null)
				{
					result = StyleCategoryDef.tmpAvailableStyles[0];
				}
				else
				{
					result = StyleCategoryDef.tmpAvailableStyles[Rand.RangeSeeded(0, StyleCategoryDef.tmpAvailableStyles.Count, precept.randomSeed)];
				}
			}
			finally
			{
				StyleCategoryDef.tmpAvailableStyles.Clear();
			}
			return result;
		}

		// Token: 0x04001089 RID: 4233
		public List<ThingDefStyle> thingDefStyles;

		// Token: 0x0400108A RID: 4234
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400108B RID: 4235
		public bool fixedIdeoOnly;

		// Token: 0x0400108C RID: 4236
		public List<BuildableDef> addDesignators;

		// Token: 0x0400108D RID: 4237
		public List<DesignatorDropdownGroupDef> addDesignatorGroups;

		// Token: 0x0400108E RID: 4238
		public SoundDef soundOngoingRitual;

		// Token: 0x0400108F RID: 4239
		public RitualVisualEffectDef ritualVisualEffectDef;

		// Token: 0x04001090 RID: 4240
		private Texture2D cachedIcon;

		// Token: 0x04001091 RID: 4241
		private List<BuildableDef> cachedAllDesignatorBuildables;

		// Token: 0x04001092 RID: 4242
		private static List<ThingStyleDef> tmpAvailableStyles = new List<ThingStyleDef>();
	}
}
