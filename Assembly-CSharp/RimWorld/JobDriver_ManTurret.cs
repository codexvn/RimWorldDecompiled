using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld;

public class JobDriver_ManTurret : JobDriver
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass13_0
	{
		public Toil refuelIfNeeded;

		public JobDriver_ManTurret _003C_003E4__this;

		public Toil gotoTurret;

		public Toil loadIfNeeded;

		public Toil loadShell;

		public Toil man;

		internal void _003CMakeNewToils_003Eb__0()
		{
			Pawn actor = refuelIfNeeded.actor;
			Building building = (Building)actor.CurJob.targetA.Thing;
			Building_TurretGun building_TurretGun = building as Building_TurretGun;
			if (!GunNeedsRefueling(building))
			{
				_003C_003E4__this.JumpToToil(gotoTurret);
				return;
			}
			Thing thing = FindFuelForTurret(_003C_003E4__this.pawn, building_TurretGun);
			if (thing == null)
			{
				CompRefuelable compRefuelable = building.TryGetComp<CompRefuelable>();
				if (actor.Faction == Faction.OfPlayer && compRefuelable != null)
				{
					Messages.Message("MessageOutOfNearbyFuelFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN"), compRefuelable.Props.fuelFilter.Summary.Named("FUEL")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent);
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			actor.CurJob.targetB = thing;
			actor.CurJob.count = 1;
		}

		internal void _003CMakeNewToils_003Eb__1()
		{
			Pawn actor = loadIfNeeded.actor;
			Building obj = (Building)actor.CurJob.targetA.Thing;
			Building_TurretGun building_TurretGun = obj as Building_TurretGun;
			if (!GunNeedsLoading(obj))
			{
				_003C_003E4__this.JumpToToil(gotoTurret);
				return;
			}
			Thing thing = FindAmmoForTurret(_003C_003E4__this.pawn, building_TurretGun);
			if (thing == null)
			{
				if (actor.Faction == Faction.OfPlayer)
				{
					Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor.LabelShort, building_TurretGun.Label, actor.Named("PAWN"), building_TurretGun.Named("GUN")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent);
				}
				actor.jobs.EndCurrentJob(JobCondition.Incompletable);
			}
			actor.CurJob.targetB = thing;
			actor.CurJob.count = 1;
		}

		internal void _003CMakeNewToils_003Eb__2()
		{
			Pawn actor = loadShell.actor;
			SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(_003C_003E4__this.Turret.Position, _003C_003E4__this.Turret.Map));
			_003C_003E4__this.Turret.gun.TryGetComp<CompChangeableProjectile>().LoadShell(_003C_003E4__this.Hauling.def, 1);
			actor.carryTracker.innerContainer.ClearAndDestroyContents();
		}

		internal void _003CMakeNewToils_003Eb__3()
		{
			Pawn actor = man.actor;
			Building building = (Building)actor.CurJob.targetA.Thing;
			if (GunNeedsLoading(building))
			{
				_003C_003E4__this.JumpToToil(loadIfNeeded);
				return;
			}
			if (GunNeedsRefueling(building))
			{
				_003C_003E4__this.JumpToToil(refuelIfNeeded);
				return;
			}
			building.GetComp<CompMannable>().ManForATick(actor);
			man.actor.rotationTracker.FaceCell(building.Position);
		}
	}

	[CompilerGenerated]
	private sealed class _003CMakeNewToils_003Ed__13 : IEnumerable<Toil>, IEnumerable, IEnumerator<Toil>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private Toil _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		public JobDriver_ManTurret _003C_003E4__this;

		private _003C_003Ec__DisplayClass13_0 _003C_003E8__1;

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
		public _003CMakeNewToils_003Ed__13(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
			_003C_003El__initialThreadId = Environment.CurrentManagedThreadId;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E8__1 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			JobDriver_ManTurret f = _003C_003E4__this;
			switch (num)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass13_0();
				_003C_003E8__1._003C_003E4__this = _003C_003E4__this;
				f.FailOnDespawnedNullOrForbidden(TargetIndex.A);
				_003C_003E8__1.gotoTurret = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E8__1.refuelIfNeeded = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.refuelIfNeeded.initAction = delegate
				{
					Pawn actor4 = _003C_003E8__1.refuelIfNeeded.actor;
					Building building2 = (Building)actor4.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun2 = building2 as Building_TurretGun;
					if (!GunNeedsRefueling(building2))
					{
						_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.gotoTurret);
					}
					else
					{
						Thing thing2 = FindFuelForTurret(_003C_003E8__1._003C_003E4__this.pawn, building_TurretGun2);
						if (thing2 == null)
						{
							CompRefuelable compRefuelable = building2.TryGetComp<CompRefuelable>();
							if (actor4.Faction == Faction.OfPlayer && compRefuelable != null)
							{
								Messages.Message("MessageOutOfNearbyFuelFor".Translate(actor4.LabelShort, building_TurretGun2.Label, actor4.Named("PAWN"), building_TurretGun2.Named("GUN"), compRefuelable.Props.fuelFilter.Summary.Named("FUEL")).CapitalizeFirst(), building_TurretGun2, MessageTypeDefOf.NegativeEvent);
							}
							actor4.jobs.EndCurrentJob(JobCondition.Incompletable);
						}
						actor4.CurJob.targetB = thing2;
						actor4.CurJob.count = 1;
					}
				};
				_003C_003E2__current = _003C_003E8__1.refuelIfNeeded;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B, 10, 1);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
				_003C_003E1__state = 4;
				return true;
			case 4:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 5;
				return true;
			case 5:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_General.Wait(240).FailOnDestroyedNullOrForbidden(TargetIndex.B).FailOnDestroyedNullOrForbidden(TargetIndex.A)
					.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch)
					.WithProgressBarToilDelay(TargetIndex.A);
				_003C_003E1__state = 6;
				return true;
			case 6:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Refuel.FinalizeRefueling(TargetIndex.A, TargetIndex.B);
				_003C_003E1__state = 7;
				return true;
			case 7:
				_003C_003E1__state = -1;
				_003C_003E8__1.loadIfNeeded = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.loadIfNeeded.initAction = delegate
				{
					Pawn actor3 = _003C_003E8__1.loadIfNeeded.actor;
					Building obj = (Building)actor3.CurJob.targetA.Thing;
					Building_TurretGun building_TurretGun = obj as Building_TurretGun;
					if (!GunNeedsLoading(obj))
					{
						_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.gotoTurret);
					}
					else
					{
						Thing thing = FindAmmoForTurret(_003C_003E8__1._003C_003E4__this.pawn, building_TurretGun);
						if (thing == null)
						{
							if (actor3.Faction == Faction.OfPlayer)
							{
								Messages.Message("MessageOutOfNearbyShellsFor".Translate(actor3.LabelShort, building_TurretGun.Label, actor3.Named("PAWN"), building_TurretGun.Named("GUN")).CapitalizeFirst(), building_TurretGun, MessageTypeDefOf.NegativeEvent);
							}
							actor3.jobs.EndCurrentJob(JobCondition.Incompletable);
						}
						actor3.CurJob.targetB = thing;
						actor3.CurJob.count = 1;
					}
				};
				_003C_003E2__current = _003C_003E8__1.loadIfNeeded;
				_003C_003E1__state = 8;
				return true;
			case 8:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Reserve.Reserve(TargetIndex.B, 10, 1);
				_003C_003E1__state = 9;
				return true;
			case 9:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell).FailOnSomeonePhysicallyInteracting(TargetIndex.B);
				_003C_003E1__state = 10;
				return true;
			case 10:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Haul.StartCarryThing(TargetIndex.B);
				_003C_003E1__state = 11;
				return true;
			case 11:
				_003C_003E1__state = -1;
				_003C_003E2__current = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch);
				_003C_003E1__state = 12;
				return true;
			case 12:
				_003C_003E1__state = -1;
				_003C_003E8__1.loadShell = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.loadShell.initAction = delegate
				{
					Pawn actor2 = _003C_003E8__1.loadShell.actor;
					SoundDefOf.Artillery_ShellLoaded.PlayOneShot(new TargetInfo(_003C_003E8__1._003C_003E4__this.Turret.Position, _003C_003E8__1._003C_003E4__this.Turret.Map));
					_003C_003E8__1._003C_003E4__this.Turret.gun.TryGetComp<CompChangeableProjectile>().LoadShell(_003C_003E8__1._003C_003E4__this.Hauling.def, 1);
					actor2.carryTracker.innerContainer.ClearAndDestroyContents();
				};
				_003C_003E2__current = _003C_003E8__1.loadShell;
				_003C_003E1__state = 13;
				return true;
			case 13:
				_003C_003E1__state = -1;
				_003C_003E2__current = _003C_003E8__1.gotoTurret;
				_003C_003E1__state = 14;
				return true;
			case 14:
				_003C_003E1__state = -1;
				_003C_003E8__1.man = ToilMaker.MakeToil("MakeNewToils");
				_003C_003E8__1.man.tickAction = delegate
				{
					Pawn actor = _003C_003E8__1.man.actor;
					Building building = (Building)actor.CurJob.targetA.Thing;
					if (GunNeedsLoading(building))
					{
						_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.loadIfNeeded);
					}
					else if (GunNeedsRefueling(building))
					{
						_003C_003E8__1._003C_003E4__this.JumpToToil(_003C_003E8__1.refuelIfNeeded);
					}
					else
					{
						building.GetComp<CompMannable>().ManForATick(actor);
						_003C_003E8__1.man.actor.rotationTracker.FaceCell(building.Position);
					}
				};
				_003C_003E8__1.man.handlingFacing = true;
				_003C_003E8__1.man.defaultCompleteMode = ToilCompleteMode.Never;
				_003C_003E8__1.man.FailOnCannotTouch(TargetIndex.A, PathEndMode.InteractionCell);
				_003C_003E2__current = _003C_003E8__1.man;
				_003C_003E1__state = 15;
				return true;
			case 15:
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
			_003CMakeNewToils_003Ed__13 result;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				result = this;
			}
			else
			{
				result = new _003CMakeNewToils_003Ed__13(0)
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

	private const float SearchRadius = 40f;

	private const int MaxPawnReservations = 10;

	private const TargetIndex TurretInd = TargetIndex.A;

	private const TargetIndex HaulingInd = TargetIndex.B;

	private Building_TurretGun Turret => (Building_TurretGun)job.GetTarget(TargetIndex.A).Thing;

	private Thing Hauling => job.GetTarget(TargetIndex.B).Thing;

	private static bool GunNeedsLoading(Building b)
	{
		if (!(b is Building_TurretGun building_TurretGun))
		{
			return false;
		}
		CompChangeableProjectile compChangeableProjectile = building_TurretGun.gun.TryGetComp<CompChangeableProjectile>();
		if (compChangeableProjectile == null || compChangeableProjectile.Loaded)
		{
			return false;
		}
		return true;
	}

	private static bool GunNeedsRefueling(Building b)
	{
		if (!(b is Building_TurretGun thing))
		{
			return false;
		}
		CompRefuelable compRefuelable = thing.TryGetComp<CompRefuelable>();
		if (compRefuelable == null || compRefuelable.HasFuel || !compRefuelable.Props.fuelIsMortarBarrel || Find.Storyteller.difficulty.classicMortars)
		{
			return false;
		}
		return true;
	}

	public static Thing FindAmmoForTurret(Pawn pawn, Building_TurretGun gun)
	{
		StorageSettings allowedShellsSettings = ((pawn.IsColonist || pawn.IsColonyMech) ? gun.gun.TryGetComp<CompChangeableProjectile>().allowedShellsSettings : null);
		return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.Shell), PathEndMode.OnCell, TraverseParms.For(pawn), 40f, ShellValidator);
		bool ShellValidator(Thing t)
		{
			if (t.IsForbidden(pawn))
			{
				return false;
			}
			if (!pawn.CanReserve(t, 10, 1))
			{
				return false;
			}
			if (allowedShellsSettings != null && !allowedShellsSettings.AllowedToAccept(t))
			{
				return false;
			}
			if (pawn.Faction != Faction.OfPlayer && t.def.projectileWhenLoaded?.projectile != null && !t.def.projectileWhenLoaded.projectile.damageDef.harmsHealth)
			{
				return false;
			}
			return true;
		}
	}

	public static Thing FindFuelForTurret(Pawn pawn, Building_TurretGun gun)
	{
		CompRefuelable refuelableComp = gun.TryGetComp<CompRefuelable>();
		if (refuelableComp == null)
		{
			return null;
		}
		return GenClosest.ClosestThingReachable(gun.Position, gun.Map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), PathEndMode.OnCell, TraverseParms.For(pawn), 40f, FuelValidator);
		bool FuelValidator(Thing t)
		{
			if (t.IsForbidden(pawn) || !pawn.CanReserve(t, 10, 1))
			{
				return false;
			}
			return refuelableComp.Props.fuelFilter.Allows(t);
		}
	}

	public override bool TryMakePreToilReservations(bool errorOnFailed)
	{
		return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
	}

	[IteratorStateMachine(typeof(_003CMakeNewToils_003Ed__13))]
	protected override IEnumerable<Toil> MakeNewToils()
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CMakeNewToils_003Ed__13(-2)
		{
			_003C_003E4__this = this
		};
	}
}
