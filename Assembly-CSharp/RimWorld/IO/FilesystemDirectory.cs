using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02003E8C RID: 16012
	internal class FilesystemDirectory : VirtualDirectory
	{
		// Token: 0x170036CE RID: 14030
		// (get) Token: 0x06015E9E RID: 89758 RVA: 0x0068E66B File Offset: 0x0068C86B
		public override string Name
		{
			get
			{
				return this.dirInfo.Name;
			}
		}

		// Token: 0x170036CF RID: 14031
		// (get) Token: 0x06015E9F RID: 89759 RVA: 0x0068E678 File Offset: 0x0068C878
		public override string FullPath
		{
			get
			{
				return this.dirInfo.FullName;
			}
		}

		// Token: 0x170036D0 RID: 14032
		// (get) Token: 0x06015EA0 RID: 89760 RVA: 0x0068E685 File Offset: 0x0068C885
		public override bool Exists
		{
			get
			{
				return this.dirInfo.Exists;
			}
		}

		// Token: 0x06015EA1 RID: 89761 RVA: 0x0068E692 File Offset: 0x0068C892
		public FilesystemDirectory(string dir)
		{
			this.dirInfo = new DirectoryInfo(dir);
		}

		// Token: 0x06015EA2 RID: 89762 RVA: 0x0068E6A6 File Offset: 0x0068C8A6
		public FilesystemDirectory(DirectoryInfo dir)
		{
			this.dirInfo = dir;
		}

		// Token: 0x06015EA3 RID: 89763 RVA: 0x0068E6B5 File Offset: 0x0068C8B5
		public override IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption)
		{
			foreach (DirectoryInfo dir in this.dirInfo.GetDirectories(searchPattern, searchOption))
			{
				yield return new FilesystemDirectory(dir);
			}
			DirectoryInfo[] array = null;
			yield break;
		}

		// Token: 0x06015EA4 RID: 89764 RVA: 0x0068E6D3 File Offset: 0x0068C8D3
		public override VirtualDirectory GetDirectory(string directoryName)
		{
			return new FilesystemDirectory(Path.Combine(this.FullPath, directoryName));
		}

		// Token: 0x06015EA5 RID: 89765 RVA: 0x0068E6E6 File Offset: 0x0068C8E6
		public override VirtualFile GetFile(string filename)
		{
			return new FilesystemFile(new FileInfo(Path.Combine(this.FullPath, filename)));
		}

		// Token: 0x06015EA6 RID: 89766 RVA: 0x0068E6FE File Offset: 0x0068C8FE
		public override IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption)
		{
			foreach (FileInfo fileInfo in this.dirInfo.GetFiles(searchPattern, searchOption))
			{
				yield return new FilesystemFile(fileInfo);
			}
			FileInfo[] array = null;
			yield break;
		}

		// Token: 0x0400F300 RID: 62208
		private DirectoryInfo dirInfo;
	}
}
