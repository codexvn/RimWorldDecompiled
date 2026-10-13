using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001DC1 RID: 7617
	public class IncidentWorker_PsychicDrone : IncidentWorker_PsychicEmanation
	{
		// Token: 0x0600AA0F RID: 43535 RVA: 0x0030A630 File Offset: 0x00308830
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (base.TryExecuteWorker(parms))
			{
				SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera((Map)parms.target);
				return true;
			}
			return false;
		}

		// Token: 0x0600AA10 RID: 43536 RVA: 0x0030A654 File Offset: 0x00308854
		protected override void DoConditionAndLetter(IncidentParms parms, Map map, int duration, Gender gender, float points)
		{
			if (points < 0f)
			{
				points = StorytellerUtility.DefaultThreatPointsNow(map);
			}
			PsychicDroneLevel level;
			if (points < 800f)
			{
				level = PsychicDroneLevel.BadLow;
			}
			else if (points < 2000f)
			{
				level = PsychicDroneLevel.BadMedium;
			}
			else
			{
				level = PsychicDroneLevel.BadHigh;
			}
			GameCondition_PsychicEmanation gameCondition_PsychicEmanation = (GameCondition_PsychicEmanation)GameConditionMaker.MakeCondition(GameConditionDefOf.PsychicDrone, duration);
			gameCondition_PsychicEmanation.gender = gender;
			gameCondition_PsychicEmanation.level = level;
			map.gameConditionManager.RegisterCondition(gameCondition_PsychicEmanation);
			base.SendStandardLetter(gameCondition_PsychicEmanation.LabelCap, gameCondition_PsychicEmanation.LetterText, gameCondition_PsychicEmanation.def.letterDef, parms, LookTargets.Invalid, Array.Empty<NamedArgument>());
		}

		// Token: 0x0400756B RID: 30059
		private const float MaxPointsDroneLow = 800f;

		// Token: 0x0400756C RID: 30060
		private const float MaxPointsDroneMedium = 2000f;
	}
}
