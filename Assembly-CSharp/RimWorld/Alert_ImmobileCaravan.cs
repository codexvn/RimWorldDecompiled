using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002FCA RID: 12234
	public class Alert_ImmobileCaravan : Alert_Critical
	{
		// Token: 0x17002CB4 RID: 11444
		// (get) Token: 0x060113E1 RID: 70625 RVA: 0x004F0C00 File Offset: 0x004EEE00
		private List<Caravan> ImmobileCaravans
		{
			get
			{
				this.immobileCaravansResult.Clear();
				foreach (Caravan caravan in Find.WorldObjects.Caravans)
				{
					if (caravan.Shuttle == null && caravan.IsPlayerControlled && caravan.ImmobilizedByMass)
					{
						this.immobileCaravansResult.Add(caravan);
					}
				}
				return this.immobileCaravansResult;
			}
		}

		// Token: 0x060113E2 RID: 70626 RVA: 0x004F0C88 File Offset: 0x004EEE88
		public Alert_ImmobileCaravan()
		{
			this.defaultLabel = "ImmobileCaravan".Translate();
			this.defaultExplanation = "ImmobileCaravanDesc".Translate();
		}

		// Token: 0x060113E3 RID: 70627 RVA: 0x004F0CC5 File Offset: 0x004EEEC5
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.ImmobileCaravans);
		}

		// Token: 0x0400B2F5 RID: 45813
		private List<Caravan> immobileCaravansResult = new List<Caravan>();
	}
}
