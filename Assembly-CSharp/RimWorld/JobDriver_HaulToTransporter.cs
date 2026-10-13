using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001290 RID: 4752
	public class JobDriver_HaulToTransporter : JobDriver_HaulToContainer
	{
		// Token: 0x1700122D RID: 4653
		// (get) Token: 0x0600745E RID: 29790 RVA: 0x0023A747 File Offset: 0x00238947
		public CompTransporter Transporter
		{
			get
			{
				Thing container = base.Container;
				if (container == null)
				{
					return null;
				}
				return container.TryGetComp<CompTransporter>();
			}
		}

		// Token: 0x0600745F RID: 29791 RVA: 0x0023A75A File Offset: 0x0023895A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.initialCount, "initialCount", 0, false);
		}

		// Token: 0x06007460 RID: 29792 RVA: 0x0023A774 File Offset: 0x00238974
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06007461 RID: 29793 RVA: 0x0023A7C4 File Offset: 0x002389C4
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
				thingCount = LoadTransportersJobUtility.FindThingToLoad(this.pawn, base.Container.TryGetComp<CompTransporter>());
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

		// Token: 0x04004FF9 RID: 20473
		public int initialCount;
	}
}
