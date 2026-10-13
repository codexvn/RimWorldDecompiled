using System;
using RimWorld;

namespace Verse
{
	// Token: 0x020008D5 RID: 2261
	public static class ThingMaker
	{
		// Token: 0x060039BD RID: 14781 RVA: 0x00133A50 File Offset: 0x00131C50
		public static Thing MakeThing(ThingDef def, ThingDef stuff = null)
		{
			if (stuff != null && !stuff.IsStuff)
			{
				string[] array = new string[5];
				array[0] = "MakeThing error: Tried to make ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " from ";
				int num = 3;
				ThingDef thingDef = stuff;
				array[num] = ((thingDef != null) ? thingDef.ToString() : null);
				array[4] = " which is not a stuff. Assigning default.";
				Log.Error(string.Concat(array));
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (def.MadeFromStuff && stuff == null)
			{
				Log.Error("MakeThing error: " + ((def != null) ? def.ToString() : null) + " is madeFromStuff but stuff=null. Assigning default.");
				stuff = GenStuff.DefaultStuffFor(def);
			}
			if (!def.MadeFromStuff && stuff != null)
			{
				string[] array2 = new string[5];
				array2[0] = "MakeThing error: ";
				array2[1] = ((def != null) ? def.ToString() : null);
				array2[2] = " is not madeFromStuff but stuff=";
				int num2 = 3;
				ThingDef thingDef2 = stuff;
				array2[num2] = ((thingDef2 != null) ? thingDef2.ToString() : null);
				array2[4] = ". Setting to null.";
				Log.Error(string.Concat(array2));
				stuff = null;
			}
			Thing thing = (Thing)Activator.CreateInstance(def.thingClass);
			thing.def = def;
			thing.SetStuffDirect(stuff);
			thing.PostMake();
			thing.PostPostMake();
			return thing;
		}
	}
}
