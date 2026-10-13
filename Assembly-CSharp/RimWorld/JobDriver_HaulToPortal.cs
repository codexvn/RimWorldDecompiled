using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200128F RID: 4751
	public class JobDriver_HaulToPortal : JobDriver_HaulToContainer
	{
		// Token: 0x1700122B RID: 4651
		// (get) Token: 0x06007458 RID: 29784 RVA: 0x0023A5B3 File Offset: 0x002387B3
		public MapPortal MapPortal
		{
			get
			{
				return base.Container as MapPortal;
			}
		}

		// Token: 0x1700122C RID: 4652
		// (get) Token: 0x06007459 RID: 29785 RVA: 0x0023A5C0 File Offset: 0x002387C0
		protected override int Duration
		{
			get
			{
				return 90;
			}
		}

		// Token: 0x0600745A RID: 29786 RVA: 0x0023A5C4 File Offset: 0x002387C4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		// Token: 0x0600745B RID: 29787 RVA: 0x0023A5E0 File Offset: 0x002387E0
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x0600745C RID: 29788 RVA: 0x0023A630 File Offset: 0x00238830
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			ThingCount thingCount;
			if (this.job.targetA.IsValid)
			{
				thingCount = new ThingCount(this.job.targetA.Thing, this.job.targetA.Thing.stackCount, false);
			}
			else
			{
				thingCount = EnterPortalUtility.FindThingToLoad(this.pawn, this.MapPortal);
			}
			if (this.job.playerForced && this.pawn.carryTracker.CarriedThing != null && this.pawn.carryTracker.CarriedThing != thingCount.Thing)
			{
				Thing thing;
				this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Near, out thing, null);
			}
			this.job.targetA = thingCount.Thing;
			this.job.count = thingCount.Count;
			this.initialCount = thingCount.Count;
			this.pawn.Reserve(thingCount.Thing, this.job, 1, -1, null, true, false);
		}

		// Token: 0x04004FF7 RID: 20471
		private const int DepositDuration = 90;

		// Token: 0x04004FF8 RID: 20472
		public int initialCount;
	}
}
