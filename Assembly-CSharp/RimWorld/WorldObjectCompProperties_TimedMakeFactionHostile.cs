using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020019DA RID: 6618
	public class WorldObjectCompProperties_TimedMakeFactionHostile : WorldObjectCompProperties
	{
		// Token: 0x06009583 RID: 38275 RVA: 0x002BC7FB File Offset: 0x002BA9FB
		public WorldObjectCompProperties_TimedMakeFactionHostile()
		{
			this.compClass = typeof(TimedMakeFactionHostile);
		}

		// Token: 0x06009584 RID: 38276 RVA: 0x002BC813 File Offset: 0x002BAA13
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_TimedMakeFactionHostile but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
