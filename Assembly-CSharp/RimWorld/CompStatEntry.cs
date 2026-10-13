using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002148 RID: 8520
	public class CompStatEntry : ThingComp
	{
		// Token: 0x17001C51 RID: 7249
		// (get) Token: 0x0600BA1F RID: 47647 RVA: 0x0035B417 File Offset: 0x00359617
		private CompProperties_StatEntry Props
		{
			get
			{
				return (CompProperties_StatEntry)this.props;
			}
		}

		// Token: 0x0600BA20 RID: 47648 RVA: 0x0035B424 File Offset: 0x00359624
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			StatCategoryDef statCategoryDef = this.Props.statCategoryDef;
			string statLabel = this.Props.statLabel;
			Func<CompStatEntry, string> valueFunc = this.Props.valueFunc;
			yield return new StatDrawEntry(statCategoryDef, statLabel, ((valueFunc != null) ? valueFunc(this) : null) ?? this.Props.valueString, this.Props.reportText, this.Props.displayPriorityInCategory, null, null, false, false);
			yield break;
		}
	}
}
