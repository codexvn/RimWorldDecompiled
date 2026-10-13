using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A60 RID: 6752
	public class KnowledgeCategoryDef : Def
	{
		// Token: 0x060097B8 RID: 38840 RVA: 0x002C4E6F File Offset: 0x002C306F
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.overflowCategory == this)
			{
				yield return "overflowCategory is this category.";
			}
			yield break;
		}

		// Token: 0x04006514 RID: 25876
		public KnowledgeCategoryDef overflowCategory;

		// Token: 0x04006515 RID: 25877
		public Color color;

		// Token: 0x04006516 RID: 25878
		[NoTranslate]
		public string texPath;
	}
}
