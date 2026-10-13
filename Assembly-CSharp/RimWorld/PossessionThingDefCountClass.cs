using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019ED RID: 6637
	public class PossessionThingDefCountClass
	{
		// Token: 0x060095D6 RID: 38358 RVA: 0x002BDF2D File Offset: 0x002BC12D
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "key", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.value = ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value);
				return;
			}
			this.value = IntRange.One;
		}

		// Token: 0x040061DF RID: 25055
		public ThingDef key;

		// Token: 0x040061E0 RID: 25056
		public IntRange value = IntRange.One;
	}
}
