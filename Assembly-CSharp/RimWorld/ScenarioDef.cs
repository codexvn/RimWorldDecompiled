using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ADD RID: 6877
	public class ScenarioDef : Def
	{
		// Token: 0x060099B3 RID: 39347 RVA: 0x002CBFD4 File Offset: 0x002CA1D4
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.scenario.name.NullOrEmpty())
			{
				this.scenario.name = this.label;
			}
			if (this.scenario.description.NullOrEmpty())
			{
				this.scenario.description = this.description;
			}
			this.scenario.Category = ScenarioCategory.FromDef;
		}

		// Token: 0x060099B4 RID: 39348 RVA: 0x002CC039 File Offset: 0x002CA239
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.scenario == null)
			{
				yield return "null scenario";
			}
			foreach (string text in this.scenario.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04006952 RID: 26962
		public Scenario scenario;
	}
}
