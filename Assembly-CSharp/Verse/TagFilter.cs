using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x0200017F RID: 383
	public class TagFilter
	{
		// Token: 0x06000AF6 RID: 2806 RVA: 0x00034B88 File Offset: 0x00032D88
		public bool Allows(List<string> otherTags)
		{
			if (otherTags != null)
			{
				for (int i = 0; i < otherTags.Count; i++)
				{
					if (this.tags.Contains(otherTags[i]))
					{
						return this.whitelist;
					}
				}
			}
			return !this.whitelist;
		}

		// Token: 0x0400063E RID: 1598
		public List<string> tags = new List<string>();

		// Token: 0x0400063F RID: 1599
		public bool whitelist = true;
	}
}
