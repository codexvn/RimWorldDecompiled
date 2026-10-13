using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E7A RID: 15994
	public class WorldGlobalControls
	{
		// Token: 0x06015E10 RID: 89616 RVA: 0x0068BA9C File Offset: 0x00689C9C
		public void WorldGlobalControlsOnGUI()
		{
			if (Event.current.type == EventType.Layout)
			{
				return;
			}
			float leftX = (float)UI.screenWidth - 250f;
			float num = (float)UI.screenHeight - 4f;
			if (Current.ProgramState == ProgramState.Playing)
			{
				num -= 35f;
			}
			GlobalControlsUtility.DoPlaySettings(this.rowVisibility, true, ref num);
			if (Current.ProgramState == ProgramState.Playing)
			{
				num -= 4f;
				GlobalControlsUtility.DoTimespeedControls(leftX, 250f, ref num);
				if (Find.CurrentMap != null || Find.WorldSelector.AnyObjectOrTileSelected)
				{
					num -= 4f;
					GlobalControlsUtility.DoDate(leftX, 250f, ref num);
				}
				float num2 = 154f;
				float num3 = Find.World.gameConditionManager.TotalHeightAt(num2);
				Rect rect = new Rect((float)UI.screenWidth - num2, num - num3, num2, num3);
				Find.World.gameConditionManager.DoConditionsUI(rect);
				num -= num3;
			}
			if (DebugViewSettings.showMemoryInfo)
			{
				GlobalControlsUtility.DrawMemoryInfo(leftX, 250f, ref num);
			}
			if (DebugViewSettings.showTpsCounter)
			{
				GlobalControlsUtility.DrawTpsCounter(leftX, 250f, ref num);
			}
			if (DebugViewSettings.showFpsCounter)
			{
				GlobalControlsUtility.DrawFpsCounter(leftX, 250f, ref num);
			}
			if (Prefs.ShowRealtimeClock)
			{
				GlobalControlsUtility.DoRealtimeClock(leftX, 250f, ref num);
			}
			if (!Find.WorldTargeter.IsTargeting)
			{
				Find.WorldRoutePlanner.DoRoutePlannerButton(ref num);
			}
			if (!Find.PlaySettings.lockNorthUp)
			{
				CompassWidget.CompassOnGUI(ref num);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				num -= 10f;
				Find.LetterStack.LettersOnGUI(num);
			}
		}

		// Token: 0x0400F2BF RID: 62143
		private WidgetRow rowVisibility = new WidgetRow();

		// Token: 0x0400F2C0 RID: 62144
		public const float Width = 250f;
	}
}
