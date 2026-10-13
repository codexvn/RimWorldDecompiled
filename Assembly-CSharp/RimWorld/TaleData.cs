using System;
using System.Collections.Generic;
using Verse;
using Verse.Grammar;

namespace RimWorld
{
	// Token: 0x02002910 RID: 10512
	public abstract class TaleData : IExposable
	{
		// Token: 0x0600E5F4 RID: 58868
		public abstract void ExposeData();

		// Token: 0x0600E5F5 RID: 58869 RVA: 0x004317D4 File Offset: 0x0042F9D4
		public virtual IEnumerable<Rule> GetRules(string prefix, Dictionary<string, string> constants = null)
		{
			Type type = base.GetType();
			Log.Error(((type != null) ? type.ToString() : null) + " cannot do GetRules with a prefix.");
			yield break;
		}

		// Token: 0x0600E5F6 RID: 58870 RVA: 0x004317E4 File Offset: 0x0042F9E4
		public virtual IEnumerable<Rule> GetRules(Dictionary<string, string> constants = null)
		{
			Type type = base.GetType();
			Log.Error(((type != null) ? type.ToString() : null) + " cannot do GetRules without a prefix.");
			yield break;
		}
	}
}
