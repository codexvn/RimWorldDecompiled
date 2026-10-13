using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000C48 RID: 3144
	public class WindowStack
	{
		// Token: 0x17000C3C RID: 3132
		// (get) Token: 0x06005067 RID: 20583 RVA: 0x0019DDD9 File Offset: 0x0019BFD9
		public int Count
		{
			get
			{
				return this.windows.Count;
			}
		}

		// Token: 0x17000C3D RID: 3133
		public Window this[int index]
		{
			get
			{
				return this.windows[index];
			}
		}

		// Token: 0x17000C3E RID: 3134
		// (get) Token: 0x06005069 RID: 20585 RVA: 0x0019DDF4 File Offset: 0x0019BFF4
		public IList<Window> Windows
		{
			get
			{
				return this.windows.AsReadOnly();
			}
		}

		// Token: 0x17000C3F RID: 3135
		// (get) Token: 0x0600506A RID: 20586 RVA: 0x0019DE01 File Offset: 0x0019C001
		public FloatMenu FloatMenu
		{
			get
			{
				return this.WindowOfType<FloatMenu>();
			}
		}

		// Token: 0x17000C40 RID: 3136
		// (get) Token: 0x0600506B RID: 20587 RVA: 0x0019DE0C File Offset: 0x0019C00C
		public bool WindowsForcePause
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].forcePause)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000C41 RID: 3137
		// (get) Token: 0x0600506C RID: 20588 RVA: 0x0019DE48 File Offset: 0x0019C048
		public bool WindowsPreventCameraMotion
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventCameraMotion)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000C42 RID: 3138
		// (get) Token: 0x0600506D RID: 20589 RVA: 0x0019DE84 File Offset: 0x0019C084
		public bool WindowsPreventDrawTutor
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventDrawTutor)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000C43 RID: 3139
		// (get) Token: 0x0600506E RID: 20590 RVA: 0x0019DEBD File Offset: 0x0019C0BD
		public float SecondsSinceClosedGameStartDialog
		{
			get
			{
				if (this.gameStartDialogOpen)
				{
					return 0f;
				}
				if (this.timeGameStartDialogClosed < 0f)
				{
					return 9999999f;
				}
				return Time.time - this.timeGameStartDialogClosed;
			}
		}

		// Token: 0x17000C44 RID: 3140
		// (get) Token: 0x0600506F RID: 20591 RVA: 0x0019DEEC File Offset: 0x0019C0EC
		public bool MouseObscuredNow
		{
			get
			{
				return this.GetWindowAt(UI.MousePosUIInvertedUseEventIfCan) != this.currentlyDrawnWindow;
			}
		}

		// Token: 0x17000C45 RID: 3141
		// (get) Token: 0x06005070 RID: 20592 RVA: 0x0019DF04 File Offset: 0x0019C104
		public bool CurrentWindowGetsInput
		{
			get
			{
				return this.GetsInput(this.currentlyDrawnWindow);
			}
		}

		// Token: 0x17000C46 RID: 3142
		// (get) Token: 0x06005071 RID: 20593 RVA: 0x0019DF14 File Offset: 0x0019C114
		public bool NonImmediateDialogWindowOpen
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (!(this.windows[i] is ImmediateWindow) && this.windows[i].layer == WindowLayer.Dialog)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000C47 RID: 3143
		// (get) Token: 0x06005072 RID: 20594 RVA: 0x0019DF64 File Offset: 0x0019C164
		public bool WindowsPreventSave
		{
			get
			{
				for (int i = 0; i < this.windows.Count; i++)
				{
					if (this.windows[i].preventSave)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000C48 RID: 3144
		// (get) Token: 0x06005073 RID: 20595 RVA: 0x0019DFA0 File Offset: 0x0019C1A0
		public bool AnyWindowAbsorbingAllInput
		{
			get
			{
				using (List<Window>.Enumerator enumerator = this.windows.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.absorbInputAroundWindow)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000C49 RID: 3145
		// (get) Token: 0x06005074 RID: 20596 RVA: 0x0019DFFC File Offset: 0x0019C1FC
		public bool AnySearchWidgetFocused
		{
			get
			{
				foreach (Window window in this.windows)
				{
					QuickSearchWidget commonSearchWidget = window.CommonSearchWidget;
					if (commonSearchWidget != null && commonSearchWidget.CurrentlyFocused())
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005075 RID: 20597 RVA: 0x0019E060 File Offset: 0x0019C260
		public void WindowsUpdate()
		{
			this.AdjustWindowsIfResolutionChanged();
			for (int i = 0; i < this.windows.Count; i++)
			{
				this.windows[i].WindowUpdate();
			}
		}

		// Token: 0x06005076 RID: 20598 RVA: 0x0019E09C File Offset: 0x0019C29C
		public void HandleEventsHighPriority()
		{
			if (Event.current.type == EventType.MouseDown && this.GetWindowAt(UI.GUIToScreenPoint(Event.current.mousePosition)) == null)
			{
				bool flag = this.CloseWindowsBecauseClicked(null);
				this.NotifyOutsideClicks(null);
				if (flag)
				{
					Event.current.Use();
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				this.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				this.Notify_PressedAccept();
			}
			if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.KeyDown) && !this.GetsInput(null))
			{
				Event.current.Use();
			}
		}

		// Token: 0x06005077 RID: 20599 RVA: 0x0019E138 File Offset: 0x0019C338
		public void WindowStackOnGUI()
		{
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			for (int i = this.windowStackOnGUITmpList.Count - 1; i >= 0; i--)
			{
				this.windowStackOnGUITmpList[i].ExtraOnGUI();
			}
			this.UpdateImmediateWindowsList();
			this.windowStackOnGUITmpList.Clear();
			this.windowStackOnGUITmpList.AddRange(this.windows);
			int j = 0;
			while (j < this.windowStackOnGUITmpList.Count)
			{
				if (!this.windowStackOnGUITmpList[j].drawShadow)
				{
					goto IL_EF;
				}
				if (this.windowStackOnGUITmpList[j].drawInScreenshotMode || !Find.UIRoot.screenshotMode.Active)
				{
					GUI.color = new Color(1f, 1f, 1f, this.windowStackOnGUITmpList[j].shadowAlpha);
					Widgets.DrawShadowAround(this.windowStackOnGUITmpList[j].windowRect);
					GUI.color = Color.white;
					goto IL_EF;
				}
				IL_100:
				j++;
				continue;
				IL_EF:
				this.windowStackOnGUITmpList[j].WindowOnGUI();
				goto IL_100;
			}
			if (this.updateInternalWindowsOrderLater)
			{
				this.updateInternalWindowsOrderLater = false;
				this.UpdateInternalWindowsOrder();
			}
		}

		// Token: 0x06005078 RID: 20600 RVA: 0x0019E270 File Offset: 0x0019C470
		public void Notify_ClickedInsideWindow(Window window)
		{
			if (this.GetsInput(window))
			{
				this.windows.Remove(window);
				this.InsertAtCorrectPositionInList(window);
				this.focusedWindow = window;
			}
			else
			{
				Event.current.Use();
			}
			this.CloseWindowsBecauseClicked(window);
			this.NotifyOutsideClicks(window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x06005079 RID: 20601 RVA: 0x0019E2C3 File Offset: 0x0019C4C3
		public void Notify_ManuallySetFocus(Window window)
		{
			this.focusedWindow = window;
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600507A RID: 20602 RVA: 0x0019E2D4 File Offset: 0x0019C4D4
		public void Notify_PressedCancel()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnCancel || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnCancelKeyPressed();
					return;
				}
			}
		}

		// Token: 0x0600507B RID: 20603 RVA: 0x0019E348 File Offset: 0x0019C548
		public void Notify_PressedAccept()
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if ((this.windows[i].closeOnAccept || this.windows[i].forceCatchAcceptAndCancelEventEvenIfUnfocused) && this.GetsInput(this.windows[i]))
				{
					this.windows[i].OnAcceptKeyPressed();
					return;
				}
			}
		}

		// Token: 0x0600507C RID: 20604 RVA: 0x0019E3B9 File Offset: 0x0019C5B9
		public void Notify_GameStartDialogOpened()
		{
			this.gameStartDialogOpen = true;
		}

		// Token: 0x0600507D RID: 20605 RVA: 0x0019E3C2 File Offset: 0x0019C5C2
		public void Notify_GameStartDialogClosed()
		{
			this.timeGameStartDialogClosed = Time.time;
			this.gameStartDialogOpen = false;
		}

		// Token: 0x0600507E RID: 20606 RVA: 0x0019E3D8 File Offset: 0x0019C5D8
		public bool IsOpen<WindowType>()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600507F RID: 20607 RVA: 0x0019E414 File Offset: 0x0019C614
		public bool IsOpen(Type type)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == type)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005080 RID: 20608 RVA: 0x0019E453 File Offset: 0x0019C653
		public bool IsOpen(Window window)
		{
			return this.windows.Contains(window);
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x0019E461 File Offset: 0x0019C661
		public bool TryGetWindow<T>(out T window) where T : class
		{
			window = this.WindowOfType<T>();
			return window != null;
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x0019E480 File Offset: 0x0019C680
		public WindowType WindowOfType<WindowType>() where WindowType : class
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] is WindowType)
				{
					return this.windows[i] as WindowType;
				}
			}
			return default(WindowType);
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x0019E4D8 File Offset: 0x0019C6D8
		public bool GetsInput(Window window)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					return true;
				}
				if (this.windows[i].absorbInputAroundWindow)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x0019E524 File Offset: 0x0019C724
		public void Add(Window window)
		{
			this.RemoveWindowsOfType(window.GetType());
			window.ID = WindowStack.uniqueWindowID++;
			window.PreOpen();
			this.InsertAtCorrectPositionInList(window);
			this.FocusAfterInsertIfShould(window);
			this.updateInternalWindowsOrderLater = true;
			window.PostOpen();
		}

		// Token: 0x06005085 RID: 20613 RVA: 0x0019E574 File Offset: 0x0019C774
		public void ImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground = true, bool absorbInputAroundWindow = false, float shadowAlpha = 1f, Action doClickOutsideFunc = null, bool ignoreScreenFader = false)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			if (ID == 0)
			{
				Log.Warning("Used 0 as immediate window ID.");
				return;
			}
			ID = -Math.Abs(ID);
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].ID == ID)
				{
					ImmediateWindow immediateWindow = (ImmediateWindow)this.windows[i];
					immediateWindow.windowRect = rect;
					immediateWindow.doWindowFunc = doWindowFunc;
					immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
					immediateWindow.layer = layer;
					immediateWindow.doWindowBackground = doBackground;
					immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
					immediateWindow.shadowAlpha = shadowAlpha;
					immediateWindow.ignoreScreenFader = ignoreScreenFader;
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				this.AddNewImmediateWindow(ID, rect, layer, doWindowFunc, doBackground, absorbInputAroundWindow, shadowAlpha, doClickOutsideFunc, ignoreScreenFader);
			}
			this.immediateWindowsRequests.Add(ID);
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x0019E648 File Offset: 0x0019C848
		public bool TryRemove(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i].GetType() == windowType)
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x0019E69C File Offset: 0x0019C89C
		public bool TryRemoveAssignableFromType(Type windowType, bool doCloseSound = true)
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (windowType.IsAssignableFrom(this.windows[i].GetType()))
				{
					return this.TryRemove(this.windows[i], doCloseSound);
				}
			}
			return false;
		}

		// Token: 0x06005088 RID: 20616 RVA: 0x0019E6F0 File Offset: 0x0019C8F0
		public bool TryRemove(Window window, bool doCloseSound = true)
		{
			bool flag = false;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (this.windows[i] == window)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (!window.OnCloseRequest())
			{
				return false;
			}
			if (doCloseSound && window.soundClose != null)
			{
				window.soundClose.PlayOneShotOnCamera(null);
			}
			window.PreClose();
			this.windows.Remove(window);
			window.PostClose();
			if (this.focusedWindow == window)
			{
				if (this.windows.Count > 0)
				{
					this.focusedWindow = this.windows[this.windows.Count - 1];
				}
				else
				{
					this.focusedWindow = null;
				}
				this.updateInternalWindowsOrderLater = true;
			}
			return true;
		}

		// Token: 0x06005089 RID: 20617 RVA: 0x0019E7B0 File Offset: 0x0019C9B0
		public Window GetWindowAt(Vector2 pos)
		{
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i].windowRect.Contains(pos))
				{
					return this.windows[i];
				}
			}
			return null;
		}

		// Token: 0x0600508A RID: 20618 RVA: 0x0019E7FC File Offset: 0x0019C9FC
		private void AddNewImmediateWindow(int ID, Rect rect, WindowLayer layer, Action doWindowFunc, bool doBackground, bool absorbInputAroundWindow, float shadowAlpha, Action doClickOutsideFunc, bool ignoreScreenFader)
		{
			if (ID >= 0)
			{
				Log.Error("Invalid immediate window ID.");
				return;
			}
			ImmediateWindow immediateWindow = new ImmediateWindow();
			immediateWindow.ID = ID;
			immediateWindow.layer = layer;
			immediateWindow.doWindowFunc = doWindowFunc;
			immediateWindow.doClickOutsideFunc = doClickOutsideFunc;
			immediateWindow.doWindowBackground = doBackground;
			immediateWindow.absorbInputAroundWindow = absorbInputAroundWindow;
			immediateWindow.shadowAlpha = shadowAlpha;
			immediateWindow.ignoreScreenFader = ignoreScreenFader;
			immediateWindow.PreOpen();
			immediateWindow.windowRect = rect;
			this.InsertAtCorrectPositionInList(immediateWindow);
			this.FocusAfterInsertIfShould(immediateWindow);
			this.updateInternalWindowsOrderLater = true;
			immediateWindow.PostOpen();
		}

		// Token: 0x0600508B RID: 20619 RVA: 0x0019E884 File Offset: 0x0019CA84
		private void UpdateImmediateWindowsList()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}
			this.updateImmediateWindowsListTmpList.Clear();
			this.updateImmediateWindowsListTmpList.AddRange(this.windows);
			for (int i = 0; i < this.updateImmediateWindowsListTmpList.Count; i++)
			{
				if (this.IsImmediateWindow(this.updateImmediateWindowsListTmpList[i]))
				{
					bool flag = false;
					for (int j = 0; j < this.immediateWindowsRequests.Count; j++)
					{
						if (this.immediateWindowsRequests[j] == this.updateImmediateWindowsListTmpList[i].ID)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.TryRemove(this.updateImmediateWindowsListTmpList[i], true);
					}
				}
			}
			this.immediateWindowsRequests.Clear();
		}

		// Token: 0x0600508C RID: 20620 RVA: 0x0019E944 File Offset: 0x0019CB44
		private void InsertAtCorrectPositionInList(Window window)
		{
			int index = 0;
			for (int i = 0; i < this.windows.Count; i++)
			{
				if (window.layer >= this.windows[i].layer)
				{
					index = i + 1;
				}
			}
			this.windows.Insert(index, window);
			this.updateInternalWindowsOrderLater = true;
		}

		// Token: 0x0600508D RID: 20621 RVA: 0x0019E99C File Offset: 0x0019CB9C
		private void FocusAfterInsertIfShould(Window window)
		{
			if (!window.focusWhenOpened)
			{
				return;
			}
			for (int i = this.windows.Count - 1; i >= 0; i--)
			{
				if (this.windows[i] == window)
				{
					this.focusedWindow = this.windows[i];
					this.updateInternalWindowsOrderLater = true;
					return;
				}
				if (this.windows[i] == this.focusedWindow)
				{
					break;
				}
			}
		}

		// Token: 0x0600508E RID: 20622 RVA: 0x0019EA08 File Offset: 0x0019CC08
		private void AdjustWindowsIfResolutionChanged()
		{
			IntVec2 a = new IntVec2(UI.screenWidth, UI.screenHeight);
			if (!UnityGUIBugsFixer.ResolutionsEqual(a, this.prevResolution))
			{
				this.prevResolution = a;
				for (int i = 0; i < this.windows.Count; i++)
				{
					this.windows[i].Notify_ResolutionChanged();
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.ColonistBar.MarkColonistsDirty();
				}
			}
		}

		// Token: 0x0600508F RID: 20623 RVA: 0x0019EA74 File Offset: 0x0019CC74
		private void RemoveWindowsOfType(Type type)
		{
			this.removeWindowsOfTypeTmpList.Clear();
			this.removeWindowsOfTypeTmpList.AddRange(this.windows);
			for (int i = 0; i < this.removeWindowsOfTypeTmpList.Count; i++)
			{
				if (this.removeWindowsOfTypeTmpList[i].onlyOneOfTypeAllowed && this.removeWindowsOfTypeTmpList[i].GetType() == type)
				{
					this.TryRemove(this.removeWindowsOfTypeTmpList[i], true);
				}
			}
		}

		// Token: 0x06005090 RID: 20624 RVA: 0x0019EAF4 File Offset: 0x0019CCF4
		private void NotifyOutsideClicks(Window clickedWindow)
		{
			foreach (Window window in this.windows)
			{
				if (window != clickedWindow)
				{
					window.Notify_ClickOutsideWindow();
				}
			}
		}

		// Token: 0x06005091 RID: 20625 RVA: 0x0019EB4C File Offset: 0x0019CD4C
		private bool CloseWindowsBecauseClicked(Window clickedWindow)
		{
			this.closeWindowsTmpList.Clear();
			this.closeWindowsTmpList.AddRange(this.windows);
			bool result = false;
			int num = this.closeWindowsTmpList.Count - 1;
			while (num >= 0 && this.closeWindowsTmpList[num] != clickedWindow)
			{
				if (this.closeWindowsTmpList[num].closeOnClickedOutside)
				{
					result = true;
					this.TryRemove(this.closeWindowsTmpList[num], true);
				}
				num--;
			}
			return result;
		}

		// Token: 0x06005092 RID: 20626 RVA: 0x0019EBC8 File Offset: 0x0019CDC8
		private bool IsImmediateWindow(Window window)
		{
			return window.ID < 0;
		}

		// Token: 0x06005093 RID: 20627 RVA: 0x0019EBD4 File Offset: 0x0019CDD4
		private void UpdateInternalWindowsOrder()
		{
			for (int i = 0; i < this.windows.Count; i++)
			{
				GUI.BringWindowToFront(this.windows[i].ID);
			}
			if (this.focusedWindow != null)
			{
				GUI.FocusWindow(this.focusedWindow.ID);
			}
		}

		// Token: 0x04003A07 RID: 14855
		public Window currentlyDrawnWindow;

		// Token: 0x04003A08 RID: 14856
		private List<Window> windows = new List<Window>();

		// Token: 0x04003A09 RID: 14857
		private List<int> immediateWindowsRequests = new List<int>();

		// Token: 0x04003A0A RID: 14858
		private bool updateInternalWindowsOrderLater;

		// Token: 0x04003A0B RID: 14859
		private Window focusedWindow;

		// Token: 0x04003A0C RID: 14860
		private static int uniqueWindowID;

		// Token: 0x04003A0D RID: 14861
		private bool gameStartDialogOpen;

		// Token: 0x04003A0E RID: 14862
		private float timeGameStartDialogClosed = -1f;

		// Token: 0x04003A0F RID: 14863
		private IntVec2 prevResolution = new IntVec2(UI.screenWidth, UI.screenHeight);

		// Token: 0x04003A10 RID: 14864
		private List<Window> windowStackOnGUITmpList = new List<Window>();

		// Token: 0x04003A11 RID: 14865
		private List<Window> updateImmediateWindowsListTmpList = new List<Window>();

		// Token: 0x04003A12 RID: 14866
		private List<Window> removeWindowsOfTypeTmpList = new List<Window>();

		// Token: 0x04003A13 RID: 14867
		private List<Window> closeWindowsTmpList = new List<Window>();
	}
}
