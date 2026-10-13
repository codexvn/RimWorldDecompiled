using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000593 RID: 1427
	public class PatchOperationAdd : PatchOperationPathed
	{
		// Token: 0x0600254F RID: 9551 RVA: 0x000C09FC File Offset: 0x000BEBFC
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				if (this.order == PatchOperationAdd.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							xmlNode.AppendChild(xmlNode.OwnerDocument.ImportNode(node2, true));
						}
						continue;
					}
				}
				if (this.order == PatchOperationAdd.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						xmlNode.PrependChild(xmlNode.OwnerDocument.ImportNode(node.ChildNodes[i], true));
					}
				}
			}
			return result;
		}

		// Token: 0x04001EA0 RID: 7840
		private XmlContainer value;

		// Token: 0x04001EA1 RID: 7841
		private PatchOperationAdd.Order order;

		// Token: 0x02000594 RID: 1428
		private enum Order
		{
			// Token: 0x04001EA3 RID: 7843
			Append,
			// Token: 0x04001EA4 RID: 7844
			Prepend
		}
	}
}
