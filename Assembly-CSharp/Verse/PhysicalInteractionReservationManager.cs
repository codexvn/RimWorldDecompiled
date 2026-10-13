using System;
using System.Collections.Generic;
using Verse.AI;

namespace Verse
{
	// Token: 0x020001C9 RID: 457
	public class PhysicalInteractionReservationManager : IExposable
	{
		// Token: 0x06000C91 RID: 3217 RVA: 0x0003AFA8 File Offset: 0x000391A8
		public void Reserve(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = new PhysicalInteractionReservationManager.PhysicalInteractionReservation();
			physicalInteractionReservation.target = target;
			physicalInteractionReservation.claimant = claimant;
			physicalInteractionReservation.job = job;
			this.reservations.Add(physicalInteractionReservation);
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x0003AFE8 File Offset: 0x000391E8
		public void Release(Pawn claimant, Job job, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant && physicalInteractionReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			string str = (claimant != null) ? claimant.ToString() : null;
			string str2 = " tried to release reservation on target ";
			LocalTargetInfo localTargetInfo = target;
			Log.Warning(str + str2 + localTargetInfo.ToString() + ", but it's not reserved by him.");
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x0003B075 File Offset: 0x00039275
		public void TryRelease(Pawn claimant, Job job, LocalTargetInfo target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				this.Release(claimant, job, target);
			}
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x0003B08C File Offset: 0x0003928C
		public bool IsReservedBy(Pawn claimant, LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target && physicalInteractionReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x0003B0D6 File Offset: 0x000392D6
		public bool IsReserved(LocalTargetInfo target)
		{
			return this.FirstReserverOf(target) != null;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x0003B0E4 File Offset: 0x000392E4
		public Pawn FirstReserverOf(LocalTargetInfo target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					return physicalInteractionReservation.claimant;
				}
			}
			return null;
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x0003B12C File Offset: 0x0003932C
		public void ReserversOf(LocalTargetInfo target, HashSet<Pawn> reserversOut)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				PhysicalInteractionReservationManager.PhysicalInteractionReservation physicalInteractionReservation = this.reservations[i];
				if (physicalInteractionReservation.target == target)
				{
					reserversOut.Add(physicalInteractionReservation.claimant);
				}
			}
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0003B178 File Offset: 0x00039378
		public LocalTargetInfo FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return LocalTargetInfo.Invalid;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x0003B1C8 File Offset: 0x000393C8
		public void ReleaseAllForTarget(LocalTargetInfo target)
		{
			this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.target == target);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0003B1FC File Offset: 0x000393FC
		public void ReleaseClaimedBy(Pawn claimant, Job job)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant && this.reservations[i].job == job)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x0003B258 File Offset: 0x00039458
		public void ReleaseAllClaimedBy(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					this.reservations.RemoveAt(i);
				}
			}
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x0003B2A0 File Offset: 0x000394A0
		public void ExposeData()
		{
			Scribe_Collections.Look<PhysicalInteractionReservationManager.PhysicalInteractionReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.reservations.RemoveAll((PhysicalInteractionReservationManager.PhysicalInteractionReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some physical interaction reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x04000728 RID: 1832
		private List<PhysicalInteractionReservationManager.PhysicalInteractionReservation> reservations = new List<PhysicalInteractionReservationManager.PhysicalInteractionReservation>();

		// Token: 0x020001CA RID: 458
		public class PhysicalInteractionReservation : IExposable
		{
			// Token: 0x06000C9E RID: 3230 RVA: 0x0003B314 File Offset: 0x00039514
			public void ExposeData()
			{
				Scribe_TargetInfo.Look(ref this.target, "target");
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x04000729 RID: 1833
			public LocalTargetInfo target;

			// Token: 0x0400072A RID: 1834
			public Pawn claimant;

			// Token: 0x0400072B RID: 1835
			public Job job;
		}
	}
}
