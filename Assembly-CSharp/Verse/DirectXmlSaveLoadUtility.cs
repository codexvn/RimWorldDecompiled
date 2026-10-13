using System;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x0200082B RID: 2091
	public static class DirectXmlSaveLoadUtility
	{
		// Token: 0x06003515 RID: 13589 RVA: 0x00116F18 File Offset: 0x00115118
		public static string GetXPath(this XmlNode node)
		{
			string text = "";
			while (node != null)
			{
				XmlNodeType nodeType = node.NodeType;
				if (nodeType != XmlNodeType.Element)
				{
					if (nodeType != XmlNodeType.Attribute)
					{
						if (nodeType == XmlNodeType.Document)
						{
							return text;
						}
						text = "/?" + text;
						node = node.ParentNode;
					}
					else
					{
						text = "/@" + node.Name + text;
						node = ((XmlAttribute)node).OwnerElement;
					}
				}
				else
				{
					bool flag;
					int elementIndexForXPath = DirectXmlSaveLoadUtility.GetElementIndexForXPath((XmlElement)node, out flag);
					if (flag || node.Name == "li")
					{
						text = string.Concat(new string[]
						{
							"/",
							node.Name,
							"[",
							elementIndexForXPath.ToString(),
							"]",
							text
						});
					}
					else
					{
						text = "/" + node.Name + text;
					}
					node = node.ParentNode;
				}
			}
			return text;
		}

		// Token: 0x06003516 RID: 13590 RVA: 0x0011700C File Offset: 0x0011520C
		public static string GetInnerXml(this XElement element)
		{
			if (element == null)
			{
				return "";
			}
			string result;
			using (XmlReader xmlReader = element.CreateReader())
			{
				xmlReader.MoveToContent();
				result = xmlReader.ReadInnerXml();
			}
			return result;
		}

		// Token: 0x06003517 RID: 13591 RVA: 0x00117054 File Offset: 0x00115254
		private static int GetElementIndexForXPath(XmlElement element, out bool multiple)
		{
			multiple = false;
			XmlNode parentNode = element.ParentNode;
			if (parentNode is XmlDocument)
			{
				return 1;
			}
			if (!(parentNode is XmlElement))
			{
				return 1;
			}
			foreach (object obj in parentNode.ChildNodes)
			{
				XmlNode xmlNode = (XmlNode)obj;
				if (xmlNode is XmlElement && xmlNode.Name == element.Name && xmlNode != element)
				{
					multiple = true;
					break;
				}
			}
			int num = 1;
			foreach (object obj2 in parentNode.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj2;
				if (xmlNode2 is XmlElement && xmlNode2.Name == element.Name)
				{
					if (xmlNode2 == element)
					{
						return num;
					}
					num++;
				}
			}
			return 1;
		}

		// Token: 0x040027FB RID: 10235
		public const BindingFlags FieldGetFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
	}
}
