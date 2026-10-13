using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using RimWorld;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Verse
{
	// Token: 0x02000143 RID: 323
	public static class LongEventHandler
	{
		// Token: 0x1700017B RID: 379
		// (get) Token: 0x060009C7 RID: 2503 RVA: 0x0002E155 File Offset: 0x0002C355
		public static bool ShouldWaitForEvent
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting && ((LongEventHandler.currentEvent != null && !LongEventHandler.currentEvent.UseStandardWindow) || (Find.UIRoot == null || Find.WindowStack == null));
			}
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060009C8 RID: 2504 RVA: 0x0002E186 File Offset: 0x0002C386
		public static bool AnyEventNowOrWaiting
		{
			get
			{
				return LongEventHandler.currentEvent != null || LongEventHandler.eventQueue.Count > 0;
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060009C9 RID: 2505 RVA: 0x0002E1A0 File Offset: 0x0002C3A0
		public static bool AnyEventWhichDoesntUseStandardWindowNowOrWaiting
		{
			get
			{
				LongEventHandler.QueuedLongEvent queuedLongEvent = LongEventHandler.currentEvent;
				if (queuedLongEvent != null && !queuedLongEvent.UseStandardWindow)
				{
					return true;
				}
				return LongEventHandler.eventQueue.Any((LongEventHandler.QueuedLongEvent x) => !x.UseStandardWindow);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060009CA RID: 2506 RVA: 0x0002E1E9 File Offset: 0x0002C3E9
		public static bool ForcePause
		{
			get
			{
				return LongEventHandler.AnyEventNowOrWaiting;
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0002E1F0 File Offset: 0x0002C3F0
		public static void QueueLongEvent(Action action, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true, bool forceHideUI = false, Action callback = null)
		{
			LongEventHandler.QueuedLongEvent item = new LongEventHandler.QueuedLongEvent
			{
				eventAction = action,
				eventTextKey = textKey,
				doAsynchronously = doAsynchronously,
				exceptionHandler = exceptionHandler,
				canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting,
				showExtraUIInfo = showExtraUIInfo,
				callback = callback,
				forceHideUI = forceHideUI
			};
			LongEventHandler.eventQueue.Enqueue(item);
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x0002E250 File Offset: 0x0002C450
		public static void QueueLongEvent(IEnumerable action, string textKey, Action<Exception> exceptionHandler = null, bool showExtraUIInfo = true, bool forceHideUI = false)
		{
			LongEventHandler.QueuedLongEvent item = new LongEventHandler.QueuedLongEvent
			{
				eventActionEnumerator = action.GetEnumerator(),
				eventTextKey = textKey,
				doAsynchronously = false,
				exceptionHandler = exceptionHandler,
				canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting,
				showExtraUIInfo = showExtraUIInfo,
				forceHideUI = forceHideUI
			};
			LongEventHandler.eventQueue.Enqueue(item);
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002E2AC File Offset: 0x0002C4AC
		public static void QueueLongEvent(Action preLoadLevelAction, string levelToLoad, string textKey, bool doAsynchronously, Action<Exception> exceptionHandler, bool showExtraUIInfo = true, bool forceHideUI = false)
		{
			LongEventHandler.QueuedLongEvent item = new LongEventHandler.QueuedLongEvent
			{
				eventAction = preLoadLevelAction,
				levelToLoad = levelToLoad,
				eventTextKey = textKey,
				doAsynchronously = doAsynchronously,
				exceptionHandler = exceptionHandler,
				canEverUseStandardWindow = !LongEventHandler.AnyEventWhichDoesntUseStandardWindowNowOrWaiting,
				showExtraUIInfo = showExtraUIInfo,
				forceHideUI = forceHideUI
			};
			LongEventHandler.eventQueue.Enqueue(item);
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x0002E30C File Offset: 0x0002C50C
		public static void ClearQueuedEvents()
		{
			LongEventHandler.eventQueue.Clear();
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0002E318 File Offset: 0x0002C518
		public static void LongEventsOnGUI()
		{
			if (LongEventHandler.currentEvent == null)
			{
				GameplayTipWindow.ResetTipTimer();
				return;
			}
			if (LongEventHandler.currentEvent.forceHideUI)
			{
				return;
			}
			float num = LongEventHandler.StatusRectSize.x;
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				Text.Font = GameFont.Small;
				num = Mathf.Max(num, Text.CalcSize(LongEventHandler.currentEvent.eventText + "...").x + 40f);
			}
			bool flag2 = Find.UIRoot != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			bool flag3 = Find.UIRoot != null && Current.Game != null && !LongEventHandler.currentEvent.UseStandardWindow && LongEventHandler.currentEvent.showExtraUIInfo;
			Vector2 vector = flag3 ? ModSummaryWindow.GetEffectiveSize() : Vector2.zero;
			float num2 = LongEventHandler.StatusRectSize.y;
			if (flag3)
			{
				num2 += 17f + vector.y;
			}
			if (flag2)
			{
				num2 += 17f + GameplayTipWindow.WindowSize.y;
			}
			float num3 = ((float)UI.screenHeight - num2) / 2f;
			Vector2 vector2 = new Vector2(((float)UI.screenWidth - GameplayTipWindow.WindowSize.x) / 2f, num3 + LongEventHandler.StatusRectSize.y + 17f);
			Vector2 offset = new Vector2(((float)UI.screenWidth - vector.x) / 2f, vector2.y + GameplayTipWindow.WindowSize.y + 17f);
			Rect rect = new Rect(((float)UI.screenWidth - num) / 2f, num3, num, LongEventHandler.StatusRectSize.y);
			rect = rect.Rounded();
			if (!LongEventHandler.currentEvent.UseStandardWindow || Find.UIRoot == null || Find.WindowStack == null)
			{
				if (UIMenuBackgroundManager.background == null)
				{
					UIMenuBackgroundManager.background = new UI_BackgroundMain();
				}
				UIMenuBackgroundManager.background.BackgroundOnGUI();
				Widgets.DrawShadowAround(rect);
				Widgets.DrawWindowBackground(rect);
				LongEventHandler.DrawLongEventWindowContents(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, false);
				}
				if (flag3)
				{
					ModSummaryWindow.DrawWindow(offset, false);
					TooltipHandler.DoTooltipGUI();
					return;
				}
			}
			else
			{
				LongEventHandler.DrawLongEventWindow(rect);
				if (flag2)
				{
					GameplayTipWindow.DrawWindow(vector2, true);
				}
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x0002E554 File Offset: 0x0002C754
		private static void DrawLongEventWindow(Rect statusRect)
		{
			Find.WindowStack.ImmediateWindow(62893994, statusRect, WindowLayer.Super, delegate
			{
				LongEventHandler.DrawLongEventWindowContents(statusRect.AtZero());
			}, true, false, 1f, null, true);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002E59C File Offset: 0x0002C79C
		public static void LongEventsUpdate(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent != null)
			{
				if (LongEventHandler.currentEvent.eventActionEnumerator != null)
				{
					LongEventHandler.UpdateCurrentEnumeratorEvent();
				}
				else if (LongEventHandler.currentEvent.doAsynchronously)
				{
					LongEventHandler.UpdateCurrentAsynchronousEvent();
				}
				else
				{
					LongEventHandler.UpdateCurrentSynchronousEvent(out sceneChanged);
				}
			}
			if (LongEventHandler.currentEvent == null && LongEventHandler.eventQueue.Count > 0)
			{
				LongEventHandler.currentEvent = LongEventHandler.eventQueue.Dequeue();
				if (LongEventHandler.currentEvent.eventTextKey == null)
				{
					LongEventHandler.currentEvent.eventText = "";
					return;
				}
				LongEventHandler.currentEvent.eventText = LongEventHandler.currentEvent.eventTextKey.Translate();
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002E63C File Offset: 0x0002C83C
		public static void ExecuteWhenFinished(Action action)
		{
			LongEventHandler.toExecuteWhenFinished.Add(action);
			if ((LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.ShouldWaitUntilDisplayed) && !LongEventHandler.executingToExecuteWhenFinished)
			{
				LongEventHandler.ExecuteToExecuteWhenFinished();
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x0002E668 File Offset: 0x0002C868
		public static void ForceExecuteToExecuteWhenFinished()
		{
			LongEventHandler.ExecuteToExecuteWhenFinished();
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x0002E670 File Offset: 0x0002C870
		public static void SetCurrentEventText(string newText)
		{
			object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
			lock (currentEventTextLock)
			{
				if (LongEventHandler.currentEvent != null)
				{
					LongEventHandler.currentEvent.eventText = newText;
				}
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002E6C0 File Offset: 0x0002C8C0
		private static void UpdateCurrentEnumeratorEvent()
		{
			try
			{
				float num = Time.realtimeSinceStartup + 0.1f;
				while (LongEventHandler.currentEvent.eventActionEnumerator.MoveNext())
				{
					if (num <= Time.realtimeSinceStartup)
					{
						return;
					}
				}
				IDisposable disposable = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
				Action callback = LongEventHandler.currentEvent.callback;
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
				if (callback != null)
				{
					callback();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception from long event: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				if (LongEventHandler.currentEvent != null)
				{
					IDisposable disposable2 = LongEventHandler.currentEvent.eventActionEnumerator as IDisposable;
					if (disposable2 != null)
					{
						disposable2.Dispose();
					}
					Action<Exception> exceptionHandler = LongEventHandler.currentEvent.exceptionHandler;
					if (exceptionHandler != null)
					{
						exceptionHandler(ex);
					}
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x0002E7B4 File Offset: 0x0002C9B4
		private static void UpdateCurrentAsynchronousEvent()
		{
			if (LongEventHandler.eventThread == null)
			{
				LongEventHandler.eventThread = new Thread(delegate()
				{
					LongEventHandler.RunEventFromAnotherThread(LongEventHandler.currentEvent.eventAction);
				});
				LongEventHandler.eventThread.Start();
				return;
			}
			if (!LongEventHandler.eventThread.IsAlive)
			{
				bool flag = false;
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					if (LongEventHandler.levelLoadOp == null)
					{
						LongEventHandler.levelLoadOp = SceneManager.LoadSceneAsync(LongEventHandler.currentEvent.levelToLoad);
					}
					else if (LongEventHandler.levelLoadOp.isDone)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					LongEventHandler.ExecuteToExecuteWhenFinished();
					Action callback = LongEventHandler.currentEvent.callback;
					if (callback != null)
					{
						callback();
					}
					LongEventHandler.currentEvent = null;
					LongEventHandler.eventThread = null;
					LongEventHandler.levelLoadOp = null;
				}
			}
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x0002E87C File Offset: 0x0002CA7C
		private static void UpdateCurrentSynchronousEvent(out bool sceneChanged)
		{
			sceneChanged = false;
			if (LongEventHandler.currentEvent.ShouldWaitUntilDisplayed)
			{
				return;
			}
			try
			{
				if (LongEventHandler.currentEvent.eventAction != null)
				{
					LongEventHandler.currentEvent.eventAction();
				}
				if (!LongEventHandler.currentEvent.levelToLoad.NullOrEmpty())
				{
					SceneManager.LoadScene(LongEventHandler.currentEvent.levelToLoad);
					sceneChanged = true;
				}
				Action callback = LongEventHandler.currentEvent.callback;
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
				LongEventHandler.ExecuteToExecuteWhenFinished();
				if (callback != null)
				{
					callback();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception from long event: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
				{
					LongEventHandler.currentEvent.exceptionHandler(ex);
				}
				LongEventHandler.currentEvent = null;
				LongEventHandler.eventThread = null;
				LongEventHandler.levelLoadOp = null;
			}
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x0002E96C File Offset: 0x0002CB6C
		private static void RunEventFromAnotherThread(Action action)
		{
			CultureInfoUtility.EnsureEnglish();
			try
			{
				if (action != null)
				{
					action();
				}
			}
			catch (Exception ex)
			{
				string str = "Exception from asynchronous event: ";
				Exception ex2 = ex;
				Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				try
				{
					if (LongEventHandler.currentEvent != null && LongEventHandler.currentEvent.exceptionHandler != null)
					{
						LongEventHandler.currentEvent.exceptionHandler(ex);
					}
				}
				catch (Exception ex3)
				{
					string str2 = "Exception was thrown while trying to handle exception. Exception: ";
					Exception ex4 = ex3;
					Log.Error(str2 + ((ex4 != null) ? ex4.ToString() : null));
				}
			}
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0002EA08 File Offset: 0x0002CC08
		private static void ExecuteToExecuteWhenFinished()
		{
			if (LongEventHandler.executingToExecuteWhenFinished)
			{
				Log.Warning("Already executing.");
				return;
			}
			LongEventHandler.executingToExecuteWhenFinished = true;
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.Start("ExecuteToExecuteWhenFinished()");
			}
			for (int i = 0; i < LongEventHandler.toExecuteWhenFinished.Count; i++)
			{
				DeepProfiler.Start(LongEventHandler.toExecuteWhenFinished[i].Method.DeclaringType.ToString() + " -> " + LongEventHandler.toExecuteWhenFinished[i].Method.ToString());
				try
				{
					LongEventHandler.toExecuteWhenFinished[i]();
				}
				catch (Exception ex)
				{
					string str = "Could not execute post-long-event action. Exception: ";
					Exception ex2 = ex;
					Log.Error(str + ((ex2 != null) ? ex2.ToString() : null));
				}
				finally
				{
					DeepProfiler.End();
				}
			}
			if (LongEventHandler.toExecuteWhenFinished.Count > 0)
			{
				DeepProfiler.End();
			}
			LongEventHandler.toExecuteWhenFinished.Clear();
			LongEventHandler.executingToExecuteWhenFinished = false;
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0002EB10 File Offset: 0x0002CD10
		private static void DrawLongEventWindowContents(Rect rect)
		{
			if (LongEventHandler.currentEvent == null || LongEventHandler.currentEvent.forceHideUI)
			{
				return;
			}
			if (Event.current.type == EventType.Repaint)
			{
				LongEventHandler.currentEvent.alreadyDisplayed = true;
			}
			Text.Font = GameFont.Small;
			Text.Anchor = TextAnchor.MiddleCenter;
			float num = 0f;
			if (LongEventHandler.levelLoadOp != null)
			{
				float f = 1f;
				if (!LongEventHandler.levelLoadOp.isDone)
				{
					f = LongEventHandler.levelLoadOp.progress;
				}
				TaggedString taggedString = "LoadingAssets".Translate() + " " + f.ToStringPercent();
				num = Text.CalcSize(taggedString).x;
				Widgets.Label(rect, taggedString);
			}
			else
			{
				object currentEventTextLock = LongEventHandler.CurrentEventTextLock;
				lock (currentEventTextLock)
				{
					num = Text.CalcSize(LongEventHandler.currentEvent.eventText).x;
					Widgets.Label(rect, LongEventHandler.currentEvent.eventText);
				}
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			rect.xMin = rect.center.x + num / 2f;
			Widgets.Label(rect, (!LongEventHandler.currentEvent.UseAnimatedDots) ? "..." : GenText.MarchingEllipsis(0f));
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x040004F2 RID: 1266
		private static Queue<LongEventHandler.QueuedLongEvent> eventQueue = new Queue<LongEventHandler.QueuedLongEvent>();

		// Token: 0x040004F3 RID: 1267
		private static LongEventHandler.QueuedLongEvent currentEvent = null;

		// Token: 0x040004F4 RID: 1268
		private static Thread eventThread = null;

		// Token: 0x040004F5 RID: 1269
		private static AsyncOperation levelLoadOp = null;

		// Token: 0x040004F6 RID: 1270
		private static List<Action> toExecuteWhenFinished = new List<Action>();

		// Token: 0x040004F7 RID: 1271
		private static bool executingToExecuteWhenFinished = false;

		// Token: 0x040004F8 RID: 1272
		private static readonly object CurrentEventTextLock = new object();

		// Token: 0x040004F9 RID: 1273
		private static readonly Vector2 StatusRectSize = new Vector2(240f, 75f);

		// Token: 0x02000144 RID: 324
		private class QueuedLongEvent
		{
			// Token: 0x1700017F RID: 383
			// (get) Token: 0x060009DC RID: 2524 RVA: 0x0002ECAF File Offset: 0x0002CEAF
			public bool UseAnimatedDots
			{
				get
				{
					return this.doAsynchronously || this.eventActionEnumerator != null;
				}
			}

			// Token: 0x17000180 RID: 384
			// (get) Token: 0x060009DD RID: 2525 RVA: 0x0002ECC4 File Offset: 0x0002CEC4
			public bool ShouldWaitUntilDisplayed
			{
				get
				{
					return !this.alreadyDisplayed && this.UseStandardWindow && !this.eventText.NullOrEmpty();
				}
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x060009DE RID: 2526 RVA: 0x0002ECE6 File Offset: 0x0002CEE6
			public bool UseStandardWindow
			{
				get
				{
					return this.canEverUseStandardWindow && !this.doAsynchronously && this.eventActionEnumerator == null;
				}
			}

			// Token: 0x040004FA RID: 1274
			public Action eventAction;

			// Token: 0x040004FB RID: 1275
			public Action callback;

			// Token: 0x040004FC RID: 1276
			public IEnumerator eventActionEnumerator;

			// Token: 0x040004FD RID: 1277
			public string levelToLoad;

			// Token: 0x040004FE RID: 1278
			public string eventTextKey = "";

			// Token: 0x040004FF RID: 1279
			public string eventText = "";

			// Token: 0x04000500 RID: 1280
			public bool doAsynchronously;

			// Token: 0x04000501 RID: 1281
			public Action<Exception> exceptionHandler;

			// Token: 0x04000502 RID: 1282
			public bool alreadyDisplayed;

			// Token: 0x04000503 RID: 1283
			public bool canEverUseStandardWindow = true;

			// Token: 0x04000504 RID: 1284
			public bool showExtraUIInfo = true;

			// Token: 0x04000505 RID: 1285
			public bool forceHideUI;
		}
	}
}
