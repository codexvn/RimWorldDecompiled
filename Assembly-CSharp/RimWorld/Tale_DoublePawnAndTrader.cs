using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002931 RID: 10545
	public class Tale_DoublePawnAndTrader : Tale_DoublePawn
	{
		// Token: 0x0600E6D6 RID: 59094 RVA: 0x004349FF File Offset: 0x00432BFF
		public Tale_DoublePawnAndTrader()
		{
		}

		// Token: 0x0600E6D7 RID: 59095 RVA: 0x00434CFB File Offset: 0x00432EFB
		public Tale_DoublePawnAndTrader(Pawn firstPawn, Pawn secondPawn, ITrader trader) : base(firstPawn, secondPawn)
		{
			this.traderData = TaleData_Trader.GenerateFrom(trader);
		}

		// Token: 0x0600E6D8 RID: 59096 RVA: 0x00434D11 File Offset: 0x00432F11
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || this.traderData.pawnID == th.thingIDNumber;
		}

		// Token: 0x0600E6D9 RID: 59097 RVA: 0x00434D31 File Offset: 0x00432F31
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Trader>(ref this.traderData, "traderData", Array.Empty<object>());
		}

		// Token: 0x0600E6DA RID: 59098 RVA: 0x00434D4E File Offset: 0x00432F4E
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

		// Token: 0x0600E6DB RID: 59099 RVA: 0x00434D65 File Offset: 0x00432F65
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.traderData = TaleData_Trader.GenerateRandom();
		}

		// Token: 0x04009A51 RID: 39505
		public TaleData_Trader traderData;
	}
}
