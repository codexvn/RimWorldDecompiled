using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200008B RID: 139
	[StaticConstructorOnStartup]
	public class Dialog_DevCelestial : Window_Dev
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000FF5A File Offset: 0x0000E15A
		protected override float Margin
		{
			get
			{
				return 4f;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000FF61 File Offset: 0x0000E161
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(230f, 275f);
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0000FF74 File Offset: 0x0000E174
		public Dialog_DevCelestial()
		{
			this.draggable = true;
			this.focusWhenOpened = false;
			this.drawShadow = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
			this.drawInScreenshotMode = false;
			this.windowPosition = Prefs.DevPalettePosition;
			this.onlyDrawInDevMode = true;
			this.doCloseX = true;
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0000FFD4 File Offset: 0x0000E1D4
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect.x, inRect.y, inRect.width, 24f);
			DevGUI.Label(rect, "Celestial Debugger");
			float num = rect.height + 6f;
			Text.Font = GameFont.Tiny;
			Map currentMap = Find.CurrentMap;
			GenCelestial.LightInfo lightSourceInfo = GenCelestial.GetLightSourceInfo(currentMap, GenCelestial.LightType.Shadow);
			GenCelestial.LightInfo lightSourceInfo2 = GenCelestial.GetLightSourceInfo(currentMap, GenCelestial.LightType.LightingSun);
			Vector2 vector = Find.WorldGrid.LongLatOf(currentMap.Tile);
			this.PrintLabel("Map", inRect, ref num);
			this.PrintLabel(string.Format("Day progress: {0:0}%", GenLocalDate.DayPercent(currentMap) * 100f), inRect, ref num);
			this.PrintLabel(string.Format("Long, Lat: {0:0.0}, {1:0.0}", vector.x, vector.y), inRect, ref num);
			this.PrintLabel("", inRect, ref num);
			this.PrintLabel("Shadow Info", inRect, ref num);
			this.PrintLabel(string.Format("Vector: {0}", lightSourceInfo.vector), inRect, ref num);
			this.PrintLabel(string.Format("Intensity: {0:0}%", lightSourceInfo.intensity * 100f), inRect, ref num);
			this.PrintLabel("", inRect, ref num);
			this.PrintLabel("Sun Info", inRect, ref num);
			this.PrintLabel(string.Format("Vector: {0}", lightSourceInfo2.vector), inRect, ref num);
			this.PrintLabel(string.Format("Intensity: {0:0}%", lightSourceInfo2.intensity * 100f), inRect, ref num);
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001016B File Offset: 0x0000E36B
		private void PrintLabel(string text, Rect container, ref float y)
		{
			DevGUI.Label(new Rect(container.x, y, container.width, 20f), text);
			y += 20f;
		}

		// Token: 0x040001B7 RID: 439
		private Vector2 windowPosition;

		// Token: 0x040001B8 RID: 440
		private const string Title = "Celestial Debugger";
	}
}
