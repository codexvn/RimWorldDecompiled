using System;

namespace Verse
{
	// Token: 0x0200029F RID: 671
	public class ManeuverDef : Def
	{
		// Token: 0x04000DE3 RID: 3555
		public ToolCapacityDef requiredCapacity;

		// Token: 0x04000DE4 RID: 3556
		public VerbProperties verb;

		// Token: 0x04000DE5 RID: 3557
		public RulePackDef combatLogRulesHit;

		// Token: 0x04000DE6 RID: 3558
		public RulePackDef combatLogRulesDeflect;

		// Token: 0x04000DE7 RID: 3559
		public RulePackDef combatLogRulesMiss;

		// Token: 0x04000DE8 RID: 3560
		public RulePackDef combatLogRulesDodge;

		// Token: 0x04000DE9 RID: 3561
		public LogEntryDef logEntryDef;
	}
}
