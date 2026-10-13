using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI.Group
{
	// Token: 0x02001066 RID: 4198
	public class PsychicRitualToil_BloodRain : PsychicRitualToil
	{
		// Token: 0x060066A9 RID: 26281 RVA: 0x0020737D File Offset: 0x0020557D
		protected PsychicRitualToil_BloodRain()
		{
		}

		// Token: 0x060066AA RID: 26282 RVA: 0x00207385 File Offset: 0x00205585
		public PsychicRitualToil_BloodRain(PsychicRitualRoleDef invokerRole, FloatRange durationHoursFromQualityRange)
		{
			this.invokerRole = invokerRole;
			this.durationHoursFromQualityRange = durationHoursFromQualityRange;
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x0020739C File Offset: 0x0020559C
		public override void Start(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			base.Start(psychicRitual, parent);
			Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.invokerRole);
			float duration = this.durationHoursFromQualityRange.LerpThroughRange(psychicRitual.PowerPercent);
			psychicRitual.ReleaseAllPawnsAndBuildings();
			if (pawn != null)
			{
				this.ApplyOutcome(psychicRitual, pawn, duration);
			}
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x002073E8 File Offset: 0x002055E8
		private void ApplyOutcome(PsychicRitual psychicRitual, Pawn invoker, float duration)
		{
			if (!(invoker.Map.gameConditionManager.GetActiveCondition(GameConditionDefOf.BloodRain) is GameCondition_BloodRain))
			{
				GameCondition_BloodRain gameCondition_BloodRain = (GameCondition_BloodRain)GameConditionMaker.MakeCondition(GameConditionDefOf.BloodRain, Mathf.FloorToInt(duration * 2500f));
				gameCondition_BloodRain.psychicRitualDef = psychicRitual.def;
				invoker.Map.GameConditionManager.RegisterCondition(gameCondition_BloodRain);
			}
			Find.LetterStack.ReceiveLetter("PsychicRitualCompleteLabel".Translate(psychicRitual.def.label), "BloodRainCompleteText".Translate(invoker, Mathf.FloorToInt(duration * 2500f).ToStringTicksToPeriod(true, false, true, true, false), psychicRitual.def.Named("RITUAL")), LetterDefOf.ThreatBig, null, 0, true);
		}

		// Token: 0x060066AD RID: 26285 RVA: 0x002074B4 File Offset: 0x002056B4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PsychicRitualRoleDef>(ref this.invokerRole, "invokerRole");
			Scribe_Values.Look<FloatRange>(ref this.durationHoursFromQualityRange, "durationHoursFromQualityRange", default(FloatRange), false);
		}

		// Token: 0x04004945 RID: 18757
		private PsychicRitualRoleDef invokerRole;

		// Token: 0x04004946 RID: 18758
		private FloatRange durationHoursFromQualityRange;
	}
}
