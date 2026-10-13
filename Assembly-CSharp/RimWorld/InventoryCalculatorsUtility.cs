using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200366A RID: 13930
	public static class InventoryCalculatorsUtility
	{
		// Token: 0x06013569 RID: 79209 RVA: 0x005C0D68 File Offset: 0x005BEF68
		public static bool ShouldIgnoreInventoryOf(Pawn pawn, IgnorePawnsInventoryMode ignoreMode)
		{
			switch (ignoreMode)
			{
			case IgnorePawnsInventoryMode.Ignore:
				return true;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload:
				return pawn.Spawned && pawn.inventory.UnloadEverything;
			case IgnorePawnsInventoryMode.IgnoreIfAssignedToUnloadOrPlayerPawn:
				return (pawn.Spawned && pawn.inventory.UnloadEverything) || Dialog_FormCaravan.CanListInventorySeparately(pawn);
			case IgnorePawnsInventoryMode.DontIgnore:
				return false;
			default:
				throw new NotImplementedException("IgnorePawnsInventoryMode");
			}
		}
	}
}
