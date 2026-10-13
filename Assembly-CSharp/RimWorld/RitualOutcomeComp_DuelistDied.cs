using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002736 RID: 10038
	public class RitualOutcomeComp_DuelistDied : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021BF RID: 8639
		// (get) Token: 0x0600DC51 RID: 56401 RVA: 0x0040D938 File Offset: 0x0040BB38
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170021C0 RID: 8640
		// (get) Token: 0x0600DC52 RID: 56402 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC53 RID: 56403 RVA: 0x0040DED0 File Offset: 0x0040C0D0
		public override bool Applies(LordJob_Ritual ritual)
		{
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ritual as LordJob_Ritual_Duel;
			if (lordJob_Ritual_Duel != null)
			{
				return lordJob_Ritual_Duel.duelists.Any((Pawn d) => d.Dead);
			}
			return false;
		}

		// Token: 0x0600DC54 RID: 56404 RVA: 0x0040DF14 File Offset: 0x0040C114
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			return new QualityFactor
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				present = false,
				uncertainOutcome = true,
				qualityChange = this.ExpectedOffsetDesc(true, -1f),
				quality = this.qualityOffset,
				positive = true
			};
		}
	}
}
