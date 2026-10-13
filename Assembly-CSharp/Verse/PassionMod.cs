using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200017D RID: 381
	public class PassionMod
	{
		// Token: 0x06000AF3 RID: 2803 RVA: 0x000024DF File Offset: 0x000006DF
		public PassionMod()
		{
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00034B33 File Offset: 0x00032D33
		public PassionMod(SkillDef skill, PassionMod.PassionModType modType)
		{
			this.skill = skill;
			this.modType = modType;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00034B4C File Offset: 0x00032D4C
		public Passion NewPassionFor(SkillRecord skillRecord)
		{
			PassionMod.PassionModType passionModType = this.modType;
			if (passionModType != PassionMod.PassionModType.AddOneLevel)
			{
				if (passionModType == PassionMod.PassionModType.DropAll)
				{
					return Passion.None;
				}
			}
			else
			{
				Passion passion = skillRecord.passion;
				if (passion == Passion.None)
				{
					return Passion.Minor;
				}
				if (passion == Passion.Minor)
				{
					return Passion.Major;
				}
			}
			return skillRecord.passion;
		}

		// Token: 0x04000638 RID: 1592
		public SkillDef skill;

		// Token: 0x04000639 RID: 1593
		public PassionMod.PassionModType modType;

		// Token: 0x0200017E RID: 382
		public enum PassionModType
		{
			// Token: 0x0400063B RID: 1595
			None,
			// Token: 0x0400063C RID: 1596
			AddOneLevel,
			// Token: 0x0400063D RID: 1597
			DropAll
		}
	}
}
