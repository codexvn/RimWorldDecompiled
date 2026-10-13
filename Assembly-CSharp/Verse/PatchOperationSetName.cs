using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x0200059A RID: 1434
	public class PatchOperationSetName : PatchOperationPathed
	{
		// Token: 0x06002559 RID: 9561 RVA: 0x000C0E7C File Offset: 0x000BF07C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode xmlNode2 = xmlNode.OwnerDocument.CreateElement(this.name);
				xmlNode2.InnerXml = xmlNode.InnerXml;
				xmlNode.ParentNode.InsertBefore(xmlNode2, xmlNode);
				xmlNode.ParentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04001EAC RID: 7852
		protected string name;
	}
}
