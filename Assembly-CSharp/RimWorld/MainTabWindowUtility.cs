using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A89 RID: 6793
	public static class MainTabWindowUtility
	{
		// Token: 0x06009858 RID: 39000 RVA: 0x002C7104 File Offset: 0x002C5304
		public static void NotifyAllPawnTables_PawnsChanged()
		{
			if (Find.WindowStack == null || !UnityData.IsInMainThread)
			{
				return;
			}
			WindowStack windowStack = Find.WindowStack;
			for (int i = 0; i < windowStack.Count; i++)
			{
				MainTabWindow_PawnTable mainTabWindow_PawnTable = windowStack[i] as MainTabWindow_PawnTable;
				if (mainTabWindow_PawnTable != null)
				{
					mainTabWindow_PawnTable.Notify_PawnsChanged();
				}
			}
		}
	}
}
