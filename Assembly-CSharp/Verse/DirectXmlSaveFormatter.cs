using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Verse
{
	// Token: 0x0200082A RID: 2090
	public static class DirectXmlSaveFormatter
	{
		// Token: 0x06003512 RID: 13586 RVA: 0x00116D68 File Offset: 0x00114F68
		public static void AddWhitespaceFromRoot(XElement root)
		{
			if (!root.Elements().Any<XElement>())
			{
				return;
			}
			foreach (XNode xnode in root.Elements().ToList<XElement>())
			{
				XText content = new XText("\n");
				xnode.AddAfterSelf(content);
			}
			root.Elements().First<XElement>().AddBeforeSelf(new XText("\n"));
			root.Elements().Last<XElement>().AddAfterSelf(new XText("\n"));
			foreach (XElement element in root.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element, 1);
			}
		}

		// Token: 0x06003513 RID: 13587 RVA: 0x00116E50 File Offset: 0x00115050
		private static void IndentXml(XElement element, int depth)
		{
			element.AddBeforeSelf(new XText(DirectXmlSaveFormatter.IndentString(depth, true)));
			bool startWithNewline = element.NextNode == null;
			element.AddAfterSelf(new XText(DirectXmlSaveFormatter.IndentString(depth - 1, startWithNewline)));
			foreach (XElement element2 in element.Elements().ToList<XElement>())
			{
				DirectXmlSaveFormatter.IndentXml(element2, depth + 1);
			}
		}

		// Token: 0x06003514 RID: 13588 RVA: 0x00116ED8 File Offset: 0x001150D8
		private static string IndentString(int depth, bool startWithNewline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (startWithNewline)
			{
				stringBuilder.Append("\n");
			}
			for (int i = 0; i < depth; i++)
			{
				stringBuilder.Append("  ");
			}
			return stringBuilder.ToString();
		}
	}
}
