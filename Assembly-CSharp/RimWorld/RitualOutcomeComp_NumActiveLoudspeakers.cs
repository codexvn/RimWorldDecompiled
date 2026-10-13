using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200274D RID: 10061
	public class RitualOutcomeComp_NumActiveLoudspeakers : RitualOutcomeComp_Quality
	{
		// Token: 0x0600DCB4 RID: 56500 RVA: 0x0040EC26 File Offset: 0x0040CE26
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x0600DCB5 RID: 56501 RVA: 0x0040F29E File Offset: 0x0040D49E
		private bool LoudspeakerActive(Thing s, TargetInfo target)
		{
			return s.GetRoom(RegionType.Set_All) == target.Cell.GetRoom(target.Map) && s.TryGetComp<CompPowerTrader>().PowerOn;
		}

		// Token: 0x0600DCB6 RID: 56502 RVA: 0x0040F2CC File Offset: 0x0040D4CC
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			TargetInfo selectedTarget = ritual.selectedTarget;
			if (selectedTarget.ThingDestroyed || !selectedTarget.HasThing)
			{
				return;
			}
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Thing thing in selectedTarget.Map.listerBuldingOfDefInProximity.GetForCell(selectedTarget.Cell, (float)this.maxDistance, ThingDefOf.Loudspeaker, null))
			{
				if (this.LoudspeakerActive(thing, selectedTarget))
				{
					if (!ritualOutcomeComp_DataThingPresence.presentForTicks.ContainsKey(thing))
					{
						ritualOutcomeComp_DataThingPresence.presentForTicks.Add(thing, 0f);
					}
					Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
					Thing key = thing;
					float num = presentForTicks[key];
					presentForTicks[key] = num + 1f;
				}
			}
		}

		// Token: 0x0600DCB7 RID: 56503 RVA: 0x0040F3AC File Offset: 0x0040D5AC
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (KeyValuePair<Thing, float> keyValuePair in ((RitualOutcomeComp_DataThingPresence)data).presentForTicks)
			{
				if (keyValuePair.Value >= (float)(ritual.DurationTicks / 2))
				{
					num++;
				}
			}
			return (float)((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x));
		}

		// Token: 0x0600DCB8 RID: 56504 RVA: 0x0040F444 File Offset: 0x0040D644
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			List<Thing> forCell = ritualTarget.Map.listerBuldingOfDefInProximity.GetForCell(ritualTarget.Cell, (float)this.maxDistance, ThingDefOf.Loudspeaker, null);
			int num = 0;
			foreach (Thing thing in forCell)
			{
				CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
				CompFlickable compFlickable = thing.TryGetComp<CompFlickable>();
				if (compPowerTrader != null && compPowerTrader.PowerNet != null && compPowerTrader.PowerNet.HasActivePowerSource && compFlickable != null && compFlickable.SwitchIsOn)
				{
					num++;
				}
			}
			float quality = this.curve.Evaluate((float)num);
			return new QualityFactor
			{
				label = "RitualPredictedOutcomeDescNumActiveLoudspeakers".Translate(),
				count = Mathf.Min((float)num, base.MaxValue).ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = (num > 0),
				priority = 1f
			};
		}

		// Token: 0x040094E2 RID: 38114
		public int maxDistance;
	}
}
