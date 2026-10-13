using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000221 RID: 545
	public class CreepJoinerFormKindDef : PawnKindDef, ICreepJoinerDef
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x00043B7B File Offset: 0x00041D7B
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000E76 RID: 3702 RVA: 0x00043B83 File Offset: 0x00041D83
		public List<CreepJoinerBaseDef> Excludes
		{
			get
			{
				return this.excludes;
			}
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000E77 RID: 3703 RVA: 0x00043B8B File Offset: 0x00041D8B
		public List<CreepJoinerBaseDef> Requires
		{
			get
			{
				return this.requires;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000E78 RID: 3704 RVA: 0x00043B93 File Offset: 0x00041D93
		public float MinCombatPoints
		{
			get
			{
				return this.minCombatPoints;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000E79 RID: 3705 RVA: 0x00043B9B File Offset: 0x00041D9B
		public bool CanOccurRandomly
		{
			get
			{
				return this.canOccurRandomly;
			}
		}

		// Token: 0x06000E7A RID: 3706 RVA: 0x00043BA4 File Offset: 0x00041DA4
		public string GetBodyGraphicPath(Pawn pawn)
		{
			for (int i = 0; i < this.bodyTypeGraphicPaths.Count; i++)
			{
				if (this.bodyTypeGraphicPaths[i].bodyType == pawn.story.bodyType)
				{
					return this.bodyTypeGraphicPaths[i].texturePath;
				}
			}
			return null;
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00043BF8 File Offset: 0x00041DF8
		public bool StyleItemAllowed(StyleItemDef styleItem)
		{
			if (!ModLister.AnomalyInstalled)
			{
				return true;
			}
			bool flag = styleItem is HairDef;
			bool flag2 = styleItem is BeardDef;
			if (!flag && !flag2)
			{
				return true;
			}
			if (flag)
			{
				if (this.hairTagFilter != null && !this.hairTagFilter.Allows(styleItem.styleTags))
				{
					return false;
				}
			}
			else if (flag2 && this.beardTagFilter != null && !this.beardTagFilter.Allows(styleItem.styleTags))
			{
				return false;
			}
			return true;
		}

		// Token: 0x040008E3 RID: 2275
		private float weight = 1f;

		// Token: 0x040008E4 RID: 2276
		private float minCombatPoints;

		// Token: 0x040008E5 RID: 2277
		private bool canOccurRandomly = true;

		// Token: 0x040008E6 RID: 2278
		[MustTranslate]
		public string letterLabel;

		// Token: 0x040008E7 RID: 2279
		[MustTranslate]
		public string letterPrompt;

		// Token: 0x040008E8 RID: 2280
		public List<BodyTypeGraphicData> bodyTypeGraphicPaths = new List<BodyTypeGraphicData>();

		// Token: 0x040008E9 RID: 2281
		public List<HeadTypeDef> forcedHeadTypes;

		// Token: 0x040008EA RID: 2282
		public TagFilter hairTagFilter;

		// Token: 0x040008EB RID: 2283
		public TagFilter beardTagFilter;

		// Token: 0x040008EC RID: 2284
		public Color? hairColorOverride;

		// Token: 0x040008ED RID: 2285
		private List<CreepJoinerBaseDef> excludes = new List<CreepJoinerBaseDef>();

		// Token: 0x040008EE RID: 2286
		private List<CreepJoinerBaseDef> requires = new List<CreepJoinerBaseDef>();
	}
}
