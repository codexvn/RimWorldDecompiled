using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200155B RID: 5467
	public class WorkGiver_ConstructSmoothFloor : WorkGiver_ConstructAffectFloor
	{
		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x060081C4 RID: 33220 RVA: 0x00222E38 File Offset: 0x00221038
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.SmoothFloor;
			}
		}

		// Token: 0x060081C5 RID: 33221 RVA: 0x00270CA3 File Offset: 0x0026EEA3
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.SmoothFloor, c);
		}
	}
}
