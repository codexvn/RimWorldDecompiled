using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001881 RID: 6273
	public class ThoughtWorker_SkillBookPassion : ThoughtWorker
	{
		// Token: 0x06008FF0 RID: 36848 RVA: 0x002A79A4 File Offset: 0x002A5BA4
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
			BookOutcomeDoerGainSkillExp doer = ((Book)p.CurJob.targetA.Thing).BookComp.GetDoer<BookOutcomeDoerGainSkillExp>();
			bool flag = false;
			if (doer != null)
			{
				foreach (KeyValuePair<SkillDef, float> keyValuePair in doer.Values)
				{
					SkillDef skillDef;
					float num;
					keyValuePair.Deconstruct(out skillDef, out num);
					SkillDef skillDef2 = skillDef;
					SkillRecord skill = p.skills.GetSkill(skillDef2);
					if (skill.passion == Passion.Major)
					{
						return ThoughtState.ActiveAtStage(1);
					}
					if (skill.passion == Passion.Minor)
					{
						flag = true;
					}
				}
			}
			if (flag)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}
	}
}
