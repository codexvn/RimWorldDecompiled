using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020005A1 RID: 1441
	public class PatchOperationConditional : PatchOperationPathed
	{
		// Token: 0x06002569 RID: 9577 RVA: 0x000C11F4 File Offset: 0x000BF3F4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			if (xml.SelectSingleNode(this.xpath) != null)
			{
				if (this.match != null)
				{
					return this.match.Apply(xml);
				}
			}
			else if (this.nomatch != null)
			{
				return this.nomatch.Apply(xml);
			}
			return this.match != null || this.nomatch != null;
		}

		// Token: 0x04001EB2 RID: 7858
		private PatchOperation match;

		// Token: 0x04001EB3 RID: 7859
		private PatchOperation nomatch;
	}
}
