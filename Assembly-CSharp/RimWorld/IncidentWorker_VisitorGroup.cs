using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001E1B RID: 7707
	public class IncidentWorker_VisitorGroup : IncidentWorker_NeutralGroup
	{
		// Token: 0x0600AB7A RID: 43898 RVA: 0x0031086C File Offset: 0x0030EA6C
		protected virtual LordJob_VisitColony CreateLordJob(IncidentParms parms, List<Pawn> pawns)
		{
			IntVec3 chillSpot;
			RCellFinder.TryFindRandomSpotJustOutsideColony(pawns[0], out chillSpot);
			return new LordJob_VisitColony(parms.faction, chillSpot, null);
		}

		// Token: 0x0600AB7B RID: 43899 RVA: 0x003108A0 File Offset: 0x0030EAA0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!base.TryResolveParms(parms))
			{
				return false;
			}
			List<Pawn> list = base.SpawnPawns(parms);
			if (list.Count == 0)
			{
				return false;
			}
			LordMaker.MakeNewLord(parms.faction, this.CreateLordJob(parms, list), map, list);
			bool traderExists = false;
			if (Rand.Value < 0.75f)
			{
				traderExists = this.TryConvertOnePawnToSmallTrader(list, parms.faction, map);
			}
			Pawn leader = list.Find((Pawn x) => parms.faction.leader == x);
			this.SendLetter(parms, list, leader, traderExists);
			return true;
		}

		// Token: 0x0600AB7C RID: 43900 RVA: 0x0031095C File Offset: 0x0030EB5C
		protected virtual void SendLetter(IncidentParms parms, List<Pawn> pawns, Pawn leader, bool traderExists)
		{
			TaggedString baseLetterLabel;
			TaggedString baseLetterText;
			if (pawns.Count == 1)
			{
				TaggedString value = traderExists ? ("\n\n" + "SingleVisitorArrivesTraderInfo".Translate(pawns[0].Named("PAWN")).AdjustedFor(pawns[0], "PAWN", true)) : "";
				TaggedString value2 = (leader != null) ? ("\n\n" + "SingleVisitorArrivesLeaderInfo".Translate(pawns[0].Named("PAWN")).AdjustedFor(pawns[0], "PAWN", true)) : "";
				baseLetterLabel = "LetterLabelSingleVisitorArrives".Translate();
				baseLetterText = "SingleVisitorArrives".Translate(pawns[0].story.Title, parms.faction.NameColored, pawns[0].Name.ToStringFull, value, value2, pawns[0].Named("PAWN")).AdjustedFor(pawns[0], "PAWN", true);
			}
			else
			{
				TaggedString value3 = traderExists ? ("\n\n" + "GroupVisitorsArriveTraderInfo".Translate()) : TaggedString.Empty;
				TaggedString value4 = (leader != null) ? ("\n\n" + "GroupVisitorsArriveLeaderInfo".Translate(leader.LabelShort, leader)) : TaggedString.Empty;
				baseLetterLabel = "LetterLabelGroupVisitorsArrive".Translate();
				baseLetterText = "GroupVisitorsArrive".Translate(parms.faction.NameColored, value3, value4);
			}
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(pawns, ref baseLetterLabel, ref baseLetterText, "LetterRelatedPawnsNeutralGroup".Translate(Faction.OfPlayer.def.pawnsPlural), true, true);
			base.SendStandardLetter(baseLetterLabel, baseLetterText, LetterDefOf.NeutralEvent, parms, pawns[0], Array.Empty<NamedArgument>());
		}

		// Token: 0x0600AB7D RID: 43901 RVA: 0x00310B6A File Offset: 0x0030ED6A
		protected override void ResolveParmsPoints(IncidentParms parms)
		{
			if (parms.points >= 0f)
			{
				return;
			}
			parms.points = Rand.ByCurve(IncidentWorker_VisitorGroup.PointsCurve);
		}

		// Token: 0x0600AB7E RID: 43902 RVA: 0x00310B8C File Offset: 0x0030ED8C
		private bool TryConvertOnePawnToSmallTrader(List<Pawn> pawns, Faction faction, Map map)
		{
			if (faction.def.visitorTraderKinds.NullOrEmpty<TraderKindDef>())
			{
				return false;
			}
			IEnumerable<Pawn> source = from p in pawns
			where p.DevelopmentalStage.Adult()
			select p;
			if (!source.Any<Pawn>())
			{
				return false;
			}
			Pawn pawn = source.RandomElement<Pawn>();
			Lord lord = pawn.GetLord();
			pawn.mindState.wantsToTradeWithColony = true;
			PawnComponentsUtility.AddAndRemoveDynamicComponents(pawn, true);
			TraderKindDef traderKindDef = faction.def.visitorTraderKinds.RandomElementByWeight((TraderKindDef traderDef) => traderDef.CalculatedCommonality);
			pawn.trader.traderKind = traderKindDef;
			pawn.inventory.DestroyAll(DestroyMode.Vanish);
			ThingSetMakerParams parms = default(ThingSetMakerParams);
			parms.traderDef = traderKindDef;
			parms.tile = new PlanetTile?(map.Tile);
			parms.makingFaction = faction;
			foreach (Thing thing in ThingSetMakerDefOf.TraderStock.root.Generate(parms))
			{
				Pawn pawn2 = thing as Pawn;
				if (pawn2 != null)
				{
					if (pawn2.Faction != pawn.Faction)
					{
						pawn2.SetFaction(pawn.Faction, null);
					}
					IntVec3 loc = CellFinder.RandomClosewalkCellNear(pawn.Position, map, 5, null);
					GenSpawn.Spawn(pawn2, loc, map, WipeMode.Vanish);
					lord.AddPawn(pawn2);
				}
				else if (!pawn.inventory.innerContainer.TryAdd(thing, true))
				{
					thing.Destroy(DestroyMode.Vanish);
				}
			}
			PawnInventoryGenerator.GiveRandomFood(pawn);
			TradeUtility.CheckGiveTraderQuest(pawn);
			return true;
		}

		// Token: 0x040075ED RID: 30189
		private const float TraderChance = 0.75f;

		// Token: 0x040075EE RID: 30190
		private static readonly SimpleCurve PointsCurve = new SimpleCurve
		{
			{
				new CurvePoint(45f, 0f),
				true
			},
			{
				new CurvePoint(50f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1f),
				true
			},
			{
				new CurvePoint(200f, 0.25f),
				true
			},
			{
				new CurvePoint(300f, 0.1f),
				true
			},
			{
				new CurvePoint(500f, 0f),
				true
			}
		};
	}
}
