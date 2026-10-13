using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200290E RID: 10510
	public class OrbitalTransition : MusicTransition
	{
		// Token: 0x0600E5F0 RID: 58864 RVA: 0x004316E8 File Offset: 0x0042F8E8
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.OdysseyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Tile.LayerDef.isSpace)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
