using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034DC RID: 13532
	public class Instruction_PlaceGrowingZone : Lesson_Instruction
	{
		// Token: 0x06013039 RID: 77881 RVA: 0x005A3488 File Offset: 0x005A1688
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<CellRect>(ref this.growingZoneRect, "growingZoneRect", default(CellRect), false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheCells();
			}
		}

		// Token: 0x0601303A RID: 77882 RVA: 0x005A34C3 File Offset: 0x005A16C3
		private void RecacheCells()
		{
			this.cachedCells = this.growingZoneRect.Cells.ToList<IntVec3>();
		}

		// Token: 0x0601303B RID: 77883 RVA: 0x005A34DB File Offset: 0x005A16DB
		public override void OnActivated()
		{
			base.OnActivated();
			this.growingZoneRect = TutorUtility.FindUsableRect(10, 8, base.Map, 0.5f, false);
			this.RecacheCells();
		}

		// Token: 0x0601303C RID: 77884 RVA: 0x005A3503 File Offset: 0x005A1703
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.growingZoneRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x0601303D RID: 77885 RVA: 0x005A3521 File Offset: 0x005A1721
		public override void LessonUpdate()
		{
			GenDraw.DrawFieldEdges(this.cachedCells, 2900);
			GenDraw.DrawArrowPointingAt(this.growingZoneRect.CenterVector3, false);
		}

		// Token: 0x0601303E RID: 77886 RVA: 0x005A3544 File Offset: 0x005A1744
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-ZoneAdd_Growing")
			{
				return TutorUtility.EventCellsMatchExactly(ep, this.cachedCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400C518 RID: 50456
		private CellRect growingZoneRect;

		// Token: 0x0400C519 RID: 50457
		private List<IntVec3> cachedCells;
	}
}
