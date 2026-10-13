using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020012E0 RID: 4832
	public class JobDriver_PutApparelOnOutfitStand : JobDriver
	{
		// Token: 0x17001292 RID: 4754
		// (get) Token: 0x06007662 RID: 30306 RVA: 0x00240F24 File Offset: 0x0023F124
		private Apparel Apparel
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing as Apparel;
			}
		}

		// Token: 0x17001293 RID: 4755
		// (get) Token: 0x06007663 RID: 30307 RVA: 0x00240F4C File Offset: 0x0023F14C
		private Building_OutfitStand DestOutfitStand
		{
			get
			{
				return this.job.GetTarget(TargetIndex.B).Thing as Building_OutfitStand;
			}
		}

		// Token: 0x17001294 RID: 4756
		// (get) Token: 0x06007664 RID: 30308 RVA: 0x00240F74 File Offset: 0x0023F174
		private bool TargetIsOnApparelSource
		{
			get
			{
				Apparel apparel = this.Apparel;
				if (apparel != null && !apparel.Spawned)
				{
					IApparelSource apparelSource = apparel.ParentHolder as IApparelSource;
					if (apparelSource != null)
					{
						return apparelSource is Thing;
					}
				}
				return false;
			}
		}

		// Token: 0x17001295 RID: 4757
		// (get) Token: 0x06007665 RID: 30309 RVA: 0x00240FB0 File Offset: 0x0023F1B0
		private IApparelSource ApparelSource
		{
			get
			{
				return (IApparelSource)this.job.GetTarget(TargetIndex.C).Thing;
			}
		}

		// Token: 0x06007666 RID: 30310 RVA: 0x00240FD6 File Offset: 0x0023F1D6
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			if (this.TargetIsOnApparelSource)
			{
				this.job.targetC = (Thing)this.Apparel.ParentHolder;
			}
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x00241008 File Offset: 0x0023F208
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (this.TargetIsOnApparelSource)
			{
				return this.pawn.Reserve((Thing)this.Apparel.ParentHolder, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestOutfitStand, this.job, 1, -1, null, errorOnFailed, false);
			}
			return this.pawn.Reserve(this.Apparel, this.job, 1, -1, null, errorOnFailed, false) && this.pawn.Reserve(this.DestOutfitStand, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007668 RID: 30312 RVA: 0x002410B4 File Offset: 0x0023F2B4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnBurningImmobile(TargetIndex.A);
			this.FailOnBurningImmobile(TargetIndex.B);
			bool usingSource = this.TargetIsOnApparelSource;
			if (usingSource)
			{
				yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.InteractionCell).FailOnDespawnedNullOrForbidden(TargetIndex.C);
			}
			else
			{
				yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, false).FailOnDespawnedNullOrForbidden(TargetIndex.A);
			}
			if (usingSource)
			{
				Toil toil = ToilMaker.MakeToil("MakeNewToils");
				toil.initAction = delegate()
				{
					this.ApparelSource.RemoveApparel(this.Apparel);
					this.pawn.carryTracker.TryStartCarry(this.Apparel);
				};
				yield return toil;
			}
			else
			{
				this.job.count = 1;
				yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			}
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell, false).FailOnDespawnedNullOrForbidden(TargetIndex.B);
			Toil toil2 = ToilMaker.MakeToil("MakeNewToils");
			toil2.initAction = delegate()
			{
				Apparel apparel = this.pawn.carryTracker.CarriedThing as Apparel;
				if (apparel == null)
				{
					return;
				}
				if (!this.DestOutfitStand.HasRoomForApparelOfDef(apparel.def) && !this.DestOutfitStand.TryDropThingsToMakeRoomForThingOfDef(apparel.def))
				{
					base.EndJobWith(JobCondition.Errored);
					return;
				}
				this.pawn.carryTracker.innerContainer.Remove(apparel);
				this.DestOutfitStand.AddApparel(apparel);
			};
			yield return toil2;
			yield break;
		}

		// Token: 0x040050E7 RID: 20711
		private const TargetIndex ApparelInd = TargetIndex.A;

		// Token: 0x040050E8 RID: 20712
		private const TargetIndex OutfitStandInd = TargetIndex.B;

		// Token: 0x040050E9 RID: 20713
		private const TargetIndex ApparelSourceIndex = TargetIndex.C;
	}
}
