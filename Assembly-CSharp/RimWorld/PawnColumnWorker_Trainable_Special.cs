using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200347F RID: 13439
	public class PawnColumnWorker_Trainable_Special : PawnColumnWorker
	{
		// Token: 0x06012E5B RID: 77403 RVA: 0x0059446E File Offset: 0x0059266E
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			MouseoverSounds.DoRegion(rect);
		}

		// Token: 0x06012E5C RID: 77404 RVA: 0x00598980 File Offset: 0x00596B80
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.training == null || pawn.RaceProps.specialTrainables.NullOrEmpty<TrainableDef>())
			{
				return;
			}
			int num = (int)((rect.width - 24f) / 2f);
			int num2 = Mathf.Max(3, 0);
			Rect rect2 = new Rect(rect.x + (float)num, rect.y + (float)num2, 24f, 24f);
			this.DoSpecialTrainableCheckbox(rect2, pawn, true);
		}

		// Token: 0x06012E5D RID: 77405 RVA: 0x005989F4 File Offset: 0x00596BF4
		private void DoSpecialTrainableCheckbox(Rect rect, Pawn pawn, bool doTooltip)
		{
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			PawnColumnWorker_Trainable_Special.GetStatus(pawn, out flag, out flag2, out flag3, out flag4);
			bool flag5 = flag2;
			Texture2D texChecked = flag ? TrainingCardUtility.LearnedTrainingTex : null;
			Texture2D texUnchecked = flag ? TrainingCardUtility.LearnedNotTrainingTex : null;
			Widgets.Checkbox(rect.position, ref flag2, rect.width, !flag3, true, texChecked, texUnchecked);
			if (flag2 != flag5)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.AnimalTraining, KnowledgeAmount.Total);
				foreach (TrainableDef td in pawn.RaceProps.specialTrainables)
				{
					pawn.training.SetWantedRecursive(td, flag2);
				}
			}
			if (doTooltip)
			{
				this.DoSpecialTrainableTooltip(rect, pawn);
			}
		}

		// Token: 0x06012E5E RID: 77406 RVA: 0x00598AB8 File Offset: 0x00596CB8
		private void DoSpecialTrainableTooltip(Rect rect, Pawn pawn)
		{
			if (!Mouse.IsOver(rect))
			{
				return;
			}
			TooltipHandler.TipRegion(rect, delegate()
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (TrainableDef trainableDef in pawn.RaceProps.specialTrainables)
				{
					bool flag;
					AcceptanceReport acceptanceReport = pawn.training.CanAssignToTrain(trainableDef, out flag);
					stringBuilder.AppendLineIfNotEmpty();
					stringBuilder.AppendLine(trainableDef.LabelCap + "\n\n" + trainableDef.description);
					if (!acceptanceReport.Accepted)
					{
						stringBuilder.AppendLine().AppendLine(acceptanceReport.Reason);
					}
					else if (!trainableDef.prerequisites.NullOrEmpty<TrainableDef>())
					{
						stringBuilder.AppendLine();
						foreach (TrainableDef trainableDef2 in trainableDef.prerequisites)
						{
							if (!pawn.training.HasLearned(trainableDef2))
							{
								stringBuilder.AppendLine("TrainingNeedsPrerequisite".Translate(trainableDef2.LabelCap));
							}
						}
					}
				}
				return stringBuilder.ToString();
			}, (int)(rect.y * 511f + rect.x));
		}

		// Token: 0x06012E5F RID: 77407 RVA: 0x00595706 File Offset: 0x00593906
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), 24);
		}

		// Token: 0x06012E60 RID: 77408 RVA: 0x0059455B File Offset: 0x0059275B
		public override int GetMaxWidth(PawnTable table)
		{
			return Mathf.Min(base.GetMaxWidth(table), this.GetMinWidth(table));
		}

		// Token: 0x06012E61 RID: 77409 RVA: 0x00594570 File Offset: 0x00592770
		public override int GetMinCellHeight(Pawn pawn)
		{
			return Mathf.Max(base.GetMinCellHeight(pawn), 24);
		}

		// Token: 0x06012E62 RID: 77410 RVA: 0x00598B04 File Offset: 0x00596D04
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06012E63 RID: 77411 RVA: 0x00598B28 File Offset: 0x00596D28
		private int GetValueToCompare(Pawn pawn)
		{
			if (pawn.training == null || pawn.RaceProps.specialTrainables.NullOrEmpty<TrainableDef>())
			{
				return int.MinValue;
			}
			bool flag;
			bool flag2;
			bool flag3;
			bool flag4;
			PawnColumnWorker_Trainable_Special.GetStatus(pawn, out flag, out flag2, out flag3, out flag4);
			if (flag)
			{
				return 4;
			}
			if (!flag4)
			{
				return 0;
			}
			if (!flag3)
			{
				return 1;
			}
			if (!flag2)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x06012E64 RID: 77412 RVA: 0x00598B78 File Offset: 0x00596D78
		private static void GetStatus(Pawn pawn, out bool learned, out bool checkOn, out bool canTrain, out bool visible)
		{
			learned = true;
			checkOn = true;
			canTrain = true;
			visible = false;
			foreach (TrainableDef td in pawn.RaceProps.specialTrainables)
			{
				if (!pawn.training.HasLearned(td))
				{
					learned = false;
				}
				if (!pawn.training.GetWanted(td))
				{
					checkOn = false;
				}
				bool flag;
				if (!pawn.training.CanAssignToTrain(td, out flag))
				{
					canTrain = false;
				}
				if (flag)
				{
					visible = true;
				}
			}
		}
	}
}
