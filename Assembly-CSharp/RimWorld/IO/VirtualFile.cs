using System;
using System.IO;

namespace RimWorld.IO
{
	// Token: 0x02003E99 RID: 16025
	public abstract class VirtualFile
	{
		// Token: 0x170036EB RID: 14059
		// (get) Token: 0x06015F1D RID: 89885
		public abstract string Name { get; }

		// Token: 0x170036EC RID: 14060
		// (get) Token: 0x06015F1E RID: 89886
		public abstract string FullPath { get; }

		// Token: 0x170036ED RID: 14061
		// (get) Token: 0x06015F1F RID: 89887
		public abstract bool Exists { get; }

		// Token: 0x170036EE RID: 14062
		// (get) Token: 0x06015F20 RID: 89888
		public abstract long Length { get; }

		// Token: 0x06015F21 RID: 89889
		public abstract Stream CreateReadStream();

		// Token: 0x06015F22 RID: 89890
		public abstract string ReadAllText();

		// Token: 0x06015F23 RID: 89891
		public abstract string[] ReadAllLines();

		// Token: 0x06015F24 RID: 89892
		public abstract byte[] ReadAllBytes();
	}
}
