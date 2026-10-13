using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034CF RID: 13519
	public class Instruction_ChopWood : Lesson_Instruction
	{
		// Token: 0x1700317D RID: 12669
		// (get) Token: 0x06013001 RID: 77825 RVA: 0x005A2AEC File Offset: 0x005A0CEC
		protected override float ProgressPercent
		{
			get
			{
				return (float)(from d in base.Map.designationManager.SpawnedDesignationsOfDef(DesignationDefOf.HarvestPlant)
				where d.target.Thing.def.plant.IsTree
				select d).Count<Designation>() / (float)this.def.targetCount;
			}
		}

		// Token: 0x06013002 RID: 77826 RVA: 0x005A2B45 File Offset: 0x005A0D45
		public override void LessonUpdate()
		{
			if (this.ProgressPercent > 0.999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
