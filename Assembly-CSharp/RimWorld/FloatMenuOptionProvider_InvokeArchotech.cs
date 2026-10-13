using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200330C RID: 13068
	public class FloatMenuOptionProvider_InvokeArchotech : FloatMenuOptionProvider
	{
		// Token: 0x17002FBC RID: 12220
		// (get) Token: 0x06012612 RID: 75282 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FBD RID: 12221
		// (get) Token: 0x06012613 RID: 75283 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FBE RID: 12222
		// (get) Token: 0x06012614 RID: 75284 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FBF RID: 12223
		// (get) Token: 0x06012615 RID: 75285 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06012616 RID: 75286 RVA: 0x00565319 File Offset: 0x00563519
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.IdeologyActive;
		}

		// Token: 0x06012617 RID: 75287 RVA: 0x0056A870 File Offset: 0x00568A70
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Building_ArchonexusCore archCore = clickedThing as Building_ArchonexusCore;
			if (archCore == null)
			{
				return null;
			}
			if (!context.FirstSelectedPawn.CanReach(archCore, PathEndMode.InteractionCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			if (!archCore.CanActivateNow)
			{
				return new FloatMenuOption("CannotInvoke".Translate("Power".Translate()) + ": " + "AlreadyInvoked".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Invoke".Translate("Power".Translate()), delegate()
			{
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.ActivateArchonexusCore, archCore), new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, archCore, "ReservedBy", null);
		}
	}
}
