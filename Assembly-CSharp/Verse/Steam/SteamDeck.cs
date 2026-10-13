using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RimWorld;
using Steamworks;
using UnityEngine;

namespace Verse.Steam
{
	// Token: 0x02000D8A RID: 3466
	public static class SteamDeck
	{
		// Token: 0x17000E03 RID: 3587
		// (get) Token: 0x06005869 RID: 22633 RVA: 0x001C81F6 File Offset: 0x001C63F6
		public static bool IsSteamDeck
		{
			get
			{
				return SteamDeck.isSteamDeck || DebugSettings.simulateUsingSteamDeck;
			}
		}

		// Token: 0x17000E04 RID: 3588
		// (get) Token: 0x0600586A RID: 22634 RVA: 0x001C8206 File Offset: 0x001C6406
		public static bool IsSteamDeckInNonKeyboardMode
		{
			get
			{
				return SteamDeck.IsSteamDeck && !Prefs.SteamDeckKeyboardMode;
			}
		}

		// Token: 0x17000E05 RID: 3589
		// (get) Token: 0x0600586B RID: 22635 RVA: 0x001C8219 File Offset: 0x001C6419
		public static bool KeyboardShowing
		{
			get
			{
				return SteamDeck.keyboardShowing;
			}
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x001C8220 File Offset: 0x001C6420
		public static void Init()
		{
			SteamDeck.isSteamDeck = SteamUtils.IsSteamRunningOnSteamDeck();
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			SteamInput.Init(false);
			SteamDeck.keyboardDismissedCallback = Callback<FloatingGamepadTextInputDismissed_t>.Create(new Callback<FloatingGamepadTextInputDismissed_t>.DispatchDelegate(SteamDeck.KeyboardDismissedCallback));
		}

		// Token: 0x0600586D RID: 22637 RVA: 0x001C8254 File Offset: 0x001C6454
		public static void ShowOnScreenKeyboard(string initialText, Rect textFieldRect, bool multiline)
		{
			if (!SteamDeck.isSteamDeck || SteamDeck.keyboardShowing)
			{
				return;
			}
			Rect rect = GUIUtility.GUIToScreenRect(textFieldRect);
			if (SteamUtils.ShowFloatingGamepadTextInput(multiline ? EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeMultipleLines : EFloatingGamepadTextInputMode.k_EFloatingGamepadTextInputModeModeSingleLine, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height))
			{
				SteamDeck.keyboardShowing = true;
			}
		}

		// Token: 0x0600586E RID: 22638 RVA: 0x001C82AB File Offset: 0x001C64AB
		public static void HideOnScreenKeyboard()
		{
			if (!SteamDeck.isSteamDeck || !SteamDeck.keyboardShowing)
			{
				return;
			}
			SteamUtils.DismissFloatingGamepadTextInput();
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x001C82C2 File Offset: 0x001C64C2
		public static void ShowConfigPage()
		{
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			SteamInput.ShowBindingPanel(new InputHandle_t(0UL));
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x001C82D9 File Offset: 0x001C64D9
		public static void Shutdown()
		{
			if (SteamDeck.isSteamDeck)
			{
				SteamInput.Shutdown();
			}
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x001C82E8 File Offset: 0x001C64E8
		public static void Update()
		{
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			if (SteamDeck.IsSteamDeckInNonKeyboardMode)
			{
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor != null && (textEditor.controlID != SteamDeck.lastFocusedTextFieldID || textEditor.cursorIndex != SteamDeck.lastFocusedTextFieldCursorIndex) && textEditor.position != default(Rect) && !UnityGUIBugsFixer.IsLeftMouseButtonPressed())
				{
					SteamDeck.lastFocusedTextFieldID = textEditor.controlID;
					SteamDeck.lastFocusedTextFieldCursorIndex = textEditor.cursorIndex;
					SteamDeck.ShowOnScreenKeyboard(textEditor.text, textEditor.position, textEditor.multiline);
					return;
				}
			}
			else
			{
				SteamDeck.keyboardShowing = false;
			}
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x001C8390 File Offset: 0x001C6590
		public static void OnGUI()
		{
			if (SteamDeck.keyboardShowing)
			{
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				string text = textEditor.text;
				if (!text.NullOrEmpty())
				{
					Text.Font = GameFont.Medium;
					Text.Anchor = TextAnchor.UpperCenter;
					float height = Text.CalcHeight(text, 464f) + 36f;
					float num = Mathf.Min(Text.CalcSize(text).x, 464f) + 36f;
					Text.Anchor = TextAnchor.UpperLeft;
					Text.Font = GameFont.Small;
					Rect rect = new Rect((float)UI.screenWidth / 2f - num / 2f, 30f, num, height);
					Find.WindowStack.ImmediateWindow(84906312, rect, WindowLayer.Super, delegate
					{
						Text.Font = GameFont.Medium;
						Text.Anchor = TextAnchor.UpperCenter;
						Rect rect = rect.AtZero().ContractedBy(18f);
						rect.height += 10f;
						Widgets.Label(rect, text);
						Text.Anchor = TextAnchor.UpperLeft;
						Text.Font = GameFont.Small;
					}, true, false, 1f, null, false);
				}
			}
		}

		// Token: 0x06005873 RID: 22643 RVA: 0x001C8484 File Offset: 0x001C6684
		public static void Vibrate()
		{
			if (!SteamDeck.isSteamDeck)
			{
				return;
			}
			SteamInput.TriggerVibration(new InputHandle_t(0UL), 10000, 10000);
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x001C84A4 File Offset: 0x001C66A4
		public static void RootOnGUI()
		{
			if (!SteamDeck.IsSteamDeck)
			{
				return;
			}
			SteamDeck.SimulateRightClickIfHoldingMiddleButton();
			if (SteamDeck.unfocusCurrentTextField && Event.current.type == EventType.Repaint)
			{
				SteamDeck.unfocusCurrentTextField = false;
				UI.UnfocusCurrentTextField();
				TextEditor textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (textEditor != null)
				{
					SteamDeck.lastFocusedTextFieldID = textEditor.controlID;
					SteamDeck.lastFocusedTextFieldCursorIndex = textEditor.cursorIndex;
				}
			}
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x001C8510 File Offset: 0x001C6710
		public static void WindowOnGUI()
		{
			if (!SteamDeck.IsSteamDeck)
			{
				return;
			}
			SteamDeck.SimulateRightClickIfHoldingMiddleButton();
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x001C8520 File Offset: 0x001C6720
		private static void SimulateRightClickIfHoldingMiddleButton()
		{
			if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp) && Event.current.button == 0 && (Input.GetMouseButton(2) || Input.GetMouseButtonUp(2) || (SteamDeck.nextLeftMouseUpEventIsRightMouseUpEvent && Event.current.type == EventType.MouseUp)))
			{
				Event.current.button = 1;
				if (Event.current.type == EventType.MouseDown)
				{
					SteamDeck.nextLeftMouseUpEventIsRightMouseUpEvent = true;
					return;
				}
				if (Event.current.type == EventType.MouseUp)
				{
					SteamDeck.nextLeftMouseUpEventIsRightMouseUpEvent = false;
				}
			}
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x001C85A7 File Offset: 0x001C67A7
		private static void KeyboardDismissedCallback(FloatingGamepadTextInputDismissed_t data)
		{
			SteamDeck.keyboardShowing = false;
			SteamDeck.unfocusCurrentTextField = true;
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x001C85B8 File Offset: 0x001C67B8
		public static void ShowSteamDeckGameControlsIfNotKnown()
		{
			if (SteamDeck.IsSteamDeckInNonKeyboardMode && Find.TickManager.TicksGame > 120 && !Find.TickManager.Paused && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.SteamDeckControlsGame))
			{
				Dialog_MessageBox dialog_MessageBox = new Dialog_MessageBox(ConceptDefOf.SteamDeckControlsGame.HelpTextAdjusted, null, null, null, null, null, false, null, null, WindowLayer.Dialog);
				dialog_MessageBox.image = ContentFinder<Texture2D>.Get("UI/Misc/SteamDeck2", true);
				Find.WindowStack.Add(dialog_MessageBox);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SteamDeckControlsGame, KnowledgeAmount.Total);
			}
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x001C8638 File Offset: 0x001C6838
		public static void ShowSteamDeckMainMenuControlsIfNotKnown()
		{
			if (SteamDeck.IsSteamDeckInNonKeyboardMode && !PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.SteamDeckControlsMainMenu))
			{
				Dialog_MessageBox dialog_MessageBox = new Dialog_MessageBox(ConceptDefOf.SteamDeckControlsMainMenu.HelpTextAdjusted, null, null, null, null, null, false, null, null, WindowLayer.Dialog);
				dialog_MessageBox.image = ContentFinder<Texture2D>.Get("UI/Misc/SteamDeck1", true);
				Find.WindowStack.Add(dialog_MessageBox);
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SteamDeckControlsMainMenu, KnowledgeAmount.Total);
			}
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x001C86A0 File Offset: 0x001C68A0
		public static void HandleTouchScreenScrollViewScroll(Rect outRect, ref Vector2 scrollPosition)
		{
			if (!SteamDeck.IsSteamDeck)
			{
				return;
			}
			if (DragAndDropWidget.Dragging || Widgets.Painting || ReorderableWidget.Dragging)
			{
				return;
			}
			if (Input.GetMouseButtonDown(0) && Mouse.IsOver(outRect) && !Mouse.IsOver(new Rect(outRect.xMax - 16f, outRect.y, 16f, outRect.height)) && !Mouse.IsOver(new Rect(outRect.x, outRect.yMax - 16f, outRect.width, 16f)))
			{
				SteamDeck.currentScrollView = new Rect?(outRect);
				SteamDeck.currentScrollViewStartMousePos = UI.MousePositionOnUIInverted;
				SteamDeck.scrollMouseTraveledEnoughDist = false;
				SteamDeck.scrollVelocityRecords.Clear();
				if (SteamDeck.scrollViewWithVelocity == SteamDeck.currentScrollView)
				{
					SteamDeck.scrollViewWithVelocity = null;
					SteamDeck.scrollViewVelocity = default(Vector2);
				}
			}
			else
			{
				if (Input.GetMouseButton(0))
				{
					Rect? rect = SteamDeck.currentScrollView;
					Rect rhs = outRect;
					if (rect != null && (rect == null || rect.GetValueOrDefault() == rhs))
					{
						if (!SteamDeck.scrollMouseTraveledEnoughDist && (UI.MousePositionOnUIInverted - SteamDeck.currentScrollViewStartMousePos).sqrMagnitude > 100f)
						{
							SteamDeck.scrollMouseTraveledEnoughDist = true;
						}
						if (SteamDeck.scrollMouseTraveledEnoughDist)
						{
							Vector2 currentEventDelta = UnityGUIBugsFixer.CurrentEventDelta;
							scrollPosition -= UnityGUIBugsFixer.CurrentEventDelta;
							SteamDeck.scrollVelocityRecords.Add(new ValueTuple<float, Vector2>(Time.time, -UnityGUIBugsFixer.CurrentEventDelta));
							SteamDeck.<HandleTouchScreenScrollViewScroll>g__ClampScrollPosition|34_0(ref scrollPosition);
							goto IL_28E;
						}
						goto IL_28E;
					}
				}
				if (!Input.GetMouseButton(0))
				{
					Rect? rect = SteamDeck.currentScrollView;
					Rect rhs = outRect;
					if (rect != null && (rect == null || rect.GetValueOrDefault() == rhs))
					{
						if (SteamDeck.scrollMouseTraveledEnoughDist)
						{
							SteamDeck.scrollViewWithVelocity = SteamDeck.currentScrollView;
							SteamDeck.scrollViewVelocity = default(Vector2);
							for (int i = 0; i < SteamDeck.scrollVelocityRecords.Count; i++)
							{
								if (Time.time - SteamDeck.scrollVelocityRecords[i].Item1 < 0.057f)
								{
									SteamDeck.scrollViewVelocity += SteamDeck.scrollVelocityRecords[i].Item2;
								}
							}
							SteamDeck.consumeAllMouseUpEventsOnFrame = Time.frameCount;
						}
						SteamDeck.currentScrollView = null;
					}
				}
			}
			IL_28E:
			if (Event.current.type == EventType.Repaint)
			{
				Rect? rect = SteamDeck.scrollViewWithVelocity;
				Rect rhs = outRect;
				if (rect != null && (rect == null || rect.GetValueOrDefault() == rhs) && SteamDeck.scrollViewVelocity != default(Vector2))
				{
					scrollPosition += SteamDeck.scrollViewVelocity;
					SteamDeck.scrollViewVelocity = Vector2.MoveTowards(SteamDeck.scrollViewVelocity, default(Vector2), Time.deltaTime * 90f);
					SteamDeck.<HandleTouchScreenScrollViewScroll>g__ClampScrollPosition|34_0(ref scrollPosition);
				}
			}
			if (SteamDeck.consumeAllMouseUpEventsOnFrame == Time.frameCount && Event.current.type == EventType.MouseUp)
			{
				Event.current.Use();
			}
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x001C89F4 File Offset: 0x001C6BF4
		public static string GetKeyBindingLabel(KeyBindingDef keyDef)
		{
			KeyCode mainKey = keyDef.MainKey;
			if (mainKey <= KeyCode.D)
			{
				if (mainKey <= KeyCode.Escape)
				{
					if (mainKey == KeyCode.Tab)
					{
						return "X";
					}
					if (mainKey == KeyCode.Return)
					{
						return "A";
					}
					if (mainKey == KeyCode.Escape)
					{
						return "B";
					}
				}
				else
				{
					if (mainKey == KeyCode.Space)
					{
						return "Y";
					}
					if (mainKey == KeyCode.A)
					{
						return "←";
					}
					if (mainKey == KeyCode.D)
					{
						return "→";
					}
				}
			}
			else if (mainKey <= KeyCode.PageDown)
			{
				if (mainKey == KeyCode.S)
				{
					return "↓";
				}
				if (mainKey == KeyCode.W)
				{
					return "↑";
				}
				switch (mainKey)
				{
				case KeyCode.Home:
					return "R4";
				case KeyCode.End:
					return "L4";
				case KeyCode.PageUp:
					return "L1";
				case KeyCode.PageDown:
					return "R1";
				}
			}
			else
			{
				if (mainKey == KeyCode.RightShift)
				{
					return "R5";
				}
				if (mainKey == KeyCode.LeftShift)
				{
					return "R5";
				}
				switch (mainKey)
				{
				case KeyCode.Mouse0:
					return "R2";
				case KeyCode.Mouse1:
					return "L2";
				case KeyCode.Mouse2:
					return "L5";
				}
			}
			if (keyDef == KeyBindingDefOf.MapZoom_In || keyDef == KeyBindingDefOf.MapZoom_Out || keyDef == KeyBindingDefOf.Accept || keyDef == KeyBindingDefOf.Cancel || keyDef == KeyBindingDefOf.TogglePause || keyDef == KeyBindingDefOf.TimeSpeed_Faster || keyDef == KeyBindingDefOf.TimeSpeed_Slower || keyDef == KeyBindingDefOf.QueueOrder)
			{
				return "?";
			}
			return keyDef.MainKeyLabel;
		}

		// Token: 0x0600587D RID: 22653 RVA: 0x001C8B70 File Offset: 0x001C6D70
		[CompilerGenerated]
		internal static void <HandleTouchScreenScrollViewScroll>g__ClampScrollPosition|34_0(ref Vector2 pos)
		{
			if (pos.x < 0f)
			{
				pos.x = 0f;
			}
			if (pos.y < 0f)
			{
				pos.y = 0f;
			}
		}

		// Token: 0x04004032 RID: 16434
		private static bool isSteamDeck;

		// Token: 0x04004033 RID: 16435
		private static bool keyboardShowing;

		// Token: 0x04004034 RID: 16436
		private static int lastFocusedTextFieldID;

		// Token: 0x04004035 RID: 16437
		private static int lastFocusedTextFieldCursorIndex;

		// Token: 0x04004036 RID: 16438
		private static bool nextLeftMouseUpEventIsRightMouseUpEvent;

		// Token: 0x04004037 RID: 16439
		private static bool unfocusCurrentTextField;

		// Token: 0x04004038 RID: 16440
		private static Callback<FloatingGamepadTextInputDismissed_t> keyboardDismissedCallback;

		// Token: 0x04004039 RID: 16441
		private static Rect? currentScrollView;

		// Token: 0x0400403A RID: 16442
		private static Vector2 currentScrollViewStartMousePos;

		// Token: 0x0400403B RID: 16443
		private static bool scrollMouseTraveledEnoughDist;

		// Token: 0x0400403C RID: 16444
		private static int consumeAllMouseUpEventsOnFrame = -1;

		// Token: 0x0400403D RID: 16445
		private static Rect? scrollViewWithVelocity;

		// Token: 0x0400403E RID: 16446
		private static Vector2 scrollViewVelocity;

		// Token: 0x0400403F RID: 16447
		private static List<ValueTuple<float, Vector2>> scrollVelocityRecords = new List<ValueTuple<float, Vector2>>();
	}
}
