using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02003DED RID: 15853
	public class SitePartWorker_DistressCall_Fleshbeasts : SitePartWorker_DistressCall
	{
		// Token: 0x06015A1A RID: 88602 RVA: 0x00679714 File Offset: 0x00677914
		public override void PostMapGenerate(Map map)
		{
			Site site = map.Parent as Site;
			Faction faction = site.Faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			if (faction.IsPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				faction = faction,
				groupKind = PawnGroupKindDefOf.Settlement,
				points = SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange * 0.33f,
				tile = map.Tile
			}, true).ToList<Pawn>();
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Fleshbeasts, null) * 1.05f;
			float num2 = Mathf.Max(SitePartWorker_DistressCall_Fleshbeasts.FleshbeastsPointsModifierCurve.Evaluate(site.desiredThreatPoints), num);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Fleshbeasts,
				points = Rand.Range(num, num2 * 0.33f),
				faction = Faction.OfEntities,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			}, true).ToList<Pawn>();
			List<Pawn> list3 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Fleshbeasts,
				points = num2,
				faction = Faction.OfEntities,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			}, true).ToList<Pawn>();
			this.SplitFleshbeasts(ref list2);
			DistressCallUtility.SpawnCorpses(map, list2, list, map.Center, 20);
			DistressCallUtility.SpawnCorpses(map, list, list2.Concat(list3), map.Center, 20);
			DistressCallUtility.SpawnPawns(map, list3, map.Center, 20);
			this.ScatterCorpsesAroundPitBurrows(map, list);
		}

		// Token: 0x06015A1B RID: 88603 RVA: 0x006798A8 File Offset: 0x00677AA8
		private void ScatterCorpsesAroundPitBurrows(Map map, IEnumerable<Pawn> killers)
		{
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.PitBurrow))
			{
				if (Rand.Chance(0.4f))
				{
					int randomInRange = SitePartWorker_DistressCall_Fleshbeasts.BurrowCorpseCountRange.RandomInRange;
					List<Pawn> list = new List<Pawn>();
					for (int i = 0; i < randomInRange; i++)
					{
						list.Add(PawnGenerator.GeneratePawn(FleshbeastUtility.AllFleshbeasts.RandomElement<PawnKindDef>(), Faction.OfEntities, null));
					}
					this.SplitFleshbeasts(ref list);
					DistressCallUtility.SpawnCorpses(map, list, killers, thing.Position, 3);
				}
			}
		}

		// Token: 0x06015A1C RID: 88604 RVA: 0x0067996C File Offset: 0x00677B6C
		private void SplitFleshbeasts(ref List<Pawn> fleshbeasts)
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn fleshbeast in fleshbeasts)
			{
				foreach (Pawn item in FleshbeastUtility.SplitFleshbeast(fleshbeast))
				{
					list.Add(item);
				}
			}
			fleshbeasts = list;
		}

		// Token: 0x0400F05F RID: 61535
		private const float CorpsePointFactor = 0.33f;

		// Token: 0x0400F060 RID: 61536
		private const int SpawnRadius = 20;

		// Token: 0x0400F061 RID: 61537
		private static readonly SimpleCurve FleshbeastsPointsModifierCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(500f, 250f),
				true
			},
			{
				new CurvePoint(1000f, 400f),
				true
			},
			{
				new CurvePoint(5000f, 800f),
				true
			}
		};

		// Token: 0x0400F062 RID: 61538
		private const float ChanceForFleshbeastCorpsesAroundBurrow = 0.4f;

		// Token: 0x0400F063 RID: 61539
		private static readonly IntRange BurrowCorpseCountRange = new IntRange(1, 2);
	}
}
