using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E1 RID: 14049
	public class StatPart_ArtificialBuildingsNearbyOffset : StatPart
	{
		// Token: 0x060137E5 RID: 79845 RVA: 0x005C97A8 File Offset: 0x005C79A8
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetArtificialBuildingOffset(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x060137E6 RID: 79846 RVA: 0x005C97C8 File Offset: 0x005C79C8
		private bool TryGetArtificialBuildingOffset(StatRequest req, out float offset)
		{
			if (!req.HasThing || !req.Thing.Spawned)
			{
				offset = 0f;
				return false;
			}
			offset = this.curve.Evaluate((float)req.Thing.Map.listerArtificialBuildingsForMeditation.GetForCell(req.Thing.Position, this.radius).Count);
			return true;
		}

		// Token: 0x060137E7 RID: 79847 RVA: 0x005C9834 File Offset: 0x005C7A34
		public override string ExplanationPart(StatRequest req)
		{
			float f;
			if (this.TryGetArtificialBuildingOffset(req, out f))
			{
				return "StatsReport_NearbyArtificialStructures".Translate().CapitalizeFirst() + ": " + f.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0400D88F RID: 55439
		public SimpleCurve curve;

		// Token: 0x0400D890 RID: 55440
		public float radius = 10f;
	}
}
