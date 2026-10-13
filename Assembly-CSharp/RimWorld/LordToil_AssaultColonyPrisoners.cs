using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E4 RID: 5860
	public class LordToil_AssaultColonyPrisoners : LordToil
	{
		// Token: 0x17001539 RID: 5433
		// (get) Token: 0x06008A56 RID: 35414 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700153A RID: 5434
		// (get) Token: 0x06008A58 RID: 35416 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A59 RID: 35417 RVA: 0x00293AB7 File Offset: 0x00291CB7
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06008A5A RID: 35418 RVA: 0x002944E8 File Offset: 0x002926E8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.PrisonerAssaultColony);
			}
		}
	}
}
