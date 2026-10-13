using System;

namespace Verse.AI
{
	// Token: 0x0200101E RID: 4126
	public static class WanderRoomUtility
	{
		// Token: 0x06006504 RID: 25860 RVA: 0x001FF0C8 File Offset: 0x001FD2C8
		public static bool IsValidWanderDest(Pawn pawn, IntVec3 loc, IntVec3 root)
		{
			Room room = root.GetRoom(pawn.Map);
			return room == null || room.IsDoorway || WanderUtility.InSameRoom(root, loc, pawn.Map);
		}
	}
}
