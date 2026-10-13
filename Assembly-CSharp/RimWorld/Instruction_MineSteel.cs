using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020034DB RID: 13531
	public class Instruction_MineSteel : Lesson_Instruction
	{
		// Token: 0x17003186 RID: 12678
		// (get) Token: 0x06013031 RID: 77873 RVA: 0x005A3288 File Offset: 0x005A1488
		protected override float ProgressPercent
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.mineCells.Count; i++)
				{
					IntVec3 c = this.mineCells[i];
					if (base.Map.designationManager.DesignationAt(c, DesignationDefOf.Mine) != null || c.GetEdifice(base.Map) == null || c.GetEdifice(base.Map).def != ThingDefOf.MineableSteel)
					{
						num++;
					}
				}
				return (float)num / (float)this.mineCells.Count;
			}
		}

		// Token: 0x06013032 RID: 77874 RVA: 0x005A330B File Offset: 0x005A150B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<IntVec3>(ref this.mineCells, "mineCells", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x06013033 RID: 77875 RVA: 0x005A332C File Offset: 0x005A152C
		public override void OnActivated()
		{
			base.OnActivated();
			CellRect cellRect = TutorUtility.FindUsableRect(10, 10, base.Map, 0f, true);
			new GenStep_ScatterLumpsMineable
			{
				forcedDefToScatter = ThingDefOf.MineableSteel
			}.ForceScatterAt(cellRect.CenterCell, base.Map);
			this.mineCells = new List<IntVec3>();
			foreach (IntVec3 intVec in cellRect)
			{
				Building edifice = intVec.GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.MineableSteel)
				{
					this.mineCells.Add(intVec);
				}
			}
		}

		// Token: 0x06013034 RID: 77876 RVA: 0x005A33E8 File Offset: 0x005A15E8
		public override void LessonOnGUI()
		{
			if (!this.mineCells.NullOrEmpty<IntVec3>())
			{
				TutorUtility.DrawLabelOnGUI(Gen.AveragePosition(this.mineCells), this.def.onMapInstruction);
			}
			base.LessonOnGUI();
		}

		// Token: 0x06013035 RID: 77877 RVA: 0x005A3418 File Offset: 0x005A1618
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(Gen.AveragePosition(this.mineCells), false);
		}

		// Token: 0x06013036 RID: 77878 RVA: 0x005A342B File Offset: 0x005A162B
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine")
			{
				return TutorUtility.EventCellsAreWithin(ep, this.mineCells);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06013037 RID: 77879 RVA: 0x005A3459 File Offset: 0x005A1659
		public override void Notify_Event(EventPack ep)
		{
			if (ep.Tag == "Designate-Mine" && this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}

		// Token: 0x0400C517 RID: 50455
		private List<IntVec3> mineCells;
	}
}
