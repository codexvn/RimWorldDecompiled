using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A0B RID: 6667
	public class PawnKindDefCount
	{
		// Token: 0x06009663 RID: 38499 RVA: 0x002C0393 File Offset: 0x002BE593
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "kindDef", "count");
		}

		// Token: 0x040062AA RID: 25258
		public PawnKindDef kindDef;

		// Token: 0x040062AB RID: 25259
		public int count;
	}
}
