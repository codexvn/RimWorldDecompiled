using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200059E RID: 1438
	public class PatchOperationAttributeSet : PatchOperationAttribute
	{
		// Token: 0x06002561 RID: 9569 RVA: 0x000C1044 File Offset: 0x000BF244
		protected override bool ApplyWorker(XmlDocument xml)
		{
			bool result = false;
			foreach (object obj in xml.SelectNodes(this.xpath))
			{
				XmlNode xmlNode = obj as XmlNode;
				if (xmlNode.Attributes[this.attribute] != null)
				{
					xmlNode.Attributes[this.attribute].Value = this.value;
				}
				else
				{
					XmlAttribute xmlAttribute = xmlNode.OwnerDocument.CreateAttribute(this.attribute);
					xmlAttribute.Value = this.value;
					xmlNode.Attributes.Append(xmlAttribute);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x04001EAF RID: 7855
		protected string value;
	}
}
