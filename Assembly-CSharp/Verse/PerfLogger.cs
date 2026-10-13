using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000D41 RID: 3393
	public static class PerfLogger
	{
		// Token: 0x06005659 RID: 22105 RVA: 0x001BDF49 File Offset: 0x001BC149
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x001BDF65 File Offset: 0x001BC165
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog != null) ? PerfLogger.currentLog.ToString() : "");
			PerfLogger.Reset();
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x001BDF8C File Offset: 0x001BC18C
		public static void Record(string label)
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (PerfLogger.currentLog == null)
			{
				PerfLogger.currentLog = new StringBuilder();
			}
			PerfLogger.currentLog.AppendLine(string.Format("{0}: {3}{1} ({2})", new object[]
			{
				(timestamp - PerfLogger.start) * 1000L / Stopwatch.Frequency,
				label,
				(timestamp - PerfLogger.current) * 1000L / Stopwatch.Frequency,
				new string(' ', PerfLogger.indent * 2)
			}));
			PerfLogger.current = timestamp;
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x001BE01E File Offset: 0x001BC21E
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x0600565D RID: 22109 RVA: 0x001BE02C File Offset: 0x001BC22C
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x0600565E RID: 22110 RVA: 0x001BE03A File Offset: 0x001BC23A
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04003EE1 RID: 16097
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04003EE2 RID: 16098
		private static long start;

		// Token: 0x04003EE3 RID: 16099
		private static long current;

		// Token: 0x04003EE4 RID: 16100
		private static int indent;
	}
}
