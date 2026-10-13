using System;

namespace Verse
{
	// Token: 0x020008DF RID: 2271
	public interface IThingHolderEvents<in T> where T : Thing
	{
		// Token: 0x060039F4 RID: 14836
		void Notify_ItemAdded(T item);

		// Token: 0x060039F5 RID: 14837
		void Notify_ItemRemoved(T item);
	}
}
