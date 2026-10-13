using System;

namespace Verse
{
	// Token: 0x02000427 RID: 1063
	public static class EdificeUtility
	{
		// Token: 0x06001B03 RID: 6915 RVA: 0x00088550 File Offset: 0x00086750
		public static bool IsEdifice(this BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isEdifice;
		}
	}
}
