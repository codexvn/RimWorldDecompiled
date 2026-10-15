using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld;

public class FloatMenuOptionProvider_CarryingPawn : FloatMenuOptionProvider
{
	[CompilerGenerated]
	private sealed class _003CGetOptionsFor_003Ed__9 : IEnumerable<FloatMenuOption>, IEnumerable, IEnumerator<FloatMenuOption>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private FloatMenuOption _003C_003E2__current;

		private int _003C_003El__initialThreadId;

		private FloatMenuContext context;

		public FloatMenuContext _003C_003E3__context;

		public FloatMenuOptionProvider_CarryingPawn _003C_003E4__this;

		private Thing clickedThing;

		public Thing _003C_003E3__clickedThing;

		private Pawn _003CcarriedPawn_003E5__2;

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
			_003CcarriedPawn_003E5__2 = null;
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			FloatMenuOptionProvider_CarryingPawn floatMenuOptionProvider_CarryingPawn = _003C_003E4__this;
			FloatMenuOption option3;
			FloatMenuOption option4;
			FloatMenuOption option5;
			switch (num)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003CcarriedPawn_003E5__2 = (Pawn)context.FirstSelectedPawn.carryTracker.CarriedThing;
				if (floatMenuOptionProvider_CarryingPawn.Capture(clickedThing, context, _003CcarriedPawn_003E5__2, out var option))
				{
					_003C_003E2__current = option;
					_003C_003E1__state = 1;
					return true;
				}
				if (floatMenuOptionProvider_CarryingPawn.CarryToBed(clickedThing, context, _003CcarriedPawn_003E5__2, out var option2))
				{
					_003C_003E2__current = option2;
					_003C_003E1__state = 2;
					return true;
				}
				goto IL_00be;
			}
			case 1:
				_003C_003E1__state = -1;
				goto IL_00be;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00be;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00f1;
			case 4:
				_003C_003E1__state = -1;
				goto IL_0125;
			case 5:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0125:
				if (ModsConfig.AnomalyActive && floatMenuOptionProvider_CarryingPawn.CaptureEntity(clickedThing, context, _003CcarriedPawn_003E5__2, out option3))
				{
					_003C_003E2__current = option3;
					_003C_003E1__state = 5;
					return true;
				}
				break;
				IL_00f1:
				if (floatMenuOptionProvider_CarryingPawn.CarryToCasket(clickedThing, context, _003CcarriedPawn_003E5__2, out option4))
				{
					_003C_003E2__current = option4;
					_003C_003E1__state = 4;
					return true;
				}
				goto IL_0125;
				IL_00be:
				if (floatMenuOptionProvider_CarryingPawn.CarryToTransporter(clickedThing, context, _003CcarriedPawn_003E5__2, out option5))
				{
					_003C_003E2__current = option5;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00f1;
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
				_003CGetOptionsFor_003Ed__ = new _003CGetOptionsFor_003Ed__9(0)
				{
					_003C_003E4__this = _003C_003E4__this
				};
			}
			_003CGetOptionsFor_003Ed__.clickedThing = _003C_003E3__clickedThing;
			_003CGetOptionsFor_003Ed__.context = _003C_003E3__context;
			return _003CGetOptionsFor_003Ed__;
		}

		[DebuggerHidden]
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<FloatMenuOption>)this).GetEnumerator();
		}
	}

	protected override bool Drafted => true;

	protected override bool Undrafted => false;

	protected override bool Multiselect => false;

	protected override bool RequiresManipulation => true;

	protected override bool AppliesInt(FloatMenuContext context)
	{
		return context.FirstSelectedPawn.IsCarryingPawn();
	}

	[IteratorStateMachine(typeof(_003CGetOptionsFor_003Ed__9))]
	public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CGetOptionsFor_003Ed__9(-2)
		{
			_003C_003E4__this = this,
			_003C_003E3__clickedThing = clickedThing,
			_003C_003E3__context = context
		};
	}

	private bool CarryToBed(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
	{
		option = null;
		if (carriedPawn.IsPrisonerOfColony)
		{
			return false;
		}
		if (!RestUtility.IsValidBedFor(clickedThing, carriedPawn, context.FirstSelectedPawn, checkSocialProperness: false, allowMedBedEvenIfSetToNoCare: true, ignoreOtherReservations: true, carriedPawn.GuestStatus))
		{
			return false;
		}
		if (context.FirstSelectedPawn.HostileTo(carriedPawn))
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "CarriedPawnHostile".Translate().CapitalizeFirst(), null);
			return true;
		}
		if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
			return true;
		}
		option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, clickedThing), delegate
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(JobDefOf.TakeDownedPawnToBedDrafted, context.FirstSelectedPawn.carryTracker.CarriedThing, clickedThing);
			job.count = 1;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), context.FirstSelectedPawn, clickedThing);
		return true;
	}

	private bool Capture(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
	{
		option = null;
		if (!carriedPawn.CanBeCaptured())
		{
			return false;
		}
		if (!RestUtility.IsValidBedFor(clickedThing, carriedPawn, context.FirstSelectedPawn, checkSocialProperness: false, allowMedBedEvenIfSetToNoCare: true, ignoreOtherReservations: true, GuestStatus.Prisoner))
		{
			return false;
		}
		if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
			return true;
		}
		TaggedString taggedString = "Capture".Translate(carriedPawn.LabelCap, carriedPawn);
		if (!carriedPawn.guest.Recruitable)
		{
			taggedString += $" ({"Unrecruitable".Translate()})";
		}
		if (carriedPawn.Faction != null && carriedPawn.Faction != Faction.OfPlayer && !carriedPawn.Faction.Hidden && !carriedPawn.Faction.HostileTo(Faction.OfPlayer) && !carriedPawn.IsPrisonerOfColony)
		{
			taggedString += $": {"AngersFaction".Translate().CapitalizeFirst()}";
		}
		option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(JobDefOf.CarryToPrisonerBedDrafted, context.FirstSelectedPawn.carryTracker.CarriedThing, clickedThing);
			job.count = 1;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), context.FirstSelectedPawn, clickedThing);
		return true;
	}

	private bool CarryToTransporter(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
	{
		option = null;
		if (!clickedThing.TryGetComp(out CompTransporter transporter))
		{
			return false;
		}
		if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
			return true;
		}
		if (transporter.Shuttle != null)
		{
			if (!transporter.Shuttle.IsAllowedNow(carriedPawn))
			{
				return false;
			}
			if (!transporter.Shuttle.IsPlayerShuttle && !transporter.LeftToLoadContains(carriedPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NotPartOfLaunchGroup".Translate(), null);
				return true;
			}
		}
		option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, clickedThing), delegate
		{
			if (!transporter.LoadingInProgressOrReadyToLaunch)
			{
				TransporterUtility.InitiateLoading(Gen.YieldSingle(transporter));
			}
			Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, carriedPawn, clickedThing);
			job.ignoreForbidden = true;
			job.count = 1;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), context.FirstSelectedPawn, clickedThing);
		return true;
	}

	private bool CarryToCasket(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
	{
		option = null;
		Building_CryptosleepCasket casket = clickedThing as Building_CryptosleepCasket;
		if (casket == null)
		{
			return false;
		}
		if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
			return true;
		}
		if (casket.HasAnyContents)
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketOccupied".Translate(), null);
			return true;
		}
		if (carriedPawn.IsQuestLodger())
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestsNotAllowed".Translate(), null);
			return true;
		}
		if (carriedPawn.GetExtraHostFaction() != null)
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestPrisonersNotAllowed".Translate(), null);
			return true;
		}
		option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, casket), delegate
		{
			Job job = JobMaker.MakeJob(JobDefOf.CarryToCryptosleepCasketDrafted, carriedPawn, casket);
			job.count = 1;
			job.playerForced = true;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), context.FirstSelectedPawn, casket);
		return true;
	}

	private bool CaptureEntity(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
	{
		option = null;
		if (!carriedPawn.TryGetComp<CompHoldingPlatformTarget>(out var comp) || !comp.CanBeCaptured || !comp.StudiedAtHoldingPlatform)
		{
			return false;
		}
		if (!clickedThing.TryGetComp(out CompEntityHolder comp2) || !comp2.Available)
		{
			return false;
		}
		if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly))
		{
			option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null);
			return true;
		}
		TaggedString taggedString = "Capture".Translate(carriedPawn.LabelCap, carriedPawn);
		if (!clickedThing.SafelyContains(carriedPawn))
		{
			float statValue = carriedPawn.GetStatValue(StatDefOf.MinimumContainmentStrength);
			taggedString += $" ({"FloatMenuContainmentStrength".Translate().ToLower()} {comp2.ContainmentStrength:F0}, {"FloatMenuContainmentRequires".Translate(carriedPawn).ToLower()} {statValue:F0})";
		}
		option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate
		{
			clickedThing.SetForbidden(value: false, warnOnFail: false);
			Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolderAlreadyHolding, clickedThing, context.FirstSelectedPawn.carryTracker.CarriedThing);
			job.count = 1;
			context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}), context.FirstSelectedPawn, clickedThing);
		return true;
	}
}
