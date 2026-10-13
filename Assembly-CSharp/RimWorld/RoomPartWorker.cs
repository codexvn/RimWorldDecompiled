using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F47 RID: 8007
	public abstract class RoomPartWorker
	{
		// Token: 0x0600B11D RID: 45341 RVA: 0x00329C7E File Offset: 0x00327E7E
		public RoomPartWorker(RoomPartDef def)
		{
			this.def = def;
		}

		// Token: 0x17001B04 RID: 6916
		// (get) Token: 0x0600B11E RID: 45342 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool FillOnPost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600B11F RID: 45343
		public abstract void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints);

		// Token: 0x04007947 RID: 31047
		public readonly RoomPartDef def;
	}
}
