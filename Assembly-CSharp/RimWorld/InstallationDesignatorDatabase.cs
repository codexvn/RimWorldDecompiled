using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002946 RID: 10566
	public static class InstallationDesignatorDatabase
	{
		// Token: 0x0600E76B RID: 59243 RVA: 0x00436FF0 File Offset: 0x004351F0
		public static Designator_Install DesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			if (InstallationDesignatorDatabase.designators.TryGetValue(artDef, out designator_Install))
			{
				return designator_Install;
			}
			designator_Install = InstallationDesignatorDatabase.NewDesignatorFor(artDef);
			InstallationDesignatorDatabase.designators.Add(artDef, designator_Install);
			return designator_Install;
		}

		// Token: 0x0600E76C RID: 59244 RVA: 0x00437024 File Offset: 0x00435224
		private static Designator_Install NewDesignatorFor(ThingDef artDef)
		{
			Designator_Install designator_Install;
			if (artDef.thingClass == typeof(MinifiedTree))
			{
				designator_Install = new Designator_Replant();
			}
			else
			{
				designator_Install = new Designator_Install();
			}
			designator_Install.hotKey = KeyBindingDefOf.Misc1;
			return designator_Install;
		}

		// Token: 0x04009A95 RID: 39573
		private static Dictionary<ThingDef, Designator_Install> designators = new Dictionary<ThingDef, Designator_Install>();
	}
}
