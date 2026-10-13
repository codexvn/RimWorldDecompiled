using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FCD RID: 4045
	public class AttackTargetReservationManager : IExposable
	{
		// Token: 0x060063DB RID: 25563 RVA: 0x001FAEE3 File Offset: 0x001F90E3
		public AttackTargetReservationManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x060063DC RID: 25564 RVA: 0x001FAF00 File Offset: 0x001F9100
		public void Reserve(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(((claimant != null) ? claimant.ToString() : null) + " tried to reserve null attack target.");
				return;
			}
			if (this.IsReservedBy(claimant, target))
			{
				return;
			}
			AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = new AttackTargetReservationManager.AttackTargetReservation();
			attackTargetReservation.target = target;
			attackTargetReservation.claimant = claimant;
			attackTargetReservation.job = job;
			this.reservations.Add(attackTargetReservation);
		}

		// Token: 0x060063DD RID: 25565 RVA: 0x001FAF60 File Offset: 0x001F9160
		public void Release(Pawn claimant, Job job, IAttackTarget target)
		{
			if (target == null)
			{
				Log.Warning(((claimant != null) ? claimant.ToString() : null) + " tried to release reservation on null attack target.");
				return;
			}
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant && attackTargetReservation.job == job)
				{
					this.reservations.RemoveAt(i);
					return;
				}
			}
			Log.Warning(string.Concat(new string[]
			{
				(claimant != null) ? claimant.ToString() : null,
				" with job ",
				(job != null) ? job.ToString() : null,
				" tried to release reservation on target ",
				(target != null) ? target.ToString() : null,
				", but it's not reserved by him."
			}));
		}

		// Token: 0x060063DE RID: 25566 RVA: 0x001FB030 File Offset: 0x001F9230
		public bool CanReserve(Pawn claimant, IAttackTarget target)
		{
			if (this.IsReservedBy(claimant, target))
			{
				return true;
			}
			int reservationsCount = this.GetReservationsCount(target, claimant.Faction);
			int maxPreferredReservationsCount = this.GetMaxPreferredReservationsCount(target);
			return reservationsCount < maxPreferredReservationsCount;
		}

		// Token: 0x060063DF RID: 25567 RVA: 0x001FB064 File Offset: 0x001F9264
		public bool IsReservedBy(Pawn claimant, IAttackTarget target)
		{
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant == claimant)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060063E0 RID: 25568 RVA: 0x001FB0AC File Offset: 0x001F92AC
		public void ReleaseAllForTarget(IAttackTarget target)
		{
			this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == target);
		}

		// Token: 0x060063E1 RID: 25569 RVA: 0x001FB0E0 File Offset: 0x001F92E0
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

		// Token: 0x060063E2 RID: 25570 RVA: 0x001FB13C File Offset: 0x001F933C
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

		// Token: 0x060063E3 RID: 25571 RVA: 0x001FB184 File Offset: 0x001F9384
		public IAttackTarget FirstReservationFor(Pawn claimant)
		{
			for (int i = this.reservations.Count - 1; i >= 0; i--)
			{
				if (this.reservations[i].claimant == claimant)
				{
					return this.reservations[i].target;
				}
			}
			return null;
		}

		// Token: 0x060063E4 RID: 25572 RVA: 0x001FB1D0 File Offset: 0x001F93D0
		public void ExposeData()
		{
			Scribe_Collections.Look<AttackTargetReservationManager.AttackTargetReservation>(ref this.reservations, "reservations", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.target == null);
				if (this.reservations.RemoveAll((AttackTargetReservationManager.AttackTargetReservation x) => x.claimant.DestroyedOrNull()) != 0)
				{
					Log.Warning("Some attack target reservations had null or destroyed claimant.");
				}
			}
		}

		// Token: 0x060063E5 RID: 25573 RVA: 0x001FB25C File Offset: 0x001F945C
		private int GetReservationsCount(IAttackTarget target, Faction faction)
		{
			int num = 0;
			for (int i = 0; i < this.reservations.Count; i++)
			{
				AttackTargetReservationManager.AttackTargetReservation attackTargetReservation = this.reservations[i];
				if (attackTargetReservation.target == target && attackTargetReservation.claimant.Faction == faction)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x060063E6 RID: 25574 RVA: 0x001FB2AC File Offset: 0x001F94AC
		private int GetMaxPreferredReservationsCount(IAttackTarget target)
		{
			int num = 0;
			CellRect cellRect = target.Thing.OccupiedRect();
			foreach (IntVec3 c in cellRect.ExpandedBy(1))
			{
				if (!cellRect.Contains(c) && c.InBounds(this.map) && c.Standable(this.map))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x040047AC RID: 18348
		private Map map;

		// Token: 0x040047AD RID: 18349
		private List<AttackTargetReservationManager.AttackTargetReservation> reservations = new List<AttackTargetReservationManager.AttackTargetReservation>();

		// Token: 0x02000FCE RID: 4046
		public class AttackTargetReservation : IExposable
		{
			// Token: 0x060063E7 RID: 25575 RVA: 0x001FB33C File Offset: 0x001F953C
			public void ExposeData()
			{
				Scribe_References.Look<IAttackTarget>(ref this.target, "target", false);
				Scribe_References.Look<Pawn>(ref this.claimant, "claimant", false);
				Scribe_References.Look<Job>(ref this.job, "job", false);
			}

			// Token: 0x040047AE RID: 18350
			public IAttackTarget target;

			// Token: 0x040047AF RID: 18351
			public Pawn claimant;

			// Token: 0x040047B0 RID: 18352
			public Job job;
		}
	}
}
