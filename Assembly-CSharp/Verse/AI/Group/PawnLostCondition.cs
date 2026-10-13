using System;

namespace Verse.AI.Group
{
	// Token: 0x0200108F RID: 4239
	public enum PawnLostCondition : byte
	{
		// Token: 0x040049E1 RID: 18913
		Undefined,
		// Token: 0x040049E2 RID: 18914
		Vanished,
		// Token: 0x040049E3 RID: 18915
		Incapped,
		// Token: 0x040049E4 RID: 18916
		Killed,
		// Token: 0x040049E5 RID: 18917
		MadePrisoner,
		// Token: 0x040049E6 RID: 18918
		ChangedFaction,
		// Token: 0x040049E7 RID: 18919
		ExitedMap,
		// Token: 0x040049E8 RID: 18920
		LeftVoluntarily,
		// Token: 0x040049E9 RID: 18921
		Drafted,
		// Token: 0x040049EA RID: 18922
		ForcedToJoinOtherLord,
		// Token: 0x040049EB RID: 18923
		ForcedByPlayerAction,
		// Token: 0x040049EC RID: 18924
		ForcedByQuest,
		// Token: 0x040049ED RID: 18925
		NoLongerEnteringTransportPods,
		// Token: 0x040049EE RID: 18926
		MadeSlave,
		// Token: 0x040049EF RID: 18927
		InMentalState,
		// Token: 0x040049F0 RID: 18928
		LordRejected
	}
}
