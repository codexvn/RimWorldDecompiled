using System;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200056D RID: 1389
	public class LoadedContentItem<T> where T : class
	{
		// Token: 0x06002407 RID: 9223 RVA: 0x000BB2D7 File Offset: 0x000B94D7
		public LoadedContentItem(VirtualFile internalFile, T contentItem, IDisposable extraDisposable = null)
		{
			this.internalFile = internalFile;
			this.contentItem = contentItem;
			this.extraDisposable = extraDisposable;
		}

		// Token: 0x04001DD6 RID: 7638
		public VirtualFile internalFile;

		// Token: 0x04001DD7 RID: 7639
		public T contentItem;

		// Token: 0x04001DD8 RID: 7640
		public IDisposable extraDisposable;
	}
}
