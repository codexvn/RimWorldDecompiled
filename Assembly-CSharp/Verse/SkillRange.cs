using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C3 RID: 707
	public class SkillRange
	{
		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001147 RID: 4423 RVA: 0x00051063 File Offset: 0x0004F263
		public SkillDef Skill
		{
			get
			{
				return this.skill;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001148 RID: 4424 RVA: 0x0005106B File Offset: 0x0004F26B
		public IntRange Range
		{
			get
			{
				return this.range;
			}
		}

		// Token: 0x04000F65 RID: 3941
		private SkillDef skill;

		// Token: 0x04000F66 RID: 3942
		private IntRange range = IntRange.One;
	}
}
