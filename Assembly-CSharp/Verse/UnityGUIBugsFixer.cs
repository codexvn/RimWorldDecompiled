using System;
using System.Collections.Generic;
using UnityEngine;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000C0D RID: 3085
	public static class UnityGUIBugsFixer
	{
		// Token: 0x17000C0E RID: 3086
		// (get) Token: 0x06004EA3 RID: 20131 RVA: 0x001910A7 File Offset: 0x0018F2A7
		public static bool IsSteamDeckOrLinuxBuild
		{
			get
			{
				return SteamDeck.IsSteamDeck || Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer;
			}
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x06004EA4 RID: 20132 RVA: 0x001910C4 File Offset: 0x0018F2C4
		public static List<Resolution> ScreenResolutionsWithoutDuplicates
		{
			get
			{
				UnityGUIBugsFixer.resolutions.Clear();
				Resolution[] array = Screen.resolutions;
				for (int i = 0; i < array.Length; i++)
				{
					bool flag = false;
					for (int j = 0; j < UnityGUIBugsFixer.resolutions.Count; j++)
					{
						if (UnityGUIBugsFixer.resolutions[j].width == array[i].width && UnityGUIBugsFixer.resolutions[j].height == array[i].height)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						UnityGUIBugsFixer.resolutions.Add(array[i]);
					}
				}
				return UnityGUIBugsFixer.resolutions;
			}
		}

		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x06004EA5 RID: 20133 RVA: 0x00191167 File Offset: 0x0018F367
		public static Vector2 CurrentEventDelta
		{
			get
			{
				return UnityGUIBugsFixer.currentEventDelta;
			}
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x0019116E File Offset: 0x0018F36E
		public static bool MouseDrag(int button = 0)
		{
			if (!UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				return Event.current.type == EventType.MouseDrag && Event.current.button == button;
			}
			return Input.GetMouseButton(button);
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x0019119A File Offset: 0x0018F39A
		public static void OnGUI()
		{
			UnityGUIBugsFixer.RememberMouseStateForIsLeftMouseButtonPressed();
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
			UnityGUIBugsFixer.FixScrolling();
			UnityGUIBugsFixer.FixShift();
			UnityGUIBugsFixer.FixDelta();
			UnityGUIBugsFixer.EnsureSliderDragReset();
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x001911BC File Offset: 0x0018F3BC
		private static void FixScrolling()
		{
			if (Event.current.type == EventType.ScrollWheel && (Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer))
			{
				Vector2 delta = Event.current.delta;
				Event.current.delta = new Vector2(delta.x, delta.y * -6f);
			}
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x00191214 File Offset: 0x0018F414
		private static void FixShift()
		{
			if ((Application.platform == RuntimePlatform.LinuxEditor || Application.platform == RuntimePlatform.LinuxPlayer) && !Event.current.shift)
			{
				Event.current.shift = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
			}
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x00191262 File Offset: 0x0018F462
		public static bool ResolutionsEqual(IntVec2 a, IntVec2 b)
		{
			return a == b;
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x0019126C File Offset: 0x0018F46C
		private static void FixDelta()
		{
			Vector2 mousePositionOnUIInverted = UI.MousePositionOnUIInverted;
			if (UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				if (Event.current.rawType == EventType.MouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.type != EventType.Repaint)
				{
					UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					return;
				}
				if (Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = mousePositionOnUIInverted - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else if (Event.current.rawType == EventType.MouseDrag)
			{
				if (mousePositionOnUIInverted != UnityGUIBugsFixer.lastMousePosition || Time.frameCount != UnityGUIBugsFixer.lastMousePositionFrame)
				{
					if (UnityGUIBugsFixer.lastMousePosition != null)
					{
						UnityGUIBugsFixer.currentEventDelta = mousePositionOnUIInverted - UnityGUIBugsFixer.lastMousePosition.Value;
					}
					else
					{
						UnityGUIBugsFixer.currentEventDelta = default(Vector2);
					}
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
			}
			else
			{
				UnityGUIBugsFixer.currentEventDelta = Event.current.delta;
				if (Event.current.rawType == EventType.MouseDown)
				{
					UnityGUIBugsFixer.lastMousePosition = new Vector2?(mousePositionOnUIInverted);
					UnityGUIBugsFixer.lastMousePositionFrame = Time.frameCount;
					return;
				}
				if (Event.current.rawType == EventType.MouseUp)
				{
					UnityGUIBugsFixer.lastMousePosition = null;
				}
			}
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x001913E3 File Offset: 0x0018F5E3
		private static void EnsureSliderDragReset()
		{
			if (Event.current.type == EventType.MouseUp)
			{
				Widgets.ResetSliderDraggingIDs();
			}
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x001913F7 File Offset: 0x0018F5F7
		public static void Notify_BeginGroup()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x001913F7 File Offset: 0x0018F5F7
		public static void Notify_EndGroup()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x001913F7 File Offset: 0x0018F5F7
		public static void Notify_BeginScrollView()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x06004EB0 RID: 20144 RVA: 0x001913F7 File Offset: 0x0018F5F7
		public static void Notify_EndScrollView()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x06004EB1 RID: 20145 RVA: 0x001913F7 File Offset: 0x0018F5F7
		public static void Notify_GUIMatrixChanged()
		{
			UnityGUIBugsFixer.FixSteamDeckMousePositionNeverUpdating();
		}

		// Token: 0x06004EB2 RID: 20146 RVA: 0x00191400 File Offset: 0x0018F600
		private static void FixSteamDeckMousePositionNeverUpdating()
		{
			if (UnityGUIBugsFixer.IsSteamDeckOrLinuxBuild)
			{
				Vector2 vector = UI.MousePositionOnUIInverted;
				vector = GUIUtility.ScreenToGUIPoint(vector * Prefs.UIScale);
				Event.current.mousePosition = vector;
			}
		}

		// Token: 0x06004EB3 RID: 20147 RVA: 0x00191436 File Offset: 0x0018F636
		private static void RememberMouseStateForIsLeftMouseButtonPressed()
		{
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
			{
				UnityGUIBugsFixer.leftMouseButtonPressed = true;
				return;
			}
			if (Event.current.rawType == EventType.MouseUp && Event.current.button == 0)
			{
				UnityGUIBugsFixer.leftMouseButtonPressed = false;
			}
		}

		// Token: 0x06004EB4 RID: 20148 RVA: 0x00191476 File Offset: 0x0018F676
		public static bool IsLeftMouseButtonPressed()
		{
			return Input.GetMouseButton(0) || UnityGUIBugsFixer.leftMouseButtonPressed;
		}

		// Token: 0x04003871 RID: 14449
		private static List<Resolution> resolutions = new List<Resolution>();

		// Token: 0x04003872 RID: 14450
		private static Vector2 currentEventDelta;

		// Token: 0x04003873 RID: 14451
		private static int lastMousePositionFrame;

		// Token: 0x04003874 RID: 14452
		private static bool leftMouseButtonPressed;

		// Token: 0x04003875 RID: 14453
		private const float ScrollFactor = -6f;

		// Token: 0x04003876 RID: 14454
		private static Vector2? lastMousePosition;
	}
}
