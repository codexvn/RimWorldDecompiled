using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A99 RID: 6809
	public class IdeoSymbolPack
	{
		// Token: 0x1700175B RID: 5979
		// (get) Token: 0x0600987F RID: 39039 RVA: 0x002C7E73 File Offset: 0x002C6073
		public string PrimarySymbol
		{
			get
			{
				if (!this.ideoName.NullOrEmpty())
				{
					return this.ideoName;
				}
				return this.theme;
			}
		}

		// Token: 0x040066B1 RID: 26289
		[MustTranslate]
		public string ideoName;

		// Token: 0x040066B2 RID: 26290
		[MustTranslate]
		public string theme;

		// Token: 0x040066B3 RID: 26291
		[MustTranslate]
		public string adjective;

		// Token: 0x040066B4 RID: 26292
		[MustTranslate]
		public string member;

		// Token: 0x040066B5 RID: 26293
		public bool prefix;
	}
}
