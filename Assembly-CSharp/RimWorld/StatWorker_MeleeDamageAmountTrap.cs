using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200374E RID: 14158
	public class StatWorker_MeleeDamageAmountTrap : StatWorker_MeleeDamageAmount
	{
		// Token: 0x06013A06 RID: 80390 RVA: 0x005D4548 File Offset: 0x005D2748
		public override bool ShouldShowFor(StatRequest req)
		{
			if (req.HasThing)
			{
				Building_Trap building_Trap = req.Thing as Building_Trap;
				if (building_Trap != null && !building_Trap.ShouldShowTrapDamageStat)
				{
					return false;
				}
			}
			ThingDef thingDef = req.Def as ThingDef;
			return thingDef != null && thingDef.category == ThingCategory.Building && thingDef.building.isTrap;
		}

		// Token: 0x06013A07 RID: 80391 RVA: 0x005D459E File Offset: 0x005D279E
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return base.GetValueUnfinalized(req, applyPostProcess) / 5f;
		}

		// Token: 0x06013A08 RID: 80392 RVA: 0x005D45B0 File Offset: 0x005D27B0
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			float num = 5f;
			return base.GetExplanationUnfinalized(req, numberSense) + "Stat_TrapDamageHitCount".Translate(num) + ": x" + (1f / num).ToStringPercent();
		}

		// Token: 0x06013A09 RID: 80393 RVA: 0x005D4600 File Offset: 0x005D2800
		protected override DamageArmorCategoryDef CategoryOfDamage(ThingDef def)
		{
			return def.building.trapDamageCategory;
		}
	}
}
