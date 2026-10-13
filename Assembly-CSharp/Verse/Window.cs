using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000C46 RID: 3142
	public abstract class Window
	{
		// Token: 0x17000C36 RID: 3126
		// (get) Token: 0x06005049 RID: 20553 RVA: 0x0001330D File Offset: 0x0001150D
		public virtual Vector2 InitialSize
		{
			get
			{
				return new Vector2(500f, 500f);
			}
		}

		// Token: 0x17000C37 RID: 3127
		// (get) Token: 0x0600504A RID: 20554 RVA: 0x0019D4BE File Offset: 0x0019B6BE
		protected virtual float Margin
		{
			get
			{
				return 18f;
			}
		}

		// Token: 0x17000C38 RID: 3128
		// (get) Token: 0x0600504B RID: 20555 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool IsDebug
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000C39 RID: 3129
		// (get) Token: 0x0600504C RID: 20556 RVA: 0x0019D4C5 File Offset: 0x0019B6C5
		public bool IsOpen
		{
			get
			{
				return Find.WindowStack.IsOpen(this);
			}
		}

		// Token: 0x17000C3A RID: 3130
		// (get) Token: 0x0600504D RID: 20557 RVA: 0x00002C42 File Offset: 0x00000E42
		public virtual QuickSearchWidget CommonSearchWidget
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000C3B RID: 3131
		// (get) Token: 0x0600504E RID: 20558 RVA: 0x0019D4D2 File Offset: 0x0019B6D2
		public virtual string CloseButtonText
		{
			get
			{
				return "CloseButton".Translate();
			}
		}

		// Token: 0x0600504F RID: 20559 RVA: 0x0019D4E4 File Offset: 0x0019B6E4
		public Window(IWindowDrawing customWindowDrawing = null)
		{
			this.soundAppear = SoundDefOf.DialogBoxAppear;
			this.soundClose = SoundDefOf.Click;
			this.onGUIProfilerLabelCached = "WindowOnGUI: " + base.GetType().Name;
			this.extraOnGUIProfilerLabelCached = "ExtraOnGUI: " + base.GetType().Name;
			this.innerWindowOnGUICached = new GUI.WindowFunction(this.InnerWindowOnGUI);
			this.notify_CommonSearchChangedCached = new Action(this.Notify_CommonSearchChanged);
			this.windowDrawing = (customWindowDrawing ?? new DefaultWindowDrawing());
		}

		// Token: 0x06005050 RID: 20560 RVA: 0x0019D5F1 File Offset: 0x0019B7F1
		public virtual void WindowUpdate()
		{
			if (this.sustainerAmbient != null)
			{
				this.sustainerAmbient.Maintain();
			}
		}

		// Token: 0x06005051 RID: 20561
		public abstract void DoWindowContents(Rect inRect);

		// Token: 0x06005052 RID: 20562 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ExtraOnGUI()
		{
		}

		// Token: 0x06005053 RID: 20563 RVA: 0x0019D608 File Offset: 0x0019B808
		public virtual void PreOpen()
		{
			this.SetInitialSizeAndPosition();
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				commonSearchWidget.Reset();
			}
			if (this.layer == WindowLayer.Dialog)
			{
				if (Current.ProgramState == ProgramState.Playing)
				{
					Find.DesignatorManager.Dragger.EndDrag();
					Find.DesignatorManager.Deselect();
					Find.Selector.Notify_DialogOpened();
				}
				if (Find.World != null)
				{
					Find.WorldSelector.Notify_DialogOpened();
				}
			}
		}

		// Token: 0x06005054 RID: 20564 RVA: 0x0019D671 File Offset: 0x0019B871
		public virtual void PostOpen()
		{
			if (this.soundAppear != null)
			{
				this.soundAppear.PlayOneShotOnCamera(null);
			}
			if (this.soundAmbient != null)
			{
				this.sustainerAmbient = this.soundAmbient.TrySpawnSustainer(SoundInfo.OnCamera(MaintenanceType.PerFrame));
			}
		}

		// Token: 0x06005055 RID: 20565 RVA: 0x000028E7 File Offset: 0x00000AE7
		public virtual bool OnCloseRequest()
		{
			return true;
		}

		// Token: 0x06005056 RID: 20566 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PreClose()
		{
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void PostClose()
		{
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x0019D6A8 File Offset: 0x0019B8A8
		public virtual void WindowOnGUI()
		{
			if (!this.drawInScreenshotMode && Find.UIRoot.screenshotMode.Active)
			{
				return;
			}
			if (this.onlyDrawInDevMode && !Prefs.DevMode)
			{
				return;
			}
			if (this.resizeable)
			{
				if (this.resizer == null)
				{
					this.resizer = new WindowResizer();
				}
				if (this.resizeLater)
				{
					this.resizeLater = false;
					this.windowRect = this.resizeLaterRect;
				}
			}
			this.windowRect = this.windowRect.Rounded();
			this.windowRect = GUI.Window(this.ID, this.windowRect, this.innerWindowOnGUICached, "", this.windowDrawing.EmptyStyle);
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x0019D754 File Offset: 0x0019B954
		private void InnerWindowOnGUI(int x)
		{
			UnityGUIBugsFixer.OnGUI();
			SteamDeck.WindowOnGUI();
			OriginalEventUtility.RecordOriginalEvent(Event.current);
			Rect rect = this.windowRect.AtZero();
			Find.WindowStack.currentlyDrawnWindow = this;
			if (this.doWindowBackground)
			{
				this.windowDrawing.DoWindowBackground(rect);
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedCancel();
			}
			if (KeyBindingDefOf.Accept.KeyDownEvent)
			{
				Find.WindowStack.Notify_PressedAccept();
			}
			if (Event.current.type == EventType.MouseDown)
			{
				Find.WindowStack.Notify_ClickedInsideWindow(this);
			}
			if (Event.current.type == EventType.KeyDown && !Find.WindowStack.GetsInput(this))
			{
				Event.current.Use();
			}
			if (!this.optionalTitle.NullOrEmpty())
			{
				GUI.Label(new Rect(this.Margin, this.Margin, this.windowRect.width, 25f), this.optionalTitle);
			}
			if (this.doCloseX && this.windowDrawing.DoClostButtonSmall(rect))
			{
				this.Close(true);
			}
			if (this.resizeable && Event.current.type != EventType.Repaint)
			{
				Rect lhs = this.resizer.DoResizeControl(this.windowRect);
				if (lhs != this.windowRect)
				{
					this.resizeLater = true;
					this.resizeLaterRect = lhs;
				}
			}
			Rect rect2 = rect.ContractedBy(this.Margin);
			if (!this.optionalTitle.NullOrEmpty())
			{
				rect2.yMin += this.Margin + 25f;
			}
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget != null)
			{
				commonSearchWidget.OnGUI(this.QuickSearchWidgetRect(rect, rect2), this.notify_CommonSearchChangedCached, null);
			}
			this.windowDrawing.BeginGroup(rect2);
			try
			{
				this.DoWindowContents(rect2.AtZero());
			}
			catch (Exception ex)
			{
				string str = "Exception filling window for ";
				Type type = base.GetType();
				string str2 = (type != null) ? type.ToString() : null;
				string str3 = ": ";
				Exception ex2 = ex;
				Log.Error(str + str2 + str3 + ((ex2 != null) ? ex2.ToString() : null));
			}
			this.windowDrawing.EndGroup();
			this.LateWindowOnGUI(rect2);
			if (this.grayOutIfOtherDialogOpen)
			{
				IList<Window> windows = Find.WindowStack.Windows;
				for (int i = 0; i < windows.Count; i++)
				{
					if (windows[i].layer == WindowLayer.Dialog && !(windows[i] is Page) && windows[i] != this)
					{
						this.windowDrawing.DoGrayOut(rect);
						break;
					}
				}
			}
			if (this.resizeable && Event.current.type == EventType.Repaint)
			{
				this.resizer.DoResizeControl(this.windowRect);
			}
			if (this.doCloseButton)
			{
				Text.Font = GameFont.Small;
				Rect rect3 = new Rect(rect.width / 2f - Window.CloseButSize.x / 2f, rect.height - 55f, Window.CloseButSize.x, Window.CloseButSize.y);
				if (this.windowDrawing.DoCloseButton(rect3, this.CloseButtonText))
				{
					this.Close(true);
				}
			}
			if (KeyBindingDefOf.Cancel.KeyDownEvent && this.IsOpen)
			{
				this.OnCancelKeyPressed();
			}
			if (this.draggable)
			{
				GUI.DragWindow();
			}
			else if (Event.current.type == EventType.MouseDown)
			{
				Event.current.Use();
			}
			if (!this.ignoreScreenFader)
			{
				ScreenFader.OverlayOnGUI(rect.size);
			}
			Find.WindowStack.currentlyDrawnWindow = null;
			OriginalEventUtility.Reset();
		}

		// Token: 0x0600505A RID: 20570 RVA: 0x0019DAC8 File Offset: 0x0019BCC8
		protected virtual Rect QuickSearchWidgetRect(Rect winRect, Rect inRect)
		{
			Vector2 vector = this.commonSearchWidgetOffset;
			return new Rect(winRect.x + vector.x, winRect.height - 55f + vector.y, Window.QuickSearchSize.x, Window.QuickSearchSize.y);
		}

		// Token: 0x0600505B RID: 20571 RVA: 0x000026EA File Offset: 0x000008EA
		protected virtual void LateWindowOnGUI(Rect inRect)
		{
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x0019DB17 File Offset: 0x0019BD17
		public virtual void Close(bool doCloseSound = true)
		{
			Find.WindowStack.TryRemove(this, doCloseSound);
		}

		// Token: 0x0600505D RID: 20573 RVA: 0x00002501 File Offset: 0x00000701
		public virtual bool CausesMessageBackground()
		{
			return false;
		}

		// Token: 0x0600505E RID: 20574 RVA: 0x0019DB28 File Offset: 0x0019BD28
		protected virtual void SetInitialSizeAndPosition()
		{
			Vector2 initialSize = this.InitialSize;
			this.windowRect = new Rect(((float)UI.screenWidth - initialSize.x) / 2f, ((float)UI.screenHeight - initialSize.y) / 2f, initialSize.x, initialSize.y);
			this.windowRect = this.windowRect.Rounded();
		}

		// Token: 0x0600505F RID: 20575 RVA: 0x0019DB8A File Offset: 0x0019BD8A
		public virtual void OnCancelKeyPressed()
		{
			if (this.closeOnCancel)
			{
				this.Close(true);
				Event.current.Use();
			}
			if (this.openMenuOnCancel)
			{
				Find.MainTabsRoot.ToggleTab(MainButtonDefOf.Menu, true);
			}
		}

		// Token: 0x06005060 RID: 20576 RVA: 0x0019DBBD File Offset: 0x0019BDBD
		public virtual void OnAcceptKeyPressed()
		{
			if (this.closeOnAccept)
			{
				this.Close(true);
				Event.current.Use();
			}
		}

		// Token: 0x06005061 RID: 20577 RVA: 0x0019DBD8 File Offset: 0x0019BDD8
		public virtual void Notify_ResolutionChanged()
		{
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x06005062 RID: 20578 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void Notify_CommonSearchChanged()
		{
		}

		// Token: 0x06005063 RID: 20579 RVA: 0x0019DBE0 File Offset: 0x0019BDE0
		public virtual void Notify_ClickOutsideWindow()
		{
			QuickSearchWidget commonSearchWidget = this.CommonSearchWidget;
			if (commonSearchWidget == null)
			{
				return;
			}
			commonSearchWidget.Unfocus();
		}

		// Token: 0x040039D6 RID: 14806
		public WindowLayer layer = WindowLayer.Dialog;

		// Token: 0x040039D7 RID: 14807
		public string optionalTitle;

		// Token: 0x040039D8 RID: 14808
		public bool doCloseX;

		// Token: 0x040039D9 RID: 14809
		public bool doCloseButton;

		// Token: 0x040039DA RID: 14810
		public bool closeOnAccept = true;

		// Token: 0x040039DB RID: 14811
		public bool closeOnCancel = true;

		// Token: 0x040039DC RID: 14812
		public bool forceCatchAcceptAndCancelEventEvenIfUnfocused;

		// Token: 0x040039DD RID: 14813
		public bool closeOnClickedOutside;

		// Token: 0x040039DE RID: 14814
		public bool forcePause;

		// Token: 0x040039DF RID: 14815
		public bool preventCameraMotion = true;

		// Token: 0x040039E0 RID: 14816
		public bool preventDrawTutor;

		// Token: 0x040039E1 RID: 14817
		public bool doWindowBackground = true;

		// Token: 0x040039E2 RID: 14818
		public bool onlyOneOfTypeAllowed = true;

		// Token: 0x040039E3 RID: 14819
		public bool absorbInputAroundWindow;

		// Token: 0x040039E4 RID: 14820
		public bool resizeable;

		// Token: 0x040039E5 RID: 14821
		public bool draggable;

		// Token: 0x040039E6 RID: 14822
		public bool drawShadow = true;

		// Token: 0x040039E7 RID: 14823
		public bool focusWhenOpened = true;

		// Token: 0x040039E8 RID: 14824
		public float shadowAlpha = 1f;

		// Token: 0x040039E9 RID: 14825
		public SoundDef soundAppear;

		// Token: 0x040039EA RID: 14826
		public SoundDef soundClose;

		// Token: 0x040039EB RID: 14827
		public SoundDef soundAmbient;

		// Token: 0x040039EC RID: 14828
		public bool silenceAmbientSound;

		// Token: 0x040039ED RID: 14829
		public bool grayOutIfOtherDialogOpen;

		// Token: 0x040039EE RID: 14830
		public Vector2 commonSearchWidgetOffset = new Vector2(0f, Window.CloseButSize.y - Window.QuickSearchSize.y) / 2f;

		// Token: 0x040039EF RID: 14831
		public bool openMenuOnCancel;

		// Token: 0x040039F0 RID: 14832
		public bool preventSave;

		// Token: 0x040039F1 RID: 14833
		public bool drawInScreenshotMode = true;

		// Token: 0x040039F2 RID: 14834
		public bool onlyDrawInDevMode;

		// Token: 0x040039F3 RID: 14835
		public bool ignoreScreenFader;

		// Token: 0x040039F4 RID: 14836
		public const float StandardMargin = 18f;

		// Token: 0x040039F5 RID: 14837
		public const float FooterRowHeight = 55f;

		// Token: 0x040039F6 RID: 14838
		public static readonly Vector2 CloseButSize = new Vector2(120f, 40f);

		// Token: 0x040039F7 RID: 14839
		public static readonly Vector2 QuickSearchSize = new Vector2(240f, 24f);

		// Token: 0x040039F8 RID: 14840
		public int ID;

		// Token: 0x040039F9 RID: 14841
		public Rect windowRect;

		// Token: 0x040039FA RID: 14842
		private Sustainer sustainerAmbient;

		// Token: 0x040039FB RID: 14843
		private WindowResizer resizer;

		// Token: 0x040039FC RID: 14844
		private bool resizeLater;

		// Token: 0x040039FD RID: 14845
		private Rect resizeLaterRect;

		// Token: 0x040039FE RID: 14846
		private IWindowDrawing windowDrawing;

		// Token: 0x040039FF RID: 14847
		private string onGUIProfilerLabelCached;

		// Token: 0x04003A00 RID: 14848
		public string extraOnGUIProfilerLabelCached;

		// Token: 0x04003A01 RID: 14849
		private GUI.WindowFunction innerWindowOnGUICached;

		// Token: 0x04003A02 RID: 14850
		private Action notify_CommonSearchChangedCached;
	}
}
