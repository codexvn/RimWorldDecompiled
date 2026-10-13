using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C4 RID: 6596
	public class SkillNeed_Curve : SkillNeed
	{
		// Token: 0x0600951E RID: 38174 RVA: 0x002BB8A4 File Offset: 0x002B9AA4
		public override float ValueFor(Pawn pawn)
		{
			if (pawn.skills == null)
			{
				return this.curve.MinY;
			}
			return this.curve.Evaluate((float)pawn.skills.GetSkill(this.skill).Level);
		}

		// Token: 0x0400611F RID: 24863
		public SimpleCurve curve;
	}
}
