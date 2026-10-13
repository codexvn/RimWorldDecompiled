using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000109 RID: 265
	public static class GenFile
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x00025070 File Offset: 0x00023270
		public static string TextFromRawFile(string filePath)
		{
			return File.ReadAllText(filePath);
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x00025078 File Offset: 0x00023278
		public static string TextFromResourceFile(string filePath)
		{
			TextAsset textAsset = Resources.Load("Text/" + filePath) as TextAsset;
			if (textAsset == null)
			{
				Log.Message("Found no text asset in resources at " + filePath);
				return null;
			}
			return GenFile.GetTextWithoutBOM(textAsset);
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000250BC File Offset: 0x000232BC
		public static string GetTextWithoutBOM(TextAsset textAsset)
		{
			string result = null;
			using (MemoryStream memoryStream = new MemoryStream(textAsset.bytes))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x0002511C File Offset: 0x0002331C
		public static IEnumerable<string> LinesFromFile(string filePath)
		{
			string text = GenFile.TextFromResourceFile(filePath);
			foreach (string text2 in GenText.LinesFromString(text))
			{
				yield return text2;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x0002512C File Offset: 0x0002332C
		public static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs, bool useLinuxLineEndings = false, Func<string, string> destFileNameGetter = null)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDirName);
			DirectoryInfo[] directories = directoryInfo.GetDirectories();
			if (!directoryInfo.Exists)
			{
				throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
			}
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = fileInfo.Name;
				if (destFileNameGetter != null)
				{
					text = destFileNameGetter(text);
				}
				string text2 = Path.Combine(destDirName, text);
				if (useLinuxLineEndings && (fileInfo.Extension == ".sh" || fileInfo.Extension == ".txt"))
				{
					if (!File.Exists(text2))
					{
						File.WriteAllText(text2, File.ReadAllText(fileInfo.FullName).Replace("\r\n", "\n").Replace("\r", "\n"));
					}
				}
				else
				{
					fileInfo.CopyTo(text2, false);
				}
			}
			if (copySubDirs)
			{
				foreach (DirectoryInfo directoryInfo2 in directories)
				{
					string destDirName2 = Path.Combine(destDirName, directoryInfo2.Name);
					GenFile.DirectoryCopy(directoryInfo2.FullName, destDirName2, copySubDirs, useLinuxLineEndings, null);
				}
			}
		}

		// Token: 0x060007CF RID: 1999 RVA: 0x00025254 File Offset: 0x00023454
		public static string SanitizedFileName(string fileName)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			string text = "";
			for (int i = 0; i < fileName.Length; i++)
			{
				if (!invalidFileNameChars.Contains(fileName[i]))
				{
					text += fileName[i].ToString();
				}
			}
			if (text.Length == 0)
			{
				text = "unnamed";
			}
			return text;
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x000252B4 File Offset: 0x000234B4
		public static string ResolveCaseInsensitiveFilePath(string dir, string targetFileName)
		{
			string text = Path.Combine(dir, targetFileName);
			if (File.Exists(text))
			{
				return text;
			}
			if (Directory.Exists(dir))
			{
				foreach (string path in Directory.GetFiles(dir))
				{
					if (string.Compare(Path.GetFileName(path), targetFileName, StringComparison.CurrentCultureIgnoreCase) == 0)
					{
						return dir + Path.DirectorySeparatorChar.ToString() + Path.GetFileName(path);
					}
				}
			}
			return dir + Path.DirectorySeparatorChar.ToString() + targetFileName;
		}
	}
}
