using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000EC2 RID: 3778
	public class JobDriver_HaulToContainer : JobDriver, IBuildableDriver
	{
		// Token: 0x17000F37 RID: 3895
		// (get) Token: 0x06005F08 RID: 24328 RVA: 0x001E354C File Offset: 0x001E174C
		public Thing ThingToCarry
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.A);
			}
		}

		// Token: 0x17000F38 RID: 3896
		// (get) Token: 0x06005F09 RID: 24329 RVA: 0x001E355F File Offset: 0x001E175F
		public Thing Container
		{
			get
			{
				return (Thing)this.job.GetTarget(TargetIndex.B);
			}
		}

		// Token: 0x17000F39 RID: 3897
		// (get) Token: 0x06005F0A RID: 24330 RVA: 0x001E3572 File Offset: 0x001E1772
		public ThingDef ThingDef
		{
			get
			{
				return this.ThingToCarry.def;
			}
		}

		// Token: 0x17000F3A RID: 3898
		// (get) Token: 0x06005F0B RID: 24331 RVA: 0x001E3580 File Offset: 0x001E1780
		protected virtual int Duration
		{
			get
			{
				if (this.Container != null)
				{
					Building building = this.Container as Building;
					if (building != null)
					{
						return building.HaulToContainerDuration(this.ThingToCarry);
					}
				}
				return 0;
			}
		}

		// Token: 0x17000F3B RID: 3899
		// (get) Token: 0x06005F0C RID: 24332 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual EffecterDef WorkEffecter
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x06005F0D RID: 24333 RVA: 0x00002C42 File Offset: 0x00000E42
		protected virtual SoundDef WorkSustainer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005F0E RID: 24334 RVA: 0x001E35B2 File Offset: 0x001E17B2
		public bool TryGetBuildableRect(out CellRect rect)
		{
			if (this.Container is Blueprint)
			{
				rect = this.Container.OccupiedRect();
				return true;
			}
			rect = default(CellRect);
			return false;
		}

		// Token: 0x06005F0F RID: 24335 RVA: 0x001E35DC File Offset: 0x001E17DC
		public override string GetReport()
		{
			Thing thing;
			if (this.pawn.CurJob == this.job && this.pawn.carryTracker.CarriedThing != null)
			{
				thing = this.pawn.carryTracker.CarriedThing;
			}
			else
			{
				thing = base.TargetThingA;
			}
			if (thing == null || !this.job.targetB.HasThing)
			{
				return "ReportHaulingUnknown".Translate();
			}
			return ((this.job.GetTarget(TargetIndex.B).Thing is Building_Grave) ? "ReportHaulingToGrave" : "ReportHaulingTo").Translate(thing.Label, this.job.targetB.Thing.LabelShort.Named("DESTINATION"), thing.Named("THING"));
		}

		// Token: 0x06005F10 RID: 24336 RVA: 0x001E36B4 File Offset: 0x001E18B4
		public override bool TryMakePreToilReservations(bool errorOnFailed)
		{
			if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.A), this.job, 1, -1, null, errorOnFailed, false))
			{
				return false;
			}
			if (this.Container.Isnt<IHaulEnroute>())
			{
				if (!this.pawn.Reserve(this.job.GetTarget(TargetIndex.B), this.job, 1, 1, null, errorOnFailed, false))
				{
					return false;
				}
				this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.B), this.job, 1, -1, null);
			}
			this.UpdateEnrouteTrackers();
			this.pawn.ReserveAsManyAsPossible(this.job.GetTargetQueue(TargetIndex.A), this.job, 1, -1, null);
			return true;
		}

		// Token: 0x06005F11 RID: 24337 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void ModifyPrepareToil(Toil toil)
		{
		}

		// Token: 0x06005F12 RID: 24338 RVA: 0x001E3764 File Offset: 0x001E1964
		private bool TryReplaceWithFrame(TargetIndex index)
		{
			Thing thing = base.GetActor().jobs.curJob.GetTarget(index).Thing;
			Building edifice = thing.Position.GetEdifice(this.pawn.Map);
			if (edifice != null)
			{
				Blueprint_Build blueprint_Build = thing as Blueprint_Build;
				if (blueprint_Build != null)
				{
					Frame frame = edifice as Frame;
					if (frame != null && frame.BuildDef == blueprint_Build.BuildDef)
					{
						this.job.SetTarget(TargetIndex.B, frame);
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005F13 RID: 24339 RVA: 0x001E37E2 File Offset: 0x001E19E2
		protected override IEnumerable<Toil> MakeNewToils()
		{
			this.FailOnDestroyedOrNull(TargetIndex.A);
			this.FailOn(delegate()
			{
				Thing thing = base.GetActor().jobs.curJob.GetTarget(TargetIndex.B).Thing;
				Thing thing2 = base.GetActor().jobs.curJob.GetTarget(TargetIndex.C).Thing;
				if (thing == null)
				{
					return true;
				}
				if (thing2 != null && thing2.Destroyed && !this.TryReplaceWithFrame(TargetIndex.C))
				{
					this.job.SetTarget(TargetIndex.C, null);
				}
				if (!thing.Spawned || (thing.Destroyed && !this.TryReplaceWithFrame(TargetIndex.B)))
				{
					if (this.job.targetQueueB.NullOrEmpty<LocalTargetInfo>())
					{
						return true;
					}
					Thing nextTarget;
					if (!Toils_Haul.TryGetNextDestinationFromQueue(TargetIndex.C, TargetIndex.B, this.ThingDef, this.job, this.pawn, out nextTarget))
					{
						return true;
					}
					this.job.targetQueueB.RemoveAll((LocalTargetInfo target) => target.Thing == nextTarget);
					this.job.targetB = nextTarget;
				}
				ThingOwner thingOwner = this.Container.TryGetInnerInteractableThingOwner();
				if (thingOwner != null && !thingOwner.CanAcceptAnyOf(this.ThingToCarry, true))
				{
					return true;
				}
				IHaulDestination haulDestination = this.Container as IHaulDestination;
				return haulDestination != null && !haulDestination.Accepts(this.ThingToCarry);
			});
			this.FailOnForbidden(TargetIndex.B);
			this.FailOn(() => EnterPortalUtility.WasLoadingCanceled(this.Container));
			this.FailOn(() => TransporterUtility.WasLoadingCanceled(this.Container));
			this.FailOn(() => CompBiosculpterPod.WasLoadingCanceled(this.Container));
			this.FailOn(() => Building_SubcoreScanner.WasLoadingCancelled(this.Container));
			Toil getToHaulTarget = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, true).FailOn(() => this.ThingToCarry.ParentHolder is MinifiedThing).FailOnSelfAndParentsDespawnedOrNull(TargetIndex.A);
			Toil uninstallIfMinifiable = Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(() => this.ThingToCarry.ParentHolder is MinifiedThing).FailOnSelfAndParentsDespawnedOrNull(TargetIndex.A).FailOnDestroyedOrNull(TargetIndex.A);
			Toil startCarryingThing = Toils_Haul.StartCarryThing(TargetIndex.A, false, true, false, true, true);
			Toil jumpIfAlsoCollectingNextTarget = Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(getToHaulTarget, TargetIndex.A);
			Toil carryToContainer = (base.TargetThingB is Blueprint || base.TargetThingB is Frame) ? Toils_Goto.GotoBuild(TargetIndex.B) : Toils_Haul.CarryHauledThingToContainer();
			yield return Toils_Jump.JumpIf(jumpIfAlsoCollectingNextTarget, () => this.pawn.IsCarryingThing(this.ThingToCarry));
			yield return getToHaulTarget;
			yield return uninstallIfMinifiable;
			yield return startCarryingThing;
			yield return jumpIfAlsoCollectingNextTarget;
			yield return carryToContainer;
			yield return Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
			Toil toil = Toils_General.Wait(this.Duration, TargetIndex.B);
			toil.WithProgressBarToilDelay(TargetIndex.B, false, -0.5f);
			EffecterDef workEffecter = this.WorkEffecter;
			if (workEffecter != null)
			{
				toil.WithEffect(workEffecter, TargetIndex.B, null);
			}
			SoundDef workSustainer = this.WorkSustainer;
			if (workSustainer != null)
			{
				toil.PlaySustainerOrSound(workSustainer, 1f);
			}
			Thing destThing = this.job.GetTarget(TargetIndex.B).Thing;
			toil.tickIntervalAction = delegate(int delta)
			{
				if (this.pawn.IsHashIntervalTick(80, delta) && destThing is Building_Grave && this.graveDigEffect == null)
				{
					this.graveDigEffect = EffecterDefOf.BuryPawn.Spawn();
					this.graveDigEffect.Trigger(destThing, destThing, -1);
				}
			};
			toil.tickAction = delegate()
			{
				Effecter effecter = this.graveDigEffect;
				if (effecter == null)
				{
					return;
				}
				effecter.EffectTick(destThing, destThing);
			};
			this.ModifyPrepareToil(toil);
			yield return toil;
			yield return Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
			yield return Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C, null);
			yield return Toils_Haul.JumpToCarryToNextContainerIfPossible(carryToContainer, TargetIndex.C);
			yield break;
		}

		// Token: 0x06005F14 RID: 24340 RVA: 0x001E37F4 File Offset: 0x001E19F4
		private void UpdateEnrouteTrackers()
		{
			int count = this.job.count;
			this.TryReserveEnroute(base.TargetThingC, ref count);
			if (base.TargetB != base.TargetC)
			{
				this.TryReserveEnroute(base.TargetThingB, ref count);
			}
			if (this.job.targetQueueB != null)
			{
				foreach (LocalTargetInfo a in this.job.targetQueueB)
				{
					if (!base.TargetC.HasThing || !(a == base.TargetThingC))
					{
						this.TryReserveEnroute(a.Thing, ref count);
					}
				}
			}
		}

		// Token: 0x06005F15 RID: 24341 RVA: 0x001E38C0 File Offset: 0x001E1AC0
		private void TryReserveEnroute(Thing thing, ref int count)
		{
			IHaulEnroute haulEnroute = thing as IHaulEnroute;
			if (haulEnroute != null && !thing.DestroyedOrNull())
			{
				this.UpdateTracker(haulEnroute, ref count);
			}
		}

		// Token: 0x06005F16 RID: 24342 RVA: 0x001E38E8 File Offset: 0x001E1AE8
		private void UpdateTracker(IHaulEnroute container, ref int count)
		{
			if (this.ThingToCarry.DestroyedOrNull())
			{
				return;
			}
			if (this.job.playerForced && container.GetSpaceRemainingWithEnroute(this.ThingDef, null) == 0)
			{
				container.Map.enrouteManager.InterruptEnroutePawns(container, this.pawn);
			}
			int num = Mathf.Min(count, container.GetSpaceRemainingWithEnroute(this.ThingDef, null));
			if (num > 0)
			{
				container.Map.enrouteManager.AddEnroute(container, this.pawn, base.TargetThingA.def, num);
			}
			count -= num;
		}

		// Token: 0x0400447A RID: 17530
		private Effecter graveDigEffect;

		// Token: 0x0400447B RID: 17531
		protected const TargetIndex CarryThingIndex = TargetIndex.A;

		// Token: 0x0400447C RID: 17532
		public const TargetIndex DestIndex = TargetIndex.B;

		// Token: 0x0400447D RID: 17533
		protected const TargetIndex PrimaryDestIndex = TargetIndex.C;

		// Token: 0x0400447E RID: 17534
		protected const int DiggingEffectInterval = 80;
	}
}
