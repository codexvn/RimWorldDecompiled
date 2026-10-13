using System;

namespace Verse
{
	// Token: 0x0200053F RID: 1343
	public static class ThingIDMaker
	{
		// Token: 0x060022DC RID: 8924 RVA: 0x000B55DC File Offset: 0x000B37DC
		public static void GiveIDTo(Thing t)
		{
			if (!t.def.HasThingIDNumber)
			{
				return;
			}
			if (t.thingIDNumber != -1)
			{
				Log.Error("Giving ID to " + ((t != null) ? t.ToString() : null) + " which already has id " + t.thingIDNumber.ToString());
			}
			t.thingIDNumber = Find.UniqueIDsManager.GetNextThingID();
		}
	}
}
