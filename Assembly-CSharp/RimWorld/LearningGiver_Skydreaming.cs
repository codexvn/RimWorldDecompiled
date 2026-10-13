using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001520 RID: 5408
	public class LearningGiver_Skydreaming : LearningGiver
	{
		// Token: 0x060080C4 RID: 32964 RVA: 0x0026C914 File Offset: 0x0026AB14
		public override bool CanDo(Pawn pawn)
		{
			IntVec3 intVec;
			return base.CanDo(pawn) && RCellFinder.TryFindAllowedUnroofedSpotOutsideColony(pawn.Position, pawn, out intVec, 35, int.MaxValue, 45, null);
		}

		// Token: 0x060080C5 RID: 32965 RVA: 0x0026C944 File Offset: 0x0026AB44
		public override Job TryGiveJob(Pawn pawn)
		{
			IntVec3 c;
			if (!RCellFinder.TryFindAllowedUnroofedSpotOutsideColony(pawn.Position, pawn, out c, 35, 2147483647, 45, null))
			{
				return null;
			}
			return JobMaker.MakeJob(this.def.jobDef, c);
		}
	}
}
