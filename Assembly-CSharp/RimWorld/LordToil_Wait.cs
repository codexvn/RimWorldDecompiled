using System;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200172F RID: 5935
	public class LordToil_Wait : LordToil
	{
		// Token: 0x1700157B RID: 5499
		// (get) Token: 0x06008B9B RID: 35739 RVA: 0x00299636 File Offset: 0x00297836
		protected virtual DutyDef IdleDutyDef
		{
			get
			{
				if (!this.allowRandomInteractions)
				{
					return DutyDefOf.IdleNoInteraction;
				}
				return DutyDefOf.Idle;
			}
		}

		// Token: 0x06008B9C RID: 35740 RVA: 0x0029964B File Offset: 0x0029784B
		public LordToil_Wait(bool allowRandomInteractions = true)
		{
			this.allowRandomInteractions = allowRandomInteractions;
		}

		// Token: 0x06008B9D RID: 35741 RVA: 0x00299664 File Offset: 0x00297864
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				PawnDuty duty = new PawnDuty(this.IdleDutyDef);
				this.DecoratePawnDuty(duty);
				this.lord.ownedPawns[i].mindState.duty = duty;
			}
		}

		// Token: 0x06008B9E RID: 35742 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void DecoratePawnDuty(PawnDuty duty)
		{
		}

		// Token: 0x04005AB3 RID: 23219
		public bool allowRandomInteractions = true;
	}
}
