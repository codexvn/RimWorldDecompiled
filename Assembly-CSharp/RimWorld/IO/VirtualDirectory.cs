using System;
using System.Collections.Generic;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02003E98 RID: 16024
	public abstract class VirtualDirectory
	{
		// Token: 0x170036E8 RID: 14056
		// (get) Token: 0x06015F12 RID: 89874
		public abstract string Name { get; }

		// Token: 0x170036E9 RID: 14057
		// (get) Token: 0x06015F13 RID: 89875
		public abstract string FullPath { get; }

		// Token: 0x170036EA RID: 14058
		// (get) Token: 0x06015F14 RID: 89876
		public abstract bool Exists { get; }

		// Token: 0x06015F15 RID: 89877
		public abstract VirtualDirectory GetDirectory(string directoryName);

		// Token: 0x06015F16 RID: 89878
		public abstract VirtualFile GetFile(string filename);

		// Token: 0x06015F17 RID: 89879
		public abstract IEnumerable<VirtualFile> GetFiles(string searchPattern, SearchOption searchOption);

		// Token: 0x06015F18 RID: 89880
		public abstract IEnumerable<VirtualDirectory> GetDirectories(string searchPattern, SearchOption searchOption);

		// Token: 0x06015F19 RID: 89881 RVA: 0x0068FCC9 File Offset: 0x0068DEC9
		public string ReadAllText(string filename)
		{
			return this.GetFile(filename).ReadAllText();
		}

		// Token: 0x06015F1A RID: 89882 RVA: 0x0068FCD7 File Offset: 0x0068DED7
		public bool FileExists(string filename)
		{
			return this.GetFile(filename).Exists;
		}

		// Token: 0x06015F1B RID: 89883 RVA: 0x0068FCE5 File Offset: 0x0068DEE5
		public override string ToString()
		{
			return this.FullPath;
		}
	}
}
