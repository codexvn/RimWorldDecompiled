using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C3 RID: 6595
	public class SkillNeed_Direct : SkillNeed
	{
		// Token: 0x0600951C RID: 38172 RVA: 0x002BB818 File Offset: 0x002B9A18
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return 1f;
			}
			int level = pawn.skills.GetSkill(this.skill).Level;
			if (this.valuesPerLevel.Count > level)
			{
				return this.valuesPerLevel[level];
			}
			if (this.valuesPerLevel.Count > 0)
			{
				return this.valuesPerLevel[this.valuesPerLevel.Count - 1];
			}
			return 1f;
		}

		// Token: 0x0400611E RID: 24862
		public List<float> valuesPerLevel = new List<float>();
	}
}
