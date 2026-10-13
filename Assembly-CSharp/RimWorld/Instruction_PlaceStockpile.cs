using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034DD RID: 13533
	public class Instruction_PlaceStockpile : Lesson_Instruction
	{
		// Token: 0x06013040 RID: 77888 RVA: 0x005A3574 File Offset: 0x005A1774
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.stockpileRect, "stockpileRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x06013041 RID: 77889 RVA: 0x005A35AF File Offset: 0x005A17AF
		private void RecacheCells()
		{
			this.cachedCells = this.stockpileRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x06013042 RID: 77890 RVA: 0x005A35C7 File Offset: 0x005A17C7
		public override void OnActivated()
		{
			base.OnActivated();
			this.stockpileRect = TutorUtility.FindUsableRect(6, 6, base.Map, 0f, false);
			this.RecacheCells();
		}

		// Token: 0x06013043 RID: 77891 RVA: 0x005A35EE File Offset: 0x005A17EE
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.stockpileRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06013044 RID: 77892 RVA: 0x005A360C File Offset: 0x005A180C
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells, 2900);
			GenDraw.DrawArrowPointingAt(this.stockpileRect.CenterVector3, false);
		}

		// Token: 0x06013045 RID: 77893 RVA: 0x005A362F File Offset: 0x005A182F
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAddStockpile_Resources")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400C51A RID: 50458
		private CellRect stockpileRect;

		// Token: 0x0400C51B RID: 50459
		private List<IntVec3> cachedCells;
	}
}
