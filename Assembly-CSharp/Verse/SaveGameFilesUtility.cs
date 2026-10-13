using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Verse
{
	// Token: 0x0200084B RID: 2123
	public static class SaveGameFilesUtility
	{
		// Token: 0x060035C2 RID: 13762 RVA: 0x0011E1EC File Offset: 0x0011C3EC
		public static bool IsAutoSave(string fileName)
		{
			return fileName.Length >= 8 && fileName.Substring(0, 8) == "Autosave";
		}

		// Token: 0x060035C3 RID: 13763 RVA: 0x0011E20C File Offset: 0x0011C40C
		public static bool SavedGameNamedExists(string fileName)
		{
			using (IEnumerator<string> enumerator = (from f in GenFilePaths.AllSavedGameFiles
			select Path.GetFileNameWithoutExtension(f.Name)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current == fileName)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060035C4 RID: 13764 RVA: 0x0011E284 File Offset: 0x0011C484
		public static string UnusedDefaultFileName(string factionLabel)
		{
			int num = 1;
			string text;
			do
			{
				text = factionLabel + num.ToString();
				num++;
			}
			while (SaveGameFilesUtility.SavedGameNamedExists(text));
			return text;
		}

		// Token: 0x060035C5 RID: 13765 RVA: 0x0011E2B4 File Offset: 0x0011C4B4
		public static FileInfo GetAutostartSaveFile()
		{
			if (!Prefs.DevMode)
			{
				return null;
			}
			return GenFilePaths.AllSavedGameFiles.FirstOrDefault((FileInfo x) => Path.GetFileNameWithoutExtension(x.Name).ToLower() == "autostart".ToLower());
		}
	}
}
