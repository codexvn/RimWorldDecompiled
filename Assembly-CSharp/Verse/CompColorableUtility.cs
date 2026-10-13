using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020008FB RID: 2299
	public static class CompColorableUtility
	{
		// Token: 0x06003B5E RID: 15198 RVA: 0x00139EEC File Offset: 0x001380EC
		public static void SetColor(this Thing t, Color newColor, bool reportFailure = true)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			if (thingWithComps == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on non-ThingWithComps " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			CompColorable comp = thingWithComps.GetComp<CompColorable>();
			if (comp == null)
			{
				if (reportFailure)
				{
					Log.Error("SetColor on Thing without CompColorable " + ((t != null) ? t.ToString() : null));
				}
				return;
			}
			if (!comp.Color.IndistinguishableFrom(newColor))
			{
				comp.SetColor(newColor);
			}
		}
	}
}
