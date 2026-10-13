using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002938 RID: 10552
	public class Tale_SinglePawnAndThing : Tale_SinglePawn
	{
		// Token: 0x0600E70C RID: 59148 RVA: 0x0043530F File Offset: 0x0043350F
		public Tale_SinglePawnAndThing()
		{
		}

		// Token: 0x0600E70D RID: 59149 RVA: 0x0043560B File Offset: 0x0043380B
		public Tale_SinglePawnAndThing(Pawn pawn, Thing item) : base(pawn)
		{
			this.thingData = TaleData_Thing.GenerateFrom(item);
		}

		// Token: 0x0600E70E RID: 59150 RVA: 0x00435620 File Offset: 0x00433820
		public override bool Concerns(Thing th)
		{
			return base.Concerns(th) || th.thingIDNumber == this.thingData.thingID;
		}

		// Token: 0x0600E70F RID: 59151 RVA: 0x00435640 File Offset: 0x00433840
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Thing>(ref this.thingData, "thingData", Array.Empty<object>());
		}

		// Token: 0x0600E710 RID: 59152 RVA: 0x0043565D File Offset: 0x0043385D
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			foreach (Rule rule in base.SpecialTextGenerationRules(outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.thingData.GetRules("THING", null))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E711 RID: 59153 RVA: 0x00435674 File Offset: 0x00433874
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.thingData = TaleData_Thing.GenerateRandom();
		}

		// Token: 0x04009A69 RID: 39529
		public TaleData_Thing thingData;
	}
}
