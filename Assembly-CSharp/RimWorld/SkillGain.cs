using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020019EC RID: 6636
	public class SkillGain
	{
		// Token: 0x060095D4 RID: 38356 RVA: 0x002BDEF9 File Offset: 0x002BC0F9
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			if (xmlRoot.HasChildNodes)
			{
				this.amount = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
			}
		}

		// Token: 0x040061DD RID: 25053
		public SkillDef skill;

		// Token: 0x040061DE RID: 25054
		public int amount;
	}
}
