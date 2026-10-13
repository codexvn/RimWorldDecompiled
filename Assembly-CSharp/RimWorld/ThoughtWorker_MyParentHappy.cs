using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018E3 RID: 6371
	public class ThoughtWorker_MyParentHappy : ThoughtWorker
	{
		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x06009109 RID: 37129 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected virtual int RequiredParentCount
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600910A RID: 37130 RVA: 0x002AAC8D File Offset: 0x002A8E8D
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.Suspended)
			{
				return ThoughtState.Inactive;
			}
			if (!this.HasRequiredHappyChildren(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x0600910B RID: 37131 RVA: 0x002AACBE File Offset: 0x002A8EBE
		protected virtual bool HasRequiredHappyChildren(Pawn p)
		{
			return this.HappyParentCount(p) == this.RequiredParentCount;
		}

		// Token: 0x0600910C RID: 37132 RVA: 0x002AACD0 File Offset: 0x002A8ED0
		protected int HappyParentCount(Pawn pawn)
		{
			int num = 0;
			Pawn father = pawn.GetFather();
			Pawn mother = pawn.GetMother();
			if (father != null && ThoughtWorker_RelatedChildMoodBase.InSameMapOrCaravan(pawn, father) && ThoughtWorker_MyParentHappy.Counts(father))
			{
				num++;
			}
			if (mother != null && ThoughtWorker_RelatedChildMoodBase.InSameMapOrCaravan(pawn, mother) && ThoughtWorker_MyParentHappy.Counts(mother))
			{
				num++;
			}
			return num;
		}

		// Token: 0x0600910D RID: 37133 RVA: 0x002AAD20 File Offset: 0x002A8F20
		public static bool Counts(Pawn pawn)
		{
			Pawn_NeedsTracker needs = pawn.needs;
			return ((needs != null) ? needs.mood : null) != null && pawn.needs.mood.CurLevelPercentage > 0.6f && !(pawn.ParentHolder is Building_GrowthVat) && !(pawn.ParentHolder is Building_CryptosleepCasket);
		}

		// Token: 0x04005C6B RID: 23659
		private const float HappyMoodThreshold = 0.6f;
	}
}
