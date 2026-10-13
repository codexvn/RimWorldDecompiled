using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E4 RID: 14052
	public class StatPart_BiosculptingSpeedFactor : StatPart
	{
		// Token: 0x060137F0 RID: 79856 RVA: 0x005C9980 File Offset: 0x005C7B80
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetFactor(req, out num))
			{
				val *= num;
			}
		}

		// Token: 0x060137F1 RID: 79857 RVA: 0x005C99A0 File Offset: 0x005C7BA0
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetFactor(req, out f))
			{
				return "StatsReport_BiosculptingSpeedFactor".Translate() + ": " + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x060137F2 RID: 79858 RVA: 0x005C99E0 File Offset: 0x005C7BE0
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
						value *= precept.def.biosculpterPodCycleSpeedFactor;
					}
					return true;
				}
			}
			value = 0f;
			return false;
		}
	}
}
