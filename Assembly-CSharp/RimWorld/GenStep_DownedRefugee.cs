using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002059 RID: 8281
	public class GenStep_DownedRefugee : GenStep_Scatterer
	{
		// Token: 0x17001BCB RID: 7115
		// (get) Token: 0x0600B5E5 RID: 46565 RVA: 0x00344150 File Offset: 0x00342350
		public override int SeedPart
		{
			get
			{
				return 931842770;
			}
		}

		// Token: 0x0600B5E6 RID: 46566 RVA: 0x00344157 File Offset: 0x00342357
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			return base.CanScatterAt(c, map) && c.Standable(map) && !c.Fogged(map);
		}

		// Token: 0x0600B5E7 RID: 46567 RVA: 0x00344178 File Offset: 0x00342378
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			Pawn pawn;
			if (parms.sitePart != null && parms.sitePart.things != null && parms.sitePart.things.Any)
			{
				pawn = (Pawn)parms.sitePart.things.Take(parms.sitePart.things[0]);
			}
			else
			{
				DownedRefugeeComp component = map.Parent.GetComponent<DownedRefugeeComp>();
				if (component != null && component.pawn.Any)
				{
					pawn = component.pawn.Take(component.pawn[0]);
				}
				else
				{
					pawn = DownedRefugeeQuestUtility.GenerateRefugee(map.Tile, null, 0.6f);
				}
			}
			HealthUtility.DamageUntilDowned(pawn, false, null, null, null);
			HealthUtility.DamageLegsUntilIncapableOfMoving(pawn, false);
			GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
			pawn.mindState.WillJoinColonyIfRescued = true;
			MapGenerator.SetVar<CellRect>("RectOfInterest", CellRect.CenteredOn(loc, 1, 1));
		}
	}
}
