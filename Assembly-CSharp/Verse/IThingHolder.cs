using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x020008DC RID: 2268
	public interface IThingHolder
	{
		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060039ED RID: 14829
		IThingHolder ParentHolder { get; }

		// Token: 0x060039EE RID: 14830
		void GetChildHolders(List<IThingHolder> outChildren);

		// Token: 0x060039EF RID: 14831
		ThingOwner GetDirectlyHeldThings();
	}
}
