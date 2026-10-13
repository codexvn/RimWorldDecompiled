using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EB RID: 6635
	public class BackstoryTrait
	{
		// Token: 0x060095D2 RID: 38354 RVA: 0x002BDEBD File Offset: 0x002BC0BD
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "def", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.degree = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
				return;
			}
			this.degree = 0;
		}

		// Token: 0x040061DB RID: 25051
		public TraitDef def;

		// Token: 0x040061DC RID: 25052
		public int degree;
	}
}
