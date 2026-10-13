using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003E3C RID: 15932
	public static class TradeRequestUtility
	{
		// Token: 0x06015C55 RID: 89173 RVA: 0x00681994 File Offset: 0x0067FB94
		public static string RequestedThingLabel(ThingDef def, int count)
		{
			string text = GenLabel.ThingLabel(def, null, count);
			if (def.HasComp(typeof(CompQuality)))
			{
				text += " (" + "NormalQualityOrBetter".Translate() + ")";
			}
			if (def.IsApparel)
			{
				text += " (" + "NotTainted".Translate() + ")";
			}
			return text;
		}
	}
}
