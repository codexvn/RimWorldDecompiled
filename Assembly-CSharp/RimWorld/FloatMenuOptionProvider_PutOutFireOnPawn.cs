using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003323 RID: 13091
	public class FloatMenuOptionProvider_PutOutFireOnPawn : FloatMenuOptionProvider
	{
		// Token: 0x17002FDE RID: 12254
		// (get) Token: 0x06012685 RID: 75397 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FDF RID: 12255
		// (get) Token: 0x06012686 RID: 75398 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FE0 RID: 12256
		// (get) Token: 0x06012687 RID: 75399 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012688 RID: 75400 RVA: 0x0056D09C File Offset: 0x0056B29C
		protected override FloatMenuOption GetSingleOptionFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn == null || clickedPawn.Dead || !clickedPawn.IsBurning())
			{
				return null;
			}
			if ((clickedPawn.Faction == null || clickedPawn.Faction != context.FirstSelectedPawn.Faction) && (clickedPawn.HostFaction == null || (clickedPawn.HostFaction != context.FirstSelectedPawn.Faction && clickedPawn.HostFaction != context.FirstSelectedPawn.HostFaction)))
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption(string.Format("{0}: {1}", "CannotGenericWorkCustom".Translate(WorkGiverDefOf.FightFires.label), "NoPath".Translate().CapitalizeFirst()), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			Thing fire = clickedPawn.GetAttachment(ThingDefOf.Fire);
			if (fire == null)
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("PrioritizeGeneric".Translate(WorkGiverDefOf.FightFires.gerund, clickedPawn.Label).CapitalizeFirst(), delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.BeatFire, fire);
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, fire, "ReservedBy", null);
		}
	}
}
