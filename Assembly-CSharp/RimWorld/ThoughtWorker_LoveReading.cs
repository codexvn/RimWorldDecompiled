using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001880 RID: 6272
	public class ThoughtWorker_LoveReading : ThoughtWorker
	{
		// Token: 0x06008FEE RID: 36846 RVA: 0x002A792C File Offset: 0x002A5B2C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (p.CurJobDef != JobDefOf.Reading)
			{
				return ThoughtState.Inactive;
			}
			JobDriver_Reading jobDriver_Reading = p.jobs.curDriver as JobDriver_Reading;
			if (jobDriver_Reading == null || !jobDriver_Reading.IsReading)
			{
				return ThoughtState.Inactive;
			}
			SkillRecord skill = p.skills.GetSkill(SkillDefOf.Intellectual);
			if (skill.passion == Passion.Minor)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			if (skill.passion == Passion.Major)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			return ThoughtState.Inactive;
		}
	}
}
