using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D56 RID: 3414
	[StaticConstructorOnStartup]
	public static class ScreenFader
	{
		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x060056E8 RID: 22248 RVA: 0x001BF191 File Offset: 0x001BD391
		private static float CurTime
		{
			get
			{
				return Time.realtimeSinceStartup;
			}
		}

		// Token: 0x060056E9 RID: 22249 RVA: 0x001BF198 File Offset: 0x001BD398
		static ScreenFader()
		{
			ScreenFader.fadeTexture = new Texture2D(1, 1);
			ScreenFader.fadeTexture.name = "ScreenFader";
			ScreenFader.backgroundStyle.normal.background = ScreenFader.fadeTexture;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060056EA RID: 22250 RVA: 0x001BF23C File Offset: 0x001BD43C
		public static void OverlayOnGUI(Vector2 windowSize)
		{
			Color color = ScreenFader.CurrentInstantColor();
			if (color.a > 0f)
			{
				if (ScreenFader.fadeTextureDirty)
				{
					ScreenFader.fadeTexture.SetPixel(0, 0, color);
					ScreenFader.fadeTexture.Apply();
				}
				GUI.Label(new Rect(-10f, -10f, windowSize.x + 10f, windowSize.y + 10f), ScreenFader.fadeTexture, ScreenFader.backgroundStyle);
			}
		}

		// Token: 0x060056EB RID: 22251 RVA: 0x001BF2B0 File Offset: 0x001BD4B0
		private static Color CurrentInstantColor()
		{
			if (ScreenFader.CurTime > ScreenFader.targetTime || ScreenFader.targetTime == ScreenFader.sourceTime)
			{
				return ScreenFader.targetColor;
			}
			return Color.Lerp(ScreenFader.sourceColor, ScreenFader.targetColor, (ScreenFader.CurTime - ScreenFader.sourceTime) / (ScreenFader.targetTime - ScreenFader.sourceTime));
		}

		// Token: 0x060056EC RID: 22252 RVA: 0x001BF301 File Offset: 0x001BD501
		public static void SetColor(Color newColor)
		{
			ScreenFader.sourceColor = newColor;
			ScreenFader.targetColor = newColor;
			ScreenFader.targetTime = 0f;
			ScreenFader.sourceTime = 0f;
			ScreenFader.fadeTextureDirty = true;
		}

		// Token: 0x060056ED RID: 22253 RVA: 0x001BF329 File Offset: 0x001BD529
		public static void StartFade(Color finalColor, float duration)
		{
			ScreenFader.StartFade(finalColor, duration, 0f);
		}

		// Token: 0x060056EE RID: 22254 RVA: 0x001BF337 File Offset: 0x001BD537
		public static void StartFade(Color finalColor, float duration, float delay)
		{
			if (duration <= 0f)
			{
				ScreenFader.SetColor(finalColor);
				return;
			}
			ScreenFader.sourceColor = ScreenFader.CurrentInstantColor();
			ScreenFader.targetColor = finalColor;
			ScreenFader.sourceTime = ScreenFader.CurTime + delay;
			ScreenFader.targetTime = ScreenFader.sourceTime + duration;
		}

		// Token: 0x060056EF RID: 22255 RVA: 0x001BF370 File Offset: 0x001BD570
		public static bool IsFading()
		{
			return Current.ProgramState == ProgramState.Playing && !LongEventHandler.ShouldWaitForEvent && ScreenFader.CurTime < ScreenFader.targetTime;
		}

		// Token: 0x04003F42 RID: 16194
		private static GUIStyle backgroundStyle = new GUIStyle();

		// Token: 0x04003F43 RID: 16195
		private static Texture2D fadeTexture;

		// Token: 0x04003F44 RID: 16196
		private static Color sourceColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F45 RID: 16197
		private static Color targetColor = new Color(0f, 0f, 0f, 0f);

		// Token: 0x04003F46 RID: 16198
		private static float sourceTime = 0f;

		// Token: 0x04003F47 RID: 16199
		private static float targetTime = 0f;

		// Token: 0x04003F48 RID: 16200
		private static bool fadeTextureDirty = true;
	}
}
