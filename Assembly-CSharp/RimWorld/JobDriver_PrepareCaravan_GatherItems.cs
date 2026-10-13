using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001142 RID: 4418
	public class JobDriver_PrepareCaravan_GatherItems : JobDriver
	{
		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06006BCA RID: 27594 RVA: 0x0021E158 File Offset: 0x0021C358
		public Thing ToHaul
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06006BCB RID: 27595 RVA: 0x0021E17C File Offset: 0x0021C37C
		public Pawn Carrier
		{
			get
			{
				return (Pawn)this.job.GetTarget(TargetIndex.B).Thing;
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06006BCC RID: 27596 RVA: 0x0021E1A2 File Offset: 0x0021C3A2
		private List<TransferableOneWay> Transferables
		{
			get
			{
				return ((LordJob_FormAndSendCaravan)this.job.lord.LordJob).transferables;
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x06006BCD RID: 27597 RVA: 0x0021E1C0 File Offset: 0x0021C3C0
		private TransferableOneWay Transferable
		{
			get
			{
				TransferableOneWay transferableOneWay = TransferableUtility.TransferableMatchingDesperate(this.ToHaul, this.Transferables, TransferAsOneMode.PodsOrCaravanPacking);
				if (transferableOneWay != null)
				{
					return transferableOneWay;
				}
				throw new InvalidOperationException("Could not find any matching transferable.");
			}
		}

		// Token: 0x06006BCE RID: 27598 RVA: 0x0021E1EF File Offset: 0x0021C3EF
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return this.pawn.Reserve(this.ToHaul, this.job, 1, -1, null, errorOnFailed, false);
		}

		// Token: 0x06006BCF RID: 27599 RVA: 0x0021E214 File Offset: 0x0021C414
		protected override IEnumerable<Toil> MakeNewToils()
		{
			if (this.gatherState == PrepareCaravanGatherState.Unset)
			{
				this.gatherState = ((!this.pawn.IsFormingCaravan() || (MassUtility.IsOverEncumbered(this.pawn) && !this.pawn.inventory.HasAnyUnpackedCaravanItems)) ? PrepareCaravanGatherState.Carry : PrepareCaravanGatherState.Haul);
			}
			if (this.gatherState == PrepareCaravanGatherState.Carry)
			{
				return this.MakeNewToilsCarry();
			}
			return this.MakeNewToilsHaulInInventory();
		}

		// Token: 0x06006BD0 RID: 27600 RVA: 0x0021E275 File Offset: 0x0021C475
		private IEnumerable<Toil> MakeNewToilsCarry()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false).FailOnDestroyedOrNull(TargetIndex.A);
			yield return reserve;
			bool inInventory = HaulAIUtility.IsInHaulableInventory(this.ToHaul);
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, inInventory);
			yield return this.DetermineNumToHaul(null);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, inInventory);
			yield return this.AddCarriedThingToTransferables();
			yield return Toils_Haul.CheckForGetOpportunityDuplicate(reserve, TargetIndex.A, TargetIndex.None, true, (Thing x) => this.Transferable.things.Contains(x));
			Toil findCarrier = this.FindCarrier();
			yield return findCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier);
			yield return Toils_General.Wait(25, TargetIndex.None).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return this.PlaceTargetInCarrierInventory();
			yield break;
		}

		// Token: 0x06006BD1 RID: 27601 RVA: 0x0021E285 File Offset: 0x0021C485
		private IEnumerable<Toil> MakeNewToilsHaulInInventory()
		{
			this.FailOn(() => !base.Map.lordManager.lords.Contains(this.job.lord));
			bool inInventory = HaulAIUtility.IsInHaulableInventory(this.ToHaul);
			Toil reserve = Toils_Reserve.Reserve(TargetIndex.A, 1, -1, null, false).FailOnDestroyedOrNull(TargetIndex.A);
			Toil findCarrier = this.FindCarrier();
			yield return reserve;
			yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch, inInventory).JumpIf(new Func<bool>(this.IsFinishedCollectingItems), findCarrier);
			yield return this.DetermineNumToHaul(findCarrier);
			yield return Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, inInventory);
			yield return this.AddCarriedThingToTransferables();
			yield return Toils_General.Wait(25, TargetIndex.None).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return this.HaulCaravanItemInInventory(reserve);
			yield return findCarrier;
			yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.Touch, false).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier);
			yield return Toils_General.Wait(25, TargetIndex.None).JumpIf(() => !JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(this.Carrier, this.pawn, true), findCarrier).WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			yield return this.AddHauledItemsToCarrier(findCarrier);
			yield break;
		}

		// Token: 0x06006BD2 RID: 27602 RVA: 0x0021E298 File Offset: 0x0021C498
		private Toil DetermineNumToHaul(Toil findCarrier = null)
		{
			Toil toil = ToilMaker.MakeToil("DetermineNumToHaul");
			toil.initAction = delegate()
			{
				int num = GatherItemsForCaravanUtility.CountLeftToTransfer(this.pawn, this.Transferable, this.job.lord);
				if (this.pawn.carryTracker.CarriedThing != null)
				{
					num -= this.pawn.carryTracker.CarriedThing.stackCount;
				}
				if (num > 0)
				{
					this.job.count = num;
					return;
				}
				if (findCarrier == null || !this.pawn.inventory.HasAnyUnpackedCaravanItems)
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
					return;
				}
				this.pawn.jobs.curDriver.JumpToToil(findCarrier);
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x0021E2E3 File Offset: 0x0021C4E3
		private Toil AddCarriedThingToTransferables()
		{
			Toil toil = ToilMaker.MakeToil("AddCarriedThingToTransferables");
			toil.initAction = delegate()
			{
				TransferableOneWay transferable = this.Transferable;
				if (!transferable.things.Contains(this.pawn.carryTracker.CarriedThing))
				{
					transferable.things.Add(this.pawn.carryTracker.CarriedThing);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			toil.atomicWithPrevious = true;
			return toil;
		}

		// Token: 0x06006BD4 RID: 27604 RVA: 0x0021E30F File Offset: 0x0021C50F
		private Toil FindCarrier()
		{
			Toil toil = ToilMaker.MakeToil("FindCarrier");
			toil.initAction = delegate()
			{
				Pawn pawn = this.FindBestCarrier(true);
				if (pawn == null)
				{
					bool flag = this.pawn.GetLord() == this.job.lord;
					if (flag && !MassUtility.IsOverEncumbered(this.pawn))
					{
						pawn = this.pawn;
					}
					else
					{
						pawn = this.FindBestCarrier(false);
						if (pawn == null)
						{
							if (flag)
							{
								pawn = this.pawn;
							}
							else
							{
								IEnumerable<Pawn> source = from x in this.job.lord.ownedPawns
								where JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(x, this.pawn, true)
								select x;
								if (!source.Any<Pawn>())
								{
									base.EndJobWith(JobCondition.Incompletable);
									return;
								}
								pawn = source.RandomElement<Pawn>();
							}
						}
					}
				}
				this.job.SetTarget(TargetIndex.B, pawn);
			};
			return toil;
		}

		// Token: 0x06006BD5 RID: 27605 RVA: 0x0021E32D File Offset: 0x0021C52D
		private bool IsFinishedCollectingItems()
		{
			return MassUtility.IsOverEncumbered(this.pawn) || (this.pickedUpFirstItemTicks > -1 && Find.TickManager.TicksGame > this.pickedUpFirstItemTicks + 7500);
		}

		// Token: 0x06006BD6 RID: 27606 RVA: 0x0021E364 File Offset: 0x0021C564
		private Toil HaulCaravanItemInInventory(Toil reserve)
		{
			Toil toil = ToilMaker.MakeToil("HaulCaravanItemInInventory");
			toil.initAction = delegate()
			{
				if (this.pickedUpFirstItemTicks == -1)
				{
					this.pickedUpFirstItemTicks = Find.TickManager.TicksGame;
				}
				this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - this.pawn.carryTracker.CarriedThing.stackCount, 0));
				this.pawn.inventory.AddHauledCaravanItem(this.pawn.carryTracker.CarriedThing);
				if (!this.IsFinishedCollectingItems())
				{
					this.SetNewHaulTargetAndJumpToReserve(reserve);
				}
			};
			return toil;
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x0021E3A4 File Offset: 0x0021C5A4
		private void SetNewHaulTargetAndJumpToReserve(Toil reserve)
		{
			if (this.CheckToilLoopBackstop())
			{
				Thing thing = GatherItemsForCaravanUtility.FindThingToHaul(this.pawn, this.pawn.GetLord());
				if (thing != null)
				{
					this.job.SetTarget(TargetIndex.A, thing);
					this.pawn.jobs.curDriver.JumpToToil(reserve);
				}
			}
		}

		// Token: 0x06006BD8 RID: 27608 RVA: 0x0021E3FC File Offset: 0x0021C5FC
		private Toil AddHauledItemsToCarrier(Toil findCarrier)
		{
			Toil toil = ToilMaker.MakeToil("AddHauledItemsToCarrier");
			toil.initAction = delegate()
			{
				if (this.Carrier == this.pawn)
				{
					this.pawn.inventory.ClearHaulingCaravanCache();
					return;
				}
				this.pawn.inventory.TransferCaravanItemsToCarrier(this.Carrier.inventory);
				if (this.pawn.inventory.HasAnyUnpackedCaravanItems && this.CheckToilLoopBackstop())
				{
					this.pawn.jobs.curDriver.JumpToToil(findCarrier);
				}
			};
			return toil;
		}

		// Token: 0x06006BD9 RID: 27609 RVA: 0x0021E43C File Offset: 0x0021C63C
		private bool CheckToilLoopBackstop()
		{
			int num = this.toilLoops + 1;
			this.toilLoops = num;
			if (num > 500)
			{
				Log.Error("Prepare caravan gather items job for pawn " + this.pawn.Label + " looped through toils too many times");
				base.EndJobWith(JobCondition.Errored);
				return false;
			}
			return true;
		}

		// Token: 0x06006BDA RID: 27610 RVA: 0x0021E48B File Offset: 0x0021C68B
		private Toil PlaceTargetInCarrierInventory()
		{
			Toil toil = ToilMaker.MakeToil("PlaceTargetInCarrierInventory");
			toil.initAction = delegate()
			{
				Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
				Thing carriedThing = carryTracker.CarriedThing;
				if (carryTracker.innerContainer.Count == 0)
				{
					carryTracker.pawn.Drawer.renderer.SetAllGraphicsDirty();
				}
				this.Transferable.AdjustTo(Mathf.Max(this.Transferable.CountToTransfer - carriedThing.stackCount, 0));
				Thing thing;
				carryTracker.innerContainer.TryTransferToContainer(carriedThing, this.Carrier.inventory.innerContainer, carriedThing.stackCount, out thing, true);
				CompForbiddable compForbiddable = (thing != null) ? thing.TryGetComp<CompForbiddable>() : null;
				if (compForbiddable != null)
				{
					compForbiddable.Forbidden = false;
				}
			};
			return toil;
		}

		// Token: 0x06006BDB RID: 27611 RVA: 0x0021E4AC File Offset: 0x0021C6AC
		public static bool IsUsableCarrier(Pawn p, Pawn forPawn, bool allowColonists)
		{
			return p.IsFormingCaravan() && (p == forPawn || (!p.DestroyedOrNull() && p.Spawned && !p.inventory.UnloadEverything && forPawn.CanReach(p, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn) && ((allowColonists && p.IsColonist) || ((p.RaceProps.packAnimal || p.HostFaction == Faction.OfPlayer) && !p.IsBurning() && !p.Downed && !MassUtility.IsOverEncumbered(p)))));
		}

		// Token: 0x06006BDC RID: 27612 RVA: 0x0021E540 File Offset: 0x0021C740
		private float GetCarrierScore(Pawn p)
		{
			float lengthHorizontal = (p.Position - this.pawn.Position).LengthHorizontal;
			float num = MassUtility.EncumbrancePercent(p);
			return 1f - num - lengthHorizontal / 10f * 0.2f;
		}

		// Token: 0x06006BDD RID: 27613 RVA: 0x0021E588 File Offset: 0x0021C788
		private Pawn FindBestCarrier(bool onlyAnimals)
		{
			Lord lord = this.job.lord;
			Pawn pawn = null;
			float num = 0f;
			if (lord != null)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn2 = lord.ownedPawns[i];
					if (pawn2 != this.pawn && (!onlyAnimals || pawn2.RaceProps.Animal) && JobDriver_PrepareCaravan_GatherItems.IsUsableCarrier(pawn2, this.pawn, false))
					{
						float carrierScore = this.GetCarrierScore(pawn2);
						if (pawn == null || carrierScore > num)
						{
							pawn = pawn2;
							num = carrierScore;
						}
					}
				}
			}
			return pawn;
		}

		// Token: 0x06006BDE RID: 27614 RVA: 0x0021E614 File Offset: 0x0021C814
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.pickedUpFirstItemTicks, "pickedUpFirstItemTicks", 0, false);
			Scribe_Values.Look<int>(ref this.toilLoops, "toilLoops", 0, false);
			Scribe_Values.Look<PrepareCaravanGatherState>(ref this.gatherState, "gatherState", PrepareCaravanGatherState.Unset, false);
		}

		// Token: 0x04004BC2 RID: 19394
		private int pickedUpFirstItemTicks = -1;

		// Token: 0x04004BC3 RID: 19395
		private int toilLoops;

		// Token: 0x04004BC4 RID: 19396
		private PrepareCaravanGatherState gatherState;

		// Token: 0x04004BC5 RID: 19397
		private const TargetIndex ToHaulInd = TargetIndex.A;

		// Token: 0x04004BC6 RID: 19398
		private const TargetIndex CarrierInd = TargetIndex.B;

		// Token: 0x04004BC7 RID: 19399
		private const int MaxTicksGatherItems = 7500;

		// Token: 0x04004BC8 RID: 19400
		private const int LoopBackstop = 500;
	}
}
