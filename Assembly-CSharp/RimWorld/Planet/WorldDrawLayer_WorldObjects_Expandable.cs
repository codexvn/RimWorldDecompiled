using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003CA1 RID: 15521
	public class WorldDrawLayer_WorldObjects_Expandable : WorldDrawLayer_WorldObjects
	{
		// Token: 0x17003492 RID: 13458
		// (get) Token: 0x06015187 RID: 86407 RVA: 0x00650237 File Offset: 0x0064E437
		protected override float Alpha
		{
			get
			{
				if (!Find.PlaySettings.showBasesExpandingIcons || !Find.PlaySettings.showImportantExpandingIcons)
				{
					return 1f;
				}
				return 1f - ExpandableWorldObjectsUtility.RawTransitionPct;
			}
		}

		// Token: 0x06015188 RID: 86408 RVA: 0x00650262 File Offset: 0x0064E462
		protected override bool ShouldSkip(WorldObject worldObject)
		{
			return !worldObject.def.expandingIcon;
		}
	}
}
