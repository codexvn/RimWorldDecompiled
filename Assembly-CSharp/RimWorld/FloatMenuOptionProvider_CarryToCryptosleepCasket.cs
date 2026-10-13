using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032D8 RID: 13016
	public class FloatMenuOptionProvider_CarryToCryptosleepCasket : FloatMenuOptionProvider
	{
		// Token: 0x17002F5F RID: 12127
		// (get) Token: 0x0601250E RID: 75022 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F60 RID: 12128
		// (get) Token: 0x0601250F RID: 75023 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F61 RID: 12129
		// (get) Token: 0x06012510 RID: 75024 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F62 RID: 12130
		// (get) Token: 0x06012511 RID: 75025 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012512 RID: 75026 RVA: 0x005659B4 File Offset: 0x00563BB4
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (!clickedPawn.Downed)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReserveAndReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, 1, -1, null, true))
			{
				return null;
			}
			if (Building_CryptosleepCasket.FindCryptosleepCasketFor(clickedPawn, context.FirstSelectedPawn, true) == null)
			{
				return null;
			}
			TaggedString taggedString = "CarryToCryptosleepCasket".Translate(clickedPawn.LabelCap, clickedPawn);
			if (clickedPawn.IsQuestLodger())
			{
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestsNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			if (clickedPawn.GetExtraHostFaction(null) != null)
			{
				return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString + " (" + "CryptosleepCasketGuestPrisonersNotAllowed".Translate() + ")", null, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			Action action = delegate()
			{
				Building_CryptosleepCasket building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(clickedPawn, context.FirstSelectedPawn, false);
				if (building_CryptosleepCasket == null)
				{
					building_CryptosleepCasket = Building_CryptosleepCasket.FindCryptosleepCasketFor(clickedPawn, context.FirstSelectedPawn, true);
				}
				if (building_CryptosleepCasket == null)
				{
					Messages.Message("CannotCarryToCryptosleepCasket".Translate() + ": " + "NoCryptosleepCasket".Translate(), clickedPawn, MessageTypeDefOf.RejectInput, false);
					return;
				}
				Job job = JobMaker.MakeJob(JobDefOf.CarryToCryptosleepCasket, clickedPawn, building_CryptosleepCasket);
				job.count = 1;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			};
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption(taggedString, action, MenuOptionPriority.Default, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
		}
	}
}
