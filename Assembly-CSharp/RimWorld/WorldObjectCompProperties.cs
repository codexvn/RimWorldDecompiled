using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020019C8 RID: 6600
	public class WorldObjectCompProperties
	{
		// Token: 0x0600952C RID: 38188 RVA: 0x002BBA7F File Offset: 0x002B9C7F
		public virtual IEnumerable<string> ConfigErrors(WorldObjectDef parentDef)
		{
			if (this.compClass == null)
			{
				yield return parentDef.defName + " has WorldObjectCompProperties with null compClass.";
			}
			yield break;
		}

		// Token: 0x0600952D RID: 38189 RVA: 0x000026EA File Offset: 0x000008EA
		public virtual void ResolveReferences(WorldObjectDef parentDef)
		{
		}

		// Token: 0x0400612A RID: 24874
		[TranslationHandle]
		public Type compClass = typeof(WorldObjectComp);
	}
}
