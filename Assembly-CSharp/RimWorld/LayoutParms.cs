using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02002129 RID: 8489
	public class LayoutParms
	{
		// Token: 0x0600B91D RID: 47389 RVA: 0x002C52BF File Offset: 0x002C34BF
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
		}

		// Token: 0x04007FB9 RID: 32697
		public LayoutDef def;

		// Token: 0x04007FBA RID: 32698
		public float weight = 1f;
	}
}
