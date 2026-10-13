using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002953 RID: 10579
	public static class InstallBlueprintUtility
	{
		// Token: 0x0600E7EB RID: 59371 RVA: 0x00438F30 File Offset: 0x00437130
		public static void CancelBlueprintsFor(Thing th)
		{
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(th);
			if (blueprint_Install != null)
			{
				blueprint_Install.Destroy(DestroyMode.Cancel);
			}
		}

		// Token: 0x0600E7EC RID: 59372 RVA: 0x00438F50 File Offset: 0x00437150
		public static Blueprint_Install ExistingBlueprintFor(Thing th)
		{
			List<Map> maps = Find.Maps;
			Thing innerIfMinified = th.GetInnerIfMinified();
			for (int i = 0; i < maps.Count; i++)
			{
				Blueprint_Install result;
				if (maps[i].listerBuildings.TryGetReinstallBlueprint(innerIfMinified, out result))
				{
					return result;
				}
			}
			return null;
		}
	}
}
