using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02003E8B RID: 16011
	public static class AbstractFilesystem
	{
		// Token: 0x06015E9B RID: 89755 RVA: 0x0068E51B File Offset: 0x0068C71B
		public static void ClearAllCache()
		{
			TarDirectory.ClearCache();
		}

		// Token: 0x06015E9C RID: 89756 RVA: 0x0068E524 File Offset: 0x0068C724
		public static List<VirtualDirectory> GetDirectories(string filesystemPath, string searchPattern, SearchOption searchOption, bool allowArchiveAndRealFolderDuplicates = false)
		{
			List<VirtualDirectory> list = new List<VirtualDirectory>();
			foreach (string text in Directory.GetDirectories(filesystemPath, searchPattern, searchOption))
			{
				string text2 = text + ".tar";
				if (!allowArchiveAndRealFolderDuplicates && File.Exists(text2))
				{
					list.Add(TarDirectory.ReadFromFileOrCache(text2));
				}
				else
				{
					list.Add(new FilesystemDirectory(text));
				}
			}
			foreach (string text3 in Directory.GetFiles(filesystemPath, searchPattern, searchOption))
			{
				if (!(Path.GetExtension(text3) != ".tar"))
				{
					if (!allowArchiveAndRealFolderDuplicates)
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text3);
						bool flag = false;
						using (List<VirtualDirectory>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.Name == fileNameWithoutExtension)
								{
									flag = true;
									break;
								}
							}
						}
						if (flag)
						{
							goto IL_D7;
						}
					}
					list.Add(TarDirectory.ReadFromFileOrCache(text3));
				}
				IL_D7:;
			}
			return list;
		}

		// Token: 0x06015E9D RID: 89757 RVA: 0x0068E624 File Offset: 0x0068C824
		public static VirtualDirectory GetDirectory(string filesystemPath)
		{
			if (Path.GetExtension(filesystemPath) == ".tar")
			{
				return TarDirectory.ReadFromFileOrCache(filesystemPath);
			}
			string text = filesystemPath + ".tar";
			if (File.Exists(text))
			{
				return TarDirectory.ReadFromFileOrCache(text);
			}
			return new FilesystemDirectory(filesystemPath);
		}
	}
}
