using System;
using System.Linq;
using System.Xml;

namespace Verse
{
	// Token: 0x02000599 RID: 1433
	public class PatchOperationReplace : PatchOperationPathed
	{
		// Token: 0x06002557 RID: 9559 RVA: 0x000C0DB8 File Offset: 0x000BEFB8
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (XmlNode xmlNode in xml.SelectNodes(this.xpath).Cast<XmlNode>().ToArray<XmlNode>())
			{
				result = true;
				XmlNode parentNode = xmlNode.ParentNode;
				foreach (object obj in node.ChildNodes)
				{
					XmlNode node2 = (XmlNode)obj;
					parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
				}
				parentNode.RemoveChild(xmlNode);
			}
			return result;
		}

		// Token: 0x04001EAB RID: 7851
		private XmlContainer value;
	}
}
