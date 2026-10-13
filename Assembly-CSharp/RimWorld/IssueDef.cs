using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A5C RID: 6748
	public class IssueDef : Def
	{
		// Token: 0x1700172F RID: 5935
		// (get) Token: 0x060097A5 RID: 38821 RVA: 0x002C4A73 File Offset: 0x002C2C73
		public Texture2D Icon
		{
			get
			{
				if (string.IsNullOrEmpty(this.iconPath))
				{
					return null;
				}
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x17001730 RID: 5936
		// (get) Token: 0x060097A6 RID: 38822 RVA: 0x002C4AAA File Offset: 0x002C2CAA
		public bool HasDefaultPrecept
		{
			get
			{
				return DefDatabase<PreceptDef>.AllDefs.Any((PreceptDef x) => x.issue == this && x.defaultSelectionWeight > 0f && x.visible);
			}
		}

		// Token: 0x040064FB RID: 25851
		public bool allowMultiplePrecepts;

		// Token: 0x040064FC RID: 25852
		public bool forceWriteLabelInPreceptFloatMenuOption;

		// Token: 0x040064FD RID: 25853
		public string iconPath;

		// Token: 0x040064FE RID: 25854
		private Texture2D icon;
	}
}
