using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020016E8 RID: 5864
	public class LordToil_AssaultThings : LordToil
	{
		// Token: 0x1700153E RID: 5438
		// (get) Token: 0x06008A69 RID: 35433 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool ForceHighStoryDanger
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700153F RID: 5439
		// (get) Token: 0x06008A6A RID: 35434 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AllowSatisfyLongNeeds
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06008A6B RID: 35435 RVA: 0x0029493D File Offset: 0x00292B3D
		public LordToil_AssaultThings(IEnumerable<Thing> things)
		{
			this.things = new List<Thing>(things);
		}

		// Token: 0x06008A6C RID: 35436 RVA: 0x00294951 File Offset: 0x00292B51
		public override void Notify_ReachedDutyLocation(Pawn pawn)
		{
			this.UpdateAllDuties();
		}

		// Token: 0x06008A6D RID: 35437 RVA: 0x0029495C File Offset: 0x00292B5C
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = this.lord.ownedPawns[i].mindState.duty;
				if (duty == null || duty.def != DutyDefOf.AssaultThing || duty.focus.ThingDestroyed)
				{
					Thing t2;
					if (!(from t in this.things
					where t != null && t.Spawned
					select t).TryRandomElement(out t2))
					{
						break;
					}
					this.lord.ownedPawns[i].mindState.duty = new PawnDuty(DutyDefOf.AssaultThing, t2, -1f);
				}
			}
		}

		// Token: 0x06008A6E RID: 35438 RVA: 0x00293E2F File Offset: 0x0029202F
		public override void LordToilTick()
		{
			if (this.lord.ticksInToil % 300 == 0)
			{
				this.UpdateAllDuties();
			}
		}

		// Token: 0x04005A31 RID: 23089
		private List<Thing> things;

		// Token: 0x04005A32 RID: 23090
		public const int UpdateIntervalTicks = 300;
	}
}
