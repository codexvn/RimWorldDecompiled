using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003706 RID: 14086
	public class StatPart_Hyperlinks : StatPart
	{
		// Token: 0x06013887 RID: 80007 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}

		// Token: 0x06013888 RID: 80008 RVA: 0x000026EA File Offset: 0x000008EA
		public override void TransformValue(StatRequest req, ref float val)
		{
		}

		// Token: 0x06013889 RID: 80009 RVA: 0x005CB9A3 File Offset: 0x005C9BA3
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			if (!this.thingDefs.NullOrEmpty<ThingDef>())
			{
				foreach (ThingDef def in this.thingDefs)
				{
					yield return new Dialog_InfoCard.Hyperlink(def, -1);
				}
				List<ThingDef>.Enumerator enumerator = default(List<ThingDef>.Enumerator);
			}
			yield break;
			yield break;
		}

		// Token: 0x0400D8C6 RID: 55494
		public List<ThingDef> thingDefs;
	}
}
