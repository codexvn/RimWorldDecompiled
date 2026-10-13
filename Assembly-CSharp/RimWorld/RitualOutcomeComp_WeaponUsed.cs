using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002738 RID: 10040
	public class RitualOutcomeComp_WeaponUsed : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021C1 RID: 8641
		// (get) Token: 0x0600DC59 RID: 56409 RVA: 0x0040D938 File Offset: 0x0040BB38
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x170021C2 RID: 8642
		// (get) Token: 0x0600DC5A RID: 56410 RVA: 0x00002501 File Offset: 0x00000701
		public override bool DataRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DC5B RID: 56411 RVA: 0x0040DF78 File Offset: 0x0040C178
		public override bool Applies(LordJob_Ritual ritual)
		{
			LordJob_Ritual_Duel lordJob_Ritual_Duel = ritual as LordJob_Ritual_Duel;
			return lordJob_Ritual_Duel != null && lordJob_Ritual_Duel.usedWeapon;
		}

		// Token: 0x0600DC5C RID: 56412 RVA: 0x0040DF98 File Offset: 0x0040C198
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
