using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200297D RID: 10621
	public class Building_TrapReleaseHunter : Building_TrapReleaseEntity
	{
		// Token: 0x17002393 RID: 9107
		// (get) Token: 0x0600E95D RID: 59741 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override int CountToSpawn
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17002394 RID: 9108
		// (get) Token: 0x0600E95E RID: 59742 RVA: 0x0043EAE7 File Offset: 0x0043CCE7
		protected override PawnKindDef PawnToSpawn
		{
			get
			{
				return PawnKindDefOf.Drone_Hunter;
			}
		}
	}
}
