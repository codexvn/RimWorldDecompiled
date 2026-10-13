using System;
using UnityEngine;
using Verse;
using Verse.Sound;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x02003356 RID: 13142
	public static class TimeControls
	{
		// Token: 0x06012784 RID: 75652 RVA: 0x00572C08 File Offset: 0x00570E08
		private static void PlaySoundOf(TimeSpeed speed)
		{
			SoundDef soundDef = null;
			switch (speed)
			{
			case TimeSpeed.Paused:
				soundDef = SoundDefOf.Clock_Stop;
				break;
			case TimeSpeed.Normal:
				soundDef = SoundDefOf.Clock_Normal;
				break;
			case TimeSpeed.Fast:
				soundDef = SoundDefOf.Clock_Fast;
				break;
			case TimeSpeed.Superfast:
				soundDef = SoundDefOf.Clock_Superfast;
				break;
			case TimeSpeed.Ultrafast:
				soundDef = SoundDefOf.Clock_Superfast;
				break;
			}
			if (soundDef != null)
			{
				soundDef.PlayOneShotOnCamera(null);
			}
			SteamDeck.Vibrate();
		}

		// Token: 0x06012785 RID: 75653 RVA: 0x00572C68 File Offset: 0x00570E68
		private static KeyBindingDef GetBindingFor(TimeSpeed speed)
		{
			switch (speed)
			{
			case TimeSpeed.Paused:
				return KeyBindingDefOf.TogglePause;
			case TimeSpeed.Normal:
				return KeyBindingDefOf.TimeSpeed_Normal;
			case TimeSpeed.Fast:
				return KeyBindingDefOf.TimeSpeed_Fast;
			case TimeSpeed.Superfast:
				return KeyBindingDefOf.TimeSpeed_Superfast;
			case TimeSpeed.Ultrafast:
				return KeyBindingDefOf.TimeSpeed_Ultrafast;
			default:
				return null;
			}
		}

		// Token: 0x06012786 RID: 75654 RVA: 0x00572CA8 File Offset: 0x00570EA8
		public static void DoTimeControlsGUI(Rect timerRect)
		{
			TickManager tickManager = Find.TickManager;
			Widgets.BeginGroup(timerRect);
			Rect rect = new Rect(0f, 0f, TimeControls.TimeButSize.x, TimeControls.TimeButSize.y);
			for (int i = 0; i < TimeControls.CachedTimeSpeedValues.Length; i++)
			{
				TimeSpeed timeSpeed = TimeControls.CachedTimeSpeedValues[i];
				if (timeSpeed != TimeSpeed.Ultrafast)
				{
					string arg = KeyPrefs.KeyPrefsData.GetBoundKeyCode(TimeControls.GetBindingFor(timeSpeed), KeyPrefs.BindingSlot.A).ToStringReadable();
					string tooltip = string.Format("{0}: {1}", "HotKeyTip".Translate(), arg);
					if (Widgets.ButtonImage(rect, TexButton.SpeedButtonTextures[(int)timeSpeed], true, tooltip) && !tickManager.ForcePaused)
					{
						if (timeSpeed == TimeSpeed.Paused)
						{
							tickManager.TogglePaused();
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
						}
						else
						{
							tickManager.CurTimeSpeed = timeSpeed;
							PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						}
						TimeControls.PlaySoundOf(tickManager.CurTimeSpeed);
					}
					if ((tickManager.ForcePaused ? TimeSpeed.Paused : tickManager.CurTimeSpeed) == timeSpeed)
					{
						GUI.DrawTexture(rect, TexUI.HighlightTex);
					}
					rect.x += rect.width;
				}
			}
			if (tickManager.slower.ForcedNormalSpeed)
			{
				Widgets.DrawLineHorizontal(rect.width * 2f, rect.height / 2f, rect.width * 2f);
			}
			if (tickManager.ForcePaused)
			{
				Widgets.DrawLineHorizontal(rect.width, rect.height / 2f, rect.width * 3f);
			}
			Widgets.EndGroup();
			GenUI.AbsorbClicksInRect(timerRect);
			UIHighlighter.HighlightOpportunity(timerRect, "TimeControls");
			if (Event.current.type == EventType.KeyDown)
			{
				if (KeyBindingDefOf.TogglePause.KeyDownEvent)
				{
					Find.TickManager.TogglePaused();
					TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
					Event.current.Use();
				}
				if (!Find.WindowStack.WindowsForcePause)
				{
					if (KeyBindingDefOf.TimeSpeed_Normal.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Fast.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Fast;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Superfast.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Superfast;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Slower.KeyDownEvent && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused)
					{
						TickManager tickManager2 = Find.TickManager;
						TimeSpeed curTimeSpeed = tickManager2.CurTimeSpeed;
						tickManager2.CurTimeSpeed = (TimeSpeed)(curTimeSpeed - TimeSpeed.Normal);
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
					if (KeyBindingDefOf.TimeSpeed_Faster.KeyDownEvent && Find.TickManager.CurTimeSpeed < TimeSpeed.Superfast)
					{
						TickManager tickManager3 = Find.TickManager;
						TimeSpeed curTimeSpeed = tickManager3.CurTimeSpeed;
						tickManager3.CurTimeSpeed = curTimeSpeed + 1;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
						Event.current.Use();
					}
				}
				if (Prefs.DevMode)
				{
					if (KeyBindingDefOf.TimeSpeed_Ultrafast.KeyDownEvent)
					{
						Find.TickManager.CurTimeSpeed = TimeSpeed.Ultrafast;
						TimeControls.PlaySoundOf(Find.TickManager.CurTimeSpeed);
						Event.current.Use();
					}
					if (KeyBindingDefOf.Dev_TickOnce.KeyDownEvent && tickManager.CurTimeSpeed == TimeSpeed.Paused)
					{
						tickManager.DoSingleTick();
						SoundDefOf.Clock_Stop.PlayOneShotOnCamera(null);
					}
				}
			}
		}

		// Token: 0x0400BFDF RID: 49119
		public static readonly Vector2 TimeButSize = new Vector2(32f, 24f);

		// Token: 0x0400BFE0 RID: 49120
		private static readonly TimeSpeed[] CachedTimeSpeedValues = (TimeSpeed[])Enum.GetValues(typeof(TimeSpeed));
	}
}
