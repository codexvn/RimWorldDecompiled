using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010A3 RID: 4259
	public class LordToil_ExitMapFighting : LordToil_ExitMap
	{
		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x0600689D RID: 26781 RVA: 0x00210F59 File Offset: 0x0020F159
		public override DutyDef ExitDuty
		{
			get
			{
				return DutyDefOf.ExitMapBestAndDefendSelf;
			}
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x00210F60 File Offset: 0x0020F160
		public LordToil_ExitMapFighting(LocomotionUrgency locomotion = LocomotionUrgency.None, bool canDig = false, bool interruptCurrentJob = false) : base(locomotion, canDig, interruptCurrentJob)
		{
		}
	}
}
