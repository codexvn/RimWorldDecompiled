using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02002128 RID: 8488
	public class PrefabParms
	{
		// Token: 0x0600B91B RID: 47387 RVA: 0x002C52BF File Offset: 0x002C34BF
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			XmlHelper.ParseElements(this, xmlRoot, "def", "weight");
		}

		// Token: 0x04007FB6 RID: 32694
		public PrefabDef def;

		// Token: 0x04007FB7 RID: 32695
		public float weight = 1f;

		// Token: 0x04007FB8 RID: 32696
		public IntRange minMaxRange = IntRange.Invalid;
	}
}
