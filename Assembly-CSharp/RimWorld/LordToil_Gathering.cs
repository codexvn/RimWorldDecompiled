using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001743 RID: 5955
	public abstract class LordToil_Gathering : LordToil
	{
		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x06008BEB RID: 35819 RVA: 0x00280E7F File Offset: 0x0027F07F
		public LordToilData_Gathering Data
		{
			get
			{
				return (LordToilData_Gathering)this.data;
			}
		}

		// Token: 0x06008BEC RID: 35820 RVA: 0x0029AAA2 File Offset: 0x00298CA2
		public LordToil_Gathering(IntVec3 spot, GatheringDef gatheringDef)
		{
			this.spot = spot;
			this.gatheringDef = gatheringDef;
			this.data = new LordToilData_Gathering();
		}

		// Token: 0x06008BED RID: 35821 RVA: 0x0029AAC3 File Offset: 0x00298CC3
		public override ThinkTreeDutyHook VoluntaryJoinDutyHookFor(Pawn p)
		{
			return this.gatheringDef.duty.hook;
		}

		// Token: 0x06008BEE RID: 35822 RVA: 0x0029AAD8 File Offset: 0x00298CD8
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.gatheringDef.duty, this.spot, -1f);
			}
		}

		// Token: 0x06008BEF RID: 35823 RVA: 0x0029AB3C File Offset: 0x00298D3C
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.spot, base.Map))
				{
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x04005AF3 RID: 23283
		protected IntVec3 spot;

		// Token: 0x04005AF4 RID: 23284
		protected GatheringDef gatheringDef;
	}
}
