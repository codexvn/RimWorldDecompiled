using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003712 RID: 14098
	public class StatPart_NoxiousHaze : StatPart
	{
		// Token: 0x060138C2 RID: 80066 RVA: 0x005CC18C File Offset: 0x005CA38C
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x060138C3 RID: 80067 RVA: 0x005CC1A8 File Offset: 0x005CA3A8
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(GameConditionDefOf.NoxiousHaze.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x060138C4 RID: 80068 RVA: 0x005CC207 File Offset: 0x005CA407
		private bool ActiveFor(Thing t)
		{
			return ModLister.CheckBiotech("Noxious haze stat part") && (t != null && t.def.deteriorateFromEnvironmentalEffects) && NoxiousHazeUtility.IsExposedToNoxiousHaze(t);
		}

		// Token: 0x0400D8D9 RID: 55513
		private float multiplier = 1f;
	}
}
