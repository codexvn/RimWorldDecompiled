using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020034C6 RID: 13510
	public class Instruction_AddBill : Lesson_Instruction
	{
		// Token: 0x17003172 RID: 12658
		// (get) Token: 0x06012FC8 RID: 77768 RVA: 0x005A1F80 File Offset: 0x005A0180
		protected override float ProgressPercent
		{
			get
			{
				int num = this.def.recipeTargetCount + 1;
				int num2 = 0;
				Bill_Production bill_Production = this.RelevantBill();
				if (bill_Production != null)
				{
					num2++;
					if (bill_Production.repeatMode == BillRepeatModeDefOf.RepeatCount)
					{
						num2 += bill_Production.repeatCount;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06012FC9 RID: 77769 RVA: 0x005A1FC8 File Offset: 0x005A01C8
		private Bill_Production RelevantBill()
		{
			if (Find.Selector.SingleSelectedThing != null && Find.Selector.SingleSelectedThing.def == this.def.thingDef)
			{
				IBillGiver billGiver = Find.Selector.SingleSelectedThing as IBillGiver;
				if (billGiver != null)
				{
					return (Bill_Production)billGiver.BillStack.Bills.FirstOrDefault((Bill b) => b.recipe == this.def.recipeDef);
				}
			}
			return null;
		}

		// Token: 0x06012FCA RID: 77770 RVA: 0x005A2033 File Offset: 0x005A0233
		private IEnumerable<Thing> ThingsToSelect()
		{
			if (Find.Selector.SingleSelectedThing == null || Find.Selector.SingleSelectedThing.def != this.def.thingDef)
			{
				foreach (Building building in base.Map.listerBuildings.AllBuildingsColonistOfDef(this.def.thingDef))
				{
					yield return building;
				}
				List<Building>.Enumerator enumerator = default(List<Building>.Enumerator);
				yield break;
			}
			yield break;
			yield break;
		}

		// Token: 0x06012FCB RID: 77771 RVA: 0x005A2044 File Offset: 0x005A0244
		public override void LessonOnGUI()
		{
			foreach (Thing t in this.ThingsToSelect())
			{
				TutorUtility.DrawLabelOnThingOnGUI(t, this.def.onMapInstruction);
			}
			if (this.RelevantBill() == null)
			{
				UIHighlighter.HighlightTag("AddBill");
			}
			base.LessonOnGUI();
		}

		// Token: 0x06012FCC RID: 77772 RVA: 0x005A20B4 File Offset: 0x005A02B4
		public override void LessonUpdate()
		{
			foreach (Thing thing in this.ThingsToSelect())
			{
				GenDraw.DrawArrowPointingAt(thing.DrawPos, false);
			}
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
