using System;
using UnityEngine;
using Verse;

// Token: 0x02000041 RID: 65
public static class PerformanceBenchmarkUtility
{
	// Token: 0x0600015C RID: 348 RVA: 0x000095FB File Offset: 0x000077FB
	public static void StartBenchmark()
	{
		PerformanceBenchmarkUtility.startBenchmarkTime = Time.realtimeSinceStartup;
		PerformanceBenchmarkUtility.startBenchmarkTicks = Find.TickManager.TicksGame;
		PerformanceBenchmarkUtility.startBenchmarkFrames = Time.frameCount;
	}

	// Token: 0x0600015D RID: 349 RVA: 0x00009620 File Offset: 0x00007820
	public static void CheckBenchmark()
	{
		if (PerformanceBenchmarkUtility.startBenchmarkTime > 0f && PerformanceBenchmarkUtility.startBenchmarkTime + 30f < Time.realtimeSinceStartup)
		{
			float num = Time.realtimeSinceStartup - PerformanceBenchmarkUtility.startBenchmarkTime;
			int num2 = Time.frameCount - PerformanceBenchmarkUtility.startBenchmarkFrames;
			int num3 = Find.TickManager.TicksGame - PerformanceBenchmarkUtility.startBenchmarkTicks;
			Dialog_MessageBox window = Dialog_MessageBox.CreateConfirmation(string.Concat(new string[]
			{
				string.Format("Frames per second: {0}\n", (float)num2 / num),
				string.Format("Ticks per second: {0}\n", (float)num3 / num),
				string.Format("Ticks + Frames per second: {0}\n", (float)(num3 + num2) / num),
				string.Format("Ticks / Frame: {0}\n\n", (float)num3 / (float)num2),
				"----RAW----\n",
				string.Format("Time elapsed: {0}s\n", num),
				string.Format("Frames: {0}\n", num2),
				string.Format("Game Ticks: {0}\n\n", num3),
				string.Format("Note: Each frame the game tries to do <tickrate> ticks or as many ticks as it can before {0}ms elapses. This means that sometimes tickrate, not framerate will increase as performance improves if the game is consistently not completing <tickrate> ticks per frame. Framerate can increase if performance improves while the game is consistently completing <tickrate> ticks per frame.", Mathf.RoundToInt(45.454544f))
			}), delegate()
			{
			}, false, null, WindowLayer.Dialog);
			PerformanceBenchmarkUtility.startBenchmarkTime = -1f;
			Find.WindowStack.Add(window);
		}
	}

	// Token: 0x040000A4 RID: 164
	public const float BenchmarkSeconds = 30f;

	// Token: 0x040000A5 RID: 165
	public static float startBenchmarkTime = -1f;

	// Token: 0x040000A6 RID: 166
	public static int startBenchmarkFrames = -1;

	// Token: 0x040000A7 RID: 167
	public static int startBenchmarkTicks = -1;
}
