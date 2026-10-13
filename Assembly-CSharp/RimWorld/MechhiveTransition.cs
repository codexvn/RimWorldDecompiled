using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200290F RID: 10511
	public class MechhiveTransition : OrbitalTransition
	{
		// Token: 0x0600E5F2 RID: 58866 RVA: 0x00431760 File Offset: 0x0042F960
		public override bool IsTransitionSatisfied()
		{
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			using (List<Map>.Enumerator enumerator = Find.Maps.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.listerThings.AnyThingWithDef(ThingDefOf.CerebrexCore))
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
