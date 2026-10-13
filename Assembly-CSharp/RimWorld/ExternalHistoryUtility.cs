using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B57 RID: 6999
	public static class ExternalHistoryUtility
	{
		// Token: 0x06009C57 RID: 40023 RVA: 0x002D5BA0 File Offset: 0x002D3DA0
		static ExternalHistoryUtility()
		{
			try
			{
				ExternalHistoryUtility.cachedFiles = GenFilePaths.AllExternalHistoryFiles.ToList<FileInfo>();
			}
			catch (Exception ex)
			{
				Log.Error("Could not get external history files: " + ex.Message);
			}
		}

		// Token: 0x17001811 RID: 6161
		// (get) Token: 0x06009C58 RID: 40024 RVA: 0x002D5BF8 File Offset: 0x002D3DF8
		public static IEnumerable<FileInfo> Files
		{
			get
			{
				int num;
				for (int i = 0; i < ExternalHistoryUtility.cachedFiles.Count; i = num)
				{
					yield return ExternalHistoryUtility.cachedFiles[i];
					num = i + 1;
				}
				yield break;
			}
		}

		// Token: 0x06009C59 RID: 40025 RVA: 0x002D5C04 File Offset: 0x002D3E04
		public static ExternalHistory Load(string path)
		{
			ExternalHistory result = null;
			try
			{
				result = new ExternalHistory();
				Scribe.loader.InitLoading(path);
				try
				{
					Scribe_Deep.Look<ExternalHistory>(ref result, "externalHistory", Array.Empty<object>());
					Scribe.loader.FinalizeLoading();
				}
				catch
				{
					Scribe.ForceStop();
					throw;
				}
			}
			catch (Exception ex)
			{
				Log.Error("Could not load external history (" + path + "): " + ex.Message);
				return null;
			}
			return result;
		}

		// Token: 0x06009C5A RID: 40026 RVA: 0x002D5C8C File Offset: 0x002D3E8C
		public static string GetRandomGameplayID()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < ExternalHistoryUtility.gameplayIDLength; i++)
			{
				int index = Rand.Range(0, ExternalHistoryUtility.gameplayIDAvailableChars.Length);
				stringBuilder.Append(ExternalHistoryUtility.gameplayIDAvailableChars[index]);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06009C5B RID: 40027 RVA: 0x002D5CD8 File Offset: 0x002D3ED8
		public static bool IsValidGameplayID(string ID)
		{
			if (ID.NullOrEmpty() || ID.Length != ExternalHistoryUtility.gameplayIDLength)
			{
				return false;
			}
			for (int i = 0; i < ID.Length; i++)
			{
				bool flag = false;
				for (int j = 0; j < ExternalHistoryUtility.gameplayIDAvailableChars.Length; j++)
				{
					if (ID[i] == ExternalHistoryUtility.gameplayIDAvailableChars[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009C5C RID: 40028 RVA: 0x002D5D44 File Offset: 0x002D3F44
		public static string GetCurrentUploadDate()
		{
			return DateTime.UtcNow.ToString("yyMMdd");
		}

		// Token: 0x06009C5D RID: 40029 RVA: 0x002D5D64 File Offset: 0x002D3F64
		public static int GetCurrentUploadTime()
		{
			return (int)(DateTime.UtcNow.TimeOfDay.TotalSeconds / 2.0);
		}

		// Token: 0x04006C6C RID: 27756
		private static List<FileInfo> cachedFiles;

		// Token: 0x04006C6D RID: 27757
		private static int gameplayIDLength = 20;

		// Token: 0x04006C6E RID: 27758
		private static string gameplayIDAvailableChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
	}
}
