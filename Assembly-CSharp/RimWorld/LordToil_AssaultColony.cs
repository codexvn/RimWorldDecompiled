using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E0 RID: 5856
	public class LordToil_AssaultColony : LordToil
	{
		// Token: 0x17001532 RID: 5426
		// (get) Token: 0x06008A38 RID: 35384 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008A39 RID: 35385 RVA: 0x00293AA1 File Offset: 0x00291CA1
		public LordToil_AssaultColony(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x17001533 RID: 5427
		// (get) Token: 0x06008A3A RID: 35386 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A3B RID: 35387 RVA: 0x00293AB7 File Offset: 0x00291CB7
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06008A3C RID: 35388 RVA: 0x00293ACC File Offset: 0x00291CCC
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].mindState != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultColony);
					this.lord.ownedPawns[i].mindState.duty.attackDownedIfStarving = this.attackDownedIfStarving;
					this.lord.ownedPawns[i].mindState.duty.pickupOpportunisticWeapon = this.canPickUpOpportunisticWeapons;
					CompCanBeDormant compCanBeDormant = this.lord.ownedPawns[i].TryGetComp<CompCanBeDormant>();
					if (compCanBeDormant != null)
					{
						compCanBeDormant.WakeUp();
					}
				}
			}
		}

		// Token: 0x04005A19 RID: 23065
		private bool attackDownedIfStarving;

		// Token: 0x04005A1A RID: 23066
		private bool canPickUpOpportunisticWeapons;
	}
}
