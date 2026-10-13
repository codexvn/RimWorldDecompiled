using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x02000827 RID: 2087
	public static class DirectXmlLoaderSimple
	{
		// Token: 0x06003506 RID: 13574 RVA: 0x00116B6B File Offset: 0x00114D6B
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(string fileContents)
		{
			return DirectXmlLoaderSimple.ValuesFromXmlFile(VirtualFileInfoExt.LoadAsXDocument(fileContents));
		}

		// Token: 0x06003507 RID: 13575 RVA: 0x00116B78 File Offset: 0x00114D78
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(VirtualFile file)
		{
			return DirectXmlLoaderSimple.ValuesFromXmlFile(file.LoadAsXDocument());
		}

		// Token: 0x06003508 RID: 13576 RVA: 0x00116B85 File Offset: 0x00114D85
		public static IEnumerable<DirectXmlLoaderSimple.XmlKeyValuePair> ValuesFromXmlFile(XDocument doc)
		{
			foreach (XElement xelement in doc.Root.Elements())
			{
				string key = xelement.Name.ToString();
				string text = xelement.Value;
				text = text.Replace("\\n", "\n");
				yield return new DirectXmlLoaderSimple.XmlKeyValuePair
				{
					key = key,
					value = text,
					lineNumber = ((IXmlLineInfo)xelement).LineNumber
				};
			}
			IEnumerator<XElement> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x02000828 RID: 2088
		public struct XmlKeyValuePair
		{
			// Token: 0x040027F2 RID: 10226
			public string key;

			// Token: 0x040027F3 RID: 10227
			public string value;

			// Token: 0x040027F4 RID: 10228
			public int lineNumber;
		}
	}
}
