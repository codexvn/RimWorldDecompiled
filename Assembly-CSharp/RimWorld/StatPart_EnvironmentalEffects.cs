using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F6 RID: 14070
	public class StatPart_EnvironmentalEffects : StatPart
	{
		// Token: 0x06013830 RID: 79920 RVA: 0x005CA318 File Offset: 0x005C8518
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = 0f;
			if (!this.ActiveFor(req.Thing))
			{
				return;
			}
			Thing thing = req.Thing;
			if (!thing.Position.Roofed(thing.Map))
			{
				num += this.factorOffsetUnroofed;
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room != null && room.UsesOutdoorTemperature)
			{
				num += this.factorOffsetOutdoors;
			}
			TerrainDef terrain = thing.Position.GetTerrain(thing.Map);
			if (terrain != null && terrain.extraDeteriorationFactor != 0f)
			{
				num += terrain.extraDeteriorationFactor;
			}
			if (!thing.Position.Roofed(thing.Map))
			{
				num *= Mathf.Lerp(1f, 5f, thing.Map.weatherManager.RainRate);
			}
			if (SteadyEnvironmentEffects.ProtectedByEdifice(thing.Position, thing.Map))
			{
				num *= this.protectedByEdificeFactor;
			}
			val *= num;
		}

		// Token: 0x06013831 RID: 79921 RVA: 0x005CA400 File Offset: 0x005C8600
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ActiveFor(req.Thing))
			{
				return null;
			}
			Thing thing = req.Thing;
			StringBuilder stringBuilder = new StringBuilder();
			if (!thing.Position.Roofed(thing.Map))
			{
				stringBuilder.AppendLine("DeterioratingUnroofed".Translate().CapitalizeFirst() + (": +" + this.factorOffsetUnroofed.ToStringPercent()));
			}
			Room room = thing.GetRoom(RegionType.Set_All);
			if (room != null && room.UsesOutdoorTemperature)
			{
				stringBuilder.AppendLine("DeterioratingOutdoors".Translate().CapitalizeFirst() + (": +" + this.factorOffsetOutdoors.ToStringPercent()));
			}
			TerrainDef terrain = thing.Position.GetTerrain(thing.Map);
			if (terrain != null && terrain.extraDeteriorationFactor != 0f)
			{
				stringBuilder.AppendLine(terrain.LabelCap + (": +" + terrain.extraDeteriorationFactor.ToStringPercent()));
			}
			if (!thing.Position.Roofed(thing.Map) && thing.Map.weatherManager.RainRate > 0f)
			{
				stringBuilder.AppendLine("DeterioratingRaining".Translate().CapitalizeFirst() + (": x" + Mathf.Lerp(1f, 5f, thing.Map.weatherManager.RainRate).ToStringPercent()));
			}
			if (SteadyEnvironmentEffects.ProtectedByEdifice(thing.Position, thing.Map))
			{
				stringBuilder.AppendLine("Protected".Translate().CapitalizeFirst() + (": x" + this.protectedByEdificeFactor.ToStringPercent()));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06013832 RID: 79922 RVA: 0x005CA5E1 File Offset: 0x005C87E1
		private bool ActiveFor(Thing t)
		{
			return t != null && t.Spawned && t.def.deteriorateFromEnvironmentalEffects;
		}

		// Token: 0x0400D89B RID: 55451
		private float factorOffsetUnroofed;

		// Token: 0x0400D89C RID: 55452
		private float factorOffsetOutdoors;

		// Token: 0x0400D89D RID: 55453
		private float protectedByEdificeFactor;
	}
}
