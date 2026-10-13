using System;
using System.Diagnostics;

namespace Verse
{
	// Token: 0x02000D55 RID: 3413
	public static class ProfilerThreadCheck
	{
		// Token: 0x060056E6 RID: 22246 RVA: 0x001BF189 File Offset: 0x001BD389
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}

		// Token: 0x060056E7 RID: 22247 RVA: 0x001BF189 File Offset: 0x001BD389
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			bool isInMainThread = UnityData.IsInMainThread;
		}
	}
}
