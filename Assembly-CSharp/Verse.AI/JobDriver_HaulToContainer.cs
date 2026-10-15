using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld;
using UnityEngine;

namespace Verse.AI;

public class JobDriver_HaulToContainer : JobDriver, IBuildableDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass22_0
	{
		public Thing nextTarget;

		internal bool _003CMakeNewToils_003Eb__8(LocalTargetInfo target)
		{
			return target.Thing == nextTarget;
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass22_1
	{
		public Thing destThing;

		public JobDriver_HaulToContainer _003C_003E4__this;

		internal void _003CMakeNewToils_003Eb__9(int delta)
		{
			if (_003C_003E4__this.pawn.IsHashIntervalTick(80, delta) && destThing is Building_Grave && _003C_003E4__this.graveDigEffect == null)
			{
				_003C_003E4__this.graveDigEffect = EffecterDefOf.BuryPawn.Spawn();
				_003C_003E4__this.graveDigEffect.Trigger(destThing, destThing);
			}
		}

		internal void _003CMakeNewToils_003Eb__10()
		{
			_003C_003E4__this.graveDigEffect?.EffectTick(destThing, destThing);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__22 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_HaulToContainer _003C_003E4__this;

		private Toil _003CgetToHaulTarget_003E5__2;

		private Toil _003CuninstallIfMinifiable_003E5__3;

		private Toil _003CstartCarryingThing_003E5__4;

		private Toil _003CjumpIfAlsoCollectingNextTarget_003E5__5;

		private Toil _003CcarryToContainer_003E5__6;

		Toil IEnumerator<Toil>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CMakeNewToils_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003CgetToHaulTarget_003E5__2 = null;
			_003CuninstallIfMinifiable_003E5__3 = null;
			_003CstartCarryingThing_003E5__4 = null;
			_003CjumpIfAlsoCollectingNextTarget_003E5__5 = null;
			_003CcarryToContainer_003E5__6 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_HaulToContainer CS_0024_003C_003E8__locals54 = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				CS_0024_003C_003E8__locals54.FailOnDestroyedOrNull(TargetIndex.A);
				CS_0024_003C_003E8__locals54.FailOn(delegate
				{
					Thing thing = CS_0024_003C_003E8__locals54.GetActor().jobs.curJob.GetTarget(TargetIndex.B).Thing;
					Thing thing2 = CS_0024_003C_003E8__locals54.GetActor().jobs.curJob.GetTarget(TargetIndex.C).Thing;
					if (thing == null)
					{
						return true;
					}
					if (thing2 != null && thing2.Destroyed && !CS_0024_003C_003E8__locals54.TryReplaceWithFrame(TargetIndex.C))
					{
						CS_0024_003C_003E8__locals54.job.SetTarget(TargetIndex.C, null);
					}
					if (!thing.Spawned || (thing.Destroyed && !CS_0024_003C_003E8__locals54.TryReplaceWithFrame(TargetIndex.B)))
					{
						_003C_003Ec__DisplayClass22_0 CS_0024_003C_003E8__locals35 = new _003C_003Ec__DisplayClass22_0();
						if (CS_0024_003C_003E8__locals54.job.targetQueueB.NullOrEmpty())
						{
							return true;
						}
						if (!Toils_Haul.TryGetNextDestinationFromQueue(TargetIndex.C, TargetIndex.B, CS_0024_003C_003E8__locals54.ThingDef, CS_0024_003C_003E8__locals54.job, CS_0024_003C_003E8__locals54.pawn, out CS_0024_003C_003E8__locals35.nextTarget))
						{
							return true;
						}
						CS_0024_003C_003E8__locals54.job.targetQueueB.RemoveAll((LocalTargetInfo target) => target.Thing == CS_0024_003C_003E8__locals35.nextTarget);
						CS_0024_003C_003E8__locals54.job.targetB = CS_0024_003C_003E8__locals35.nextTarget;
					}
					ThingOwner thingOwner = CS_0024_003C_003E8__locals54.Container.TryGetInnerInteractableThingOwner();
					if (thingOwner != null && !thingOwner.CanAcceptAnyOf(CS_0024_003C_003E8__locals54.ThingToCarry))
					{
						return true;
					}
					return (CS_0024_003C_003E8__locals54.Container is IHaulDestination haulDestination && !haulDestination.Accepts(CS_0024_003C_003E8__locals54.ThingToCarry)) ? true : false;
				});
				CS_0024_003C_003E8__locals54.FailOnForbidden(TargetIndex.B);
				CS_0024_003C_003E8__locals54.FailOn(() => EnterPortalUtility.WasLoadingCanceled(CS_0024_003C_003E8__locals54.Container));
				CS_0024_003C_003E8__locals54.FailOn(() => TransporterUtility.WasLoadingCanceled(CS_0024_003C_003E8__locals54.Container));
				CS_0024_003C_003E8__locals54.FailOn(() => CompBiosculpterPod.WasLoadingCanceled(CS_0024_003C_003E8__locals54.Container));
				CS_0024_003C_003E8__locals54.FailOn(() => Building_SubcoreScanner.WasLoadingCancelled(CS_0024_003C_003E8__locals54.Container));
				_003CgetToHaulTarget_003E5__2 = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch, canGotoSpawnedParent: true).FailOn(() => CS_0024_003C_003E8__locals54.ThingToCarry.ParentHolder is MinifiedThing).FailOnSelfAndParentsDespawnedOrNull(TargetIndex.A);
				_003CuninstallIfMinifiable_003E5__3 = Toils_Construct.UninstallIfMinifiable(TargetIndex.A).FailOnSomeonePhysicallyInteracting(TargetIndex.A).FailOn(() => CS_0024_003C_003E8__locals54.ThingToCarry.ParentHolder is MinifiedThing)
					.FailOnSelfAndParentsDespawnedOrNull(TargetIndex.A)
					.FailOnDestroyedOrNull(TargetIndex.A);
				_003CstartCarryingThing_003E5__4 = Toils_Haul.StartCarryThing(TargetIndex.A, putRemainderInQueue: false, subtractNumTakenFromJobCount: true, failIfStackCountLessThanJobCount: false, reserve: true, canTakeFromInventory: true);
				_003CjumpIfAlsoCollectingNextTarget_003E5__5 = Toils_Haul.JumpIfAlsoCollectingNextTargetInQueue(_003CgetToHaulTarget_003E5__2, TargetIndex.A);
				_003CcarryToContainer_003E5__6 = ((CS_0024_003C_003E8__locals54.TargetThingB is Blueprint || CS_0024_003C_003E8__locals54.TargetThingB is Frame) ? Toils_Goto.GotoBuild(TargetIndex.B) : Toils_Haul.CarryHauledThingToContainer());
				_003C_003E2__current = Toils_Jump.JumpIf(_003CjumpIfAlsoCollectingNextTarget_003E5__5, () => CS_0024_003C_003E8__locals54.pawn.IsCarryingThing(CS_0024_003C_003E8__locals54.ThingToCarry));
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CgetToHaulTarget_003E5__2;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CuninstallIfMinifiable_003E5__3;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CstartCarryingThing_003E5__4;
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CjumpIfAlsoCollectingNextTarget_003E5__5;
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003CcarryToContainer_003E5__6;
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.MoveOffTargetBlueprint(TargetIndex.B);
				_003C_003E1__state = 7;
				return true;
			case 7:
			{
				_003C_003E1__state = -1;
				_003C_003Ec__DisplayClass22_1 CS_0024_003C_003E8__locals17 = new _003C_003Ec__DisplayClass22_1
				{
					_003C_003E4__this = CS_0024_003C_003E8__locals54
				};
				Toil toil = Toils_General.Wait(CS_0024_003C_003E8__locals54.Duration, TargetIndex.B);
				toil.WithProgressBarToilDelay(TargetIndex.B);
				EffecterDef workEffecter = CS_0024_003C_003E8__locals54.WorkEffecter;
				if (workEffecter != null)
				{
					toil.WithEffect(workEffecter, TargetIndex.B, null);
				}
				SoundDef workSustainer = CS_0024_003C_003E8__locals54.WorkSustainer;
				if (workSustainer != null)
				{
					toil.PlaySustainerOrSound(workSustainer);
				}
				CS_0024_003C_003E8__locals17.destThing = CS_0024_003C_003E8__locals54.job.GetTarget(TargetIndex.B).Thing;
				toil.tickIntervalAction = delegate(int delta)
				{
					if (CS_0024_003C_003E8__locals17._003C_003E4__this.pawn.IsHashIntervalTick(80, delta) && CS_0024_003C_003E8__locals17.destThing is Building_Grave && CS_0024_003C_003E8__locals17._003C_003E4__this.graveDigEffect == null)
					{
						CS_0024_003C_003E8__locals17._003C_003E4__this.graveDigEffect = EffecterDefOf.BuryPawn.Spawn();
						CS_0024_003C_003E8__locals17._003C_003E4__this.graveDigEffect.Trigger(CS_0024_003C_003E8__locals17.destThing, CS_0024_003C_003E8__locals17.destThing);
					}
				};
				toil.tickAction = delegate
				{
					CS_0024_003C_003E8__locals17._003C_003E4__this.graveDigEffect?.EffectTick(CS_0024_003C_003E8__locals17.destThing, CS_0024_003C_003E8__locals17.destThing);
				};
				CS_0024_003C_003E8__locals54.ModifyPrepareToil(toil);
				_003C_003E2__current = toil;
				_003C_003E1__state = 8;
				return true;
			}
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Construct.MakeSolidThingFromBlueprintIfNecessary(TargetIndex.B, TargetIndex.C);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.DepositHauledThingInContainer(TargetIndex.B, TargetIndex.C);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.JumpToCarryToNextContainerIfPossible(_003CcarryToContainer_003E5__6, TargetIndex.C);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				return false;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}

		[DebuggerHidden]
		IEnumerator<Toil> IEnumerable<Toil>.GetEnumerator()
		{
			_003CMakeNewToils_003Ed__22 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__22(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			return result;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Toil>)this).GetEnumerator();
		}
	}

	private Effecter graveDigEffect;

	protected const TargetIndex CarryThingIndex = TargetIndex.A;

	public const TargetIndex DestIndex = TargetIndex.B;

	protected const TargetIndex PrimaryDestIndex = TargetIndex.C;

	protected const int DiggingEffectInterval = 80;

	public Thing ThingToCarry => (Thing)job.GetTarget(TargetIndex.A);

	public Thing Container => (Thing)job.GetTarget(TargetIndex.B);

	public ThingDef ThingDef => ThingToCarry.def;

	protected virtual int Duration
	{
		get
		{
			if (Container == null || !(Container is Building building))
			{
				return 0;
			}
			return building.HaulToContainerDuration(ThingToCarry);
		}
	}

	protected virtual EffecterDef WorkEffecter => null;

	protected virtual SoundDef WorkSustainer => null;

	public bool TryGetBuildableRect(out CellRect rect)
	{
		if (Container is Blueprint)
		{
			rect = Container.OccupiedRect();
			return true;
		}
		rect = default(CellRect);
		return false;
	}

	public override string GetReport()
	{
		Thing thing = ((pawn.CurJob != job || pawn.carryTracker.CarriedThing == null) ? base.TargetThingA : pawn.carryTracker.CarriedThing);
		if (thing == null || !job.targetB.HasThing)
		{
			return "ReportHaulingUnknown".Translate();
		}
		return ((job.GetTarget(TargetIndex.B).Thing is Building_Grave) ? "ReportHaulingToGrave" : "ReportHaulingTo").Translate(thing.Label, job.targetB.Thing.LabelShort.Named("DESTINATION"), thing.Named("THING"));
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		if (!pawn.Reserve(job.GetTarget(TargetIndex.A), job, 1, -1, null, errorOnFailed))
		{
			return false;
		}
		if (Container.Isnt<IHaulEnroute>())
		{
			if (!pawn.Reserve(job.GetTarget(TargetIndex.B), job, 1, 1, null, errorOnFailed))
			{
				return false;
			}
			pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.B), job);
		}
		UpdateEnrouteTrackers();
		pawn.ReserveAsManyAsPossible(job.GetTargetQueue(TargetIndex.A), job);
		return true;
	}

	protected virtual void ModifyPrepareToil(Toil toil)
	{
	}

	private bool TryReplaceWithFrame(TargetIndex index)
	{
		Thing thing = GetActor().jobs.curJob.GetTarget(index).Thing;
		Building edifice = thing.Position.GetEdifice(pawn.Map);
		if (edifice != null && thing is Blueprint_Build blueprint_Build && edifice is Frame frame && frame.BuildDef == blueprint_Build.BuildDef)
		{
			job.SetTarget(TargetIndex.B, frame);
			return true;
		}
		return false;
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__22))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__22(-2)
		{
			_003C_003E4__this = this
		};
	}

	private void UpdateEnrouteTrackers()
	{
		int count = job.count;
		TryReserveEnroute(base.TargetThingC, ref count);
		if (base.TargetB != base.TargetC)
		{
			TryReserveEnroute(base.TargetThingB, ref count);
		}
		if (job.targetQueueB == null)
		{
			return;
		}
		foreach (LocalTargetInfo item in job.targetQueueB)
		{
			if (!base.TargetC.HasThing || !(item == base.TargetThingC))
			{
				TryReserveEnroute(item.Thing, ref count);
			}
		}
	}

	private void TryReserveEnroute(Thing thing, ref int count)
	{
		if (thing is IHaulEnroute container && !thing.DestroyedOrNull())
		{
			UpdateTracker(container, ref count);
		}
	}

	private void UpdateTracker(IHaulEnroute container, ref int count)
	{
		if (!ThingToCarry.DestroyedOrNull())
		{
			if (job.playerForced && container.GetSpaceRemainingWithEnroute(ThingDef) == 0)
			{
				container.Map.enrouteManager.InterruptEnroutePawns(container, pawn);
			}
			int num = Mathf.Min(count, container.GetSpaceRemainingWithEnroute(ThingDef));
			if (num > 0)
			{
				container.Map.enrouteManager.AddEnroute(container, pawn, base.TargetThingA.def, num);
			}
			count -= num;
		}
	}
}
