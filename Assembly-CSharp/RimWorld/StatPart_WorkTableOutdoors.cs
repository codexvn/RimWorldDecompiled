using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200372F RID: 14127
	public class StatPart_WorkTableOutdoors : StatPart
	{
		// Token: 0x0601393B RID: 80187 RVA: 0x005CDCE0 File Offset: 0x005CBEE0
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				val *= 0.8f;
			}
		}

		// Token: 0x0601393C RID: 80188 RVA: 0x005CDD04 File Offset: 0x005CBF04
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && StatPart_WorkTableOutdoors.Applies(req.Thing))
			{
				return "Outdoors".Translate().CapitalizeFirst() + ": x" + 0.8f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0601393D RID: 80189 RVA: 0x005CDD5A File Offset: 0x005CBF5A
		public static bool Applies(Thing t)
		{
			return StatPart_WorkTableOutdoors.Applies(t.def, t.Map, t.Position);
		}

		// Token: 0x0601393E RID: 80190 RVA: 0x005CDD74 File Offset: 0x005CBF74
		public static bool Applies(ThingDef def, Map map, IntVec3 c)
		{
			if (def.building == null)
			{
				return false;
			}
			if (map == null)
			{
				return false;
			}
			Room room = c.GetRoom(map);
			return room != null && room.PsychologicallyOutdoors;
		}

		// Token: 0x0400D913 RID: 55571
		public const float WorkRateFactor = 0.8f;
	}
}
