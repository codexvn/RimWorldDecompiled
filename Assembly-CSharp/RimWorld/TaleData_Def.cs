using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002913 RID: 10515
	public class TaleData_Def : TaleData
	{
		// Token: 0x0600E608 RID: 58888 RVA: 0x0043196C File Offset: 0x0042FB6C
		public override void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpDefName = ((this.def != null) ? this.def.defName : null);
				this.tmpDefType = ((this.def != null) ? this.def.GetType() : null);
			}
			Scribe_Values.Look<string>(ref this.tmpDefName, "defName", null, false);
			Scribe_Values.Look<Type>(ref this.tmpDefType, "defType", null, false);
			if (Scribe.mode == LoadSaveMode.LoadingVars && this.tmpDefName != null)
			{
				this.def = GenDefDatabase.GetDef(this.tmpDefType, BackCompatibility.BackCompatibleDefName(this.tmpDefType, this.tmpDefName, false, null), true);
			}
		}

		// Token: 0x0600E609 RID: 58889 RVA: 0x00431A12 File Offset: 0x0042FC12
		public override IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			if (this.def != null)
			{
				yield return new Rule_String(prefix + "_label", this.def.label);
				yield return new Rule_String(prefix + "_definite", Find.ActiveLanguageWorker.WithDefiniteArticle(this.def.label, false, false));
				yield return new Rule_String(prefix + "_indefinite", Find.ActiveLanguageWorker.WithIndefiniteArticle(this.def.label, false, false));
			}
			yield break;
		}

		// Token: 0x0600E60A RID: 58890 RVA: 0x00431A29 File Offset: 0x0042FC29
		public static TaleData_Def GenerateFrom(Def def)
		{
			return new TaleData_Def
			{
				def = def
			};
		}

		// Token: 0x040099CB RID: 39371
		public Def def;

		// Token: 0x040099CC RID: 39372
		private string tmpDefName;

		// Token: 0x040099CD RID: 39373
		private Type tmpDefType;
	}
}
