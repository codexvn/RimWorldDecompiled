using System;
using System.Collections.Generic;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x02000222 RID: 546
	public class CreepJoinerBenefitDef : CreepJoinerBaseDef
	{
		// Token: 0x040008EF RID: 2287
		[MustTranslate]
		public string letterExtra;

		// Token: 0x040008F0 RID: 2288
		public List<BackstoryTrait> traits = new List<BackstoryTrait>();

		// Token: 0x040008F1 RID: 2289
		public List<CreepJoinerBenefitDef.SkillValue> skills = new List<CreepJoinerBenefitDef.SkillValue>();

		// Token: 0x040008F2 RID: 2290
		public List<HediffDef> hediffs = new List<HediffDef>();

		// Token: 0x040008F3 RID: 2291
		public List<AbilityDef> abilities = new List<AbilityDef>();

		// Token: 0x02000223 RID: 547
		public class SkillValue
		{
			// Token: 0x06000E7E RID: 3710 RVA: 0x00043CDA File Offset: 0x00041EDA
			public void LoadDataFromXmlCustom(XmlNode xmlRoot)
			{
				DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, null);
				this.range = (xmlRoot.HasChildNodes ? ParseHelper.FromString<IntRange>(xmlRoot.FirstChild.Value) : IntRange.Zero);
			}

			// Token: 0x040008F4 RID: 2292
			public SkillDef skill;

			// Token: 0x040008F5 RID: 2293
			public IntRange range;
		}
	}
}
