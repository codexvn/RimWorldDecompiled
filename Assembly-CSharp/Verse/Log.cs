using System;
using System.Collections.Generic;
using System.Threading;
using LudeonTK;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200013E RID: 318
	public static class Log
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060009AB RID: 2475 RVA: 0x0002DB15 File Offset: 0x0002BD15
		public static IEnumerable<LogMessage> Messages
		{
			get
			{
				return Log.messageQueue.Messages;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x0002DB21 File Offset: 0x0002BD21
		private static bool PreventLogging
		{
			get
			{
				return Log.reachedMaxMessagesLimit || Log.logDisablers > 0;
			}
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x0002DB34 File Offset: 0x0002BD34
		public static Log.LogLock LockMessages()
		{
			return new Log.LogLock();
		}

		// Token: 0x060009AE RID: 2478 RVA: 0x0002DB3C File Offset: 0x0002BD3C
		public static void ResetMessageCount()
		{
			object obj = Log.logLock;
			lock (obj)
			{
				Log.messageCount = 0;
				if (Log.reachedMaxMessagesLimit)
				{
					Debug.unityLogger.logEnabled = true;
					Log.reachedMaxMessagesLimit = false;
					Log.Message("Message logging is now once again on.");
				}
			}
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x0002DBA0 File Offset: 0x0002BDA0
		public static void Message(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					bool flag2;
					Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Message, text, StackTraceUtility.ExtractStackTrace()), out flag2);
					if (!flag2)
					{
						Debug.Log(text);
						Log.PostMessage();
					}
				}
			}
		}

		// Token: 0x060009B0 RID: 2480 RVA: 0x0002DC0C File Offset: 0x0002BE0C
		public static void Message(object obj)
		{
			Log.Message(obj.ToString());
		}

		// Token: 0x060009B1 RID: 2481 RVA: 0x0002DC1C File Offset: 0x0002BE1C
		public static void Warning(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					bool flag2;
					Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Warning, text, StackTraceUtility.ExtractStackTrace()), out flag2);
					if (!flag2)
					{
						Debug.LogWarning(text);
						Log.PostMessage();
						if (Prefs.OpenLogOnWarnings)
						{
							Log.TryOpenLogWindow();
						}
					}
				}
			}
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0002DC94 File Offset: 0x0002BE94
		public static void WarningOnce(string text, int key)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					if (Log.usedKeys.Add(key))
					{
						Log.Warning(text);
					}
				}
			}
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0002DCEC File Offset: 0x0002BEEC
		public static void Error(string text)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					if (!Log.currentlyLoggingError)
					{
						Log.currentlyLoggingError = true;
						try
						{
							if (DebugSettings.pauseOnError && Current.ProgramState == ProgramState.Playing)
							{
								Find.TickManager.Pause();
							}
							bool flag2;
							Log.messageQueue.Enqueue(new LogMessage(LogMessageType.Error, text, StackTraceUtility.ExtractStackTrace()), out flag2);
							if (flag2)
							{
								return;
							}
							Log.PostMessage();
							if (!PlayDataLoader.Loaded || Prefs.DevMode)
							{
								Log.TryOpenLogWindow();
							}
						}
						catch (Exception ex)
						{
							string str = "An error occurred while logging an error: ";
							Exception ex2 = ex;
							Debug.LogError(str + ((ex2 != null) ? ex2.ToString() : null));
						}
						finally
						{
							Log.currentlyLoggingError = false;
						}
					}
					Debug.LogError(text);
				}
			}
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0002DDD4 File Offset: 0x0002BFD4
		public static void ErrorOnce(string text, int key)
		{
			object obj = Log.logLock;
			lock (obj)
			{
				if (!Log.PreventLogging)
				{
					if (Log.usedKeys.Add(key))
					{
						Log.Error(text);
					}
				}
			}
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0002DE2C File Offset: 0x0002C02C
		public static void Clear()
		{
			object obj = Log.logLock;
			lock (obj)
			{
				EditWindow_Log.ClearSelectedMessage();
				Log.messageQueue.Clear();
				Log.ResetMessageCount();
			}
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0002DE7C File Offset: 0x0002C07C
		public static void TryOpenLogWindow()
		{
			if (StaticConstructorOnStartupUtility.coreStaticAssetsLoaded || UnityData.IsInMainThread)
			{
				EditWindow_Log.TryAutoOpen();
			}
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0002DE91 File Offset: 0x0002C091
		private static void PostMessage()
		{
			if (Log.openOnMessage)
			{
				Log.TryOpenLogWindow();
				EditWindow_Log.SelectLastMessage(true);
			}
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0002DEA5 File Offset: 0x0002C0A5
		public static void Notify_MessageReceivedThreadedInternal(string msg, string stackTrace, LogType type)
		{
			if (++Log.messageCount == 10000)
			{
				Log.Warning("Reached max messages limit. Stopping logging to avoid spam.");
				Log.reachedMaxMessagesLimit = true;
				Debug.unityLogger.logEnabled = false;
			}
		}

		// Token: 0x040004DD RID: 1245
		private static LogMessageQueue messageQueue = new LogMessageQueue();

		// Token: 0x040004DE RID: 1246
		private static HashSet<int> usedKeys = new HashSet<int>();

		// Token: 0x040004DF RID: 1247
		public static bool openOnMessage = false;

		// Token: 0x040004E0 RID: 1248
		private static bool currentlyLoggingError;

		// Token: 0x040004E1 RID: 1249
		private static int messageCount;

		// Token: 0x040004E2 RID: 1250
		private static bool reachedMaxMessagesLimit;

		// Token: 0x040004E3 RID: 1251
		private static object logLock = new object();

		// Token: 0x040004E4 RID: 1252
		private static int logDisablers;

		// Token: 0x040004E5 RID: 1253
		private const int StopLoggingAtMessageCount = 10000;

		// Token: 0x0200013F RID: 319
		public class LogLock : IDisposable
		{
			// Token: 0x060009BA RID: 2490 RVA: 0x0002DEFC File Offset: 0x0002C0FC
			public LogLock()
			{
				Log.logDisablers = Interlocked.Increment(ref Log.logDisablers);
			}

			// Token: 0x060009BB RID: 2491 RVA: 0x0002DF13 File Offset: 0x0002C113
			public void Dispose()
			{
				Log.logDisablers = Interlocked.Decrement(ref Log.logDisablers);
			}
		}
	}
}
