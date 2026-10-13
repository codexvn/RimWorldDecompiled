using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02002829 RID: 10281
	public class MutatorChance
	{
		// Token: 0x0600E0C9 RID: 57545 RVA: 0x0041F7EC File Offset: 0x0041D9EC
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "mutator", xmlRoot.Name, null, null, null);
			XmlAttribute xmlAttribute = xmlRoot.Attributes["Required"];
			this.required = (xmlAttribute != null && xmlAttribute.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase));
			if (xmlRoot.HasChildNodes)
			{
				this.chance = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
				return;
			}
			this.chance = 1f;
		}

		// Token: 0x0400970B RID: 38667
		public TileMutatorDef mutator;

		// Token: 0x0400970C RID: 38668
		public float chance = 1f;

		// Token: 0x0400970D RID: 38669
		public bool required;
	}
}
