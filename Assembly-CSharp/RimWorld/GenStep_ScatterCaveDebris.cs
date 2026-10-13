using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001FF9 RID: 8185
	public class GenStep_ScatterCaveDebris : GenStep_Scatterer
	{
		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x0600B459 RID: 46169 RVA: 0x0033BAA0 File Offset: 0x00339CA0
		public override int SeedPart
		{
			get
			{
				return 85037593;
			}
		}

		// Token: 0x0600B45A RID: 46170 RVA: 0x0033BAA7 File Offset: 0x00339CA7
		public override void Generate(Map map, GenStepParams parms)
		{
			if (!ModLister.CheckIdeology("Scatter cave debris"))
			{
				return;
			}
			if (!Find.World.HasCaves(map.Tile))
			{
				return;
			}
			this.count = 1;
			base.Generate(map, parms);
		}

		// Token: 0x0600B45B RID: 46171 RVA: 0x0033BAD8 File Offset: 0x00339CD8
		protected override bool CanScatterAt(IntVec3 c, Map map)
		{
			RoofDef roof = c.GetRoof(map);
			if (roof == null || !roof.isNatural)
			{
				return false;
			}
			int num = Rand.RangeInclusive(1, 4);
			for (int i = 0; i < 4; i++)
			{
				this.rotation = new Rot4(i % num);
				if (this.CanPlace(c, this.rotation, map))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B45C RID: 46172 RVA: 0x0033BB30 File Offset: 0x00339D30
		private bool CanPlace(IntVec3 c, Rot4 r, Map map)
		{
			if (!map.reachability.CanReachMapEdge(c, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false)))
			{
				return false;
			}
			CellRect cellRect = GenAdj.OccupiedRect(c, r, ThingDefOf.AncientBed.size);
			using (CellRect.Enumerator enumerator = cellRect.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.GetEdifice(map) != null)
					{
						return false;
					}
				}
			}
			GenStep_ScatterCaveDebris.edgesToCheck.Clear();
			if (cellRect.Width > cellRect.Height)
			{
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.North);
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.South);
			}
			else
			{
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.East);
				GenStep_ScatterCaveDebris.edgesToCheck.Add(Rot4.West);
			}
			GenStep_ScatterCaveDebris.edgesToCheck.Shuffle<Rot4>();
			foreach (Rot4 dir in GenStep_ScatterCaveDebris.edgesToCheck)
			{
				bool flag = true;
				foreach (IntVec3 a in cellRect.GetEdgeCells(dir))
				{
					IntVec3 c2 = a + dir.FacingCell;
					if (c2.InBounds(map) && c2.GetEdifice(map) != null && c2.GetEdifice(map).def.building.isNaturalRock)
					{
						flag = false;
						break;
					}
				}
				foreach (IntVec3 a2 in cellRect.GetEdgeCells(dir.Opposite))
				{
					IntVec3 c3 = a2 + dir.Opposite.FacingCell;
					if (c3.InBounds(map) && c3.GetEdifice(map) == null)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					GenStep_ScatterCaveDebris.edgesToCheck.Clear();
					return true;
				}
			}
			GenStep_ScatterCaveDebris.edgesToCheck.Clear();
			return false;
		}

		// Token: 0x0600B45D RID: 46173 RVA: 0x0033BD98 File Offset: 0x00339F98
		protected override void ScatterAt(IntVec3 loc, Map map, GenStepParams parms, int count = 1)
		{
			GenStep_ScatterCaveDebris.<>c__DisplayClass8_0 CS$<>8__locals1 = new GenStep_ScatterCaveDebris.<>c__DisplayClass8_0();
			CS$<>8__locals1.map = map;
			GenSpawn.Spawn(ThingMaker.MakeThing(ThingDefOf.AncientBed, null), loc, CS$<>8__locals1.map, this.rotation, WipeMode.Vanish, false, false);
			IntVec3 invalid = IntVec3.Invalid;
			if (CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_MoldyUniform, 1, FilthSourceFlags.None, true);
			}
			if (CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_CorpseBile, 1, FilthSourceFlags.None, true);
			}
			if (Rand.Bool && CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_ScatteredDocuments, 1, FilthSourceFlags.None, true);
			}
			if (Rand.Bool && CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid, -1))
			{
				FilthMaker.TryMakeFilth(invalid, CS$<>8__locals1.map, ThingDefOf.Filth_DriedBlood, 1, FilthSourceFlags.None, true);
			}
			IntVec3 invalid2 = IntVec3.Invalid;
			if (Rand.Chance(0.25f) && CellFinder.TryFindRandomCellNear(loc, CS$<>8__locals1.map, 5, new Predicate<IntVec3>(CS$<>8__locals1.<ScatterAt>g__CellPredicate|0), out invalid2, -1))
			{
				Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Drifter, null, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, true, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, true, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
				pawn.Corpse.Age = GenStep_ScatterCaveDebris.CorpseRandomAgeRange.RandomInRange;
				pawn.relations.hidePawnRelations = true;
				pawn.Corpse.GetComp<CompRottable>().RotProgress += (float)pawn.Corpse.Age;
				GenSpawn.Spawn(pawn.Corpse, invalid2, CS$<>8__locals1.map, WipeMode.Vanish);
			}
		}

		// Token: 0x04007BAC RID: 31660
		private static readonly IntRange CorpseRandomAgeRange = new IntRange(900000, 18000000);

		// Token: 0x04007BAD RID: 31661
		private Rot4 rotation;

		// Token: 0x04007BAE RID: 31662
		private static List<Rot4> edgesToCheck = new List<Rot4>();
	}
}
