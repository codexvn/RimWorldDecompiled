using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F92 RID: 3986
	public class MentalState_SlaveRebellion : MentalState
	{
		// Token: 0x06006252 RID: 25170 RVA: 0x001F0AD8 File Offset: 0x001EECD8
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(500, delta) && this.pawn.CurJobDef != JobDefOf.InduceSlaveToRebel && SlaveRebellionUtility.FindSlaveForRebellion(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x04004670 RID: 18032
		private const int NoSlaveToRebelWithCheckInterval = 500;
	}
}
