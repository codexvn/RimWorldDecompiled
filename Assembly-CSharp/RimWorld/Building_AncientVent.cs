using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020029CE RID: 10702
	public class Building_AncientVent : Building
	{
		// Token: 0x0600EBE0 RID: 60384 RVA: 0x00447F30 File Offset: 0x00446130
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			CompFleckEmitterLongTerm comp = base.GetComp<CompFleckEmitterLongTerm>();
			if (comp != null)
			{
				GameConditionDef conditionToCause = base.GetComp<CompAncientVent>().Props.conditionToCause;
				bool enabled = map.GameConditionManager.ConditionIsActive(conditionToCause);
				comp.Enabled = enabled;
			}
			base.SpawnSetup(map, respawningAfterLoad);
		}
	}
}
