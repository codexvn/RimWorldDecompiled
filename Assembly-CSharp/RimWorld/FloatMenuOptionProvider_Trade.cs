using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02003340 RID: 13120
	public class FloatMenuOptionProvider_Trade : FloatMenuOptionProvider
	{
		// Token: 0x17003006 RID: 12294
		// (get) Token: 0x06012707 RID: 75527 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003007 RID: 12295
		// (get) Token: 0x06012708 RID: 75528 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17003008 RID: 12296
		// (get) Token: 0x06012709 RID: 75529 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Multiselect
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0601270A RID: 75530 RVA: 0x0056F2E7 File Offset: 0x0056D4E7
		public override IEnumerable<FloatMenuOption> GetOptionsFor(Pawn clickedPawn, FloatMenuContext context)
		{
			if (clickedPawn == null || !((ITrader)clickedPawn).CanTradeNow)
			{
				yield break;
			}
			if (!context.FirstSelectedPawn.CanReach(clickedPawn, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption("CannotTrade".Translate() + ": " + "NoPath".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (context.FirstSelectedPawn.skills.GetSkill(SkillDefOf.Social).TotallyDisabled)
			{
				yield return new FloatMenuOption("CannotPrioritizeWorkTypeDisabled".Translate(SkillDefOf.Social.LabelCap), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (clickedPawn.mindState.traderDismissed)
			{
				yield return new FloatMenuOption("TraderDismissed".Translate(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			if (!context.FirstSelectedPawn.CanTradeWith(clickedPawn.Faction, clickedPawn.TraderKind).Accepted)
			{
				yield return new FloatMenuOption("CannotTrade".Translate() + ": " + "MissingTitleAbility".Translate().CapitalizeFirst(), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			else
			{
				Action action = delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.TradeWithPawn, clickedPawn);
					job.playerForced = true;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.InteractingWithTraders, KnowledgeAmount.Total);
				};
				string t = "";
				if (clickedPawn.Faction != null)
				{
					t = " (" + clickedPawn.Faction.Name + ")";
				}
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("TradeWith".Translate(clickedPawn.LabelShort + ", " + clickedPawn.TraderKind.label) + t, action, MenuOptionPriority.InitiateSocial, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			if (clickedPawn.GetLord().LordJob is LordJob_TradeWithColony && !clickedPawn.mindState.traderDismissed)
			{
				Action action2 = delegate()
				{
					Job job = JobMaker.MakeJob(JobDefOf.DismissTrader, clickedPawn);
					job.playerForced = true;
					context.FirstSelectedPawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
				};
				yield return FloatMenuUtility.DecoratePrioritizedTask(new FloatMenuOption("DismissTrader".Translate(), action2, MenuOptionPriority.InitiateSocial, null, clickedPawn, 0f, null, null, true, 0), context.FirstSelectedPawn, clickedPawn, "ReservedBy", null);
			}
			yield break;
		}
	}
}
