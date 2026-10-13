using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020034E0 RID: 13536
	public class Instruction_UndraftAll : Lesson_Instruction
	{
		// Token: 0x17003188 RID: 12680
		// (get) Token: 0x0601304E RID: 77902 RVA: 0x005A36EF File Offset: 0x005A18EF
		protected override float ProgressPercent
		{
			get
			{
				return 1f - (float)this.DraftedPawns().Count<Pawn>() / (float)base.Map.mapPawns.FreeColonistsSpawnedCount;
			}
		}

		// Token: 0x0601304F RID: 77903 RVA: 0x005A3715 File Offset: 0x005A1915
		private IEnumerable<Pawn> DraftedPawns()
		{
			return from p in base.Map.mapPawns.FreeColonistsSpawned
			where p.Drafted
			select p;
		}

		// Token: 0x06013050 RID: 77904 RVA: 0x005A374C File Offset: 0x005A194C
		public override void LessonUpdate()
		{
			foreach (Pawn pawn in this.DraftedPawns())
			{
				GenDraw.DrawArrowPointingAt(pawn.DrawPos, false);
			}
			if (this.ProgressPercent > 0.9999f)
			{
				Find.ActiveLesson.Deactivate();
			}
		}
	}
}
