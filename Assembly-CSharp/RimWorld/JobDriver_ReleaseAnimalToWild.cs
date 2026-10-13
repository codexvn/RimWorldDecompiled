using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001127 RID: 4391
	public class JobDriver_ReleaseAnimalToWild : JobDriver
	{
		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06006B18 RID: 27416 RVA: 0x0021B747 File Offset: 0x00219947
		protected Pawn Animal
		{
			get
			{
				return (Pawn)this.job.targetA.Thing;
			}
		}

		// Token: 0x06006B19 RID: 27417 RVA: 0x0021C1D0 File Offset: 0x0021A3D0
		public static bool TryFindClosestOutsideCell(IntVec3 root, Map map, TraverseParms traverseParms, Pawn pawn, out IntVec3 cell)
		{
			cell = default(IntVec3);
			Region region;
			if (CellFinder.TryFindClosestRegionWith(root.GetRegion(map, RegionType.Set_Passable), traverseParms, (Region r) => r.District.TouchesMapEdge, 99999, out region, RegionType.Set_Passable))
			{
				float num = float.PositiveInfinity;
				foreach (IntVec3 intVec in region.Cells)
				{
					if (intVec.Standable(map) && (pawn == null || !intVec.IsForbidden(pawn)))
					{
						float num2 = (float)intVec.DistanceToSquared(root);
						if (num2 < num)
						{
							num = num2;
							cell = intVec;
						}
					}
				}
				return num != float.PositiveInfinity;
			}
			return false;
		}

		// Token: 0x06006B1A RID: 27418 RVA: 0x001D81A7 File Offset: 0x001D63A7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006B1B RID: 27419 RVA: 0x0021C29C File Offset: 0x0021A49C
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOn(() => this.Animal.Dead);
			this.FailOn(() => this.Animal.Faction != Faction.OfPlayer);
			this.FailOn(() => this.Animal.InAggroMentalState);
			this.FailOn(() => this.Animal.MapHeld.designationManager.DesignationOn(this.Animal, DesignationDefOf.ReleaseAnimalToWild) == null);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.OnCell, false);
			Toil waitToil = Toils_General.WaitWith(TargetIndex.A, 180, false, false, false, TargetIndex.None, PathEndMode.Touch).WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
			yield return Toils_Jump.JumpIf(waitToil, () => this.Animal.Position.GetRegion(base.Map, RegionType.Set_Passable).District.TouchesMapEdge);
			JobDriver_ReleaseAnimalToWild.<>c__DisplayClass6_0 CS$<>8__locals1 = new JobDriver_ReleaseAnimalToWild.<>c__DisplayClass6_0();
			CS$<>8__locals1.<>4__this = this;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			CS$<>8__locals1.toil = ToilMaker.MakeToil("MakeNewToils");
			CS$<>8__locals1.toil.initAction = delegate()
			{
				Pawn actor = CS$<>8__locals1.toil.actor;
				IntVec3 c;
				if (!JobDriver_ReleaseAnimalToWild.TryFindClosestOutsideCell(actor.Position, actor.Map, TraverseParms.For(actor, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), actor, out c))
				{
					CS$<>8__locals1.<>4__this.EndJobWith(JobCondition.Incompletable);
					return;
				}
				actor.pather.StartPath(c, PathEndMode.OnCell);
			};
			CS$<>8__locals1.toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			yield return CS$<>8__locals1.toil;
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.A, null, false, false);
			CS$<>8__locals1 = null;
			yield return waitToil;
			yield return Toils_General.Do(delegate
			{
				ReleaseAnimalToWildUtility.DoReleaseAnimal(this.Animal, this.pawn);
			});
			yield break;
		}

		// Token: 0x04004B81 RID: 19329
		private const TargetIndex AnimalInd = TargetIndex.A;

		// Token: 0x04004B82 RID: 19330
		private const int WaitForTicks = 180;
	}
}
