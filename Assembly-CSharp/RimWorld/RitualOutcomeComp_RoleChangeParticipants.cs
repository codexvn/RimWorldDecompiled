using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200275B RID: 10075
	public class RitualOutcomeComp_RoleChangeParticipants : RitualOutcomeComp_ParticipantCount
	{
		// Token: 0x0600DCF8 RID: 56568 RVA: 0x004104D7 File Offset: 0x0040E6D7
		public override RitualOutcomeComp_Data MakeData()
		{
			return new RitualOutcomeComp_DataRoleChangeParticipants();
		}

		// Token: 0x0600DCF9 RID: 56569 RVA: 0x004104E0 File Offset: 0x0040E6E0
		public override void Notify_AssignmentsChanged(RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int num = 0;
			Pawn pawn = assignments.FirstAssignedPawn("role_changer");
			if (pawn == null)
			{
				return;
			}
			foreach (Pawn pawn2 in pawn.Map.mapPawns.FreeColonistsSpawned)
			{
				if (pawn2 != pawn && pawn2.Ideo == pawn.Ideo && !pawn2.Downed)
				{
					num++;
				}
			}
			((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount = num / 2;
		}

		// Token: 0x0600DCFA RID: 56570 RVA: 0x00410574 File Offset: 0x0040E774
		protected override string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return quality.ToStringPercent();
		}

		// Token: 0x0600DCFB RID: 56571 RVA: 0x0041057C File Offset: 0x0040E77C
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int desiredParticipantCount = ((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount;
			return (float)(((int)this.Count(ritual, data) >= desiredParticipantCount) ? 1 : 0);
		}

		// Token: 0x0600DCFC RID: 56572 RVA: 0x004105A8 File Offset: 0x0040E7A8
		public override float Count(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			int num = 0;
			foreach (KeyValuePair<Thing, float> keyValuePair in ((RitualOutcomeComp_DataThingPresence)data).presentForTicks)
			{
				Pawn p = (Pawn)keyValuePair.Key;
				if (base.Counts(ritual.assignments, p) && keyValuePair.Value >= (float)ritual.DurationTicks / 2f)
				{
					num++;
				}
			}
			return (float)num;
		}

		// Token: 0x0600DCFD RID: 56573 RVA: 0x00410634 File Offset: 0x0040E834
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			int desiredParticipantCount = ((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount;
			return string.Concat(new string[]
			{
				this.Count(ritual, data).ToString(),
				" / ",
				desiredParticipantCount.ToString(),
				" ",
				this.label,
				": +",
				this.QualityOffset(ritual, data).ToStringPercent()
			});
		}

		// Token: 0x0600DCFE RID: 56574 RVA: 0x004106A8 File Offset: 0x0040E8A8
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			int desiredParticipantCount = ((RitualOutcomeComp_DataRoleChangeParticipants)data).desiredParticipantCount;
			int num = assignments.Participants.Count((Pawn p) => this.Counts(assignments, p));
			bool flag = num >= desiredParticipantCount;
			int num2 = flag ? 1 : 0;
			return new QualityFactor
			{
				label = this.label.CapitalizeFirst(),
				count = num.ToString() + " / " + desiredParticipantCount.ToString(),
				qualityChange = this.ExpectedOffsetDesc(flag, (float)num2),
				quality = (float)num2,
				positive = flag,
				priority = 4f
			};
		}
	}
}
