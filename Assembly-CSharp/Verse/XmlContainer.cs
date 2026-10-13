using System;
using System.Xml;

namespace Verse
{
	// Token: 0x0200058F RID: 1423
	public class XmlContainer
	{
		// Token: 0x06002546 RID: 9542 RVA: 0x000C08F2 File Offset: 0x000BEAF2
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			this.node = xmlRoot;
		}

		// Token: 0x04001E96 RID: 7830
		public XmlNode node;
	}
}
