using System;
using System.Collections.Generic;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02001932 RID: 6450
	public class BookOutcomeProperties_GainSkillExp : BookOutcomeProperties
	{
		// Token: 0x17001637 RID: 5687
		// (get) Token: 0x060092AE RID: 37550 RVA: 0x002B31B7 File Offset: 0x002B13B7
		public override Type DoerClass
		{
			get
			{
				return typeof(BookOutcomeDoerGainSkillExp);
			}
		}

		// Token: 0x04005D98 RID: 23960
		public List<BookOutcomeProperties_GainSkillExp.BookStatReward> skills = new List<BookOutcomeProperties_GainSkillExp.BookStatReward>();

		// Token: 0x02001933 RID: 6451
		public class BookStatReward
		{
			// Token: 0x060092B0 RID: 37552 RVA: 0x002B31D6 File Offset: 0x002B13D6
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
			}

			// Token: 0x04005D99 RID: 23961
			public SkillDef skill;
		}
	}
}
