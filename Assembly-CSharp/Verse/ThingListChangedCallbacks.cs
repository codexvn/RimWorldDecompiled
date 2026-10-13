using System;

namespace Verse
{
	// Token: 0x02000449 RID: 1097
	public class ThingListChangedCallbacks
	{
		// Token: 0x04001852 RID: 6226
		public Action<Thing> onThingAdded = delegate(Thing t)
		{
		};

		// Token: 0x04001853 RID: 6227
		public Action<Thing> onThingRemoved = delegate(Thing t)
		{
		};
	}
}
