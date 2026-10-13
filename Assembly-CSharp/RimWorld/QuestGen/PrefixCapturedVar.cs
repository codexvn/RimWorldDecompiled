using System;
using System.Xml;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x02003A01 RID: 14849
	public class PrefixCapturedVar
	{
		// Token: 0x060145A9 RID: 83369 RVA: 0x00615BC0 File Offset: 0x00613DC0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured PrefixCapturedVar: " + xmlRoot.OuterXml);
				return;
			}
			this.name = xmlRoot.Name;
			this.value = new SlateRef<object>(DirectXmlToObject.InnerTextWithReplacedNewlinesOrXML(xmlRoot));
			TKeySystem.MarkTreatAsList(xmlRoot.ParentNode);
		}

		// Token: 0x0400E0BF RID: 57535
		[NoTranslate]
		[TranslationHandle]
		public string name;

		// Token: 0x0400E0C0 RID: 57536
		public SlateRef<object> value;
	}
}
