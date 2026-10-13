using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020010D6 RID: 4310
	public class Trigger_PawnLostViolently : Trigger
	{
		// Token: 0x0600692C RID: 26924 RVA: 0x0021292E File Offset: 0x00210B2E
		public Trigger_PawnLostViolently(bool allowRoofCollapse = true)
		{
			this.allowRoofCollapse = allowRoofCollapse;
		}

		// Token: 0x0600692D RID: 26925 RVA: 0x0021293D File Offset: 0x00210B3D
		public Trigger_PawnLostViolently(bool allowRoofCollapse, Faction ignoreDamageFromFaction)
		{
			this.allowRoofCollapse = allowRoofCollapse;
			this.ignoreDamageFromFaction = ignoreDamageFromFaction;
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x00212954 File Offset: 0x00210B54
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.PawnLost)
			{
				if (signal.dinfo.Instigator != null && this.ignoreDamageFromFaction != null && signal.dinfo.Instigator.Faction == this.ignoreDamageFromFaction)
				{
					return false;
				}
				if (signal.condition == PawnLostCondition.MadePrisoner)
				{
					return true;
				}
				if ((signal.condition == PawnLostCondition.Incapped || signal.condition == PawnLostCondition.Killed) && (signal.dinfo.Category != DamageInfo.SourceCategory.Collapse || this.allowRoofCollapse))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04004A73 RID: 19059
		public bool allowRoofCollapse;

		// Token: 0x04004A74 RID: 19060
		public Faction ignoreDamageFromFaction;
	}
}
