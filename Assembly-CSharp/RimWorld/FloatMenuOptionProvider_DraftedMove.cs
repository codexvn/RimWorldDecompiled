using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032EB RID: 13035
	public class FloatMenuOptionProvider_DraftedMove : FloatMenuOptionProvider
	{
		// Token: 0x17002F7C RID: 12156
		// (get) Token: 0x0601256F RID: 75119 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F7D RID: 12157
		// (get) Token: 0x06012570 RID: 75120 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool Undrafted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17002F7E RID: 12158
		// (get) Token: 0x06012571 RID: 75121 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F7F RID: 12159
		// (get) Token: 0x06012572 RID: 75122 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool MechanoidCanDo
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F80 RID: 12160
		// (get) Token: 0x06012573 RID: 75123 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool IgnoreFogged
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06012574 RID: 75124 RVA: 0x005677FC File Offset: 0x005659FC
		protected override FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			IntVec3 curLoc = CellFinder.StandableCellNear(context.ClickedCell, context.map, 2.9f, null);
			if (!curLoc.IsValid)
			{
				return null;
			}
			FloatMenuOption floatMenuOption;
			if (!context.IsMultiselect)
			{
				if (curLoc == context.FirstSelectedPawn.Position)
				{
					return null;
				}
				AcceptanceReport acceptanceReport = FloatMenuOptionProvider_DraftedMove.PawnCanGoto(context.FirstSelectedPawn, curLoc);
				if (!acceptanceReport.Accepted)
				{
					return new FloatMenuOption(acceptanceReport.Reason, null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				floatMenuOption = new FloatMenuOption("GoHere".Translate(), delegate()
				{
					FloatMenuOptionProvider_DraftedMove.PawnGotoAction(context.ClickedCell, context.FirstSelectedPawn, RCellFinder.BestOrderedGotoDestNear(curLoc, context.FirstSelectedPawn, null, true));
				}, MenuOptionPriority.GoHere, null, null, 0f, null, null, true, 0);
			}
			else
			{
				FloatMenuOptionProvider_DraftedMove.tmpPawns.Clear();
				foreach (Pawn pawn in context.ValidSelectedPawns)
				{
					if (FloatMenuOptionProvider_DraftedMove.PawnCanGoto(pawn, curLoc).Accepted)
					{
						FloatMenuOptionProvider_DraftedMove.tmpPawns.Add(pawn);
					}
				}
				if (FloatMenuOptionProvider_DraftedMove.tmpPawns.Count == 0)
				{
					return null;
				}
				floatMenuOption = new FloatMenuOption("GoHere".Translate(), delegate()
				{
					Find.Selector.gotoController.StartInteraction(curLoc);
					foreach (Pawn pawn2 in FloatMenuOptionProvider_DraftedMove.tmpPawns)
					{
						Find.Selector.gotoController.AddPawn(pawn2);
					}
					Find.Selector.gotoController.FinalizeInteraction();
				}, MenuOptionPriority.GoHere, null, null, 0f, null, null, true, 0);
			}
			floatMenuOption.isGoto = true;
			floatMenuOption.autoTakeable = true;
			floatMenuOption.autoTakeablePriority = 10f;
			return floatMenuOption;
		}

		// Token: 0x06012575 RID: 75125 RVA: 0x005679AC File Offset: 0x00565BAC
		public static AcceptanceReport PawnCanGoto(Pawn pawn, IntVec3 gotoLoc)
		{
			if (ModsConfig.BiotechActive && pawn.IsColonyMech && !MechanitorUtility.InMechanitorCommandRange(pawn, gotoLoc))
			{
				return "CannotGoOutOfRange".Translate() + ": " + "OutOfCommandRange".Translate();
			}
			if (!pawn.CanReach(gotoLoc, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn))
			{
				return "CannotGoNoPath".Translate();
			}
			return true;
		}

		// Token: 0x06012576 RID: 75126 RVA: 0x00567A28 File Offset: 0x00565C28
		public static void PawnGotoAction(IntVec3 clickCell, Pawn pawn, IntVec3 gotoLoc)
		{
			bool flag;
			if (pawn.Position == gotoLoc)
			{
				flag = true;
				if (pawn.CurJobDef == JobDefOf.Goto)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Succeeded, true, true);
				}
			}
			else if (pawn.CurJobDef == JobDefOf.Goto && pawn.CurJob.targetA.Cell == gotoLoc)
			{
				flag = true;
			}
			else
			{
				Job job = JobMaker.MakeJob(JobDefOf.Goto, gotoLoc);
				if (pawn.Map.exitMapGrid.IsExitCell(clickCell))
				{
					job.exitMapOnArrival = !pawn.IsColonyMech;
				}
				else if (!pawn.Map.IsPlayerHome && !pawn.Map.exitMapGrid.MapUsesExitGrid && CellRect.WholeMap(pawn.Map).IsOnEdge(clickCell, 3) && pawn.Map.Parent.GetComponent<FormCaravanComp>() != null && MessagesRepeatAvoider.MessageShowAllowed("MessagePlayerTriedToLeaveMapViaExitGrid-" + pawn.Map.uniqueID.ToString(), 60f))
				{
					Messages.Message(pawn.Map.Parent.GetComponent<FormCaravanComp>().CanFormOrReformCaravanNow ? "MessagePlayerTriedToLeaveMapViaExitGrid_CanReform".Translate() : "MessagePlayerTriedToLeaveMapViaExitGrid_CantReform".Translate(), pawn.Map.Parent, MessageTypeDefOf.RejectInput, false);
				}
				flag = pawn.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
			}
			if (flag)
			{
				FleckMaker.Static(gotoLoc, pawn.Map, FleckDefOf.FeedbackGoto, 1f);
			}
		}

		// Token: 0x0400BEC1 RID: 48833
		private static List<Pawn> tmpPawns = new List<Pawn>();
	}
}
