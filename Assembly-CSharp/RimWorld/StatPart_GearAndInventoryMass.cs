using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036FA RID: 14074
	public class StatPart_GearAndInventoryMass : StatPart
	{
		// Token: 0x06013840 RID: 79936 RVA: 0x005CA8E0 File Offset: 0x005C8AE0
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num;
			if (this.TryGetValue(req, out num))
			{
				val += num;
			}
		}

		// Token: 0x06013841 RID: 79937 RVA: 0x005CA900 File Offset: 0x005C8B00
		public override string ExplanationPart(StatRequest req)
		{
			float mass;
			if (this.TryGetValue(req, out mass))
			{
				return "StatsReport_GearAndInventoryMass".Translate() + ": " + mass.ToStringMassOffset();
			}
			return null;
		}

		// Token: 0x06013842 RID: 79938 RVA: 0x005CA940 File Offset: 0x005C8B40
		private bool TryGetValue(StatRequest req, out float value)
		{
			return PawnOrCorpseStatUtility.TryGetPawnOrCorpseStat(req, (Pawn x) => MassUtility.GearAndInventoryMass(x), (ThingDef x) => 0f, out value);
		}

		// Token: 0x06013843 RID: 79939 RVA: 0x005CA992 File Offset: 0x005C8B92
		public override bool ForceShow(StatRequest req)
		{
			return req.Pawn != null || req.Thing is Pawn;
		}
	}
}
