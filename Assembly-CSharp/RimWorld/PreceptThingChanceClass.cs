using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AB9 RID: 6841
	public class PreceptThingChanceClass
	{
		// Token: 0x06009931 RID: 39217 RVA: 0x002CA1A0 File Offset: 0x002C83A0
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot, null, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0400682E RID: 26670
		public ThingDef def;

		// Token: 0x0400682F RID: 26671
		public float chance;
	}
}
