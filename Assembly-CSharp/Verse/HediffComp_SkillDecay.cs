using System;
using RimWorld;

namespace Verse
{
	// Token: 0x0200070C RID: 1804
	public class HediffComp_SkillDecay : HediffComp
	{
		// Token: 0x170007C9 RID: 1993
		// (get) Token: 0x06002C11 RID: 11281 RVA: 0x000E50E3 File Offset: 0x000E32E3
		public HediffCompProperties_SkillDecay Props
		{
			get
			{
				return (HediffCompProperties_SkillDecay)this.props;
			}
		}

		// Token: 0x06002C12 RID: 11282 RVA: 0x000E50F0 File Offset: 0x000E32F0
		public override void CompPostTickInterval(ref float severityAdjustment, int delta)
		{
			Pawn_SkillTracker skills = base.Pawn.skills;
			if (skills == null)
			{
				return;
			}
			for (int i = 0; i < skills.skills.Count; i++)
			{
				SkillRecord skillRecord = skills.skills[i];
				float num = this.parent.Severity * this.Props.decayPerDayPercentageLevelCurve.Evaluate((float)skillRecord.GetLevel(false));
				float num2 = skillRecord.XpRequiredForLevelUp * num / 60000f * (float)delta;
				skillRecord.Learn(-num2, false, false);
			}
		}
	}
}
