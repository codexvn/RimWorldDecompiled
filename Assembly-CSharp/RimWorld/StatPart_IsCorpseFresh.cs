using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003708 RID: 14088
	public class StatPart_IsCorpseFresh : StatPart
	{
		// Token: 0x06013894 RID: 80020 RVA: 0x005CBB44 File Offset: 0x005C9D44
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x06013895 RID: 80021 RVA: 0x005CBB64 File Offset: 0x005C9D64
		public override string ExplanationPart(StatRequest req)
		{
			float num;
			if (this.TryGetIsFreshFactor(req, out num) && num != 1f)
			{
				return "StatsReport_NotFresh".Translate() + ": x" + num.ToStringPercent();
			}
			return null;
		}

		// Token: 0x06013896 RID: 80022 RVA: 0x005CBBAC File Offset: 0x005C9DAC
		private bool TryGetIsFreshFactor(StatRequest req, out float factor)
		{
			if (!req.HasThing)
			{
				factor = 1f;
				return false;
			}
			Corpse corpse = req.Thing as Corpse;
			if (corpse == null)
			{
				factor = 1f;
				return false;
			}
			factor = ((corpse.GetRotStage() == RotStage.Fresh) ? 1f : 0f);
			return true;
		}
	}
}
