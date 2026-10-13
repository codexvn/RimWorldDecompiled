using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E8 RID: 14056
	public class StatPart_BlindPsychicSensitivityOffset : StatPart
	{
		// Token: 0x060137FE RID: 79870 RVA: 0x005C9C00 File Offset: 0x005C7E00
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetPsychicOffset(req.Thing, out num))
			{
				val += num;
			}
		}

		// Token: 0x060137FF RID: 79871 RVA: 0x005C9C24 File Offset: 0x005C7E24
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (req.HasThing && this.TryGetPsychicOffset(req.Thing, out f))
			{
				return "StatsReport_BlindPsychicSensitivityOffset".Translate() + (": +" + f.ToStringPercent());
			}
			return null;
		}

		// Token: 0x06013800 RID: 79872 RVA: 0x005C9C74 File Offset: 0x005C7E74
		private bool TryGetPsychicOffset(Thing t, out float offset)
		{
			offset = 0f;
			Pawn pawn = t as Pawn;
			if (pawn == null)
			{
				return false;
			}
			if (!this.ConsideredBlind(pawn) || pawn.Ideo == null)
			{
				return false;
			}
			foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
			{
				offset += precept.def.blindPsychicSensitivityOffset;
			}
			return !Mathf.Approximately(offset, 0f);
		}

		// Token: 0x06013801 RID: 79873 RVA: 0x005C9D0C File Offset: 0x005C7F0C
		private bool ConsideredBlind(Pawn pawn)
		{
			foreach (BodyPartRecord part in pawn.RaceProps.body.GetPartsWithTag(BodyPartTagDefOf.SightSource))
			{
				if (!pawn.health.hediffSet.PartIsMissing(part))
				{
					return false;
				}
			}
			return true;
		}
	}
}
