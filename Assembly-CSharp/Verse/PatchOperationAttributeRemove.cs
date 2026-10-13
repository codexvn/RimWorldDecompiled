using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200059D RID: 1437
	public class PatchOperationAttributeRemove : PatchOperationAttribute
	{
		// Token: 0x0600255F RID: 9567 RVA: 0x000C0FB4 File Offset: 0x000BF1B4
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes.Remove(xmlNode.Attributes[this.attribute]);
					result = true;
				}
			}
			return result;
		}
	}
}
