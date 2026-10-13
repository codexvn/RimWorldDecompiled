using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003332 RID: 13106
	public class FloatMenuOptionProvider_RemoveMechlink : FloatMenuOptionProvider
	{
		// Token: 0x17002FEE RID: 12270
		// (get) Token: 0x060126C8 RID: 75464 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FEF RID: 12271
		// (get) Token: 0x060126C9 RID: 75465 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FF0 RID: 12272
		// (get) Token: 0x060126CA RID: 75466 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FF1 RID: 12273
		// (get) Token: 0x060126CB RID: 75467 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060126CC RID: 75468 RVA: 0x00563089 File Offset: 0x00561289
		protected override bool AppliesInt(FloatMenuContext context)
		{
			return ModsConfig.BiotechActive;
		}

		// Token: 0x060126CD RID: 75469 RVA: 0x0056E124 File Offset: 0x0056C324
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			Corpse corpse = clickedThing as Corpse;
			if (corpse == null)
			{
				return null;
			}
			if (!corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.MechlinkImplant, false))
			{
				return null;
			}
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Extract".Translate() + " " + HediffDefOf.MechlinkImplant.label, delegate()
			{
				Job job = JobMaker.MakeJob(JobDefOf.RemoveMechlink, corpse);
				context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(corpse), "ReservedBy", null);
		}
	}
}
