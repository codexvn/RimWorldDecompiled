using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020026DB RID: 9947
	public class RitualCancellationTrigger_CannotScarify : RitualCancellationTrigger
	{
		// Token: 0x0600DA02 RID: 55810 RVA: 0x00407E9A File Offset: 0x0040609A
		public override IEnumerable<Trigger> CancellationTriggers(RitualRoleAssignments assignments)
		{
			yield return new Trigger_Custom(delegate(TriggerSignal signal)
			{
				if (signal.type == TriggerSignalType.Tick && GenTicks.TicksGame % 60 == 0)
				{
					Pawn pawn = assignments.FirstAssignedPawn(this.roleId);
					if (pawn == null)
					{
						return true;
					}
					if (!JobDriver_Scarify.AvailableOnNow(pawn, null))
					{
						return true;
					}
				}
				return false;
			});
			yield break;
		}

		// Token: 0x04009393 RID: 37779
		[NoTranslate]
		public string roleId;
	}
}
