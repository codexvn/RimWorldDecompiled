using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x0200292F RID: 10543
	public class Tale_DoublePawnAndDef : Tale_DoublePawn
	{
		// Token: 0x0600E6C6 RID: 59078 RVA: 0x004349FF File Offset: 0x00432BFF
		public Tale_DoublePawnAndDef()
		{
		}

		// Token: 0x0600E6C7 RID: 59079 RVA: 0x00434A07 File Offset: 0x00432C07
		public Tale_DoublePawnAndDef(Pawn firstPawn, Pawn secondPawn, Def def) : base(firstPawn, secondPawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x0600E6C8 RID: 59080 RVA: 0x00434A1D File Offset: 0x00432C1D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x0600E6C9 RID: 59081 RVA: 0x00434A3A File Offset: 0x00432C3A
		protected override IEnumerable<Rule> SpecialTextGenerationRules(Dictionary<string, string> outConstants)
		{
			if (this.def.defSymbol.NullOrEmpty())
			{
				TaleDef def = this.def;
				Log.Error(((def != null) ? def.ToString() : null) + " uses tale type with def but defSymbol is not set.");
			}
			foreach (Rule rule in base.SpecialTextGenerationRules(outConstants))
			{
				yield return rule;
			}
			IEnumerator<Rule> enumerator = null;
			foreach (Rule rule2 in this.defData.GetRules(this.def.defSymbol, null))
			{
				yield return rule2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600E6CA RID: 59082 RVA: 0x00434A51 File Offset: 0x00432C51
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04009A49 RID: 39497
		public TaleData_Def defData;
	}
}
