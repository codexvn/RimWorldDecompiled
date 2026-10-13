using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A4C RID: 6732
	public class IdeoSymbolPartDef : Def
	{
		// Token: 0x0600976C RID: 38764 RVA: 0x002C36B8 File Offset: 0x002C18B8
		public bool CanBeChosenForIdeo(Ideo ideo)
		{
			if (this.memes == null && this.cultures == null)
			{
				return true;
			}
			if (this.cultures != null && this.cultures.Contains(ideo.culture))
			{
				return true;
			}
			if (this.memes != null)
			{
				for (int i = 0; i < ideo.memes.Count; i++)
				{
					if (this.memes.Contains(ideo.memes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04006479 RID: 25721
		public List<MemeDef> memes;

		// Token: 0x0400647A RID: 25722
		public List<CultureDef> cultures;
	}
}
