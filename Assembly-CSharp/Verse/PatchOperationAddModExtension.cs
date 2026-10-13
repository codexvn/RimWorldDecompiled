using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000595 RID: 1429
	public class PatchOperationAddModExtension : PatchOperationPathed
	{
		// Token: 0x06002551 RID: 9553 RVA: 0x000C0B24 File Offset: 0x000BED24
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				XmlNode xmlNode2 = xmlNode["modExtensions"];
				if (xmlNode2 == null)
				{
					xmlNode2 = xmlNode.OwnerDocument.CreateElement("modExtensions");
					xmlNode.AppendChild(xmlNode2);
				}
				foreach (object obj2 in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj2;
					xmlNode2.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001EA5 RID: 7845
		private XmlContainer value;
	}
}
