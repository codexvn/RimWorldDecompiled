using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FA7 RID: 8103
	public class GenStep_Dreadmeld : GenStep
	{
		// Token: 0x17001B3D RID: 6973
		// (get) Token: 0x0600B2E8 RID: 45800 RVA: 0x00333D73 File Offset: 0x00331F73
		public override int SeedPart
		{
			get
			{
				return 21341517;
			}
		}

		// Token: 0x0600B2E9 RID: 45801 RVA: 0x00333D7C File Offset: 0x00331F7C
		public override void Generate(Map map, GenStepParams parms)
		{
			PocketMapExit pitGateExit = (PocketMapExit)map.listerThings.ThingsOfDef(ThingDefOf.CaveExit).First<Thing>();
			IntVec3 intVec;
			CellFinder.TryFindRandomCell(map, (IntVec3 c) => c.Standable(map) && !c.InHorDistOf(pitGateExit.Position, 20f) && c.DistanceToEdge(map) > 10, out intVec);
			List<IntVec3> list = GridShapeMaker.IrregularLump(intVec, map, 100, null);
			List<IntVec3> list2 = GridShapeMaker.IrregularLump(intVec, map, 20, null);
			foreach (IntVec3 intVec2 in list)
			{
				GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.Fleshmass, null), intVec2, map, Rot4.Random, WipeMode.Vanish, false, false).SetFaction(Faction.OfEntities, null);
				map.terrainGrid.SetTerrain(intVec2, TerrainDefOf.Flesh);
			}
			foreach (IntVec3 c2 in list2)
			{
				foreach (Thing thing in from t in c2.GetThingList(map).ToList<Thing>()
				where t.def.destroyable
				select t)
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			int randomInRange = GenStep_Dreadmeld.NumFleshBulbsRange.RandomInRange;
			Predicate<IntVec3> <>9__2;
			for (int i = 0; i < randomInRange; i++)
			{
				IntVec3 root = intVec;
				Map map2 = map;
				int squareRadius = 5;
				Predicate<IntVec3> validator;
				if ((validator = <>9__2) == null)
				{
					validator = (<>9__2 = ((IntVec3 c) => c.GetEdifice(map) == null));
				}
				IntVec3 loc;
				if (CellFinder.TryFindRandomCellNear(root, map2, squareRadius, validator, out loc, -1))
				{
					GenSpawn.Spawn(ThingDefOf.Fleshbulb, loc, map, WipeMode.Vanish).SetFaction(Faction.OfEntities, null);
				}
			}
			GenSpawn.Spawn(PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Dreadmeld, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false)), intVec, map, Rot4.Random, WipeMode.Vanish, false, false);
			string signalTag = "dreadmeldApproached-" + Find.UniqueIDsManager.GetNextSignalTagID().ToString();
			CellRect rect = CellRect.FromCellList(list).ExpandedBy(2).ClipInsideMap(map);
			RectTrigger rectTrigger = (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			rectTrigger.destroyIfUnfogged = true;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			SignalAction_Letter signalAction_Letter = (SignalAction_Letter)ThingMaker.MakeThing(ThingDefOf.SignalAction_Letter, null);
			signalAction_Letter.signalTag = signalTag;
			signalAction_Letter.letterDef = LetterDefOf.ThreatBig;
			signalAction_Letter.letterLabelKey = "LetterLabelDreadmeldWarning";
			signalAction_Letter.letterMessageKey = "LetterDreadmeldWarning";
			GenSpawn.Spawn(signalAction_Letter, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x04007A82 RID: 31362
		private const int HiveSize = 100;

		// Token: 0x04007A83 RID: 31363
		private const int HiveInteriorSize = 20;

		// Token: 0x04007A84 RID: 31364
		private const float MinSpawnDistFromGateExit = 20f;

		// Token: 0x04007A85 RID: 31365
		private static readonly IntRange NumFleshBulbsRange = new IntRange(2, 4);
	}
}
