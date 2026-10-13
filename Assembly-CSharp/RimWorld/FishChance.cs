using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019FE RID: 6654
	public class FishChance
	{
		// Token: 0x0600961D RID: 38429 RVA: 0x002BF00E File Offset: 0x002BD20E
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "fishDef", xmlRoot, null, null, null);
			this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04006229 RID: 25129
		public ThingDef fishDef;

		// Token: 0x0400622A RID: 25130
		public float chance = 1f;
	}
}
