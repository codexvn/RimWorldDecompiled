using System;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200170D RID: 5901
	public class LordToil_MetalhorrorActive : LordToil
	{
		// Token: 0x17001555 RID: 5461
		// (get) Token: 0x06008AF7 RID: 35575 RVA: 0x00002501 File Offset: 0x00000701
		public override bool AssignsDuties
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001556 RID: 5462
		// (get) Token: 0x06008AF8 RID: 35576 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool AllowAggressiveTargetingOfRoamers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008AF9 RID: 35577 RVA: 0x000026EA File Offset: 0x000008EA
		public override void UpdateAllDuties()
		{
		}

		// Token: 0x06008AFA RID: 35578 RVA: 0x000026EA File Offset: 0x000008EA
		public override void LordToilTick()
		{
		}

		// Token: 0x04005A5F RID: 23135
		private const int UpdateIntervalTicks = 300;
	}
}
