using System;
using System.Xml;

namespace Verse
{
	// Token: 0x020005A0 RID: 1440
	public class PatchOperationTest : PatchOperationPathed
	{
		// Token: 0x06002567 RID: 9575 RVA: 0x000C11E2 File Offset: 0x000BF3E2
		protected override bool ApplyWorker(XmlDocument xml)
		{
			return xml.SelectSingleNode(this.xpath) != null;
		}
	}
}
