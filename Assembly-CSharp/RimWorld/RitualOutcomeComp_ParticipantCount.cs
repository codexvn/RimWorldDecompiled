using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002747 RID: 10055
	public class RitualOutcomeComp_ParticipantCount : RitualOutcomeComp_Quality
	{
		// Token: 0x0600DC9A RID: 56474 RVA: 0x0040EC26 File Offset: 0x0040CE26
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataThingPresence();
		}

		// Token: 0x0600DC9B RID: 56475 RVA: 0x0040EC30 File Offset: 0x0040CE30
		public override void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
			base.Tick(ritual, data, progressAmount);
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			foreach (Pawn pawn in ritual.PawnsToCountTowardsPresence)
			{
				if (ritual.Ritual != null)
				{
					RitualRole ritualRole = ritual.RoleFor(pawn, true);
					if (ritualRole != null && !ritualRole.countsAsParticipant)
					{
						continue;
					}
				}
				if (GatheringsUtility.InGatheringArea(pawn.Position, ritual.Spot, pawn.MapHeld))
				{
					if (!ritualOutcomeComp_DataThingPresence.presentForTicks.ContainsKey(pawn))
					{
						ritualOutcomeComp_DataThingPresence.presentForTicks.Add(pawn, 0f);
					}
					Dictionary<Thing, float> presentForTicks = ritualOutcomeComp_DataThingPresence.presentForTicks;
					Thing key = pawn;
					presentForTicks[key] += progressAmount;
				}
			}
		}

		// Token: 0x0600DC9C RID: 56476 RVA: 0x0040ECFC File Offset: 0x0040CEFC
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			RitualOutcomeComp_DataThingPresence ritualOutcomeComp_DataThingPresence = (RitualOutcomeComp_DataThingPresence)data;
			float num2 = (ritual.DurationTicks != 0) ? ((float)ritual.DurationTicks) : ritual.TicksPassedWithProgress;
			foreach (KeyValuePair<Thing, float> keyValuePair in ritualOutcomeComp_DataThingPresence.presentForTicks)
			{
				Pawn p = (Pawn)keyValuePair.Key;
				if (this.Counts(ritual.assignments, p) && keyValuePair.Value >= num2 / 2f)
				{
					num++;
				}
			}
			return (float)((this.curve != null) ? ((int)Math.Min((float)num, this.curve.Points[this.curve.PointsCount - 1].x)) : num);
		}

		// Token: 0x0600DC9D RID: 56477 RVA: 0x0040EDD4 File Offset: 0x0040CFD4
		protected bool Counts(RitualRoleAssignments assignments, Pawn p)
		{
			if (assignments != null && assignments.Ritual == null && assignments.Required(p))
			{
				return false;
			}
			RitualRole ritualRole = (assignments != null) ? assignments.RoleForPawn(p, true) : null;
			return (ritualRole == null || ritualRole.countsAsParticipant) && p.RaceProps.Humanlike;
		}

		// Token: 0x0600DC9E RID: 56478 RVA: 0x0040EE20 File Offset: 0x0040D020
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			float quality = this.curve.Evaluate((float)num);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = num.ToString() + " / " + Mathf.Max(base.MaxValue, (float)num).ToString(),
				qualityChange = this.ExpectedOffsetDesc(true, quality),
				quality = quality,
				positive = true,
				priority = 4f
			};
		}
	}
}
