using System;

namespace Verse
{
	// Token: 0x02000133 RID: 307
	public static class UnityDataInitializer
	{
		// Token: 0x06000983 RID: 2435 RVA: 0x0002D478 File Offset: 0x0002B678
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}

		// Token: 0x040004BD RID: 1213
		public static bool initializing;
	}
}
