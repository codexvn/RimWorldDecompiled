using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200170A RID: 5898
	public class LordToil_LoadAndEnterPortal : LordToil
	{
		// Token: 0x17001553 RID: 5459
		// (get) Token: 0x06008AEF RID: 35567 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008AF0 RID: 35568 RVA: 0x00296C30 File Offset: 0x00294E30
		public LordToil_LoadAndEnterPortal(MapPortal portal)
		{
			this.portal = portal;
		}

		// Token: 0x06008AF1 RID: 35569 RVA: 0x00296C40 File Offset: 0x00294E40
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty pawnDuty = new PawnDuty(DutyDefOf.LoadAndEnterPortal);
				pawnDuty.focus = new LocalTargetInfo(this.portal);
				this.lord.ownedPawns[i].mindState.duty = pawnDuty;
			}
		}

		// Token: 0x04005A5D RID: 23133
		public MapPortal portal;
	}
}
