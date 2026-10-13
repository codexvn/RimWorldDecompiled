using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200170F RID: 5903
	public class LordToil_NestAssault : LordToil
	{
		// Token: 0x17001559 RID: 5465
		// (get) Token: 0x06008B01 RID: 35585 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x06008B02 RID: 35586 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008B03 RID: 35587 RVA: 0x00296E80 File Offset: 0x00295080
		public LordToil_NestAssault(bool attackDownedIfStarving = false, bool canPickUpOpportunisticWeapons = false)
		{
			this.attackDownedIfStarving = attackDownedIfStarving;
			this.canPickUpOpportunisticWeapons = canPickUpOpportunisticWeapons;
		}

		// Token: 0x06008B04 RID: 35588 RVA: 0x00293AB7 File Offset: 0x00291CB7
		public override void Init()
		{
			base.Init();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.Drafting, OpportunityType.Critical);
		}

		// Token: 0x06008B05 RID: 35589 RVA: 0x00296E98 File Offset: 0x00295098
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				if (this.lord.ownedPawns[i].mindState != null)
				{
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.NestAssault);
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

		// Token: 0x04005A62 RID: 23138
		private bool attackDownedIfStarving;

		// Token: 0x04005A63 RID: 23139
		private bool canPickUpOpportunisticWeapons;
	}
}
