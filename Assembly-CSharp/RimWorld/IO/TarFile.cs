using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02003E97 RID: 16023
	internal class TarFile : VirtualFile
	{
		// Token: 0x170036E4 RID: 14052
		// (get) Token: 0x06015F05 RID: 89861 RVA: 0x0068FB07 File Offset: 0x0068DD07
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x170036E5 RID: 14053
		// (get) Token: 0x06015F06 RID: 89862 RVA: 0x0068FB0F File Offset: 0x0068DD0F
		public override string FullPath
		{
			get
			{
				return this.fullPath;
			}
		}

		// Token: 0x170036E6 RID: 14054
		// (get) Token: 0x06015F07 RID: 89863 RVA: 0x0068FB17 File Offset: 0x0068DD17
		public override bool Exists
		{
			get
			{
				return this.data != null;
			}
		}

		// Token: 0x170036E7 RID: 14055
		// (get) Token: 0x06015F08 RID: 89864 RVA: 0x0068FB22 File Offset: 0x0068DD22
		public override long Length
		{
			get
			{
				return (long)this.data.Length;
			}
		}

		// Token: 0x06015F09 RID: 89865 RVA: 0x0068FB2D File Offset: 0x0068DD2D
		public TarFile(byte[] data, string fullPath, string name)
		{
			this.data = data;
			this.fullPath = fullPath;
			this.name = name;
		}

		// Token: 0x06015F0A RID: 89866 RVA: 0x0068FB4A File Offset: 0x0068DD4A
		private TarFile()
		{
		}

		// Token: 0x06015F0B RID: 89867 RVA: 0x0068FB52 File Offset: 0x0068DD52
		private void CheckAccess()
		{
			if (this.data == null)
			{
				throw new FileNotFoundException();
			}
		}

		// Token: 0x06015F0C RID: 89868 RVA: 0x0068FB62 File Offset: 0x0068DD62
		public override Stream CreateReadStream()
		{
			this.CheckAccess();
			return new MemoryStream(this.ReadAllBytes());
		}

		// Token: 0x06015F0D RID: 89869 RVA: 0x0068FB78 File Offset: 0x0068DD78
		public override byte[] ReadAllBytes()
		{
			this.CheckAccess();
			byte[] array = new byte[this.data.Length];
			Buffer.BlockCopy(this.data, 0, array, 0, this.data.Length);
			return array;
		}

		// Token: 0x06015F0E RID: 89870 RVA: 0x0068FBB0 File Offset: 0x0068DDB0
		public override string[] ReadAllLines()
		{
			this.CheckAccess();
			List<string> list = new List<string>();
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					while (!streamReader.EndOfStream)
					{
						list.Add(streamReader.ReadLine());
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06015F0F RID: 89871 RVA: 0x0068FC2C File Offset: 0x0068DE2C
		public override string ReadAllText()
		{
			this.CheckAccess();
			string result;
			using (MemoryStream memoryStream = new MemoryStream(this.data))
			{
				using (StreamReader streamReader = new StreamReader(memoryStream, true))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}

		// Token: 0x06015F10 RID: 89872 RVA: 0x0068FC90 File Offset: 0x0068DE90
		public override string ToString()
		{
			return string.Format("TarFile [{0}], Length {1}", this.fullPath, this.data.Length.ToString());
		}

		// Token: 0x0400F349 RID: 62281
		public static readonly TarFile NotFound = new TarFile();

		// Token: 0x0400F34A RID: 62282
		public byte[] data;

		// Token: 0x0400F34B RID: 62283
		public string fullPath;

		// Token: 0x0400F34C RID: 62284
		public string name;
	}
}
