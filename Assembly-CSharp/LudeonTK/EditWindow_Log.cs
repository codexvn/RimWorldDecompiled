using System;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x020000A7 RID: 167
	[StaticConstructorOnStartup]
	public class EditWindow_Log : EditWindow
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600038F RID: 911 RVA: 0x000151EF File Offset: 0x000133EF
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2((float)UI.screenWidth / 2f, (float)UI.screenHeight / 2f);
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000390 RID: 912 RVA: 0x000028E7 File Offset: 0x00000AE7
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000391 RID: 913 RVA: 0x0001520E File Offset: 0x0001340E
		// (set) Token: 0x06000392 RID: 914 RVA: 0x00015215 File Offset: 0x00013415
		private static LogMessage SelectedMessage
		{
			get
			{
				return EditWindow_Log.selectedMessage;
			}
			set
			{
				if (EditWindow_Log.selectedMessage == value)
				{
					return;
				}
				EditWindow_Log.selectedMessage = value;
				if (UnityData.IsInMainThread && GUI.GetNameOfFocusedControl() == EditWindow_Log.MessageDetailsControlName)
				{
					UI.UnfocusCurrentControl();
				}
			}
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00015243 File Offset: 0x00013443
		public EditWindow_Log()
		{
			this.optionalTitle = "Debug log";
			this.closeOnAccept = false;
			this.closeOnCancel = Prefs.CloseLogWindowOnEscape;
			this.drawInScreenshotMode = true;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x0001526F File Offset: 0x0001346F
		public static void TryAutoOpen()
		{
			if (EditWindow_Log.canAutoOpen)
			{
				EditWindow_Log.wantsToOpen = true;
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x0001527E File Offset: 0x0001347E
		public static void ClearSelectedMessage()
		{
			EditWindow_Log.SelectedMessage = null;
			EditWindow_Log.detailsScrollPosition = Vector2.zero;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00015290 File Offset: 0x00013490
		public static void SelectLastMessage(bool expandDetailsPane = false)
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.SelectedMessage = Log.Messages.LastOrDefault<LogMessage>();
			EditWindow_Log.messagesScrollPosition.y = (float)Log.Messages.Count<LogMessage>() * 30f;
			if (expandDetailsPane)
			{
				EditWindow_Log.detailsPaneHeight = 9999f;
			}
		}

		// Token: 0x06000397 RID: 919 RVA: 0x000152CE File Offset: 0x000134CE
		public static void ClearAll()
		{
			EditWindow_Log.ClearSelectedMessage();
			EditWindow_Log.messagesScrollPosition = Vector2.zero;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x000152DF File Offset: 0x000134DF
		public override void PostClose()
		{
			base.PostClose();
			EditWindow_Log.wantsToOpen = false;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x000152F0 File Offset: 0x000134F0
		public override void DoWindowContents(Rect inRect)
		{
			Text.Font = GameFont.Tiny;
			float x = inRect.x;
			float y = inRect.y;
			base.DoRowButton(ref x, y, "Clear", "Clear all log messages.", delegate
			{
				Log.Clear();
				EditWindow_Log.ClearAll();
			});
			base.DoRowButton(ref x, y, "Trace big", "Set the stack trace to be large on screen.", delegate
			{
				EditWindow_Log.detailsPaneHeight = 700f;
			});
			base.DoRowButton(ref x, y, "Trace medium", "Set the stack trace to be medium-sized on screen.", delegate
			{
				EditWindow_Log.detailsPaneHeight = 300f;
			});
			base.DoRowButton(ref x, y, "Trace small", "Set the stack trace to be small on screen.", delegate
			{
				EditWindow_Log.detailsPaneHeight = 100f;
			});
			if (EditWindow_Log.canAutoOpen)
			{
				base.DoRowButton(ref x, y, "Auto-open is ON", null, delegate
				{
					EditWindow_Log.canAutoOpen = false;
				});
			}
			else
			{
				base.DoRowButton(ref x, y, "Auto-open is OFF", null, delegate
				{
					EditWindow_Log.canAutoOpen = true;
				});
			}
			base.DoRowButton(ref x, y, "Copy to clipboard", "Copy all messages to the clipboard.", delegate
			{
				this.CopyAllMessagesToClipboard();
			});
			if (DebugSettings.pauseOnError)
			{
				base.DoRowButton(ref x, y, "Pause on error is ON", null, delegate
				{
					DebugSettings.pauseOnError = false;
				});
			}
			else
			{
				base.DoRowButton(ref x, y, "Pause on error is OFF", null, delegate
				{
					DebugSettings.pauseOnError = true;
				});
			}
			base.DoImageToggle(ref x, y, EditWindow_Log.InfoIcon, "Show info messages", ref EditWindow_Log.showMessages);
			base.DoImageToggle(ref x, y, EditWindow_Log.WarningIcon, "Show warning messages", ref EditWindow_Log.showWarnings);
			base.DoImageToggle(ref x, y, EditWindow_Log.ErrorIcon, "Show error messages", ref EditWindow_Log.showErrors);
			Text.Font = GameFont.Small;
			Rect rect = new Rect(inRect);
			rect.yMin += 26f;
			rect.yMax = inRect.height;
			if (EditWindow_Log.selectedMessage != null)
			{
				rect.yMax -= EditWindow_Log.detailsPaneHeight;
			}
			Rect detailsRect = new Rect(inRect);
			detailsRect.yMin = rect.yMax;
			this.DoMessagesListing(rect);
			this.DoMessageDetails(detailsRect, inRect);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && Mouse.IsOver(rect))
			{
				EditWindow_Log.ClearSelectedMessage();
			}
			EditWindow_Log.detailsPaneHeight = Mathf.Max(EditWindow_Log.detailsPaneHeight, 10f);
			EditWindow_Log.detailsPaneHeight = Mathf.Min(EditWindow_Log.detailsPaneHeight, inRect.height - 80f);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x000155CF File Offset: 0x000137CF
		public static void Notify_MessageDequeued(LogMessage oldMessage)
		{
			if (EditWindow_Log.SelectedMessage == oldMessage)
			{
				EditWindow_Log.SelectedMessage = null;
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x000155E0 File Offset: 0x000137E0
		private void DoMessagesListing(Rect listingRect)
		{
			Rect viewRect = new Rect(0f, 0f, listingRect.width - 16f, this.listingViewHeight + 100f);
			DevGUI.BeginScrollView(listingRect, ref EditWindow_Log.messagesScrollPosition, viewRect, true);
			float width = viewRect.width - 28f;
			Text.Font = GameFont.Tiny;
			float num = 0f;
			bool flag = false;
			foreach (LogMessage logMessage in Log.Messages)
			{
				if ((!EditWindow_Log.showMessages && logMessage.type == LogMessageType.Message) || (!EditWindow_Log.showWarnings && logMessage.type == LogMessageType.Warning) || (!EditWindow_Log.showErrors && logMessage.type == LogMessageType.Error))
				{
					if (EditWindow_Log.selectedMessage == logMessage)
					{
						EditWindow_Log.ClearSelectedMessage();
					}
				}
				else
				{
					string text = logMessage.ToString();
					if (text.Length > 1000)
					{
						text = text.Substring(0, 1000);
					}
					float num2 = Math.Min(Text.TinyFontSupported ? 30f : Text.LineHeight, Text.CalcHeight(text, width));
					GUI.color = new Color(1f, 1f, 1f, 0.7f);
					DevGUI.Label(new Rect(4f, num, 28f, num2), logMessage.repeats.ToStringCached());
					Rect rect = new Rect(28f, num, width, num2);
					if (EditWindow_Log.selectedMessage == logMessage)
					{
						GUI.DrawTexture(rect, EditWindow_Log.SelectedMessageTex);
					}
					else if (flag)
					{
						GUI.DrawTexture(rect, EditWindow_Log.AltMessageTex);
					}
					if (DevGUI.ButtonInvisible(rect))
					{
						EditWindow_Log.ClearSelectedMessage();
						EditWindow_Log.SelectedMessage = logMessage;
					}
					GUI.color = logMessage.Color;
					DevGUI.Label(rect, text);
					num += num2;
					flag = !flag;
				}
			}
			if (Event.current.type == EventType.Layout)
			{
				this.listingViewHeight = num;
			}
			DevGUI.EndScrollView();
			GUI.color = Color.white;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x000157F0 File Offset: 0x000139F0
		private void DoMessageDetails(Rect detailsRect, Rect outRect)
		{
			if (EditWindow_Log.selectedMessage == null)
			{
				return;
			}
			Rect rect = detailsRect;
			rect.height = 7f;
			Rect rect2 = detailsRect;
			rect2.yMin = rect.yMax;
			GUI.DrawTexture(rect, EditWindow_Log.StackTraceBorderTex);
			if (Mouse.IsOver(rect))
			{
				DevGUI.DrawHighlight(rect);
			}
			if (Event.current.type == EventType.MouseDown && Mouse.IsOver(rect))
			{
				this.borderDragging = true;
				Event.current.Use();
			}
			if (this.borderDragging)
			{
				EditWindow_Log.detailsPaneHeight = outRect.height + Mathf.Round(3.5f) - Event.current.mousePosition.y;
			}
			if (Event.current.rawType == EventType.MouseUp)
			{
				this.borderDragging = false;
			}
			GUI.DrawTexture(rect2, EditWindow_Log.StackTraceAreaTex);
			string text = EditWindow_Log.selectedMessage.text + "\n" + EditWindow_Log.selectedMessage.StackTrace;
			GUI.SetNextControlName(EditWindow_Log.MessageDetailsControlName);
			DevGUI.TextAreaScrollable(rect2, text, ref EditWindow_Log.detailsScrollPosition, true);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000158E8 File Offset: 0x00013AE8
		private void CopyAllMessagesToClipboard()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (LogMessage logMessage in Log.Messages)
			{
				if (stringBuilder.Length != 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine(logMessage.text);
				stringBuilder.Append(logMessage.StackTrace);
				if (stringBuilder[stringBuilder.Length - 1] != '\n')
				{
					stringBuilder.AppendLine();
				}
			}
			GUIUtility.systemCopyBuffer = stringBuilder.ToString();
		}

		// Token: 0x04000229 RID: 553
		private static LogMessage selectedMessage = null;

		// Token: 0x0400022A RID: 554
		private static Vector2 messagesScrollPosition;

		// Token: 0x0400022B RID: 555
		private static Vector2 detailsScrollPosition;

		// Token: 0x0400022C RID: 556
		private static float detailsPaneHeight = 100f;

		// Token: 0x0400022D RID: 557
		private static bool canAutoOpen = true;

		// Token: 0x0400022E RID: 558
		private static bool showErrors = true;

		// Token: 0x0400022F RID: 559
		private static bool showWarnings = true;

		// Token: 0x04000230 RID: 560
		private static bool showMessages = true;

		// Token: 0x04000231 RID: 561
		public static bool wantsToOpen = false;

		// Token: 0x04000232 RID: 562
		private float listingViewHeight;

		// Token: 0x04000233 RID: 563
		private bool borderDragging;

		// Token: 0x04000234 RID: 564
		private const float CountWidth = 28f;

		// Token: 0x04000235 RID: 565
		private const float Yinc = 25f;

		// Token: 0x04000236 RID: 566
		private const float DetailsPaneBorderHeight = 7f;

		// Token: 0x04000237 RID: 567
		private const float DetailsPaneMinHeight = 10f;

		// Token: 0x04000238 RID: 568
		private const float ListingMinHeight = 80f;

		// Token: 0x04000239 RID: 569
		private const float TopAreaHeight = 26f;

		// Token: 0x0400023A RID: 570
		private const float MessageMaxHeight = 30f;

		// Token: 0x0400023B RID: 571
		private static readonly Texture2D AltMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.17f, 0.17f, 0.17f, 0.85f));

		// Token: 0x0400023C RID: 572
		private static readonly Texture2D SelectedMessageTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.25f, 0.25f, 0.17f, 0.85f));

		// Token: 0x0400023D RID: 573
		private static readonly Texture2D StackTraceAreaTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.1f, 0.1f, 0.1f, 0.5f));

		// Token: 0x0400023E RID: 574
		private static readonly Texture2D StackTraceBorderTex = SolidColorMaterials.NewSolidColorTexture(new Color(0.3f, 0.3f, 0.3f, 1f));

		// Token: 0x0400023F RID: 575
		private static readonly Texture2D WarningIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/YellowWarning");

		// Token: 0x04000240 RID: 576
		private static readonly Texture2D ErrorIcon = Resources.Load<Texture2D>("Textures/UI/Widgets/Error");

		// Token: 0x04000241 RID: 577
		private static readonly Texture2D InfoIcon = Resources.Load<Texture2D>("Textures/UI/Buttons/InfoButton");

		// Token: 0x04000242 RID: 578
		private static readonly string MessageDetailsControlName = "MessageDetailsTextArea";
	}
}
