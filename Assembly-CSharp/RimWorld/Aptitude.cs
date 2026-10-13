using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x02002515 RID: 9493
	public class Aptitude
	{
		// Token: 0x0600CF61 RID: 53089 RVA: 0x000024DF File Offset: 0x000006DF
		public Aptitude()
		{
		}

		// Token: 0x0600CF62 RID: 53090 RVA: 0x003CDB05 File Offset: 0x003CBD05
		public Aptitude(SkillDef skill, int level)
		{
			this.skill = skill;
			this.level = level;
		}

		// Token: 0x0600CF63 RID: 53091 RVA: 0x003CDB1B File Offset: 0x003CBD1B
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "skill", xmlRoot.Name, null, null, typeof(SkillDef));
			this.level = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04008D0A RID: 36106
		public SkillDef skill;

		// Token: 0x04008D0B RID: 36107
		public int level;
	}
}
