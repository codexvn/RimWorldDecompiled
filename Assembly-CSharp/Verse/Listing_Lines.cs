using System;

namespace Verse
{
	// Token: 0x02000BBB RID: 3003
	public abstract class Listing_Lines : Listing
	{
		// Token: 0x06004CB7 RID: 19639 RVA: 0x00185E48 File Offset: 0x00184048
		protected void EndLine()
		{
			this.curY += this.lineHeight + this.verticalSpacing;
		}

		// Token: 0x040036A3 RID: 13987
		public float lineHeight = 20f;
	}
}
