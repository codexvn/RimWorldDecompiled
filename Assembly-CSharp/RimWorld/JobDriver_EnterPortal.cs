using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001265 RID: 4709
	public class JobDriver_EnterPortal : JobDriver
	{
		// Token: 0x170011F5 RID: 4597
		// (get) Token: 0x0600732C RID: 29484 RVA: 0x00236DCF File Offset: 0x00234FCF
		public MapPortal MapPortal
		{
			get
			{
				return base.TargetThingA as MapPortal;
			}
		}

		// Token: 0x0600732D RID: 29485 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			return true;
		}

		// Token: 0x0600732E RID: 29486 RVA: 0x00236DDC File Offset: 0x00234FDC
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDespawnedOrNull(this.PortalInd);
			this.FailOn(delegate()
			{
				string text;
				return !((MapPortal)base.TargetThingA).IsEnterable(out text);
			});
			yield return Toils_Goto.GotoThing(this.PortalInd, PathEndMode.Touch, false);
			Toil toil = Toils_General.Wait(90, TargetIndex.None).FailOnCannotTouch(this.PortalInd, PathEndMode.Touch).WithProgressBarToilDelay(this.PortalInd, true, -0.5f);
			Toil toil2 = toil;
			toil2.tickIntervalAction = (Action<int>)Delegate.Combine(toil2.tickIntervalAction, new Action<int>(delegate(int delta)
			{
				this.pawn.rotationTracker.FaceTarget(base.TargetA);
			}));
			toil.handlingFacing = true;
			yield return toil;
			Toil toil3 = ToilMaker.MakeToil("MakeNewToils");
			toil3.initAction = delegate()
			{
				MapPortal mapPortal = base.TargetThingA as MapPortal;
				Map otherMap = mapPortal.GetOtherMap();
				IntVec3 intVec = mapPortal.GetDestinationLocation();
				if (!intVec.Standable(otherMap))
				{
					intVec = CellFinder.StandableCellNear(intVec, otherMap, 5f, null);
				}
				if (intVec == IntVec3.Invalid)
				{
					Messages.Message("UnableToEnterPortal".Translate(base.TargetThingA.Label), base.TargetThingA, MessageTypeDefOf.NegativeEvent, true);
					return;
				}
				bool flag = false;
				bool fireAtWill = false;
				if (this.pawn.IsPlayerControlled)
				{
					flag = this.pawn.Drafted;
					fireAtWill = this.pawn.drafter.FireAtWill;
				}
				this.pawn.DeSpawnOrDeselect(DestroyMode.Vanish);
				GenSpawn.Spawn(this.pawn, intVec, otherMap, Rot4.Random, WipeMode.Vanish, false, false);
				mapPortal.OnEntered(this.pawn);
				if (this.pawn.inventory != null)
				{
					this.pawn.inventory.UnloadEverything = !otherMap.IsPocketMap;
				}
				if (this.pawn.IsPlayerControlled && (flag || mapPortal.AutoDraftOnEnter))
				{
					this.pawn.drafter.Drafted = true;
					this.pawn.drafter.FireAtWill = fireAtWill;
				}
				Pawn_CarryTracker carryTracker = this.pawn.carryTracker;
				if (((carryTracker != null) ? carryTracker.CarriedThing : null) != null && !this.pawn.Drafted)
				{
					Thing thing;
					this.pawn.carryTracker.TryDropCarriedThing(this.pawn.Position, ThingPlaceMode.Direct, out thing, null);
				}
				this.pawn.mindState.priorityWork.ClearPrioritizedWorkAndJobQueue();
				Lord lord = this.pawn.GetLord();
				if (lord == null)
				{
					return;
				}
				lord.Notify_PawnLost(this.pawn, PawnLostCondition.ExitedMap, null);
			};
			yield return toil3;
			yield break;
		}

		// Token: 0x04004F79 RID: 20345
		private TargetIndex PortalInd = TargetIndex.A;

		// Token: 0x04004F7A RID: 20346
		private const int EnterDelay = 90;
	}
}
