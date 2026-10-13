using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02003305 RID: 13061
	public class FloatMenuOptionProvider_HackAncientTerminal : FloatMenuOptionProvider
	{
		// Token: 0x17002FAF RID: 12207
		// (get) Token: 0x060125F1 RID: 75249 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FB0 RID: 12208
		// (get) Token: 0x060125F2 RID: 75250 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002FB1 RID: 12209
		// (get) Token: 0x060125F3 RID: 75251 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002FB2 RID: 12210
		// (get) Token: 0x060125F4 RID: 75252 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060125F5 RID: 75253 RVA: 0x00569D8C File Offset: 0x00567F8C
		protected override FloatMenuOption GetSingleOptionFor(Thing clickedThing, FloatMenuContext context)
		{
			if (!ModsConfig.IdeologyActive || clickedThing.def != ThingDefOf.AncientEnemyTerminal)
			{
				return null;
			}
			CompHackable compHackable;
			if (!clickedThing.TryGetComp(out compHackable) || compHackable.Props.onlyRemotelyHackable)
			{
				return null;
			}
			if (!compHackable.CanHackNow(context.FirstSelectedPawn).Accepted)
			{
				return null;
			}
			Action <>9__1;
			return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("Hack".Translate(clickedThing.Label), delegate()
			{
				WindowStack windowStack = Find.WindowStack;
				TaggedString text = "ConfirmHackEnemyTerminal".Translate(ThingDefOf.AncientEnemyTerminal.label);
				Action confirmedAct;
				if ((confirmedAct = <>9__1) == null)
				{
					confirmedAct = (<>9__1 = delegate()
					{
						context.FirstSelectedPawn.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.Hack, clickedThing), new JobTag?(JobTag.Misc), false);
					});
				}
				windowStack.Add(Dialog_MessageBox.CreateConfirmation(text, confirmedAct, false, null, WindowLayer.Dialog));
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0), context.FirstSelectedPawn, new LocalTargetInfo(clickedThing), "ReservedBy", null);
		}
	}
}
