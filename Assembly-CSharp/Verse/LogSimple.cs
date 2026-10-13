using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CA9 RID: 3241
	public static class LogSimple
	{
		// Token: 0x06005316 RID: 21270 RVA: 0x001AC990 File Offset: 0x001AAB90
		public static void Message(string text)
		{
			for (int i = 0; i < LogSimple.tabDepth; i++)
			{
				text = "  " + text;
			}
			LogSimple.messages.Add(text);
		}

		// Token: 0x06005317 RID: 21271 RVA: 0x001AC9C5 File Offset: 0x001AABC5
		public static void BeginTabMessage(string text)
		{
			LogSimple.Message(text);
			LogSimple.tabDepth++;
		}

		// Token: 0x06005318 RID: 21272 RVA: 0x001AC9D9 File Offset: 0x001AABD9
		public static void EndTab()
		{
			LogSimple.tabDepth--;
		}

		// Token: 0x06005319 RID: 21273 RVA: 0x001AC9E8 File Offset: 0x001AABE8
		public static void FlushToFileAndOpen()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			string value = LogSimple.CompiledLog();
			string path = GenFilePaths.SaveDataFolderPath + Path.DirectorySeparatorChar.ToString() + "LogSimple.txt";
			using (StreamWriter streamWriter = new StreamWriter(path, false))
			{
				streamWriter.Write(value);
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				Application.OpenURL(path);
			});
			LogSimple.messages.Clear();
		}

		// Token: 0x0600531A RID: 21274 RVA: 0x001ACA7C File Offset: 0x001AAC7C
		public static void FlushToStandardLog()
		{
			if (LogSimple.messages.Count == 0)
			{
				return;
			}
			Log.Message(LogSimple.CompiledLog());
			LogSimple.messages.Clear();
		}

		// Token: 0x0600531B RID: 21275 RVA: 0x001ACAA0 File Offset: 0x001AACA0
		private static string CompiledLog()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string value in LogSimple.messages)
			{
				stringBuilder.AppendLine(value);
			}
			return stringBuilder.ToString().TrimEnd();
		}

		// Token: 0x04003C06 RID: 15366
		private static List<string> messages = new List<string>();

		// Token: 0x04003C07 RID: 15367
		private static int tabDepth = 0;
	}
}
