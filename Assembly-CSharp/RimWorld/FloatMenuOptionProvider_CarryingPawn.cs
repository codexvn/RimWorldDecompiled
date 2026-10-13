using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032C6 RID: 12998
	public class FloatMenuOptionProvider_CarryingPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002F48 RID: 12104
		// (get) Token: 0x060124C1 RID: 74945 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F49 RID: 12105
		// (get) Token: 0x060124C2 RID: 74946 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F4A RID: 12106
		// (get) Token: 0x060124C3 RID: 74947 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F4B RID: 12107
		// (get) Token: 0x060124C4 RID: 74948 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060124C5 RID: 74949 RVA: 0x00563D2B File Offset: 0x00561F2B
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return context.FirstSelectedPawn.IsCarryingPawn(null);
		}

		// Token: 0x060124C6 RID: 74950 RVA: 0x00563D39 File Offset: 0x00561F39
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Thing clickedThing, FloatMenuContext context)
		{
			Pawn carriedPawn = (Pawn)context.FirstSelectedPawn.carryTracker.CarriedThing;
			FloatMenuOption floatMenuOption;
			FloatMenuOption floatMenuOption2;
			if (this.Capture(clickedThing, context, carriedPawn, out floatMenuOption))
			{
				yield return floatMenuOption;
			}
			else if (this.CarryToBed(clickedThing, context, carriedPawn, out floatMenuOption2))
			{
				yield return floatMenuOption2;
			}
			FloatMenuOption floatMenuOption3;
			if (this.CarryToTransporter(clickedThing, context, carriedPawn, out floatMenuOption3))
			{
				yield return floatMenuOption3;
			}
			FloatMenuOption floatMenuOption4;
			if (this.CarryToCasket(clickedThing, context, carriedPawn, out floatMenuOption4))
			{
				yield return floatMenuOption4;
			}
			FloatMenuOption floatMenuOption5;
			if (ModsConfig.AnomalyActive && this.CaptureEntity(clickedThing, context, carriedPawn, out floatMenuOption5))
			{
				yield return floatMenuOption5;
			}
			yield break;
		}

		// Token: 0x060124C7 RID: 74951 RVA: 0x00563D58 File Offset: 0x00561F58
		private bool CarryToBed(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			if (carriedPawn.IsPrisonerOfColony)
			{
				return false;
			}
			if (!RestUtility.IsValidBedFor(clickedThing, carriedPawn, context.FirstSelectedPawn, false, true, true, carriedPawn.GuestStatus))
			{
				return false;
			}
			if (context.FirstSelectedPawn.HostileTo(carriedPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "CarriedPawnHostile".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, clickedThing), delegate()
			{
				clickedThing.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.TakeDownedPawnToBedDrafted, context.FirstSelectedPawn.carryTracker.CarriedThing, clickedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}

		// Token: 0x060124C8 RID: 74952 RVA: 0x00563EF4 File Offset: 0x005620F4
		private bool Capture(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			if (!carriedPawn.CanBeCaptured())
			{
				return false;
			}
			if (!RestUtility.IsValidBedFor(clickedThing, carriedPawn, context.FirstSelectedPawn, false, true, true, new GuestStatus?(GuestStatus.Prisoner)))
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			TaggedString taggedString = "Capture".Translate(carriedPawn.LabelCap, carriedPawn);
			if (!carriedPawn.guest.Recruitable)
			{
				taggedString += string.Format(" ({0})", "Unrecruitable".Translate());
			}
			if (carriedPawn.Faction != null && carriedPawn.Faction != Faction.OfPlayer && !carriedPawn.Faction.Hidden && !carriedPawn.Faction.HostileTo(Faction.OfPlayer) && !carriedPawn.IsPrisonerOfColony)
			{
				taggedString += string.Format(": {0}", "AngersFaction".Translate().CapitalizeFirst());
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				clickedThing.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.CarryToPrisonerBedDrafted, context.FirstSelectedPawn.carryTracker.CarriedThing, clickedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}

		// Token: 0x060124C9 RID: 74953 RVA: 0x005640B8 File Offset: 0x005622B8
		private bool CarryToTransporter(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			CompTransporter transporter;
			if (!clickedThing.TryGetComp(out transporter))
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
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
					option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NotPartOfLaunchGroup".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
					return true;
				}
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, clickedThing), delegate()
			{
				if (!transporter.LoadingInProgressOrReadyToLaunch)
				{
					TransporterUtility.InitiateLoading(Gen.YieldSingle<CompTransporter>(transporter));
				}
				Job job = JobMaker.MakeJob(JobDefOf.HaulToTransporter, carriedPawn, clickedThing);
				job.ignoreForbidden = true;
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}

		// Token: 0x060124CA RID: 74954 RVA: 0x00564284 File Offset: 0x00562484
		private bool CarryToCasket(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			Building_CryptosleepCasket casket = clickedThing as Building_CryptosleepCasket;
			if (casket == null)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (casket.HasAnyContents)
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketOccupied".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (carriedPawn.IsQuestLodger())
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestsNotAllowed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			if (carriedPawn.GetExtraHostFaction(null) != null)
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, casket) + ": " + "CryptosleepCasketGuestPrisonersNotAllowed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PlaceIn".Translate(carriedPawn, casket), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.CarryToCryptosleepCasketDrafted, carriedPawn, casket);
				job.count = 1;
				job.playerForced = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, casket, "ReservedBy", null);
			return true;
		}

		// Token: 0x060124CB RID: 74955 RVA: 0x005644C8 File Offset: 0x005626C8
		private bool CaptureEntity(Thing clickedThing, FloatMenuContext context, Pawn carriedPawn, out FloatMenuOption option)
		{
			option = null;
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (!carriedPawn.TryGetComp(out compHoldingPlatformTarget) || !compHoldingPlatformTarget.CanBeCaptured || !compHoldingPlatformTarget.StudiedAtHoldingPlatform)
			{
				return false;
			}
			CompEntityHolder compEntityHolder;
			if (!clickedThing.TryGetComp(out compEntityHolder) || !compEntityHolder.Available)
			{
				return false;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedThing, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				option = new FloatMenuOption("CannotPlaceIn".Translate(carriedPawn, clickedThing) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				return true;
			}
			TaggedString taggedString = "Capture".Translate(carriedPawn.LabelCap, carriedPawn);
			if (!clickedThing.SafelyContains(carriedPawn))
			{
				float statValue = carriedPawn.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1);
				taggedString += string.Format(" ({0} {1:F0}, {2} {3:F0})", new object[]
				{
					"FloatMenuContainmentStrength".Translate().ToLower(),
					compEntityHolder.ContainmentStrength,
					"FloatMenuContainmentRequires".Translate(carriedPawn).ToLower(),
					statValue
				});
			}
			option = FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, delegate()
			{
				clickedThing.SetForbidden(false, false);
				Job job = JobMaker.MakeJob(JobDefOf.CarryToEntityHolderAlreadyHolding, clickedThing, context.FirstSelectedPawn.carryTracker.CarriedThing);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedThing, "ReservedBy", null);
			return true;
		}
	}
}
