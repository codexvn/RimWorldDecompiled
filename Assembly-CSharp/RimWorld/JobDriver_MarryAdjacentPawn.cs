using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020011B0 RID: 4528
	public class JobDriver_MarryAdjacentPawn : JobDriver
	{
		// Token: 0x1700112C RID: 4396
		// (get) Token: 0x06006EB9 RID: 28345 RVA: 0x0022859C File Offset: 0x0022679C
		private Pawn OtherFiance
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x1700112D RID: 4397
		// (get) Token: 0x06006EBA RID: 28346 RVA: 0x002285C2 File Offset: 0x002267C2
		public int TicksLeftToMarry
		{
			get
			{
				return this.ticksLeftToMarry;
			}
		}

		// Token: 0x06006EBB RID: 28347 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06006EBC RID: 28348 RVA: 0x002285CA File Offset: 0x002267CA
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(TargetIndex.A);
			this.FailOn(() => this.OtherFiance.Drafted || !this.pawn.Position.AdjacentTo8WayOrInside(this.OtherFiance));
			Toil toil = ToilMaker.MakeToil("MakeNewToils");
			toil.initAction = delegate()
			{
				this.ticksLeftToMarry = 2500;
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				this.ticksLeftToMarry -= delta;
				if (this.ticksLeftToMarry <= 0)
				{
					this.ticksLeftToMarry = 0;
					base.ReadyForNextToil();
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			toil.FailOn(() => !this.pawn.relations.DirectRelationExists(PawnRelationDefOf.Fiance, this.OtherFiance));
			yield return toil;
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.defaultCompleteMode = ToilCompleteMode.Instant;
			toil2.initAction = delegate()
			{
				if (this.pawn.thingIDNumber < this.OtherFiance.thingIDNumber)
				{
					MarriageCeremonyUtility.Married(this.pawn, this.OtherFiance);
				}
			};
			yield return toil2;
			yield break;
		}

		// Token: 0x06006EBD RID: 28349 RVA: 0x002285DA File Offset: 0x002267DA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeftToMarry, "ticksLeftToMarry", 0, false);
		}

		// Token: 0x04004D46 RID: 19782
		private int ticksLeftToMarry = 2500;

		// Token: 0x04004D47 RID: 19783
		private const TargetIndex OtherFianceInd = TargetIndex.A;

		// Token: 0x04004D48 RID: 19784
		private const int Duration = 2500;
	}
}
