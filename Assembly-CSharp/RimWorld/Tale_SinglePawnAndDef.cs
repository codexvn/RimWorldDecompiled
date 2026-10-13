using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002936 RID: 10550
	public class Tale_SinglePawnAndDef : Tale_SinglePawn
	{
		// Token: 0x0600E6FC RID: 59132 RVA: 0x0043530F File Offset: 0x0043350F
		public Tale_SinglePawnAndDef()
		{
		}

		// Token: 0x0600E6FD RID: 59133 RVA: 0x00435317 File Offset: 0x00433517
		public Tale_SinglePawnAndDef(Pawn pawn, Def def) : base(pawn)
		{
			this.defData = TaleData_Def.GenerateFrom(def);
		}

		// Token: 0x0600E6FE RID: 59134 RVA: 0x0043532C File Offset: 0x0043352C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<TaleData_Def>(ref this.defData, "defData", Array.Empty<object>());
		}

		// Token: 0x0600E6FF RID: 59135 RVA: 0x00435349 File Offset: 0x00433549
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

		// Token: 0x0600E700 RID: 59136 RVA: 0x00435360 File Offset: 0x00433560
		public override void GenerateTestData()
		{
			base.GenerateTestData();
			this.defData = TaleData_Def.GenerateFrom((Def)GenGeneric.InvokeStaticMethodOnGenericType(typeof(DefDatabase<>), this.def.defType, "GetRandom"));
		}

		// Token: 0x04009A61 RID: 39521
		public TaleData_Def defData;
	}
}
