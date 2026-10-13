using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001C42 RID: 7234
	public static class ExtraFactionTypeExt
	{
		// Token: 0x0600A108 RID: 41224 RVA: 0x002E5749 File Offset: 0x002E3949
		public static string GetLabel(this ExtraFactionType factionType)
		{
			return ("ExtraFactionType_" + factionType.ToString()).Translate();
		}
	}
}
