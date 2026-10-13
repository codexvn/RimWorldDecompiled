using System;

namespace Verse
{
	// Token: 0x020002D9 RID: 729
	public abstract class RoomRoleWorker
	{
		// Token: 0x060011FF RID: 4607 RVA: 0x00009C2B File Offset: 0x00007E2B
		public virtual string PostProcessedLabel(string baseLabel, Room room)
		{
			return baseLabel;
		}

		// Token: 0x06001200 RID: 4608
		public abstract float GetScore(Room room);

		// Token: 0x06001201 RID: 4609 RVA: 0x00006AAC File Offset: 0x00004CAC
		public virtual float GetScoreDeltaIfBuildingPlaced(Room room, ThingDef buildingDef)
		{
			return 0f;
		}
	}
}
