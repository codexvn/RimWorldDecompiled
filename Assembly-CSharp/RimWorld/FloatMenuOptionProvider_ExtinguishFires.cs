using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020032FE RID: 13054
	public class FloatMenuOptionProvider_ExtinguishFires : FloatMenuOptionProvider
	{
		// Token: 0x17002F9C RID: 12188
		// (get) Token: 0x060125C8 RID: 75208 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Drafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F9D RID: 12189
		// (get) Token: 0x060125C9 RID: 75209 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Undrafted
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F9E RID: 12190
		// (get) Token: 0x060125CA RID: 75210 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool Multiselect
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17002F9F RID: 12191
		// (get) Token: 0x060125CB RID: 75211 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool RequiresManipulation
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060125CC RID: 75212 RVA: 0x00569770 File Offset: 0x00567970
		protected override FloatMenuOption GetSingleOption(FloatMenuContext context)
		{
			if (!new TargetInfo(context.ClickedCell, context.FirstSelectedPawn.Map, false).IsBurning())
			{
				return null;
			}
			bool flag = false;
			AcceptanceReport acceptanceReport = null;
			foreach (Pawn pawn in context.ValidSelectedPawns)
			{
				acceptanceReport = this.PawnCanExtinguish(pawn, context.ClickedCell);
				if (acceptanceReport.Accepted)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return new FloatMenuOption(string.Format("{0}: {1}", "CannotGenericWorkCustom".Translate(WorkGiverDefOf.FightFires.label), acceptanceReport.Reason), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			return new FloatMenuOption("ExtinguishFiresNearby".Translate(), delegate()
			{
				foreach (Pawn pawn2 in context.ValidSelectedPawns)
				{
					if (this.PawnCanExtinguish(pawn2, context.ClickedCell).Accepted)
					{
						Job job = JobMaker.MakeJob(JobDefOf.ExtinguishFiresNearby);
						foreach (Fire t in context.ClickedCell.GetFiresNearCell(context.FirstSelectedPawn.Map))
						{
							job.AddQueuedTarget(TargetIndex.A, t);
						}
						pawn2.jobs.TryTakeOrderedJob(job, new JobTag?(JobTag.Misc), false);
					}
				}
			}, ThingDefOf.Fire, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null);
		}

		// Token: 0x060125CD RID: 75213 RVA: 0x005698A8 File Offset: 0x00567AA8
		private AcceptanceReport PawnCanExtinguish(Pawn pawn, IntVec3 cell)
		{
			if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Firefighter))
			{
				return "IncapableOf".Translate().CapitalizeFirst() + " " + WorkTypeDefOf.Firefighter.gerundLabel;
			}
			Fire t;
			if (!pawn.CanReach(cell, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn) && (!cell.TryGetFirstThing(pawn.Map, out t) || !pawn.CanReach(t, PathEndMode.ClosestTouch, Danger.Deadly, false, false, TraverseMode.ByPawn)))
			{
				return "NoPath".Translate().CapitalizeFirst();
			}
			if (!cell.InAllowedArea(pawn))
			{
				return "CannotPrioritizeForbiddenOutsideAllowedArea".Translate() + " ( " + pawn.playerSettings.EffectiveAreaRestrictionInPawnCurrentMap.Label + ")";
			}
			return true;
		}
	}
}
