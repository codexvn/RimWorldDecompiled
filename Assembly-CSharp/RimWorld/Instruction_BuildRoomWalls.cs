using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034CD RID: 13517
	public class Instruction_BuildRoomWalls : Lesson_Instruction
	{
		// Token: 0x1700317A RID: 12666
		// (get) Token: 0x06012FEF RID: 77807 RVA: 0x005A24BC File Offset: 0x005A06BC
		// (set) Token: 0x06012FF0 RID: 77808 RVA: 0x005A2645 File Offset: 0x005A0845
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
			set
			{
				Find.TutorialState.roomRect = value;
			}
		}

		// Token: 0x1700317B RID: 12667
		// (get) Token: 0x06012FF1 RID: 77809 RVA: 0x005A2654 File Offset: 0x005A0854
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				int num2 = 0;
				using (IEnumerator<IntVec3> enumerator = this.RoomRect.EdgeCells.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, ThingDefOf.Wall))
						{
							num2++;
						}
						num++;
					}
				}
				return (float)num2 / (float)num;
			}
		}

		// Token: 0x06012FF2 RID: 77810 RVA: 0x005A26C4 File Offset: 0x005A08C4
		public override void OnActivated()
		{
			base.OnActivated();
			this.RoomRect = TutorUtility.FindUsableRect(12, 8, base.Map, 0f, false);
		}

		// Token: 0x06012FF3 RID: 77811 RVA: 0x005A26E6 File Offset: 0x005A08E6
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06012FF4 RID: 77812 RVA: 0x005A2704 File Offset: 0x005A0904
		public override void LessonUpdate()
		{
			this.cachedEdgeCells.Clear();
			this.cachedEdgeCells.AddRange((from c in this.RoomRect.EdgeCells
			where !TutorUtility.BuildingOrBlueprintOrFrameCenterExists(c, base.Map, ThingDefOf.Wall)
			select c).ToList<IntVec3>());
			GenDraw.DrawFieldEdges((from c in this.cachedEdgeCells
			where c.GetEdifice(base.Map) == null
			select c).ToList<IntVec3>(), 2900);
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x06012FF5 RID: 77813 RVA: 0x005A279C File Offset: 0x005A099C
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Wall")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.cachedEdgeCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x0400C507 RID: 50439
		private List<IntVec3> cachedEdgeCells = new List<IntVec3>();
	}
}
