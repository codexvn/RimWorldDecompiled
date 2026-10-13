using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020028F3 RID: 10483
	public class ScenPart_ScatterThingsAnywhere : ScenPart_ScatterThings
	{
		// Token: 0x170022C8 RID: 8904
		// (get) Token: 0x0600E53D RID: 58685 RVA: 0x00002501 File Offset: 0x00000701
		protected override bool NearPlayerStart
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600E53E RID: 58686 RVA: 0x0042E8A7 File Offset: 0x0042CAA7
		public override string Summary(Scenario scen)
		{
			return ScenSummaryList.SummaryWithList(scen, "MapScatteredWith", "ScenPart_MapScatteredWith".Translate());
		}

		// Token: 0x0600E53F RID: 58687 RVA: 0x0042E8C3 File Offset: 0x0042CAC3
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == "MapScatteredWith")
			{
				yield return GenLabel.ThingLabel(this.thingDef, this.stuff, this.count).CapitalizeFirst();
			}
			yield break;
		}

		// Token: 0x04009961 RID: 39265
		public const string MapScatteredWithTag = "MapScatteredWith";
	}
}
