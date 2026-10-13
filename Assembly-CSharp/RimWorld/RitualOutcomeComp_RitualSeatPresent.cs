using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02002744 RID: 10052
	public class RitualOutcomeComp_RitualSeatPresent : RitualOutcomeComp_BuildingsPresent
	{
		// Token: 0x170021C9 RID: 8649
		// (get) Token: 0x0600DC8D RID: 56461 RVA: 0x0040EA33 File Offset: 0x0040CC33
		protected override string LabelForDesc
		{
			get
			{
				return "RitualSeat".Translate();
			}
		}

		// Token: 0x0600DC8E RID: 56462 RVA: 0x0040EA44 File Offset: 0x0040CC44
		public override QualityFactor GetQualityFactor(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments, RitualOutcomeComp_Data data)
		{
			if (assignments.SpectatorsForReading.Count > 0)
			{
				return base.GetQualityFactor(ritual, ritualTarget, obligation, assignments, data);
			}
			return null;
		}

		// Token: 0x0600DC8F RID: 56463 RVA: 0x0040EA64 File Offset: 0x0040CC64
		public override bool Applies(LordJob_Ritual ritual)
		{
			return !Find.IdeoManager.classicMode && (base.Applies(ritual) && ritual.assignments != null && ritual.assignments.SpectatorsForReading.Count > 0) && ritual.Ritual.ideo.RitualSeatDef != null;
		}

		// Token: 0x0600DC90 RID: 56464 RVA: 0x0040EAB8 File Offset: 0x0040CCB8
		protected override int RequiredAmount(RitualRoleAssignments assignments)
		{
			if (assignments.Ritual.ideo.RitualSeatDef == null)
			{
				return 0;
			}
			int num = assignments.Ritual.ideo.RitualSeatDef.Size.x * assignments.Ritual.ideo.RitualSeatDef.Size.z;
			return Mathf.Max(1, Mathf.CeilToInt((float)assignments.SpectatorsForReading.Count / (float)num));
		}

		// Token: 0x0600DC91 RID: 56465 RVA: 0x0040EB2C File Offset: 0x0040CD2C
		protected override string LabelForPredictedOutcomeDesc(Precept_Ritual ritual)
		{
			ThingDef ritualSeatDef = ritual.ideo.RitualSeatDef;
			if (ritualSeatDef == null)
			{
				return this.LabelForDesc;
			}
			return ritualSeatDef.LabelCap;
		}

		// Token: 0x0600DC92 RID: 56466 RVA: 0x0040EB5C File Offset: 0x0040CD5C
		protected override Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual)
		{
			ThingDef ritualSeatDef = ritual.ideo.RitualSeatDef;
			if (ritualSeatDef == null)
			{
				return null;
			}
			Thing firstThing = cell.GetFirstThing(map, ritualSeatDef);
			if (firstThing != null)
			{
				return firstThing;
			}
			return null;
		}
	}
}
