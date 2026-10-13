using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013C3 RID: 5059
	public class JobDriver_RemoveApparel : JobDriver
	{
		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06007BDC RID: 31708 RVA: 0x00254E44 File Offset: 0x00253044
		private Apparel Apparel
		{
			get
			{
				return (Apparel)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06007BDD RID: 31709 RVA: 0x00254E6A File Offset: 0x0025306A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.duration, "duration", 0, false);
		}

		// Token: 0x06007BDE RID: 31710 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x06007BDF RID: 31711 RVA: 0x00254E84 File Offset: 0x00253084
		public override void Notify_Starting()
		{
			base.Notify_Starting();
			this.duration = (int)(this.Apparel.GetStatValue(StatDefOf.EquipDelay, true, -1) * 60f);
		}

		// Token: 0x06007BE0 RID: 31712 RVA: 0x00254EAB File Offset: 0x002530AB
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			yield return Toils_General.Wait(this.duration, TargetIndex.None).PlaySustainerOrSound(this.Apparel.def.apparel.soundRemove, 1f).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_General.Do(delegate
			{
				if (!this.pawn.apparel.WornApparel.Contains(this.Apparel))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				Apparel apparel;
				if (!this.pawn.apparel.TryDrop(this.Apparel, out apparel))
				{
					base.EndJobWith(JobCondition.Incompletable);
					return;
				}
				this.job.targetA = apparel;
				if (!this.job.haulDroppedApparel)
				{
					base.EndJobWith(JobCondition.Succeeded);
					return;
				}
				apparel.SetForbidden(false, false);
				StoragePriority currentPriority = StoreUtility.CurrentStoragePriorityOf(apparel, false);
				IntVec3 c;
				if (StoreUtility.TryFindBestBetterStoreCellFor(apparel, this.pawn, base.Map, currentPriority, this.pawn.Faction, out c, true))
				{
					this.job.count = apparel.stackCount;
					this.job.targetB = c;
					return;
				}
				base.EndJobWith(JobCondition.Incompletable);
			});
			if (!this.job.haulDroppedApparel)
			{
				yield break;
			}
			yield return Toils_Reserve.Reserve(TargetIndex.B, 1, -1, null, false);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false).FailOn(() => !this.pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation));
			Toil carryToCell = Toils_Haul.CarryHauledThingToCell(TargetIndex.B, PathEndMode.ClosestTouch);
			yield return carryToCell;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.B, carryToCell, true, false);
			yield break;
		}

		// Token: 0x040053AD RID: 21421
		private int duration;

		// Token: 0x040053AE RID: 21422
		private const TargetIndex ApparelInd = TargetIndex.A;
	}
}
