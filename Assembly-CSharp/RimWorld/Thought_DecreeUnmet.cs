using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020018AD RID: 6317
	public class Thought_DecreeUnmet : Thought_Situational
	{
		// Token: 0x170015E3 RID: 5603
		// (get) Token: 0x06009062 RID: 36962 RVA: 0x002A91E4 File Offset: 0x002A73E4
		public override string LabelCap
		{
			get
			{
				string text = base.LabelCap;
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					int num = this.TicksSinceQuestUnmet(questPart_SituationalThought);
					if (num > 0)
					{
						text = text + " (" + num.ToStringTicksToDays("F0") + ")";
					}
				}
				return text;
			}
		}

		// Token: 0x170015E4 RID: 5604
		// (get) Token: 0x06009063 RID: 36963 RVA: 0x002A9240 File Offset: 0x002A7440
		public override string Description
		{
			get
			{
				QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
				if (questPart_SituationalThought != null)
				{
					return base.Description.Formatted("(" + questPart_SituationalThought.quest.name + ")");
				}
				return "";
			}
		}

		// Token: 0x06009064 RID: 36964 RVA: 0x002A92A4 File Offset: 0x002A74A4
		public override float MoodOffset()
		{
			if (ThoughtUtility.ThoughtNullified(this.pawn, this.def))
			{
				return 0f;
			}
			QuestPart_SituationalThought questPart_SituationalThought = ((ThoughtWorker_QuestPart)this.def.Worker).FindQuestPart(this.pawn);
			if (questPart_SituationalThought == null)
			{
				return 0f;
			}
			float x = (float)this.TicksSinceQuestUnmet(questPart_SituationalThought) / 60000f;
			return (float)Mathf.RoundToInt(Thought_DecreeUnmet.MoodOffsetFromUnmetDaysCurve.Evaluate(x));
		}

		// Token: 0x06009065 RID: 36965 RVA: 0x002A930F File Offset: 0x002A750F
		private int TicksSinceQuestUnmet(QuestPart_SituationalThought questPart)
		{
			return questPart.quest.TicksSinceAccepted - questPart.delayTicks;
		}

		// Token: 0x04005C43 RID: 23619
		private static readonly SimpleCurve MoodOffsetFromUnmetDaysCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, -5f),
				true
			},
			{
				new CurvePoint(15f, -15f),
				true
			}
		};
	}
}
