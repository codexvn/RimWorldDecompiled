using System;

namespace RimWorld.Planet
{
	// Token: 0x02003CA5 RID: 15525
	public class WorldDrawLayer_WorldObjects_NonExpandable : WorldDrawLayer_WorldObjects
	{
		// Token: 0x0601519D RID: 86429 RVA: 0x00650923 File Offset: 0x0064EB23
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return worldObject.def.expandingIcon;
		}
	}
}
