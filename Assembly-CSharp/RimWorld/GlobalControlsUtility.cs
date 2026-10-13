using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003355 RID: 13141
	public static class GlobalControlsUtility
	{
		// Token: 0x0601277C RID: 75644 RVA: 0x0057272C File Offset: 0x0057092C
		public static void DoPlaySettings(WidgetRow rowVisibility, bool worldView, ref float curBaseY)
		{
			float y = curBaseY - TimeControls.TimeButSize.y;
			rowVisibility.Init((float)UI.screenWidth, y, UIDirection.LeftThenUp, 141f, 4f);
			Find.PlaySettings.DoPlaySettingsGlobalControls(rowVisibility, worldView);
			curBaseY = rowVisibility.FinalY;
		}

		// Token: 0x0601277D RID: 75645 RVA: 0x00572774 File Offset: 0x00570974
		public static void DoTimespeedControls(float leftX, float width, ref float curBaseY)
		{
			leftX += Mathf.Max(0f, width - 150f);
			width = Mathf.Min(width, 150f);
			float y = TimeControls.TimeButSize.y;
			Rect timerRect = new Rect(leftX + 16f, curBaseY - y, width, y);
			TimeControls.DoTimeControlsGUI(timerRect);
			curBaseY -= timerRect.height;
		}

		// Token: 0x0601277E RID: 75646 RVA: 0x005727D4 File Offset: 0x005709D4
		public static void DoDate(float leftX, float width, ref float curBaseY)
		{
			Rect dateRect = new Rect(leftX, curBaseY - DateReadout.Height, width, DateReadout.Height);
			DateReadout.DateOnGUI(dateRect);
			curBaseY -= dateRect.height;
		}

		// Token: 0x0601277F RID: 75647 RVA: 0x0057280C File Offset: 0x00570A0C
		public static void DoRealtimeClock(float leftX, float width, ref float curBaseY)
		{
			Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f);
			string format = Prefs.TwelveHourClockMode ? "hh:mm" : "HH:mm";
			string str = "";
			if (Prefs.TwelveHourClockMode)
			{
				str = string.Format(" {0}", (DateTime.Now.Hour >= 12) ? "PM".Translate() : "AM".Translate());
			}
			using (new TextBlock(TextAnchor.MiddleRight))
			{
				Widgets.Label(rect, DateTime.Now.ToString(format) + str);
			}
			curBaseY -= 26f;
		}

		// Token: 0x06012780 RID: 75648 RVA: 0x005728E8 File Offset: 0x00570AE8
		public static void DrawFpsCounter(float leftX, float width, ref float curBaseY)
		{
			float averageFrameTime = Root.AverageFrameTime;
			float num = 1000f / averageFrameTime;
			Rect rect = new Rect(leftX, curBaseY - 26f, width - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, string.Format("FPS: {0:F1} ({1:F2}ms/frame)", num, averageFrameTime));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x06012781 RID: 75649 RVA: 0x00572950 File Offset: 0x00570B50
		public static void DrawTpsCounter(float leftX, float width, ref float curBaseY)
		{
			float meanTickTime = Find.TickManager.MeanTickTime;
			float num = 1000f / meanTickTime;
			float b = 60f * Find.TickManager.TickRateMultiplier;
			num = Mathf.Min(num, b);
			Rect rect = new Rect(leftX, curBaseY - 26f, width - 7f, 26f);
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, string.Format("TPS: {0:F1} ({1:F2}ms/tick)", num, meanTickTime));
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 26f;
		}

		// Token: 0x06012782 RID: 75650 RVA: 0x005729D4 File Offset: 0x00570BD4
		private static void FormatMemoryUsage(StringBuilder sb, long bytes)
		{
			if (bytes > 1073741824L)
			{
				sb.Append(((float)bytes / 1.0737418E+09f).ToString("F2"));
				sb.Append(" GiB");
				return;
			}
			if (bytes > 1048576L)
			{
				sb.Append(((float)bytes / 1048576f).ToString("F0"));
				sb.Append(" MiB");
				return;
			}
			if (bytes > 1024L)
			{
				sb.Append(((float)bytes / 1024f).ToString("F0"));
				sb.Append(" KiB");
				return;
			}
			sb.Append(bytes.ToString("F0"));
			sb.Append(" B");
		}

		// Token: 0x06012783 RID: 75651 RVA: 0x00572A98 File Offset: 0x00570C98
		public static void DrawMemoryInfo(float leftX, float width, ref float curBaseY)
		{
			long trackedMemoryUsageBytes = MemoryUsageUtility.TrackedMemoryUsageBytes;
			long osMemoryUsageBytes = MemoryUsageUtility.OsMemoryUsageBytes;
			long graphicsMemoryUsageBytes = MemoryUsageUtility.GraphicsMemoryUsageBytes;
			long managedMemoryUsageBytes = MemoryUsageUtility.ManagedMemoryUsageBytes;
			long managedMemoryReservedBytes = MemoryUsageUtility.ManagedMemoryReservedBytes;
			long audioMemoryUsageBytes = MemoryUsageUtility.AudioMemoryUsageBytes;
			long textureMemoryUsageBytes = MemoryUsageUtility.TextureMemoryUsageBytes;
			Rect rect = new Rect(leftX, curBaseY - 104f, width - 7f, 104f);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Memory: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, osMemoryUsageBytes);
			stringBuilder.AppendLine();
			stringBuilder.Append("(");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, trackedMemoryUsageBytes);
			stringBuilder.AppendLine(" tracked)");
			stringBuilder.Append("C#: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, managedMemoryUsageBytes);
			stringBuilder.Append("/");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, managedMemoryReservedBytes);
			stringBuilder.AppendLine();
			stringBuilder.Append("VRAM: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, graphicsMemoryUsageBytes);
			stringBuilder.AppendLine();
			stringBuilder.Append("(");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, textureMemoryUsageBytes);
			stringBuilder.AppendLine(" by textures)");
			stringBuilder.Append("Audio: ");
			GlobalControlsUtility.FormatMemoryUsage(stringBuilder, audioMemoryUsageBytes);
			if (Mouse.IsOver(rect))
			{
				Widgets.DrawHighlight(rect);
				TooltipHandler.TipRegion(rect, new TipSignal("Memory usage may not be accurate in optimized builds.", 5670913));
			}
			Text.Anchor = TextAnchor.MiddleRight;
			Widgets.Label(rect, stringBuilder.ToString());
			Text.Anchor = TextAnchor.UpperLeft;
			curBaseY -= 104f;
		}

		// Token: 0x0400BFDE RID: 49118
		private const int VisibilityControlsPerRow = 5;
	}
}
