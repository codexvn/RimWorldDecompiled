using System;
using System.Collections;
using System.Xml;

namespace Verse
{
	// Token: 0x02000596 RID: 1430
	public class PatchOperationInsert : PatchOperationPathed
	{
		// Token: 0x06002553 RID: 9555 RVA: 0x000C0C24 File Offset: 0x000BEE24
		protected override bool ApplyWorker(XmlDocument xml)
		{
			XmlNode node = this.value.node;
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				result = true;
				XmlNode xmlNode = obj as XmlNode;
				XmlNode parentNode = xmlNode.ParentNode;
				if (this.order == PatchOperationInsert.Order.Append)
				{
					using (IEnumerator enumerator2 = node.ChildNodes.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							XmlNode node2 = (XmlNode)obj2;
							parentNode.InsertAfter(parentNode.OwnerDocument.ImportNode(node2, true), xmlNode);
						}
						goto IL_E0;
					}
					goto IL_98;
				}
				goto IL_98;
				IL_E0:
				if (xmlNode.NodeType == XmlNodeType.Text)
				{
					parentNode.Normalize();
					continue;
				}
				continue;
				IL_98:
				if (this.order == PatchOperationInsert.Order.Prepend)
				{
					for (int i = node.ChildNodes.Count - 1; i >= 0; i--)
					{
						parentNode.InsertBefore(parentNode.OwnerDocument.ImportNode(node.ChildNodes[i], true), xmlNode);
					}
					goto IL_E0;
				}
				goto IL_E0;
			}
			return result;
		}

		// Token: 0x04001EA6 RID: 7846
		private XmlContainer value;

		// Token: 0x04001EA7 RID: 7847
		private PatchOperationInsert.Order order = PatchOperationInsert.Order.Prepend;

		// Token: 0x02000597 RID: 1431
		private enum Order
		{
			// Token: 0x04001EA9 RID: 7849
			Append,
			// Token: 0x04001EAA RID: 7850
			Prepend
		}
	}
}
