using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020028F5 RID: 10485
	public class ScenPart_ScatterThingsNearPlayerStart : ScenPart_ScatterThings
	{
		// Token: 0x170022CB RID: 8907
		// (get) Token: 0x0600E549 RID: 58697 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override bool NearPlayerStart
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600E54A RID: 58698 RVA: 0x0042D59E File Offset: 0x0042B79E
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "PlayerStartsWith", ScenPart_StartingThing_Defined.PlayerStartWithIntro);
		}

		// Token: 0x0600E54B RID: 58699 RVA: 0x0042E9CB File Offset: 0x0042CBCB
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "PlayerStartsWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}
	}
}
