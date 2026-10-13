using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003731 RID: 14129
	public class StatPart_WorkTableTemperature : StatPart
	{
		// Token: 0x06013945 RID: 80197 RVA: 0x005CDF79 File Offset: 0x005CC179
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				val *= 0.7f;
			}
		}

		// Token: 0x06013946 RID: 80198 RVA: 0x005CDF9C File Offset: 0x005CC19C
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableTemperature.Applies(req.Thing))
			{
				return "BadTemperature".Translate().CapitalizeFirst() + ": x" + 0.7f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06013947 RID: 80199 RVA: 0x005CDFF2 File Offset: 0x005CC1F2
		public static bool Applies(Thing t)
		{
			return t.Spawned && StatPart_WorkTableTemperature.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x06013948 RID: 80200 RVA: 0x005CE018 File Offset: 0x005CC218
		public static bool Applies(ThingDef tDef, Map map, IntVec3 c)
		{
			if (map == null)
			{
				return false;
			}
			if (tDef.building == null)
			{
				return false;
			}
			float temperatureForCell = GenTemperature.GetTemperatureForCell(c, map);
			return temperatureForCell < 9f || temperatureForCell > 35f;
		}

		// Token: 0x0400D914 RID: 55572
		public const float WorkRateFactor = 0.7f;

		// Token: 0x0400D915 RID: 55573
		public const float MinTemp = 9f;

		// Token: 0x0400D916 RID: 55574
		public const float MaxTemp = 35f;
	}
}
