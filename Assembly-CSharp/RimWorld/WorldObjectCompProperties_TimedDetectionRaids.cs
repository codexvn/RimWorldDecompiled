using System;
using System.Collections.Generic;
using RimWorld.Planet;

namespace RimWorld
{
	// Token: 0x020019D6 RID: 6614
	public class WorldObjectCompProperties_TimedDetectionRaids : WorldObjectCompProperties
	{
		// Token: 0x0600956B RID: 38251 RVA: 0x002BC3FB File Offset: 0x002BA5FB
		public WorldObjectCompProperties_TimedDetectionRaids()
		{
			this.compClass = typeof(TimedDetectionRaids);
		}

		// Token: 0x0600956C RID: 38252 RVA: 0x002BC413 File Offset: 0x002BA613
		public override IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			foreach (string text in base.ConfigErrors(parentDef))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			if (!typeof(MapParent).IsAssignableFrom(parentDef.worldObjectClass))
			{
				yield return parentDef.defName + " has WorldObjectCompProperties_TimedDetectionRaids but it's not MapParent.";
			}
			yield break;
			yield break;
		}
	}
}
