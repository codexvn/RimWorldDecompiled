using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200059C RID: 1436
	public class PatchOperationAttributeAdd : PatchOperationAttribute
	{
		// Token: 0x0600255D RID: 9565 RVA: 0x000C0F0C File Offset: 0x000BF10C
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] == null)
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x04001EAE RID: 7854
		protected string value;
	}
}
