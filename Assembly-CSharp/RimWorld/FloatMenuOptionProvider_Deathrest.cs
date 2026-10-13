using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032E3 RID: 13027
	public class FloatMenuOptionProvider_Deathrest : FloatMenuOptionProvider
	{
		// Token: 0x17002F71 RID: 12145
		// (get) Token: 0x06012540 RID: 75072 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F72 RID: 12146
		// (get) Token: 0x06012541 RID: 75073 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F73 RID: 12147
		// (get) Token: 0x06012542 RID: 75074 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012543 RID: 75075 RVA: 0x005668C4 File Offset: 0x00564AC4
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive && context.FirstSelectedPawn.CanDeathrest();
		}

		// Token: 0x06012544 RID: 75076 RVA: 0x005668E0 File Offset: 0x00564AE0
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_Bed bed = clickedThing as Building_Bed;
			if (bed == null || !bed.def.building.bed_humanlike)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(bed, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			AcceptanceReport acceptanceReport = bed.CompAssignableToPawn.CanAssignTo(context.FirstSelectedPawn);
			if (!acceptanceReport.Accepted)
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if ((!bed.CompAssignableToPawn.HasFreeSlot || !RestUtility.BedOwnerWillShare(bed, context.FirstSelectedPawn, new GuestStatus?(context.FirstSelectedPawn.guest.GuestStatus))) && !bed.IsOwner(context.FirstSelectedPawn))
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + "AssignedToOtherPawn".Translate(bed).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			bool flag = false;
			using (CellRect.Enumerator enumerator = bed.OccupiedRect().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetRoof(bed.Map) == null)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				return new FloatMenuOption("CannotDeathrest".Translate().CapitalizeFirst() + ": " + "ThingIsSkyExposed".Translate(bed).CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!RestUtility.IsValidBedFor(bed, context.FirstSelectedPawn, context.FirstSelectedPawn, true, false, false, context.FirstSelectedPawn.GuestStatus))
			{
				return null;
			}
			return new FloatMenuOption("StartDeathrest".Translate(), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.Deathrest, bed);
				job.forceSleep = true;
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
		}
	}
}
