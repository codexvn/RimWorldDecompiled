using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200128D RID: 4749
	public class JobDriver_HaulCorpseToPublicPlace : JobDriver
	{
		// Token: 0x17001225 RID: 4645
		// (get) Token: 0x06007440 RID: 29760 RVA: 0x00239FF4 File Offset: 0x002381F4
		private Corpse Corpse
		{
			get
			{
				return (Corpse)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x17001226 RID: 4646
		// (get) Token: 0x06007441 RID: 29761 RVA: 0x0023A01C File Offset: 0x0023821C
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x17001227 RID: 4647
		// (get) Token: 0x06007442 RID: 29762 RVA: 0x0023A042 File Offset: 0x00238242
		private bool InGrave
		{
			get
			{
				return this.Grave != null;
			}
		}

		// Token: 0x17001228 RID: 4648
		// (get) Token: 0x06007443 RID: 29763 RVA: 0x0023A04D File Offset: 0x0023824D
		private Thing Target
		{
			get
			{
				return this.Grave ?? this.Corpse;
			}
		}

		// Token: 0x06007444 RID: 29764 RVA: 0x0023A05F File Offset: 0x0023825F
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.Target, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06007445 RID: 29765 RVA: 0x0023A082 File Offset: 0x00238282
		public override string GetReport()
		{
			if (this.InGrave && this.Grave.def == ThingDefOf.Grave)
			{
				return "ReportDiggingUpCorpse".Translate();
			}
			return base.GetReport();
		}

		// Token: 0x06007446 RID: 29766 RVA: 0x0023A0B4 File Offset: 0x002382B4
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			Toil gotoCorpse = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, false).FailOnDespawnedOrNull(TargetIndex.A);
			yield return Toils_Jump.JumpIfTargetInvalid(TargetIndex.B, gotoCorpse);
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.InteractionCell, false).FailOnDespawnedOrNull(TargetIndex.B);
			Building_Grave grave = this.Grave;
			yield return Toils_General.Wait((grave != null) ? grave.OpenTicks : 60, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f).FailOnDespawnedOrNull(TargetIndex.B).FailOnCannotTouch(TargetIndex.B, PathEndMode.InteractionCell);
			yield return Toils_General.Open(TargetIndex.B);
			yield return Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false);
			yield return gotoCorpse;
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, false, false, true, false);
			yield return this.FindCellToDropCorpseToil();
			yield return Toils_Reserve.Reserve(TargetIndex.C, 1, -1, null, false);
			yield return Toils_Goto.GotoCell(TargetIndex.C, PathEndMode.Touch);
			yield return Toils_Haul.PlaceHauledThingInCell(TargetIndex.C, null, false, false);
			yield return this.ForbidAndNotifyMentalStateToil();
			yield break;
		}

		// Token: 0x06007447 RID: 29767 RVA: 0x0023A0C4 File Offset: 0x002382C4
		private Toil FindCellToDropCorpseToil()
		{
			Toil toil = ToilMaker.MakeToil("FindCellToDropCorpseToil");
			toil.initAction = delegate()
			{
				IntVec3 c = IntVec3.Invalid;
				if (!Rand.Chance(0.8f) || !this.TryFindTableCell(out c))
				{
					bool flag = false;
					IntVec3 root;
					if (RCellFinder.TryFindRandomSpotJustOutsideColony(this.pawn, out root) && CellFinder.TryRandomClosewalkCellNear(root, this.pawn.Map, 5, out c, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null))
					{
						flag = true;
					}
					if (!flag)
					{
						c = CellFinder.RandomClosewalkCellNear(this.pawn.Position, this.pawn.Map, 10, (IntVec3 x) => this.pawn.CanReserve(x, 1, -1, null, false) && x.GetFirstItem(this.pawn.Map) == null);
					}
				}
				this.job.SetTarget(TargetIndex.C, c);
			};
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06007448 RID: 29768 RVA: 0x0023A0E9 File Offset: 0x002382E9
		private Toil ForbidAndNotifyMentalStateToil()
		{
			Toil toil = ToilMaker.MakeToil("ForbidAndNotifyMentalStateToil");
			toil.initAction = delegate()
			{
				Corpse corpse = this.Corpse;
				if (corpse != null)
				{
					corpse.SetForbidden(true, true);
				}
				MentalState_CorpseObsession mentalState_CorpseObsession = this.pawn.MentalState as MentalState_CorpseObsession;
				if (mentalState_CorpseObsession != null)
				{
					mentalState_CorpseObsession.Notify_CorpseHauled();
				}
			};
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06007449 RID: 29769 RVA: 0x0023A110 File Offset: 0x00238310
		private bool TryFindTableCell(out IntVec3 cell)
		{
			JobDriver_HaulCorpseToPublicPlace.tmpCells.Clear();
			List<Building> allBuildingsColonist = this.pawn.Map.listerBuildings.allBuildingsColonist;
			for (int i = 0; i < allBuildingsColonist.Count; i++)
			{
				Building building = allBuildingsColonist[i];
				if (building.def.IsTable)
				{
					foreach (IntVec3 intVec in building.OccupiedRect())
					{
						if (this.pawn.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, false) && intVec.GetFirstItem(this.pawn.Map) == null)
						{
							JobDriver_HaulCorpseToPublicPlace.tmpCells.Add(intVec);
						}
					}
				}
			}
			if (!JobDriver_HaulCorpseToPublicPlace.tmpCells.Any<IntVec3>())
			{
				cell = IntVec3.Invalid;
				return false;
			}
			cell = JobDriver_HaulCorpseToPublicPlace.tmpCells.RandomElement<IntVec3>();
			return true;
		}

		// Token: 0x04004FEE RID: 20462
		private const TargetIndex CorpseInd = TargetIndex.A;

		// Token: 0x04004FEF RID: 20463
		private const TargetIndex GraveInd = TargetIndex.B;

		// Token: 0x04004FF0 RID: 20464
		private const TargetIndex CellInd = TargetIndex.C;

		// Token: 0x04004FF1 RID: 20465
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
