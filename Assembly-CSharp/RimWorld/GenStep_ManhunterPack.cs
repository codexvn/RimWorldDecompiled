using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200206B RID: 8299
	public class GenStep_ManhunterPack : GenStep
	{
		// Token: 0x17001BD2 RID: 7122
		// (get) Token: 0x0600B61C RID: 46620 RVA: 0x00344E97 File Offset: 0x00343097
		public override int SeedPart
		{
			get
			{
				return 457293335;
			}
		}

		// Token: 0x0600B61D RID: 46621 RVA: 0x00344EA0 File Offset: 0x003430A0
		public override void Generate(Map map, GenStepParams parms)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false).WithFenceblocked(true);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map).CellCount >= this.MinRoomCells, map, out root))
			{
				float points = (parms.sitePart != null) ? parms.sitePart.parms.threatPoints : this.defaultPointsRange.RandomInRange;
				PawnKindDef animalKind;
				if (parms.sitePart != null && parms.sitePart.parms.animalKind != null)
				{
					animalKind = parms.sitePart.parms.animalKind;
				}
				else if (!ManhunterPackGenStepUtility.TryGetAnimalsKind(points, map.Tile, out animalKind))
				{
					return;
				}
				List<Pawn> list = AggressiveAnimalIncidentUtility.GenerateAnimals(animalKind, map.Tile, points, 0);
				for (int i = 0; i < list.Count; i++)
				{
					IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
					GenSpawn.Spawn(list[i], loc, map, Rot4.Random, WipeMode.Vanish, false, false);
					list[i].health.AddHediff(HediffDefOf.Scaria, null, null, null);
					list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, false, null, false, false, false);
				}
			}
		}

		// Token: 0x04007CE8 RID: 31976
		public FloatRange defaultPointsRange = new FloatRange(300f, 500f);

		// Token: 0x04007CE9 RID: 31977
		private int MinRoomCells = 225;
	}
}
