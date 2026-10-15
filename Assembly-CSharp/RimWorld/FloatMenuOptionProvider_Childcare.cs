using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_Childcare : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_0
	{
		public FloatMenuContext context;
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_1
	{
		public Job job;

		public _003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals1;

		internal void _003CGetOptionsFor_003Eb__0()
		{
			CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_2
	{
		public Job job;

		public _003C_003Ec__DisplayClass9_0 CS_0024_003C_003E8__locals2;

		internal void _003CGetOptionsFor_003Eb__1()
		{
			job.count = 1;
			CS_0024_003C_003E8__locals2.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}
	}

	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__9 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		private Pawn clickedPawn;

		public Pawn _003C_003E3__clickedPawn;

		private _003C_003Ec__DisplayClass9_0 _003C_003E8__1;

		FloatMenuOption IEnumerator<FloatMenuOption>.Current
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
		public _003CGetOptionsFor_003Ed__9(int _003C_003E1__state)
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
			ChildcareUtility.BreastfeedFailReason? reason3;
			LocalTargetInfo targetB;
			_003C_003Ec__DisplayClass9_2 CS_0024_003C_003E8__locals16;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass9_0();
				_003C_003E8__1.context = context;
				if (!ChildcareUtility.CanSuckle(clickedPawn, out var reason))
				{
					return false;
				}
				if (ChildcareUtility.CanBreastfeed(_003C_003E8__1.context.FirstSelectedPawn, out reason))
				{
					if (!ChildcareUtility.HasBreastfeedCompatibleFactions(_003C_003E8__1.context.FirstSelectedPawn, clickedPawn))
					{
						return false;
					}
					if (!ChildcareUtility.CanMomAutoBreastfeedBabyNow(_003C_003E8__1.context.FirstSelectedPawn, clickedPawn, forced: true, out var reason2))
					{
						TaggedString taggedString = "BabyCareBreastfeedUnable".Translate(clickedPawn.Named("BABY")) + ": " + reason2.Value.Translate(_003C_003E8__1.context.FirstSelectedPawn, _003C_003E8__1.context.FirstSelectedPawn, clickedPawn).CapitalizeFirst();
						_003C_003E2__current = new FloatMenuOption(taggedString, null);
						_003C_003E1__state = 1;
						return true;
					}
					_003C_003Ec__DisplayClass9_1 CS_0024_003C_003E8__locals7 = new _003C_003Ec__DisplayClass9_1
					{
						CS_0024_003C_003E8__locals1 = _003C_003E8__1,
						job = ChildcareUtility.MakeBreastfeedJob(clickedPawn)
					};
					if (CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.curJob != null && CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.curJob.JobIsSameAs(CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, CS_0024_003C_003E8__locals7.job))
					{
						_003C_003E2__current = new FloatMenuOption("AlreadyBreastfeeding".Translate(clickedPawn.Named("BABY")), null);
						_003C_003E1__state = 2;
						return true;
					}
					_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("BabyCareBreastfeed".Translate(clickedPawn.Named("BABY")), delegate
					{
						CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(CS_0024_003C_003E8__locals7.job, JobTag.Misc);
					}), CS_0024_003C_003E8__locals7.CS_0024_003C_003E8__locals1.context.FirstSelectedPawn, clickedPawn);
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_02a7;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_02a7;
			case 2:
				_003C_003E1__state = -1;
				goto IL_02a7;
			case 3:
				_003C_003E1__state = -1;
				goto IL_02a7;
			case 4:
				_003C_003E1__state = -1;
				goto IL_04bc;
			case 5:
				_003C_003E1__state = -1;
				goto IL_04bc;
			case 6:
				_003C_003E1__state = -1;
				goto IL_04bc;
			case 7:
				_003C_003E1__state = -1;
				return false;
			case 8:
				_003C_003E1__state = -1;
				break;
			case 9:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_04bc:
				return false;
				IL_02a7:
				if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(clickedPawn))
				{
					return false;
				}
				if (!ChildcareUtility.CanHaulBabyNow(_003C_003E8__1.context.FirstSelectedPawn, clickedPawn, ignoreOtherReservations: false, out reason3))
				{
					if (_003C_003E8__1.context.FirstSelectedPawn.MapHeld.reservationManager.TryGetReserver(clickedPawn, _003C_003E8__1.context.FirstSelectedPawn.Faction, out var reserver))
					{
						_003C_003E2__current = new FloatMenuOption($"{"CannotCarryToSafePlace".Translate()}: {clickedPawn.LabelShort} {"ReservedBy".Translate(reserver.LabelShort, reserver).Resolve().StripTags()}", null);
						_003C_003E1__state = 4;
						return true;
					}
					if (reason3.HasValue && reason3 == ChildcareUtility.BreastfeedFailReason.HaulerCannotReachBaby)
					{
						_003C_003E2__current = new FloatMenuOption($"{"CannotCarryToSafePlace".Translate()}: {"NoPath".Translate().CapitalizeFirst()}", null);
						_003C_003E1__state = 5;
						return true;
					}
					_003C_003E2__current = new FloatMenuOption($"{"CannotCarryToSafePlace".Translate()}: {"Incapable".Translate().CapitalizeFirst()}", null);
					_003C_003E1__state = 6;
					return true;
				}
				targetB = ChildcareUtility.SafePlaceForBaby(clickedPawn, _003C_003E8__1.context.FirstSelectedPawn);
				if (!targetB.IsValid)
				{
					return false;
				}
				if (targetB.Thing is Building_Bed building_Bed)
				{
					if (clickedPawn.CurrentBed() == building_Bed)
					{
						return false;
					}
				}
				else if (clickedPawn.Spawned && clickedPawn.Position == targetB.Cell)
				{
					_003C_003E2__current = new FloatMenuOption($"{"CannotCarryToSafePlace".Translate()}: {"NoBetterSafePlace".Translate().CapitalizeFirst()}", null);
					_003C_003E1__state = 7;
					return true;
				}
				CS_0024_003C_003E8__locals16 = new _003C_003Ec__DisplayClass9_2
				{
					CS_0024_003C_003E8__locals2 = _003C_003E8__1,
					job = JobMaker.MakeJob(JobDefOf.BringBabyToSafety, clickedPawn, targetB)
				};
				if (CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals2.context.FirstSelectedPawn.jobs.curJob != null && CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals2.context.FirstSelectedPawn.jobs.curJob.JobIsSameAs(CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals2.context.FirstSelectedPawn, CS_0024_003C_003E8__locals16.job))
				{
					_003C_003E2__current = new FloatMenuOption("AlreadyCarryingToSafePlace".Translate(clickedPawn.Named("BABY")), null);
					_003C_003E1__state = 8;
					return true;
				}
				_003C_003E2__current = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("CarryToSafePlace".Translate(clickedPawn.Named("BABY")), delegate
				{
					CS_0024_003C_003E8__locals16.job.count = 1;
					CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals2.context.FirstSelectedPawn.jobs.TryTakeOrderedJob(CS_0024_003C_003E8__locals16.job, JobTag.Misc);
				}), CS_0024_003C_003E8__locals16.CS_0024_003C_003E8__locals2.context.FirstSelectedPawn, clickedPawn);
				_003C_003E1__state = 9;
				return true;
			}
			return false;
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
		IEnumerator<FloatMenuOption> IEnumerable<FloatMenuOption>.GetEnumerator()
		{
			_003CGetOptionsFor_003Ed__9 _003CGetOptionsFor_003Ed__;
			if (_003C_003E1__state == -2 && _003C_003El__initialThreadId == Environment.CurrentManagedThreadId)
			{
				_003C_003E1__state = 0;
				_003CGetOptionsFor_003Ed__ = this;
			}
			else
			{
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__9(0);
			}
			_003CGetOptionsFor_003Ed__.clickedPawn = _003C_003E3__clickedPawn;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected override bool Drafted => false;

	protected override bool Undrafted => true;

	protected override bool Multiselect => false;

	public override bool CanTargetDespawned => true;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		return ModsConfig.BiotechActive;
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__9))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__9(-2)
		{
			_003C_003E3__clickedPawn = clickedPawn,
			_003C_003E3__context = context
		};
	}
}
