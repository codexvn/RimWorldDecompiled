using System;
using System.Collections.Generic;
using System.Text;

namespace Verse
{
	// Token: 0x020004FD RID: 1277
	public class RegionLinkDatabase
	{
		// Token: 0x06002129 RID: 8489 RVA: 0x000AD6B4 File Offset: 0x000AB8B4
		public RegionLink LinkFrom(EdgeSpan span)
		{
			ulong key = span.UniqueHashCode();
			RegionLink regionLink;
			if (!this.links.TryGetValue(key, out regionLink))
			{
				regionLink = new RegionLink();
				regionLink.span = span;
				this.links.Add(key, regionLink);
			}
			return regionLink;
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x000AD6F4 File Offset: 0x000AB8F4
		public void Notify_LinkHasNoRegions(RegionLink link)
		{
			this.links.Remove(link.UniqueHashCode());
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x000AD708 File Offset: 0x000AB908
		public void DebugLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (KeyValuePair<ulong, RegionLink> keyValuePair in this.links)
			{
				stringBuilder.AppendLine(keyValuePair.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x04001C27 RID: 7207
		private Dictionary<ulong, RegionLink> links = new Dictionary<ulong, RegionLink>();
	}
}
