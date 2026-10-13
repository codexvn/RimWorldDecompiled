using System;

namespace Verse.AI.Group
{
	// Token: 0x020010F1 RID: 4337
	public enum TriggerSignalType : byte
	{
		// Token: 0x04004A8D RID: 19085
		Undefined,
		// Token: 0x04004A8E RID: 19086
		Tick,
		// Token: 0x04004A8F RID: 19087
		Memo,
		// Token: 0x04004A90 RID: 19088
		PawnDamaged,
		// Token: 0x04004A91 RID: 19089
		PawnArrestAttempted,
		// Token: 0x04004A92 RID: 19090
		PawnLost,
		// Token: 0x04004A93 RID: 19091
		BuildingDamaged,
		// Token: 0x04004A94 RID: 19092
		BuildingLost,
		// Token: 0x04004A95 RID: 19093
		FactionRelationsChanged,
		// Token: 0x04004A96 RID: 19094
		DormancyWakeup,
		// Token: 0x04004A97 RID: 19095
		Clamor,
		// Token: 0x04004A98 RID: 19096
		MechClusterDefeated,
		// Token: 0x04004A99 RID: 19097
		Signal,
		// Token: 0x04004A9A RID: 19098
		CorpseLost,
		// Token: 0x04004A9B RID: 19099
		AcquiredTarget
	}
}
