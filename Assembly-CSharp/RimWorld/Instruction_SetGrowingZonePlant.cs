using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034DE RID: 13534
	public class Instruction_SetGrowingZonePlant : Lesson_Instruction
	{
		// Token: 0x17003187 RID: 12679
		// (get) Token: 0x06013047 RID: 77895 RVA: 0x005A365D File Offset: 0x005A185D
		private Zone_Growing GrowZone
		{
			get
			{
				return (Zone_Growing)base.Map.zoneManager.AllZones.FirstOrDefault((Zone z) => z is Zone_Growing);
			}
		}

		// Token: 0x06013048 RID: 77896 RVA: 0x005A3698 File Offset: 0x005A1898
		public override void LessonOnGUI()
		{
			TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.GrowZone.cells), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06013049 RID: 77897 RVA: 0x005A36C0 File Offset: 0x005A18C0
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.GrowZone.cells), false);
		}
	}
}
