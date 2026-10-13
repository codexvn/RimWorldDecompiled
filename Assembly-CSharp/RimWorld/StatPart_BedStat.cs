using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020036E2 RID: 14050
	public class StatPart_BedStat : StatPart
	{
		// Token: 0x060137E9 RID: 79849 RVA: 0x005C9890 File Offset: 0x005C7A90
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.BedMultiplier(pawn);
				}
			}
		}

		// Token: 0x060137EA RID: 79850 RVA: 0x005C98C4 File Offset: 0x005C7AC4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.ageTracker != null)
				{
					return "StatsReport_InBed".Translate() + ": x" + this.BedMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060137EB RID: 79851 RVA: 0x005C991E File Offset: 0x005C7B1E
		private float BedMultiplier(Pawn pawn)
		{
			if (pawn.InBed())
			{
				return pawn.CurrentBed().GetStatValue(this.stat, true, -1);
			}
			if (pawn.InCaravanBed())
			{
				return pawn.CurrentCaravanBed().GetStatValue(this.stat, true, -1);
			}
			return 1f;
		}

		// Token: 0x0400D891 RID: 55441
		private StatDef stat;
	}
}
