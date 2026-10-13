using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200371D RID: 14109
	public class StatPart_Resting : StatPart
	{
		// Token: 0x060138F1 RID: 80113 RVA: 0x005CCDBC File Offset: 0x005CAFBC
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					val *= this.RestingMultiplier(pawn);
				}
			}
		}

		// Token: 0x060138F2 RID: 80114 RVA: 0x005CCDF0 File Offset: 0x005CAFF0
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Resting".Translate() + ": x" + this.RestingMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060138F3 RID: 80115 RVA: 0x005CCE44 File Offset: 0x005CB044
		private float RestingMultiplier(Pawn pawn)
		{
			if (pawn.InBed() || (pawn.GetPosture() != PawnPosture.Standing && !pawn.Downed) || (pawn.IsCaravanMember() && !pawn.GetCaravan().pather.MovingNow) || pawn.InCaravanBed() || pawn.CarriedByCaravan())
			{
				return this.factor;
			}
			return 1f;
		}

		// Token: 0x0400D8FC RID: 55548
		public float factor = 1f;
	}
}
