using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4D RID: 6733
	public class IdeoIconDef : IdeoSymbolPartDef
	{
		// Token: 0x17001724 RID: 5924
		// (get) Token: 0x0600976E RID: 38766 RVA: 0x002C372E File Offset: 0x002C192E
		public Texture2D Icon
		{
			get
			{
				if (this.cachedIcon == null)
				{
					this.cachedIcon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.cachedIcon;
			}
		}

		// Token: 0x0400647B RID: 25723
		[NoTranslate]
		public string iconPath;

		// Token: 0x0400647C RID: 25724
		[Unsaved(false)]
		private Texture2D cachedIcon;
	}
}
