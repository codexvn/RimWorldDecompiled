using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000598 RID: 1432
	public class PatchOperationRemove : PatchOperationPathed
	{
		// Token: 0x06002555 RID: 9557 RVA: 0x000C0D70 File Offset: 0x000BEF70
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}
	}
}
