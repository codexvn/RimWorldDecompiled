using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000D13 RID: 3347
	public static class GenWorker<T>
	{
		// Token: 0x06005593 RID: 21907 RVA: 0x001B97B1 File Offset: 0x001B79B1
		private static T Get<D>() where D : T
		{
			return GenWorker<T>.Get(typeof(D));
		}

		// Token: 0x06005594 RID: 21908 RVA: 0x001B97C4 File Offset: 0x001B79C4
		public static T Get(Type type)
		{
			T result;
			if (!GenWorker<T>.Workers.TryGetValue(type, out result))
			{
				result = (GenWorker<T>.Workers[type] = (T)((object)Activator.CreateInstance(type)));
			}
			return result;
		}

		// Token: 0x06005595 RID: 21909 RVA: 0x001B97FC File Offset: 0x001B79FC
		public static T Get(Type type, params object[] args)
		{
			T result;
			if (!GenWorker<T>.Workers.TryGetValue(type, out result))
			{
				result = (GenWorker<T>.Workers[type] = (T)((object)Activator.CreateInstance(type, args)));
			}
			return result;
		}

		// Token: 0x04003DE1 RID: 15841
		private static readonly Dictionary<Type, T> Workers = new Dictionary<Type, T>();
	}
}
