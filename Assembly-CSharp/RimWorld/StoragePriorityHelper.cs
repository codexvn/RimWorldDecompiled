using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029BD RID: 10685
	public static class StoragePriorityHelper
	{
		// Token: 0x0600EB92 RID: 60306 RVA: 0x00446F48 File Offset: 0x00445148
		public static string Label(this StoragePriority p)
		{
			switch (p)
			{
			case StoragePriority.Unstored:
				return "StoragePriorityUnstored".Translate();
			case StoragePriority.Low:
				return "StoragePriorityLow".Translate();
			case StoragePriority.Normal:
				return "StoragePriorityNormal".Translate();
			case StoragePriority.Preferred:
				return "StoragePriorityPreferred".Translate();
			case StoragePriority.Important:
				return "StoragePriorityImportant".Translate();
			case StoragePriority.Critical:
				return "StoragePriorityCritical".Translate();
			default:
				return "Unknown";
			}
		}
	}
}
