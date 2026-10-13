using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4B RID: 6731
	public class IdeoPresetDef : Def
	{
		// Token: 0x17001723 RID: 5923
		// (get) Token: 0x0600976A RID: 38762 RVA: 0x002C366B File Offset: 0x002C186B
		public Texture2D Icon
		{
			get
			{
				if (this.iconPath.NullOrEmpty())
				{
					return null;
				}
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x04006474 RID: 25716
		public IdeoPresetCategoryDef categoryDef;

		// Token: 0x04006475 RID: 25717
		public List<MemeDef> memes = new List<MemeDef>();

		// Token: 0x04006476 RID: 25718
		[NoTranslate]
		public string iconPath;

		// Token: 0x04006477 RID: 25719
		public bool classicPlus;

		// Token: 0x04006478 RID: 25720
		private Texture2D cachedIcon;
	}
}
