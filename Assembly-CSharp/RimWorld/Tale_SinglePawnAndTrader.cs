using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200293A RID: 10554
	public class Tale_SinglePawnAndTrader : Tale_SinglePawn
	{
		// Token: 0x0600E71D RID: 59165 RVA: 0x0043530F File Offset: 0x0043350F
		public Tale_SinglePawnAndTrader()
		{
		}

		// Token: 0x0600E71E RID: 59166 RVA: 0x004358AF File Offset: 0x00433AAF
		public Tale_SinglePawnAndTrader(Pawn pawn, ITrader trader) : base(pawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x0600E71F RID: 59167 RVA: 0x004358C4 File Offset: 0x00433AC4
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x0600E720 RID: 59168 RVA: 0x004358E4 File Offset: 0x00433AE4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x0600E721 RID: 59169 RVA: 0x00435901 File Offset: 0x00433B01
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			foreach (Rule rule in base.SpecialTextGenerationRules(outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.traderData.GetRules("TRADER", null))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E722 RID: 59170 RVA: 0x00435918 File Offset: 0x00433B18
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04009A71 RID: 39537
		public TaleData_Trader traderData;
	}
}
