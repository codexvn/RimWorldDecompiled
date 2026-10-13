using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010DE RID: 4318
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x0600693E RID: 26942 RVA: 0x00212BF2 File Offset: 0x00210DF2
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x0600693F RID: 26943 RVA: 0x00212C04 File Offset: 0x00210E04
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Need_Rest rest = lord.ownedPawns[i].needs.rest;
					if (rest != null && rest.CurLevelPercentage < 0.14f + this.extraRestThreshOffset && !lord.ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04004A77 RID: 19063
		private float extraRestThreshOffset;
	}
}
