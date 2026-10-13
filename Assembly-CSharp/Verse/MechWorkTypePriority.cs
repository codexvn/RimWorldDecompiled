using System;
using System.Xml;

namespace Verse
{
	// Token: 0x02000252 RID: 594
	public class MechWorkTypePriority
	{
		// Token: 0x06000F13 RID: 3859 RVA: 0x000467C8 File Offset: 0x000449C8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.Name, null, null, null);
			this.priority = (xmlRoot.HasChildNodes ? ParseHelper.FromString<int>(xmlRoot.FirstChild.Value) : 3);
		}

		// Token: 0x04000A87 RID: 2695
		public WorkTypeDef def;

		// Token: 0x04000A88 RID: 2696
		public int priority;
	}
}
