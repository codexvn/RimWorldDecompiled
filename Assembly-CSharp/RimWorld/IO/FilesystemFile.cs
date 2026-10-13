using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02003E8F RID: 16015
	internal class FilesystemFile : VirtualFile
	{
		// Token: 0x170036D5 RID: 14037
		// (get) Token: 0x06015EB7 RID: 89783 RVA: 0x0068E973 File Offset: 0x0068CB73
		public override string Name
		{
			get
			{
				return this.fileInfo.Name;
			}
		}

		// Token: 0x170036D6 RID: 14038
		// (get) Token: 0x06015EB8 RID: 89784 RVA: 0x0068E980 File Offset: 0x0068CB80
		public override string FullPath
		{
			get
			{
				return this.fileInfo.FullName;
			}
		}

		// Token: 0x170036D7 RID: 14039
		// (get) Token: 0x06015EB9 RID: 89785 RVA: 0x0068E98D File Offset: 0x0068CB8D
		public override bool Exists
		{
			get
			{
				return this.fileInfo.Exists;
			}
		}

		// Token: 0x170036D8 RID: 14040
		// (get) Token: 0x06015EBA RID: 89786 RVA: 0x0068E99A File Offset: 0x0068CB9A
		public override long Length
		{
			get
			{
				return this.fileInfo.Length;
			}
		}

		// Token: 0x06015EBB RID: 89787 RVA: 0x0068E9A7 File Offset: 0x0068CBA7
		public FilesystemFile(FileInfo fileInfo)
		{
			this.fileInfo = fileInfo;
		}

		// Token: 0x06015EBC RID: 89788 RVA: 0x0068E9B6 File Offset: 0x0068CBB6
		public override Stream CreateReadStream()
		{
			return this.fileInfo.OpenRead();
		}

		// Token: 0x06015EBD RID: 89789 RVA: 0x0068E9C3 File Offset: 0x0068CBC3
		public override byte[] ReadAllBytes()
		{
			return File.ReadAllBytes(this.fileInfo.FullName);
		}

		// Token: 0x06015EBE RID: 89790 RVA: 0x0068E9D5 File Offset: 0x0068CBD5
		public override string[] ReadAllLines()
		{
			return File.ReadAllLines(this.fileInfo.FullName);
		}

		// Token: 0x06015EBF RID: 89791 RVA: 0x0068E9E7 File Offset: 0x0068CBE7
		public override string ReadAllText()
		{
			return File.ReadAllText(this.fileInfo.FullName);
		}

		// Token: 0x06015EC0 RID: 89792 RVA: 0x0068E9F9 File Offset: 0x0068CBF9
		public static implicit operator FilesystemFile(FileInfo fileInfo)
		{
			return new FilesystemFile(fileInfo);
		}

		// Token: 0x06015EC1 RID: 89793 RVA: 0x0068EA04 File Offset: 0x0068CC04
		public override string ToString()
		{
			return string.Format("FilesystemFile [{0}], Length {1}", this.FullPath, this.fileInfo.Length.ToString());
		}

		// Token: 0x0400F315 RID: 62229
		private FileInfo fileInfo;
	}
}
