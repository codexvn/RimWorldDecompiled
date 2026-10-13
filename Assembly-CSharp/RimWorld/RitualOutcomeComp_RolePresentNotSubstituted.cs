using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002739 RID: 10041
	public class RitualOutcomeComp_RolePresentNotSubstituted : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x170021C3 RID: 8643
		// (get) Token: 0x0600DC5E RID: 56414 RVA: 0x0040D938 File Offset: 0x0040BB38
		protected override string LabelForDesc
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x0600DC5F RID: 56415 RVA: 0x0040DFEE File Offset: 0x0040C1EE
		public override string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			if (this.disableInClassicMode && Find.IdeoManager.classicMode)
			{
				return string.Empty;
			}
			return base.GetDesc(ritual, data);
		}

		// Token: 0x0600DC60 RID: 56416 RVA: 0x0040E012 File Offset: 0x0040C212
		public override bool Applies(LordJob_Ritual ritual)
		{
			return (!this.disableInClassicMode || !Find.IdeoManager.classicMode) && ritual.RoleFilled(this.roleId) && !ritual.assignments.RoleSubstituted(this.roleId);
		}

		// Token: 0x0600DC61 RID: 56417 RVA: 0x0040E050 File Offset: 0x0040C250
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			if (this.disableInClassicMode && Find.IdeoManager.classicMode)
			{
				return null;
			}
			bool flag = assignments.AnyPawnAssigned(this.roleId) && !assignments.RoleSubstituted(this.roleId);
			return new QualityFactor
			{
				label = this.LabelForDesc.CapitalizeFirst(),
				present = flag,
				qualityChange = this.ExpectedOffsetDesc(flag, -1f),
				quality = (flag ? this.qualityOffset : 0f),
				positive = flag,
				priority = 3f
			};
		}

		// Token: 0x040094C3 RID: 38083
		public string roleId;

		// Token: 0x040094C4 RID: 38084
		public bool disableInClassicMode;
	}
}
