using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Verse
{
	// Token: 0x02000CAC RID: 3244
	public static class ProfilerPairValidation
	{
		// Token: 0x06005328 RID: 21288 RVA: 0x001ACCD1 File Offset: 0x001AAED1
		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x001ACCE4 File Offset: 0x001AAEE4
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace, stackTrace2));
				return;
			}
			for (int i = 0; i < stackTrace2.FrameCount; i++)
			{
				if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod() && (!(stackTrace.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck)) || !(stackTrace2.GetFrame(i).GetMethod().DeclaringType == typeof(ProfilerThreadCheck))) && (!(stackTrace.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic)) || !(stackTrace2.GetFrame(i).GetMethod() == typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))))
				{
					Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace, stackTrace2));
					return;
				}
			}
		}

		// Token: 0x04003C0F RID: 15375
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();
	}
}
