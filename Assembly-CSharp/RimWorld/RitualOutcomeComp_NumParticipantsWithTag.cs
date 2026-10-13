using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200274F RID: 10063
	public class RitualOutcomeComp_NumParticipantsWithTag : RitualOutcomeComp_Quality
	{
		// Token: 0x170021CA RID: 8650
		// (get) Token: 0x0600DCBF RID: 56511 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DCC0 RID: 56512 RVA: 0x0040F7D4 File Offset: 0x0040D9D4
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (Pawn p in ritual.assignments.Participants)
			{
				if (ritual.PawnTagSet(p, this.tag))
				{
					num++;
				}
			}
			return (float)num;
		}

		// Token: 0x0600DCC1 RID: 56513 RVA: 0x0040F83C File Offset: 0x0040DA3C
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = assignments.Participants.Count((Pawn p) => p.RaceProps.Humanlike);
			float quality = this.curve.Evaluate((float)num);
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = Mathf.Min((float)num, base.MaxValue).ToString() + " / " + base.MaxValue.ToString(),
				qualityChange = this.ExpectedOffsetDesc(num > 0, quality),
				quality = quality,
				positive = (num > 0),
				priority = 4f
			};
		}

		// Token: 0x040094E4 RID: 38116
		public string tag;
	}
}
