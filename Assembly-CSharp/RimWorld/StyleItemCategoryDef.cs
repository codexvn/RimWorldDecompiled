using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AF9 RID: 6905
	public class StyleItemCategoryDef : Def
	{
		// Token: 0x170017AC RID: 6060
		// (get) Token: 0x06009A40 RID: 39488 RVA: 0x002CDE27 File Offset: 0x002CC027
		public List<StyleItemDef> ItemsInCategory
		{
			get
			{
				if (this.cachedStyleItems == null)
				{
					this.cachedStyleItems = new List<StyleItemDef>();
					this.cachedStyleItems.AddRange(from x in StyleItemDef.AllStyleItemDefs
					where x.StyleItemCategory == this
					select x);
				}
				return this.cachedStyleItems;
			}
		}

		// Token: 0x04006A44 RID: 27204
		private List<StyleItemDef> cachedStyleItems;
	}
}
