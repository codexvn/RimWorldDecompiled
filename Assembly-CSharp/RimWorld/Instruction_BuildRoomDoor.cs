using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034CC RID: 13516
	public class Instruction_BuildRoomDoor : Lesson_Instruction
	{
		// Token: 0x17003179 RID: 12665
		// (get) Token: 0x06012FE7 RID: 77799 RVA: 0x005A24BC File Offset: 0x005A06BC
		private CellRect RoomRect
		{
			get
			{
				return Find.TutorialState.roomRect;
			}
		}

		// Token: 0x06012FE8 RID: 77800 RVA: 0x005A24C8 File Offset: 0x005A06C8
		public override void OnActivated()
		{
			base.OnActivated();
			this.allowedPlaceCells = this.RoomRect.EdgeCells.ToList<IntVec3>();
			this.allowedPlaceCells.RemoveAll((IntVec3 c) => (c.x == this.RoomRect.minX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.minX && c.z == this.RoomRect.maxZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.minZ) || (c.x == this.RoomRect.maxX && c.z == this.RoomRect.maxZ));
		}

		// Token: 0x06012FE9 RID: 77801 RVA: 0x005A250C File Offset: 0x005A070C
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.RoomRect, this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06012FEA RID: 77802 RVA: 0x005A252C File Offset: 0x005A072C
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.RoomRect.CenterVector3, false);
		}

		// Token: 0x06012FEB RID: 77803 RVA: 0x005A254D File Offset: 0x005A074D
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.allowedPlaceCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06012FEC RID: 77804 RVA: 0x005A257B File Offset: 0x005A077B
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Door")
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400C506 RID: 50438
		private List<IntVec3> allowedPlaceCells;
	}
}
