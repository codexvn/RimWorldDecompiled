using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032BB RID: 12987
	public class FloatMenuOptionProvider_Arrest : FloatMenuOptionProvider
	{
		// Token: 0x17002F31 RID: 12081
		// (get) Token: 0x06012489 RID: 74889 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F32 RID: 12082
		// (get) Token: 0x0601248A RID: 74890 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F33 RID: 12083
		// (get) Token: 0x0601248B RID: 74891 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F34 RID: 12084
		// (get) Token: 0x0601248C RID: 74892 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0601248D RID: 74893 RVA: 0x00562D4C File Offset: 0x00560F4C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.CanBeArrestedBy(context.FirstSelectedPawn))
			{
				return null;
			}
			if (clickedPawn.Downed && clickedPawn.guilt.IsGuilty)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.Drafted && (!clickedPawn.IsWildMan() || clickedPawn.IsPrisonerOfColony))
			{
				return null;
			}
			if (context.FirstSelectedPawn.InSameExtraFaction(clickedPawn, ExtraFactionType.HomeFaction, null) || context.FirstSelectedPawn.InSameExtraFaction(clickedPawn, ExtraFactionType.MiniFaction, null))
			{
				return new FloatMenuOption("CannotArrest".Translate() + ": " + "SameFaction".Translate(clickedPawn), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotArrest".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Action action = delegate()
			{
				Building_Bed building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, false, new GuestStatus?(GuestStatus.Prisoner));
				if (building_Bed == null)
				{
					building_Bed = RestUtility.FindBedFor(clickedPawn, context.FirstSelectedPawn, false, true, new GuestStatus?(GuestStatus.Prisoner));
				}
				if (building_Bed == null)
				{
					Messages.Message("CannotArrest".Translate() + ": " + "NoPrisonerBed".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.Arrest, clickedPawn, building_Bed);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				if (clickedPawn.Faction != null && ((clickedPawn.Faction != Faction.OfPlayer && !clickedPawn.Faction.Hidden) || clickedPawn.IsQuestLodger()))
				{
					TutorUtility.DoModalDialogIfNotKnown(ConceptDefOf.ArrestingCreatesEnemies, new string[]
					{
						clickedPawn.GetAcceptArrestChance(context.FirstSelectedPawn).ToStringPercent()
					});
				}
			};
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TryToArrest".Translate(clickedPawn.LabelCap, clickedPawn, clickedPawn.GetAcceptArrestChance(context.FirstSelectedPawn).ToStringPercent()), action, MenuOptionPriority.High, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
