using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003754 RID: 14164
	public class StatWorker_PsyfocusCost : StatWorker
	{
		// Token: 0x06013A2B RID: 80427 RVA: 0x005D524C File Offset: 0x005D344C
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			if (optionalReq.ForAbility)
			{
				AbilityDef abilityDef = optionalReq.AbilityDef;
				if (abilityDef.AnyCompOverridesPsyfocusCost)
				{
					if (abilityDef.PsyfocusCostRange.Span > 1E-45f)
					{
						return (abilityDef.PsyfocusCostRange.min * 100f).ToString("0.##") + "-" + stat.ValueToString(abilityDef.PsyfocusCostRange.max, numberSense, finalized);
					}
					return stat.ValueToString(abilityDef.PsyfocusCostRange.max, numberSense, finalized);
				}
			}
			return base.GetStatDrawEntryLabel(stat, value, numberSense, optionalReq, finalized);
		}

		// Token: 0x06013A2C RID: 80428 RVA: 0x005D52E8 File Offset: 0x005D34E8
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			if (req.ForAbility)
			{
				foreach (AbilityCompProperties abilityCompProperties in req.AbilityDef.comps)
				{
					if (abilityCompProperties.OverridesPsyfocusCost)
					{
						return abilityCompProperties.PsyfocusCostExplanation;
					}
				}
			}
			return base.GetExplanationUnfinalized(req, numberSense);
		}
	}
}
