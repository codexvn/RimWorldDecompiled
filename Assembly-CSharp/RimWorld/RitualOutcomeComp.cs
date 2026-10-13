using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002731 RID: 10033
	public abstract class RitualOutcomeComp
	{
		// Token: 0x170021B9 RID: 8633
		// (get) Token: 0x0600DC27 RID: 56359 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool DataRequired
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600DC28 RID: 56360 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return 0f;
		}

		// Token: 0x0600DC29 RID: 56361 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual RitualOutcomeComp_Data MakeData()
		{
			return null;
		}

		// Token: 0x0600DC2A RID: 56362
		public abstract bool Applies(LordJob_Ritual ritual);

		// Token: 0x0600DC2B RID: 56363 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Tick(LordJob_Ritual ritual, RitualOutcomeComp_Data data, float progressAmount)
		{
		}

		// Token: 0x0600DC2C RID: 56364 RVA: 0x0040D938 File Offset: 0x0040BB38
		public virtual string GetDesc(LordJob_Ritual ritual = null, RitualOutcomeComp_Data data = null)
		{
			return this.label;
		}

		// Token: 0x0600DC2D RID: 56365 RVA: 0x0040D938 File Offset: 0x0040BB38
		public virtual string GetDescAbstract(bool positive, float quality = -1f)
		{
			return this.label;
		}

		// Token: 0x0600DC2E RID: 56366 RVA: 0x0040D938 File Offset: 0x0040BB38
		public virtual string GetBonusDescShort()
		{
			return this.label;
		}

		// Token: 0x0600DC2F RID: 56367 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			return null;
		}

		// Token: 0x0600DC30 RID: 56368 RVA: 0x00035ADE File Offset: 0x00033CDE
		protected virtual string ExpectedOffsetDesc(bool positive, float quality = 0f)
		{
			return "";
		}

		// Token: 0x0600DC31 RID: 56369 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_AssignmentsChanged(RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
		}

		// Token: 0x0600DC32 RID: 56370 RVA: 0x0040D940 File Offset: 0x0040BB40
		public virtual IEnumerable<string> BlockingIssues(Precept_Ritual ritual, TargetInfo target, RitualRoleAssignments assignments)
		{
			yield break;
		}

		// Token: 0x040094B8 RID: 38072
		[MustTranslate]
		protected string label;

		// Token: 0x040094B9 RID: 38073
		[MustTranslate]
		protected string labelAbstract;

		// Token: 0x040094BA RID: 38074
		protected float qualityOffset;
	}
}
