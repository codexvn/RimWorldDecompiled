using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018CD RID: 6349
	public class ThoughtWorker_Pyrophobia : ThoughtWorker
	{
		// Token: 0x060090BD RID: 37053 RVA: 0x002AA0D1 File Offset: 0x002A82D1
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (p.genes == null || !p.genes.HasActiveGene(GeneDefOf.FireTerror) || !ThoughtWorker_Pyrophobia.NearFire(p))
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveAtStage(0);
		}

		// Token: 0x060090BE RID: 37054 RVA: 0x002AA110 File Offset: 0x002A8310
		public static bool NearFire(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return false;
			}
			IntVec3 positionHeld = pawn.PositionHeld;
			int num = GenRadial.NumCellsInRadius(19.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 intVec = pawn.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(mapHeld) && !intVec.Fogged(mapHeld) && GenSight.LineOfSight(positionHeld, intVec, mapHeld, true, null, 0, 0) && intVec.ContainsStaticFire(mapHeld))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005C5A RID: 23642
		public const float FireRadius = 19.9f;
	}
}
