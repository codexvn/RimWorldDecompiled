using System;
using RimWorld;
using UnityEngine;

namespace Verse.AI
{
	// Token: 0x02000E73 RID: 3699
	public static class EnrouteUtility
	{
		// Token: 0x06005D33 RID: 23859 RVA: 0x001DB1A0 File Offset: 0x001D93A0
		public static int GetSpaceRemainingWithEnroute(this IHaulEnroute enroute, ThingDef stuff, Pawn excludeEnrouteFor = null)
		{
			int enroute2 = enroute.Map.enrouteManager.GetEnroute(enroute, stuff, excludeEnrouteFor);
			return Mathf.Max(enroute.SpaceRemainingFor(stuff) - enroute2, 0);
		}
	}
}
