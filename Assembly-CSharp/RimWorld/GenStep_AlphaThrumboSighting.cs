using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002049 RID: 8265
	public class GenStep_AlphaThrumboSighting : GenStep
	{
		// Token: 0x17001BA8 RID: 7080
		// (get) Token: 0x0600B58C RID: 46476 RVA: 0x00342E01 File Offset: 0x00341001
		public override int SeedPart
		{
			get
			{
				return 792525399;
			}
		}

		// Token: 0x0600B58D RID: 46477 RVA: 0x00342E08 File Offset: 0x00341008
		public override void Generate(Map map, GenStepParams parms)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true);
			List<CellRect> usedRects = MapGenerator.GetOrGenerateVar<List<CellRect>>("UsedRects");
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map).CellCount >= this.MinRoomCells && !usedRects.Any((CellRect ur) => ur.Contains(x)), map, out root))
			{
				List<Pawn> list = new List<Pawn>();
				PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.AlphaThrumbo, null, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)
				{
					ExcludeBiologicalAgeRange = new FloatRange?(GenStep_AlphaThrumboSighting.ExcludeBiologicalAgeRange)
				};
				Pawn item = PawnGenerator.GeneratePawn(request);
				list.Add(item);
				int randomInRange = this.normalThrumboCountRange.RandomInRange;
				for (int i = 0; i < randomInRange; i++)
				{
					request = new PawnGenerationRequest(PawnKindDefOf.Thrumbo, null, PawnGenerationContext.NonPlayer, new PlanetTile?(map.Tile), false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
					item = PawnGenerator.GeneratePawn(request);
					list.Add(item);
				}
				for (int j = 0; j < list.Count; j++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					GenSpawn.Spawn(list[j], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
				}
			}
		}

		// Token: 0x04007CA0 RID: 31904
		public IntRange normalThrumboCountRange = new IntRange(3, 5);

		// Token: 0x04007CA1 RID: 31905
		private int MinRoomCells = 225;

		// Token: 0x04007CA2 RID: 31906
		private static readonly FloatRange ExcludeBiologicalAgeRange = new FloatRange(0f, 250f);
	}
}
