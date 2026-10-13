using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000F8D RID: 3981
	public class MentalState_Jailbreaker : MentalState
	{
		// Token: 0x06006237 RID: 25143 RVA: 0x001F06F8 File Offset: 0x001EE8F8
		public override void MentalStateTick(int delta)
		{
			base.MentalStateTick(delta);
			if (this.pawn.IsHashIntervalTick(500, delta) && this.pawn.CurJobDef != JobDefOf.InducePrisonerToEscape && JailbreakerMentalStateUtility.FindPrisoner(this.pawn) == null)
			{
				base.RecoverFromState();
			}
		}

		// Token: 0x06006238 RID: 25144 RVA: 0x001F0744 File Offset: 0x001EE944
		public void Notify_InducedPrisonerToEscape()
		{
			MentalStateUtility.TryTransitionToWanderOwnRoom(this);
		}

		// Token: 0x04004669 RID: 18025
		private const int NoPrisonerToFreeCheckInterval = 500;
	}
}
