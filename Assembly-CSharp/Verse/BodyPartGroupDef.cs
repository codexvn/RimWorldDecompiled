using System;

namespace Verse
{
	// Token: 0x02000213 RID: 531
	public class BodyPartGroupDef : Def
	{
		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000E29 RID: 3625 RVA: 0x00042DE7 File Offset: 0x00040FE7
		public string LabelShort
		{
			get
			{
				if (!this.labelShort.NullOrEmpty())
				{
					return this.labelShort;
				}
				return this.label;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00042E03 File Offset: 0x00041003
		public string LabelShortCap
		{
			get
			{
				if (this.labelShort.NullOrEmpty())
				{
					return this.LabelCap;
				}
				if (this.cachedLabelShortCap == null)
				{
					this.cachedLabelShortCap = this.labelShort.CapitalizeFirst();
				}
				return this.cachedLabelShortCap;
			}
		}

		// Token: 0x0400087E RID: 2174
		[MustTranslate]
		public string labelShort;

		// Token: 0x0400087F RID: 2175
		public int listOrder;

		// Token: 0x04000880 RID: 2176
		[Unsaved(false)]
		private string cachedLabelShortCap;
	}
}
