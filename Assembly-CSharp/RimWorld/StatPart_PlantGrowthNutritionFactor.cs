using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003716 RID: 14102
	public class StatPart_PlantGrowthNutritionFactor : StatPart
	{
		// Token: 0x060138D3 RID: 80083 RVA: 0x005CC4D0 File Offset: 0x005CA6D0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x060138D4 RID: 80084 RVA: 0x005CC4F0 File Offset: 0x005CA6F0
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				Plant plant = (Plant)req.Thing;
				TaggedString taggedString = "StatsReport_PlantGrowth".Translate(plant.Growth.ToStringPercent()) + ": x" + f.ToStringPercent();
				if (!plant.def.plant.Sowable)
				{
					taggedString += " (" + "StatsReport_PlantGrowth_Wild".Translate() + ")";
				}
				return taggedString;
			}
			return null;
		}

		// Token: 0x060138D5 RID: 80085 RVA: 0x005CC584 File Offset: 0x005CA784
		private bool TryGetFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Plant plant = req.Thing as Plant;
			if (plant == null)
			{
				factor = 1f;
				return false;
			}
			factor = PlantUtility.NutritionFactorFromGrowth(plant.def, plant.Growth);
			return true;
		}
	}
}
