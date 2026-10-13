using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001733 RID: 5939
	public class LordToil_TravelAndWaitForItems : LordToil_Travel, IWaitForItemsLordToil
	{
		// Token: 0x17001582 RID: 5506
		// (get) Token: 0x06008BB0 RID: 35760 RVA: 0x00299947 File Offset: 0x00297B47
		public bool HasAllRequestedItems
		{
			get
			{
				return this.CountRemaining <= 0;
			}
		}

		// Token: 0x17001583 RID: 5507
		// (get) Token: 0x06008BB1 RID: 35761 RVA: 0x00299955 File Offset: 0x00297B55
		public int CountRemaining
		{
			get
			{
				return GiveItemsToPawnUtility.GetCountRemaining(this.target, this.requestedThingDef, this.requestedThingCount);
			}
		}

		// Token: 0x06008BB2 RID: 35762 RVA: 0x0029996E File Offset: 0x00297B6E
		public LordToil_TravelAndWaitForItems(IntVec3 dest, Pawn target, ThingDef thingDef, int amount) : base(dest)
		{
			this.target = target;
			this.requestedThingDef = thingDef;
			this.requestedThingCount = amount;
		}

		// Token: 0x06008BB3 RID: 35763 RVA: 0x0029998D File Offset: 0x00297B8D
		public override void DrawPawnGUIOverlay(Pawn pawn)
		{
			if (pawn == this.target)
			{
				pawn.Map.overlayDrawer.DrawOverlay(pawn, OverlayTypes.QuestionMark);
			}
		}

		// Token: 0x06008BB4 RID: 35764 RVA: 0x002999AB File Offset: 0x00297BAB
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

		// Token: 0x04005AC2 RID: 23234
		public Pawn target;

		// Token: 0x04005AC3 RID: 23235
		public ThingDef requestedThingDef;

		// Token: 0x04005AC4 RID: 23236
		public int requestedThingCount;
	}
}
