using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020034C9 RID: 13513
	public abstract class Instruction_BuildAtRoom : Lesson_Instruction
	{
		// Token: 0x17003175 RID: 12661
		// (get) Token: 0x06012FD9 RID: 77785
		protected abstract CellRect BuildableRect { get; }

		// Token: 0x17003176 RID: 12662
		// (get) Token: 0x06012FDA RID: 77786 RVA: 0x005A22EF File Offset: 0x005A04EF
		protected override float ProgressPercent
		{
			get
			{
				if (this.def.targetCount <= 1)
				{
					return -1f;
				}
				return (float)this.NumPlaced() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06012FDB RID: 77787 RVA: 0x005A231C File Offset: 0x005A051C
		protected int NumPlaced()
		{
			int num = 0;
			using (CellRect.Enumerator enumerator = this.BuildableRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TutorUtility.BuildingOrBlueprintOrFrameCenterExists(enumerator.Current, base.Map, this.def.thingDef))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06012FDC RID: 77788 RVA: 0x005A238C File Offset: 0x005A058C
		public override void LessonOnGUI()
		{
			TutorUtility.DrawCellRectOnGUI(this.BuildableRect.ContractedBy(1), this.def.onMapInstruction);
			base.LessonOnGUI();
		}

		// Token: 0x06012FDD RID: 77789 RVA: 0x005A23C0 File Offset: 0x005A05C0
		public override void LessonUpdate()
		{
			GenDraw.DrawArrowPointingAt(this.BuildableRect.CenterVector3, true);
		}

		// Token: 0x06012FDE RID: 77790 RVA: 0x005A23E4 File Offset: 0x005A05E4
		public override AcceptanceReport AllowAction(EventPack ep)
		{
			if (ep.Tag == "Designate-" + this.def.thingDef.defName)
			{
				return this.AllowBuildAt(ep.Cell);
			}
			return base.AllowAction(ep);
		}

		// Token: 0x06012FDF RID: 77791 RVA: 0x005A2434 File Offset: 0x005A0634
		protected virtual bool AllowBuildAt(IntVec3 c)
		{
			return this.BuildableRect.Contains(c);
		}

		// Token: 0x06012FE0 RID: 77792 RVA: 0x005A2450 File Offset: 0x005A0650
		public override void Notify_Event(EventPack ep)
		{
			if (this.NumPlaced() >= this.def.targetCount)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
