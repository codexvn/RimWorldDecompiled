using System;

namespace Verse
{
	// Token: 0x02000D1F RID: 3359
	public class WeakReference<T> : WeakReference where T : class
	{
		// Token: 0x060055C2 RID: 21954 RVA: 0x001BB2AC File Offset: 0x001B94AC
		public WeakReference(T target) : base(target)
		{
		}

		// Token: 0x17000D5A RID: 3418
		// (get) Token: 0x060055C3 RID: 21955 RVA: 0x001BB2BA File Offset: 0x001B94BA
		// (set) Token: 0x060055C4 RID: 21956 RVA: 0x001BB2C7 File Offset: 0x001B94C7
		public new T Target
		{
			get
			{
				return (T)((object)base.Target);
			}
			set
			{
				base.Target = value;
			}
		}
	}
}
