using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003703 RID: 14083
	public class StatPart_GrowthVatSpeedFactor : StatPart
	{
		// Token: 0x0601387B RID: 79995 RVA: 0x005CB76C File Offset: 0x005C996C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x0601387C RID: 79996 RVA: 0x005CB78C File Offset: 0x005C998C
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				return "StatsReport_GrowthVatSpeedFactor".Translate() + ": " + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0601387D RID: 79997 RVA: 0x005CB7CC File Offset: 0x005C99CC
		private bool TryGetFactor(StatRequest req, out float value)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Ideo != null)
				{
					value = 1f;
					foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
					{
						value *= precept.def.growthVatSpeedFactor;
					}
					return true;
				}
			}
			value = 0f;
			return false;
		}
	}
}
