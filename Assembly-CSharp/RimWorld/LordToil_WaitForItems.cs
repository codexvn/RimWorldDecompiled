using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001731 RID: 5937
	public class LordToil_WaitForItems : LordToil, IWaitForItemsLordToil
	{
		// Token: 0x1700157E RID: 5502
		// (get) Token: 0x06008BA1 RID: 35745 RVA: 0x002996BB File Offset: 0x002978BB
		public bool HasAllRequestedItems
		{
			get
			{
				return this.CountRemaining <= 0;
			}
		}

		// Token: 0x1700157F RID: 5503
		// (get) Token: 0x06008BA2 RID: 35746 RVA: 0x002996C9 File Offset: 0x002978C9
		public int CountRemaining
		{
			get
			{
				return GiveItemsToPawnUtility.GetCountRemaining(this.target, this.requestedThingDef, this.requestedThingCount);
			}
		}

		// Token: 0x06008BA3 RID: 35747 RVA: 0x002996E2 File Offset: 0x002978E2
		public LordToil_WaitForItems(Pawn target, ThingDef thingDef, int amount, IntVec3 waitSpot)
		{
			this.waitSpot = waitSpot;
			this.target = target;
			this.requestedThingDef = thingDef;
			this.requestedThingCount = amount;
		}

		// Token: 0x06008BA4 RID: 35748 RVA: 0x00299708 File Offset: 0x00297908
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.WanderClose_NoNeeds, this.waitSpot, 10f);
			}
		}

		// Token: 0x06008BA5 RID: 35749 RVA: 0x00299765 File Offset: 0x00297965
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06008BA6 RID: 35750 RVA: 0x00299783 File Offset: 0x00297983
		public override IEnumerable<FloatMenuOption> ExtraFloatMenuOptions(Pawn requester, Pawn current)
		{
			if (this.target == requester)
			{
				foreach (FloatMenuOption floatMenuOption in GiveItemsToPawnUtility.GetFloatMenuOptionsForPawn(requester, current, this.requestedThingDef, this.requestedThingCount))
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04005AB4 RID: 23220
		public IntVec3 waitSpot;

		// Token: 0x04005AB5 RID: 23221
		public Pawn target;

		// Token: 0x04005AB6 RID: 23222
		public ThingDef requestedThingDef;

		// Token: 0x04005AB7 RID: 23223
		public int requestedThingCount;

		// Token: 0x04005AB8 RID: 23224
		private const float WanderRadius = 10f;
	}
}
