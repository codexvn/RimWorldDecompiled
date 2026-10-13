using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001227 RID: 4647
	public class JobDriver_CarryDownedPawn : JobDriver
	{
		// Token: 0x170011A4 RID: 4516
		// (get) Token: 0x06007186 RID: 29062 RVA: 0x00231838 File Offset: 0x0022FA38
		protected Pawn Takee
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007187 RID: 29063 RVA: 0x0023185E File Offset: 0x0022FA5E
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Takee, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007188 RID: 29064 RVA: 0x00231881 File Offset: 0x0022FA81
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOnAggroMentalStateAndHostile(TargetIndex.A);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A).FailOn(() => !this.Takee.Downed && !this.Takee.IsSelfShutdown()).FailOnSomeonePhysicallyInteracting(TargetIndex.A);
			Toil toil = Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			toil.AddPreInitAction(new Action(this.CheckMakeTakeeGuest));
			yield return toil;
			yield break;
		}

		// Token: 0x06007189 RID: 29065 RVA: 0x00231894 File Offset: 0x0022FA94
		private void CheckMakeTakeeGuest()
		{
			if (!this.job.def.makeTargetPrisoner && !this.Takee.HostileTo(Faction.OfPlayer) && this.Takee.Faction != Faction.OfPlayer && this.Takee.HostFaction != Faction.OfPlayer && this.Takee.guest != null && !this.Takee.IsWildMan())
			{
				this.Takee.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Guest);
			}
		}

		// Token: 0x04004EA6 RID: 20134
		private const TargetIndex TakeeIndex = TargetIndex.A;
	}
}
