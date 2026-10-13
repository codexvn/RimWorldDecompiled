using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200297E RID: 10622
	public class Building_TrapReleaseWasp : Building_TrapReleaseEntity
	{
		// Token: 0x17002395 RID: 9109
		// (get) Token: 0x0600E960 RID: 59744 RVA: 0x00075162 File Offset: 0x00073362
		protected override int CountToSpawn
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17002396 RID: 9110
		// (get) Token: 0x0600E961 RID: 59745 RVA: 0x0043EAF6 File Offset: 0x0043CCF6
		protected override PawnKindDef PawnToSpawn
		{
			get
			{
				return PawnKindDefOf.Drone_Wasp;
			}
		}
	}
}
